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
        public Order Order { get; set; }
        public ICollection<Product> Basket { get; set; }

        public Customer()
        {
            Basket = new List<Product>();
        }

        //public int Balance { get; set; }

        public void BasketInfo() // Вывод корзины покупателя / Output user's basket
        {
            var productList = new List<Product>();
            int generalCost = 0;

            foreach(Product product in Basket)
            {
                productList.Add(product);
            }

            for (int i = 0; i < Basket.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Название: {productList[i].Name}; Цена: {productList[i].Price}");
                generalCost = generalCost + productList[i].Price;
            }

            Console.WriteLine();
            Console.WriteLine($"Общая стоимость: {generalCost}");
            Console.WriteLine();
        }
    }
}
