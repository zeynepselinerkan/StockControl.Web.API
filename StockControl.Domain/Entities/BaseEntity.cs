using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Domain.Entities
{
    public class BaseEntity // Ortak özellikler
    {
        [Column(Order =1)] // Bütün entitylerde Id hep birinci sırada olacak şekilde ayarlandı.
        public int Id { get; set; }
        public bool IsActive { get; set; } // false a çekince kullanıcı görmüyor db den silmiyorum. O yüzden silinme tarihi koymadım.
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
