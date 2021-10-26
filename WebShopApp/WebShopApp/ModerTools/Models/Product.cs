using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebShopApp
{
    class Product // Класс Товара
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Category ProductCategory { get; set; }
        
        public void ProductInfo() // Инфо товара
        {
            Console.WriteLine($"Название товара: {Name}");
            if(ProductCategory.Name == null)
            {
                Console.WriteLine("Категории нет");
            }
            else
            {
                Console.WriteLine($"Категория: {ProductCategory.Name}");
            }            
            Console.WriteLine($"Цена: {Price}");
            Console.WriteLine();
        }
    }
}
