using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopUser
{
    class Basket // Корзина
    {
        public Customer customer { get; set; }
        public List<Product> BasketProducts { get; set; }
        public int GeneralCost { get; set; }
        
        public Basket()
        {
            BasketProducts = new List<Product>();
        }
    }
}
