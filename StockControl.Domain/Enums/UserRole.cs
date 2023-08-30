using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Domain.Enums
{
    public enum UserRole
    {
        Admin=1,// ürün, tedarikçi ekleme gibi işlemleri yapan
        Supplier=3,
        User=5 // Stoktan çeken, stoğa ekleyen

    }
}
