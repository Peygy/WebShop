using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    public class Product // Класс Товара / Product class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }       
        public Category ProductCategory { get; set; }

        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Order> Orders { get; set; } = new List<Order>();

        public void ProductInfo() // Информация товара / Product information
        {
            Console.WriteLine();
            Console.WriteLine($"Название товара: {Name}");
            if (ProductCategory.Name == null)
            {
                Console.WriteLine("Категории нет");
            }
            else
            {
                Console.WriteLine($"Категория: {ProductCategory.Name}");
            }
            Console.WriteLine($"Цена: {Price} ₽");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
