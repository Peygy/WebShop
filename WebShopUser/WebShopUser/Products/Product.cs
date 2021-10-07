using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopUser
{
    class Product // Продукт
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Category ProductCategory { get; set; }
    }
}
