using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopUser
{
    class Basket // Корзина
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [NotMapped]
        public List<Product> BasketProducts { get; set; }
        public int GeneralCost { get; set; }
        
        public Basket()
        {
            BasketProducts = new List<Product>();
        }
    }
}
