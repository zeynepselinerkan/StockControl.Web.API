using Microsoft.EntityFrameworkCore;
using StockControl.Domain.Entities;
using StockControl.Repository.Abstract;
using StockControl.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StockControl.Repository.Concrete
{

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StockControlContext _context; // Dependency injection program kısmında yazıcaz.
        public GenericRepository(StockControlContext context)
        {
            _context = context;
        }

        public bool Add(T item) // DB ile işlem yaptığımız bir method
        {
            try
            {

                item.AddedDate = DateTime.Now;
                _context.Set<T>().Add(item); // İlgili entity'i buluyor. Tekrar yapılara gerek kalmıyor. Özel method yoksa herbiri için.

                // Solidin s'si olan single responsibility dahil et.
                return Save() > 0; // 1 satır döndürüyorsa true döndürür. Burada 2 döndüremez çünkü tek bir item.
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope()) // TS : Ramde işlem bitince garbage collectorı beklemeden ramden silen bir blok. Buradaki işlemi yaptıktan sonra Ram'den siliyor.Ram üzerinde kullanılmadığı fark edilen herhangi bir şeyin temizlenmesi : garbage collector.
                                                                     // Transaction --> 10 ürün eklicem 2 si eklenmediyse diğer 8 i de ekleme. Tamamlanmaması gerekiyor.Bu işlem transaction yapılmalı. En başa dön kardeşim. Örnek : Bu olmazsa örnek : sepet onayında hata varsa da kısmen onaylanmış olur ve bankadan para çekilmeyebilir ama sipariş onayı görebilirsin. Bu işlem db de Commit Rollback. Hata durumunda geriye sarar.
                {
                    //_context.Set<T>().AddRange(items); 
                    foreach (T item in items) // Hepsine teker teker addeddate eklemek için ama olmasa addrange le yapabilir. Addeddate için kullandık foreachi.
                    {
                        item.AddedDate = DateTime.Now;
                        _context.Set<T>().Add(item);
                    }
                    ts.Complete();
                    return Save() > 0; // 1 veya daha fazla satır etkileniyorsa true döndür. --> List eklediğim için birden fazla.
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Any(Expression<Func<T, bool>> exp) => _context.Set<T>().Any(exp); // LINQ sorgusu geliyor buraya exp olarak.

        public List<T> GetActive() => _context.Set<T>().Where(x => x.IsActive == true).ToList();

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes) //
        {
            var query = _context.Set<T>().Where(x => x.IsActive == true);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include)); // current o an queryden dönen tablom, include de onunla ilişkili olacak tablom.
            }
            return query;
        }

        public List<T> GetAll() => _context.Set<T>().ToList();

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include)); // current o an queryden dönen tablom, include de onunla ilişkili olacak tablom.
            }
            return query;
        }

        public T GetByDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().FirstOrDefault(exp);

        public T GetById(int id) => _context.Set<T>().Find(id);

        public IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.Id == id);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include)); // current o an queryden dönen tablom, include de onunla ilişkili olacak tablom.
            }
            return query;
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().Where(exp).ToList();

        public bool Delete(T item) // Silme işleminde silme yapmıyoruz. Update ediyoruz.
        {
            item.IsActive = false;
            return Update(item);
        }

        public bool Delete(int id)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope()) // Birden çok işlem var.
                {
                    T item = GetById(id);
                    item.IsActive = false;
                    ts.Complete();
                    return Update(item);
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteAll(Expression<Func<T, bool>> exp)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope()) // Bu nesne usingte tanımlanıp çıkınca atılıyor.
                {
                    var collection = GetDefault(exp); // Verilen ifadeye göre ilgili nesneler collectiona atıyor.
                    int counter = 0;
                    foreach (var item in collection)
                    {
                        item.IsActive = false;
                        bool operationResult = Update(item);
                        if (operationResult) counter++; // Sıradaki item'ın silinme işlemi(yani silindi işaretleme) başarılı ise sayacı 1 arttırıyoruz.
                    }
                    if (collection.Count == counter) ts.Complete(); // Koleksiyondaki eleman sayısı ile silinme işlemi gerçekleşen eleman sayısı(counter'daki sayı) eşit ise bu işlemlerin tümü başarılıdır.
                    else return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public int Save()
        {
            return _context.SaveChanges(); // DB ye girilen girdi sayısını döner. rows affected --> sayısı döner.
        }

        public bool Update(T item)
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                ; _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void DetachEntity(T item) => _context.Entry<T>(item).State = EntityState.Detached; // Entry takibi bırakmak için method. Bırakmazsak o entryle ilgili başka işlem yapamıyoruz. Change tracker araştırabilirsin.
        public bool Activate(int id)
        {
            T item = GetById(id);
            item.IsActive = true;
            return Update(item);
        }
    }
}

