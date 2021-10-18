using System;
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
            Console.WriteLine("Все категории:");
            Console.WriteLine();

            using (AdminDataContext data = new AdminDataContext())
            {
                foreach (Product product in data.Warehouse)
                {
                    Console.WriteLine($"{product.Id}. {product.Name} => Категория: {product.ProductCategory.Name}");
                }               
            }
        }

        public void ViewAllProducts() // Посмотреть продукты
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine("Все продукты:");
                Console.WriteLine();

                foreach (Product product in data.Warehouse)
                {
                    Console.WriteLine($"{product.Id}. {product.Name} => Категория: {product.ProductCategory.Name}, Стоимость: {product.Price}");
                }
            }
        }

        public void ViewAllOrders() // Посмотреть все заказы
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine("Все заказы:");
                Console.WriteLine();

                foreach (Order order in data.Orders)
                {
                    Console.WriteLine($"{order.Id}. {order.OrderNum} => Пользователь: {order.User.Login}, Статус: {order.Status}");
                }
            }
        }

        public void ViewAllUsers() // Посмотреть всех пользователей
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine("Все пользователи:");
                Console.WriteLine();

                foreach (Customer customer in data.Users)
                {
                    Console.WriteLine($"{customer.Id}. {customer.Login} => Кол-во заказов: {customer.UserOrder.Count}, Кол-во товаров в корзине: {customer.Basket.Count}");
                }
            }
        }        



        public void EditCategory() // Редактировать категорию
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine();
                Console.Write("Выберите категорию: ");
                string choice = Console.ReadLine();

                int.TryParse(choice, out int categoryChoice);

                if (choice == "menu")
                {
                    return;
                }
                if (data.Categories.Any(p => p.Id == categoryChoice))
                {
                    Category category = data.Categories.FirstOrDefault(p => p.Id == categoryChoice);
                    Console.WriteLine($"Категория '{category.Name}':");
                    Console.WriteLine();

                    foreach (Product product in category.Products)
                    {
                        Console.WriteLine($"{product.Id}. {product.Name} =>  Стоимость: {product.Price}");
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

        public void AddProduct(Category category) // Добавить продукт
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine($"Доступные товары для добавления в категорию '{category.Name}' :");
                Console.WriteLine();

                var products = data.Warehouse.Where(p => p.ProductCategory == null);
                foreach (Product product in products)
                {
                    Console.WriteLine($"{product.Id}. {product.Name} => Стоимость: {product.Price}");
                }

                Console.WriteLine();
                Console.Write("Введите товар для добавления: ");
                int.TryParse(Console.ReadLine(), out int choice);

                if (data.Categories.Any(p => p.Id == choice))
                {
                    Product addedProduct = data.Warehouse.FirstOrDefault(p => p.Id == choice);
                    addedProduct.ProductCategory = category;
                    category.Products.Add(addedProduct);
                    data.SaveChanges();
                }
            }
        }

        public void RemoveProduct(Category category) // Удалить продукт
        {
            Console.WriteLine($"Категория '{category.Name}':");
            Console.WriteLine();

            foreach (Product product in category.Products)
            {
                Console.WriteLine($"{product.Id}. {product.Name} =>  Стоимость: {product.Price}");
            }

            Console.WriteLine();
            Console.Write("Введите товар для удаления: ");
            int.TryParse(Console.ReadLine(), out int choice);

            if (category.Products.Any(p => p.Id == choice - 1))
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    category.Products[choice - 1].ProductCategory = null;
                    category.Products.RemoveAt(choice - 1);
                    data.SaveChanges();
                }
            }    
        }               
    }
}
