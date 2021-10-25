using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class Order // Класс заказа
    {
        public int Id { get; set; }
        public int OrderNum { get; set; }
        public Customer User { get; set; }
        public string Status { get; set; }
        public List<Product> OrderProducts { get; set; }
    }
}
