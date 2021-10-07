using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopUser
{
    class Customer // Покупатель; Login = Name
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }
        public Basket Purchases { get; set; }
        public List<Order> UserOrder { get; set; }
    }
}
