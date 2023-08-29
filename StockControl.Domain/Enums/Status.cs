using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Domain.Enums
{
    public enum Status // Sipariş durumu --> bekliyor, onaylanmış gibi...

    {
        Pending = 0,
        Cancelled= 1,
        Confirmed=3 

    }
}
