using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    public class AdminBackTools // Класс инструментов(реализации) модерации для админов / Admin moderation tools(realization) class
    {
        public bool AddNewCategory_Back(string categoryName) // Добавление новой категории / Adding new category
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

        public bool RenameCategory_Back(string oldName, string newName) // Переименование категории / Renaming category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    string categoryNameOld = oldName;
                    string categoryNameNew = newName;

                    if (!data.Categories.Any(u => u.Name == categoryNameNew))
                    {
                        data.Categories.Include(c => c.Products).FirstOrDefault(c => c.Name == categoryNameOld).Name = categoryNameNew;
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

        public bool RemoveCategory_Back(Category category) // Удаление категории / Removing category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    foreach (Product product in category.Products)
                    {
                        product.ProductCategory = null;
                    }

                    category.Products.Clear();
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



        public bool AddNewProduct_Back(int categoryChoice, string productName, ref string productCategoryName, int productPrice, int key) // Добавление нового товара / Adding new product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories.Include(c => c.Products).ToList();

                    if (key == 0)
                    {
                        if (data.Categories.Any(c => c.Id == categories[categoryChoice].Id))
                        {
                            productCategoryName = data.Categories.FirstOrDefault(c => c.Id == categories[categoryChoice].Id).Name;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        Category category = data.Categories.Include(c => c.Products).FirstOrDefault(с => с.Name == data.Categories.FirstOrDefault(c => c.Id == categories[categoryChoice].Id).Name);
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

        public bool EditProduct_Back(int choice, ref Product product) // Редактирование товара / Editing product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {                   
                    if (data.Warehouse.Any(p => p.Id == choice))
                    {
                        product = data.Warehouse.Include(p => p.ProductCategory).FirstOrDefault(p => p.Id == choice);                        

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

        public bool EditProductRename_Back(string oldName, string newName) // Переименование товара / Renaming product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    string productNameOld = oldName;
                    string productNameNew = newName;

                    if (!data.Warehouse.Any(p => p.Name == productNameNew))
                    {
                        data.Warehouse.FirstOrDefault(p => p.Name == productNameOld).Name = productNameNew;
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

        public bool EditProductCategory_Back(int newCategId, string productCategOld, ref Category category) // Изменение категории товара / Changing product category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories.Include(c => c.Products).ToList();

                    if (data.Categories.Any(c => c.Id == categories[newCategId].Id))
                    {
                        category = data.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == categories[newCategId].Id);
                        data.Warehouse.Include(p => p.ProductCategory).FirstOrDefault(p => p.ProductCategory.Name == productCategOld).ProductCategory = category;
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

        public bool EditProductPrice_Back(Product product, int productPriceNew) // Изменение цены товара / Changing product price
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    data.Warehouse.FirstOrDefault(p => p.Name == product.Name).Price = productPriceNew;
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


        public bool RemoveProduct_Back(int productChoice, ref Product deletedProduct) // Удаление товара / Removing product
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var products = data.Warehouse.Include(p => p.ProductCategory).ToList();

                    if (data.Warehouse.Any(p => p.Id == products[productChoice].Id))
                    {
                        deletedProduct = products[productChoice];
                        Category category = deletedProduct.ProductCategory;

                        category.Products.Remove(deletedProduct);
                        data.Warehouse.Remove(deletedProduct);
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



        public bool OrdersView_Back(int orderNumber, ref Order order) // Просмотр заказа / Viewing order
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var orders = data.Orders.Include(o => o.OrderProducts).Include(o => o.User).ToList();

                    if(data.Orders.Any(o => o.Id == orders[orderNumber].Id))
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

        public bool OrderRemove_Back(int orderId, ref Order order) // Удаление заказа / Removing order
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var orders = data.Orders.Include(o => o.OrderProducts).Include(o => o.User).ToList();

                    if (data.Orders.Any(o => o.Id == orders[orderId].Id))
                    {
                        order = orders[orderId];
                        order.User.Orders.Remove(order);
                        data.Orders.Remove(order);
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



        public bool UserRemove_Back(int userId, ref Customer customer) // Удаление пользователя / Removing user
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var customers = data.Users.Include(u => u.Basket).Include(u => u.Orders).ToList();

                    if (data.Users.Any(u => u.Id == customers[userId].Id))
                    {
                        customer = data.Users.Include(u => u.Basket).Include(u => u.Orders).FirstOrDefault(u => u.Id == customers[userId].Id);

                        for (int i = 0; i < customer.Orders.Count; i++)
                        {
                            data.Orders.Remove(customer.Orders[i]);
                        }

                        customer.Orders.Clear();
                        customer.Basket.Clear();
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



        public bool AddModer_Back(string moderLogin, string moderPassword, int key) // Добавление модератора / Adding moder
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Moders.Any(m => m.Login == moderLogin))
                    {
                        if(key == 0)
                        {
                            return true;
                        }
                        else
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

        public bool ModerRemove_Back(int moderId, ref Moderator moder) // Удаление модератора / Removing moder
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var moders = data.Moders.Where(m => m.Login != "Peygy").ToList();

                    if (data.Moders.Any(m => m.Id == moders[moderId].Id))
                    {
                        moder = moders[moderId];
                        data.Moders.Remove(data.Moders.FirstOrDefault(m => m.Id == moders[moderId].Id));
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



        public bool AddAdmin_Back(string adminLogin, string adminPassword, int key) // Добавление админа / Adding admin
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Moders.Any(m => m.Login == adminLogin))
                    {
                        if (key == 0)
                        {
                            return true;
                        }
                        else
                        {
                            Admin moderator = new Admin { Login = adminLogin, Password = adminPassword, SpecialKey = "011" };
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

        public bool ViewAllModers_Back() // Посмотреть всех модераторов / View all moderators
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var moderators = data.Moders.Where(m => m.Login != "Peygy").ToList();

                    for (int i = 0; i < moderators.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {moderators[i].Login} => Пароль: {moderators[i].Password}, Спец. ключ: {moderators[i].SpecialKey}");
                    }

                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }
    }
}
