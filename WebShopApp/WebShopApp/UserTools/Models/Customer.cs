using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class Customer// Покупатель; Login = Name
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int SpecialKey { get; set; }
        //public int Balance { get; set; }
        public List<Order> UserOrder { get; set; }
        [NotMapped]
        public List<Product> Basket { get; set; }

        public Customer()
        {
            UserOrder = new List<Order>();
            Basket = new List<Product>();
        }

        public void BasketInfo() // Вывод корзины
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
