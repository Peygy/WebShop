using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    public class Order // Класс заказа
                       // Order class 
    {
        public int Id { get; set; }
        public int OrderNum { get; set; }
        public string Status { get; set; }

        public int UserId { get; set; } // А нужно ли (внизу же тоже самое)? мб удалить)
        public Customer User { get; set; }

        public List<Product> OrderProducts { get; set; } = new List<Product>();
    }
}
