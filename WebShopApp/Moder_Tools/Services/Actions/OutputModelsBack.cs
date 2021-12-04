using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    class OutputModelsBack
    {
        public bool ViewAllCategories_Back() // Вывод всех категорий / Output all categories
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories
                        .Include(c => c.Products).ToList();

                    foreach (Category category in categories)
                    {
                        Console.WriteLine($"{category.Id}. {category.Name}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool ViewAllProducts_Back() // Вывод всех продутков / Output all products
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var products = data.Warehouse
                        .Include(p => p.ProductCategory).ToList();

                    for (int i = 0; i < products.Count; i++)
                    {
                        string category = products[i].ProductCategory == 
                            null ? "Пусто" : $"{products[i].ProductCategory.Name}";

                        Console.WriteLine($"{i + 1}. {products[i].Name} => " +
                            $"Категория: {category}, " +
                            $"Цена: {products[i].Price} ₽");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool ViewAllOrders_Back() // Вывод всех заказов / Output all orders
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var orders = data.Orders
                        .Include(o => o.User).ToList();

                    for (int i = 0; i < orders.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {orders[i].OrderNum} => " +
                            $"Пользователь: {orders[i].User.Login}, " +
                            $"Статус: {orders[i].Status}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool ViewAllUsers_Back() // Вывод всех пользователей / Output all users
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var customers = data.Users
                        .Include(u => u.Basket)
                        .Include(u => u.Orders).ToList();

                    for (int i = 0; i < customers.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {customers[i].Login} => " +
                            $"Кол-во заказов: {customers[i].Orders.Count}, " +
                            $"Кол-во товаров в корзине: {customers[i].Basket.Count}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool ViewAllModers_Back() // Посмотреть всех модераторов / View all moderators
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var moderators = data.Moders
                        .Where(m => m.Login != "Peygy").ToList();

                    for (int i = 0; i < moderators.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {moderators[i].Login} => " +
                            $"Пароль: {moderators[i].Password}, " +
                            $"Спец. ключ: {moderators[i].SpecialKey}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }
    }
}

