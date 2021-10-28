﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    class ModerTools // Инструменты модерации
    {              
        public void ViewAllCategories() // Посмотреть все категории
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine("Все категории:");
                Console.WriteLine();
                var categories = data.Categories.Include(p => p.Products).ToList();

                foreach (Category category in categories)
                {
                    Console.WriteLine($"{category.Id}. {category.Name}");
                }               
            }
        }

        public void ViewAllProducts() // Посмотреть все товары
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine("Все товары:");
                Console.WriteLine();
                var products = data.Warehouse.Include(p => p.ProductCategory).ToList();

                for (int i = 0; i < products.Count; i++)
                {
                    string category = products[i].ProductCategory == null ? "Пусто" : $"{products[i].ProductCategory.Name}";

                    Console.WriteLine($"{i + 1}. {products[i].Name} => Категория: {category}, Цена: {products[i].Price} рублей");
                }
            }
        }

        public void ViewAllOrders() // Посмотреть все заказы
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine("Все заказы:");
                Console.WriteLine();
                var orders = data.Orders.Include(p => p.User).ToList();

                for (int i = 0; i < orders.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {orders[i].OrderNum} => Пользователь: {orders[i].User.Login}, Статус: {orders[i].Status}");
                }
            }
        }

        public void ViewAllUsers() // Посмотреть всех пользователей
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine("Все пользователи:");
                Console.WriteLine();
                var customers = data.Users.Include(p => p.Basket ).ToList();                

                for (int i = 0; i < customers.Count; i++)
                {
                    var orders = data.Orders.Where(p => p.User == customers[i]).ToList();

                    Console.WriteLine($"{i + 1}. {customers[i].Login} => Кол-во заказов: {orders.Count}, Кол-во товаров в корзине: {customers[i].Basket.Count}");
                }
            }
        }        



        public void EditCategory() // Редактировать категорию
        {
            Console.WriteLine();
            Console.Write("Выберите категорию: ");
            string choice = Console.ReadLine();

            int.TryParse(choice, out int categoryChoice);

            if (choice == "menu")
            {
                return;
            }

            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (data.Categories.Any(p => p.Id == categoryChoice))
                    {
                        Category category = data.Categories.Include(p => p.Products).FirstOrDefault(c => c.Id == categoryChoice);
                        Console.WriteLine($"Категория '{category.Name}':");
                        Console.WriteLine();

                        for (int i = 0; i < category.Products.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {category.Products[i].Name} =>  Цена: {category.Products[i].Price} рублей");
                        }

                        Console.WriteLine("1. Добавить товар");
                        Console.WriteLine("2. Удалить товар");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                {
                                    Console.Clear();
                                    AddProduct(category);
                                    break;
                                }
                            case "2":
                                {
                                    Console.Clear();
                                    RemoveProduct(category);
                                    break;
                                }
                        }
                    }
                }
            }
            catch(DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }   
        }

        public void AddProduct(Category category) // Добавить товар
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    Console.WriteLine($"Доступные товары для добавления в категорию '{category.Name}' :");
                    Console.WriteLine();
                    var products = data.Warehouse.Where(p => p.ProductCategory == null).ToList();

                    for (int i = 0; i < products.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {products[i].Name} => Цена: {products[i].Price} рублей");
                    }

                    Console.WriteLine();
                    Console.Write("Введите товар для добавления: ");
                    string addChoice = Console.ReadLine();
                    int.TryParse(addChoice, out int choice);

                    if (addChoice == "menu")
                    {
                        return;
                    }

                    if (data.Warehouse.Any(p => p.Name == products[choice - 1].Name))
                    {
                        Console.Clear();
                        Console.WriteLine($"Товар {products[choice - 1].Name} добавлен в '{category.Name}'");
                        Console.ReadLine();

                        Product addedProduct = data.Warehouse.FirstOrDefault(p => p.Id == products[choice - 1].Id);
                        addedProduct.ProductCategory = category;
                        category.Products.Add(addedProduct);
                        data.SaveChanges();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Такого товара нет");
                        Console.ReadLine();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }  
        }

        public void RemoveProduct(Category category) // Удалить товар
        {
            Console.WriteLine($"Категория '{category.Name}':");
            Console.WriteLine();

            for (int i = 0; i < category.Products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {category.Products[i].Name} => Цена: {category.Products[i].Price} рублей");
            }

            Console.WriteLine();
            Console.Write("Введите товар для удаления: ");
            string removeChoice = Console.ReadLine();
            int.TryParse(Console.ReadLine(), out int choice);

            if (removeChoice == "menu")
            {
                return;
            }

            if (category.Products.Contains(category.Products[choice - 1]))
            {
                Console.Clear();
                Console.WriteLine($"Товар {category.Products[choice - 1].Name} удален из '{category.Name}'");
                Console.ReadLine();

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        category.Products[choice - 1].ProductCategory = null;
                        category.Products.RemoveAt(choice - 1);
                        data.SaveChanges();
                    }
                }
                catch(DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }    
            else
            {
                Console.Clear();
                Console.WriteLine("Такого товара нет");
                Console.ReadLine();
            }
        }               
    }
}
