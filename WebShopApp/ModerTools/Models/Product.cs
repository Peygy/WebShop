﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class Product // Класс Товара / Product class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Category ProductCategory { get; set; }

        public void ProductInfo() // Информация товара / Product information
        {
            Console.WriteLine($"Название товара: {Name}");
            if (ProductCategory.Name == null)
            {
                Console.WriteLine("Категории нет");
            }
            else
            {
                Console.WriteLine($"Категория: {ProductCategory.Name}");
            }
            Console.WriteLine($"Цена: {Price}");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
