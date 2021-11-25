using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebShopApp
{
    public class Customer // Покупатель / Customer
    {
        public int Id { get; set; }
        public string Login { get; set; } // Login = Name
        public string Password { get; set; }
        public string SpecialKey { get; set; }
        //public int Balance { get; set; }

        public List<Product> Basket { get; set; } = new List<Product>();
        public List<Order> Orders { get; set; } = new List<Order>();

        public void BasketInfo() // Вывод корзины покупателя / Output user's basket
        {
            int generalCost = 0;

            for (int i = 0; i < Basket.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Название: {Basket[i].Name}; Цена: {Basket[i].Price}");
                generalCost = generalCost + Basket[i].Price;
            }

            Console.WriteLine();
            Console.WriteLine($"Общая стоимость: {generalCost}");
            Console.WriteLine();
        }
    }
}
