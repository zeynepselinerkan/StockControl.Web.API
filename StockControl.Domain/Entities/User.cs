﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Domain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Orders=new List<Order>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoURL { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Hashleme bak!

        // Navigation Property
        // Bir kullanıcı birden fazla sipariş verebilir.
        public  virtual List<Order> Orders { get; set; }
    }
}
