using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    public class Category // Класс Категории / Category class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new List<Product>();
        }
    }
}
