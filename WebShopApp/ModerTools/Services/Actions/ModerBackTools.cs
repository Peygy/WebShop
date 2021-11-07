using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace WebShopApp
{
    class ModerBackTools // Инструменты модерации
    {
        public void ViewAllCategories_Back() // Посмотреть все категории
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                var categories = data.Categories.Include(p => p.Products).ToList();

                foreach (Category category in categories)
                {
                    Console.WriteLine($"{category.Id}. {category.Name}");
                }
            }
        }

        public void ViewAllProducts_Back() // Посмотреть все товары
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                var products = data.Warehouse.Include(p => p.ProductCategory).ToList();

                for (int i = 0; i < products.Count; i++)
                {
                    string category = products[i].ProductCategory == null ? "Пусто" : $"{products[i].ProductCategory.Name}";

                    Console.WriteLine($"{i + 1}. {products[i].Name} => Категория: {category}, Цена: {products[i].Price} рублей");
                }
            }
        }

        public void ViewAllOrders_Back() // Посмотреть все заказы
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                var orders = data.Orders.Include(p => p.User).ToList();

                for (int i = 0; i < orders.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {orders[i].OrderNum} => Пользователь: {orders[i].User.Login}, Статус: {orders[i].Status}");
                }
            }
        }

        public void ViewAllUsers_Back() // Посмотреть всех пользователей
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                var customers = data.Users.Include(p => p.Basket).ToList();

                for (int i = 0; i < customers.Count; i++)
                {
                    var orders = data.Orders.Where(p => p.User == customers[i]).ToList();

                    Console.WriteLine($"{i + 1}. {customers[i].Login} => Кол-во заказов: {orders.Count}, Кол-во товаров в корзине: {customers[i].Basket.Count}");
                }
            }
        }



        public bool EditCategory_Back(int categoryChoice, ref Category category) // Редактировать категорию
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

            return true;
        }

        public bool AddProduct_Back(int choice, ref Product addedProduct, Category category, int key) // Добавить товар
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
                            addedProduct = data.Warehouse.FirstOrDefault(p => p.Id == products[choice].Id);
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

            return true;
        }

        public bool RemoveProduct_Back(int choice, Category category) // Удалить товар
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {                    
                    if(data.Warehouse.Any(p => p.Name == category.Products[choice].Name))
                    {
                        category.Products[choice].ProductCategory = null;
                        category.Products.RemoveAt(choice);
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

            return true;
        }
    }
}
