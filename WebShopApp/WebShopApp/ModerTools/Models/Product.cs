using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebShopApp
{
    class Product // Продукт
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Category ProductCategory { get; set; }
        
        public void ProductInfo() // Инфо продукта
        {
            Console.WriteLine($"Название продукта: {Name}");
            Console.WriteLine($"Категория: {ProductCategory.Name}");
            Console.WriteLine($"Стоимость: {Price}");
            Console.WriteLine();
        }
    }
}
