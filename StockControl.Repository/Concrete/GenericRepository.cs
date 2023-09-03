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
	}
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
    private readonly StockControlContext _context; // Dependency injection program kısmında yazıcaz.
    public GenericRepository(StockControlContext context)
    {
         _context = context;
    }
    public bool Activate(int id) 
        {
            throw new NotImplementedException();
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
            using(TransactionScope ts = new TransactionScope()) // TS : Ramde işlem bitince garbage collectorı beklemeden ramden silen bir blok. Buradaki işlemi yaptıktan sonra Ram'den siliyor.Ram üzerinde kullanılmadığı fark edilen herhangi bir şeyin temizlenmesi : garbage collector.
              // Transaction --> 10 ürün eklicem 2 si eklenmediyse diğer 8 i de ekleme. Tamamlanmaması gerekiyor.Bu işlem transaction yapılmalı. En başa dön kardeşim. Örnek : Bu olmazsa örnek : sepet onayında hata varsa da kısmen onaylanmış olur ve bankadan para çekilmeyebilir ama sipariş onayı görebilirsin. Bu işlem db de Commit Rollback. Hata durumunda geriye sarar.
            {
                //_context.Set<T>().AddRange(items); 
                foreach (T item in items) // Hepsine teker teker addeddate eklemek için ama olmasa addrange le yapabilir. Addeddate için kullandık foreachi.
                {
                    item.AddedDate= DateTime.Now;
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
        public bool Any(Expression<Func<T, bool>>[] exp)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAll(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public void DetachEntity(T item)
        {
            throw new NotImplementedException();
        }

        public List<T> GetActive()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
        return _context.SaveChanges(); // DB ye girilen girdi sayısını döner. rows affected --> sayısı döner.
        }

        public bool Update(T item)
        {
            throw new NotImplementedException();
        }
    }
}
