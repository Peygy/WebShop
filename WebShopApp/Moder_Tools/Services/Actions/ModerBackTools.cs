using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace WebShopApp
{
    class ModerBackTools // Класс инструментов(реализации) модерации / Moderation tool(realization) class
    {
        public bool ViewAllCategories_Back() // Вывод всех категорий / Output all categories
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories.Include(p => p.Products).ToList();

                    foreach (Category category in categories)
                    {
                        Console.WriteLine($"{category.Id}. {category.Name}");
                    }

                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool ViewAllProducts_Back() // Вывод всех продутков / Output all products
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var products = data.Warehouse.Include(p => p.ProductCategory).ToList();

                    for (int i = 0; i < products.Count; i++)
                    {
                        string category = products[i].ProductCategory == null ? "Пусто" : $"{products[i].ProductCategory.Name}";

                        Console.WriteLine($"{i + 1}. {products[i].Name} => Категория: {category}, Цена: {products[i].Price} рублей");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool ViewAllOrders_Back() // Вывод всех заказов / Output all orders
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var orders = data.Orders.Include(p => p.User).ToList();

                    for (int i = 0; i < orders.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {orders[i].OrderNum} => Пользователь: {orders[i].User.Login}, Статус: {orders[i].Status}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool ViewAllUsers_Back() // Вывод всех пользователей / Output all users
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var customers = data.Users.Include(p => p.Basket).ToList();

                    for (int i = 0; i < customers.Count; i++)
                    {
                        var orders = data.Orders.Where(p => p.User == customers[i]).ToList();

                        Console.WriteLine($"{i + 1}. {customers[i].Login} => Кол-во заказов: {orders.Count}, Кол-во товаров в корзине: {customers[i].Basket.Count}");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            

            return false;
        }



        public bool EditCategory_Back(int categoryChoice, ref Category category) // Редактирование категории / Editing category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories.Include(p => p.Products).ToList();

                    if (data.Categories.Any(p => p.Id == categories[categoryChoice].Id))
                    {
                        category = data.Categories.Include(p => p.Products).FirstOrDefault(c => c.Id == categories[categoryChoice].Id);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool AddProduct_Back(int choice, ref Product addedProduct, Category category, int key) // Добавление товара / Adding product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var products = data.Warehouse.Where(p => p.ProductCategory == null).ToList();

                    if(key == 0)
                    {
                        for (int i = 0; i < products.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {products[i].Name} => Цена: {products[i].Price} рублей");
                        }

                        return true;
                    }
                    else
                    {
                        if (data.Warehouse.Any(p => p.Name == products[choice].Name))
                        {
                            addedProduct = data.Warehouse.Include(u => u.ProductCategory).FirstOrDefault(p => p.Id == products[choice].Id);
                            addedProduct.ProductCategory = category;
                            category.Products.Add(addedProduct);
                            data.SaveChanges();

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }                    
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool RemoveProduct_Back(int choice, Category category) // Удаление товара / Removing product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var products = new List<Product>();

                    foreach(Product product in category.Products)
                    {
                        products.Add(product);
                    }

                    if (data.Warehouse.Any(p => p.Name == products[choice].Name))
                    {
                        Product product = data.Warehouse.Include(u => u.ProductCategory).FirstOrDefault(p => p.Id == choice);
                        products[choice].ProductCategory = null;
                        category.Products.Remove(product);
                        data.SaveChanges();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
