using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class Category // Класс Категории
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
