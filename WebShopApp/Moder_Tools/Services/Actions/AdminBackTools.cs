using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    public class AdminBackTools // Класс инструментов(реализации) модерации для админов
                                // Admin moderation tools(realization) class
    {
        public bool AddNewCategory_Back(string categoryName) // Добавление новой категории
                                                             // Adding new category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Categories.Any(u => u.Name == categoryName))
                    {
                        Category category = new Category { Name = categoryName };
                        data.Categories.Add(category);
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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool RenameCategory_Back(string oldName, string newName) // Переименование категории
                                                                        // Renaming category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Categories.Any(u => u.Name == newName))
                    {
                        data.Categories
                            .Include(c => c.Products)
                            .FirstOrDefault(c => c.Name == oldName)
                            .Name = newName;

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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool RemoveCategory_Back(Category category) // Удаление категории
                                                           // Removing category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    category = data.Categories
                            .Include(c => c.Products)
                            .FirstOrDefault(c => c == category);

                    foreach (Product product in category.Products)
                    {
                        product.ProductCategory = null;
                    }

                    data.Categories.Remove(category);
                    data.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }



        public bool AddNewProduct_Back
            (int categoryChoice, string productName, ref string productCategoryName, int productPrice, int key) // Добавление нового товара
                                                                                                                // Adding new product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories
                        .Include(c => c.Products).ToList();

                    if (key == 0)
                    {
                        if (data.Categories.Any(c => c == categories[categoryChoice]))
                        {
                            productCategoryName = data.Categories
                                .FirstOrDefault(c => c == categories[categoryChoice]).Name;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        Category category = data.Categories
                            .Include(c => c.Products)
                            .FirstOrDefault(с => с == categories[categoryChoice]);

                        Product newProduct = new Product { Name = productName, ProductCategory = category, Price = productPrice };

                        data.Warehouse.Add(newProduct);
                        category.Products.Add(newProduct);
                        data.SaveChanges();

                        return true;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool EditProduct_Back(int choice, ref Product product) // Редактирование товара
                                                                      // Editing product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var products = data.Warehouse
                        .Include(p => p.Customers)
                        .Include(p => p.Orders).ToList();

                    if (data.Warehouse.Any(p => p == products[choice]))
                    {
                        product = data.Warehouse
                            .Include(p => p.Customers)
                            .Include(p => p.Orders)
                            .FirstOrDefault(p => p == products[choice]);                        

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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool EditProductRename_Back(string oldName, string newName) // Переименование товара
                                                                           // Renaming product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Warehouse.Any(p => p.Name == newName))
                    {
                        data.Warehouse
                            .FirstOrDefault(p => p.Name == oldName).Name = newName;

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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool EditProductCategory_Back(int newCategId, Product product, ref Category category) // Изменение категории товара
                                                                                                     // Changing product category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories
                        .Include(c => c.Products).ToList();

                    if (data.Categories.Any(c => c == categories[newCategId]))
                    {
                        category = data.Categories
                            .Include(c => c.Products)
                            .FirstOrDefault(c => c == categories[newCategId]);

                        data.Warehouse
                            .Include(p => p.ProductCategory)
                            .FirstOrDefault(p => p == product)
                            .ProductCategory = category;

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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool EditProductPrice_Back(Product product, int productPriceNew) // Изменение цены товара
                                                                                // Changing product price
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    data.Warehouse
                        .FirstOrDefault(p => p == product)
                        .Price = productPriceNew;

                    data.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool RemoveProduct_Back(int productChoice, ref Product product) // Удаление товара
                                                                               // Removing product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var products = data.Warehouse
                        .Include(p => p.ProductCategory).ToList();

                    if (data.Warehouse.Any(p => p == products[productChoice]))
                    {
                        product = products[productChoice];
                        data.Warehouse.Remove(product);
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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }



        public bool OrdersView_Back(int orderNumber, ref Order order) // Просмотр заказа
                                                                      // Viewing order
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var orders = data.Orders
                        .Include(o => o.OrderProducts)
                        .Include(o => o.User).ToList();

                    if(data.Orders.Any(o => o == orders[orderNumber]))
                    {
                        order = orders[orderNumber];

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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool OrderRemove_Back(int orderId, ref Order order) // Удаление заказа
                                                                   // Removing order
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var orders = data.Orders
                        .Include(o => o.OrderProducts)
                        .Include(o => o.User).ToList();                   

                    if (data.Orders.Any(o => o == orders[orderId]))
                    {
                        order = orders[orderId];
                        Order orderForRemove = order;

                        Customer user = data.Users
                            .Include(u => u.Basket)
                            .Include(u => u.Orders)
                            .FirstOrDefault(u => u == orderForRemove.User);

                        user.Orders.Remove(orderForRemove);
                        data.Orders.Remove(orderForRemove);
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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }



        public bool UserRemove_Back(int userId, ref Customer customer) // Удаление пользователя
                                                                       // Removing user
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var customers = data.Users
                        .Include(u => u.Basket)
                        .Include(u => u.Orders).ToList();

                    if (data.Users.Any(u => u == customers[userId]))
                    {
                        customer = data.Users
                            .Include(u => u.Basket)
                            .Include(u => u.Orders)
                            .FirstOrDefault(u => u == customers[userId]);

                        for (int i = 0; i < customer.Orders.Count; i++)
                        {
                            data.Orders.Remove(customer.Orders[i]);
                        }

                        data.Users.Remove(customer);
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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }



        public bool AddModer_Back(string moderLogin, string moderPassword, int key) // Добавление модератора
                                                                                    // Adding moder
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Moders.Any(m => m.Login == moderLogin && m.Password == moderPassword))
                    {
                        if(key == 0) // 'moder' emptyCheck
                        {
                            return true;
                        }
                        else // 'moder' ModerAdding
                        {
                            Moderator moderator = new Moderator { Login = moderLogin, Password = moderPassword, SpecialKey = "01" };
                            data.Moders.Add(moderator);
                            data.SaveChanges();

                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool ModerRemove_Back(int moderId, ref Moderator moder) // Удаление модератора
                                                                       // Removing moder
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var moders = data.Moders
                        .Where(m => m.Login != "Peygy").ToList();

                    if (data.Moders.Any(m => m == moders[moderId]))
                    {
                        moder = moders[moderId];

                        data.Moders.Remove(data.Moders
                            .FirstOrDefault(m => m == moders[moderId]));

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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }



        public bool AddAdmin_Back(string adminLogin, string adminPassword, int key) // Добавление админа
                                                                                    // Adding admin
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Moders.Any(a => a.Login == adminLogin && a.Password == adminPassword))
                    {
                        if (key == 0) // 'admin' emptyCheck
                        {
                            return true;
                        }
                        else // 'admin' AdminAdding
                        {
                            Admin admin = new Admin { Login = adminLogin, Password = adminPassword, SpecialKey = "011" };
                            data.Moders.Add(admin);
                            data.SaveChanges();

                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }
    }
}
