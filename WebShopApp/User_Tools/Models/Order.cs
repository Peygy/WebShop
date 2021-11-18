using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    public class Order // Класс заказа / Order class 
    {
        public int Id { get; set; }
        public int OrderNum { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public Customer User { get; set; }
        public ICollection<Product> OrderProducts { get; set; }

        public Order()
        {
            OrderProducts = new List<Product>();
        }
    }
}
