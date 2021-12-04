using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    public class Customer // Покупатель / Customer
    {
        public int Id { get; set; }
        public string Login { get; set; } // Login = Name
        public string Password { get; set; }
        public string SpecialKey { get; set; }
        //public int Balance { get; set; }

        public List<Product> Basket { get; set; } = new List<Product>();
        public List<Order> Orders { get; set; } = new List<Order>();       
    }
}
