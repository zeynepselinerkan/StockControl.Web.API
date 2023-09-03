using StockControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Repository.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity // Base Entity'den inherit olan classlar
    {
        // Methodlarımızın başlıklarını tanımlayacağız. Repositorydeki methodlar, işlemi gerçekleştirecek methodlar. Service katmanında bunu uygulamaya taşıyacağımız methodları yazacağız.
        bool Add(T item);
        bool Add(List<T> items);
        bool Update(T item);
        bool Delete(T item);
        bool Delete(int id);
        bool DeleteAll(Expression<Func<T,bool>> exp);
        T GetById(int id); // T tipinde dönecek.
        IQueryable<T> GetById(int id,params Expression<Func<T, object>>[] includes); // IQ queryleri göndermeden önce birleştirip db ye gönderiyor. Geriye sonuçtan çıkan collection dönüyor. params : kaç tane dönecek bilmediğim için. Bağlantılı olduğu bilgileri de getirsin diye include kullandım linq sorgusu ile.
        T GetByDefault(Expression<Func<T, bool>> exp); // EF Core metodu first or def. çalışsın.Tek bir tane döndürecek.
        List<T> GetActive();
        IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes);

        List<T> GetDefault(Expression<Func<T, bool>> exp);  // where ile liste döndürecek.
        List<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);// IQueryable ile collection dönüyor.
        bool Activate(int id);
        bool Any(Expression<Func<T, bool>> exp);
        int Save(); // Kaç satırda işlem yapıldığını döner. SaveChanges methodunun barındırdığı method.
        void DetachEntity(T item); // İlgili entityi takip etmeyi bırak. DB de entityle işlem yapınca başka işlem yapamıyorum. Takibi bırakmak gerekiyor.

    }
}
