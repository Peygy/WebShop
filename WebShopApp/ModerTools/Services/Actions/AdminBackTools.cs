using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class AdminBackTools // Инструменты админа
    {
        ModerBackTools moderAct = new ModerBackTools();
      
        public bool AddNewCategory_Back(string categoryName) //Добавить новую категорию
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Categories.Any(p => p.Name == categoryName))
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
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        public bool EditCategory_Back(int categoryChoice, ref Category category) //Редактировать категорию
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories.Include(p => p.Products).ToList();

                    if (data.Categories.Any(p => p.Id == categories[categoryChoice].Id))
                    {
                        category = data.Categories.Include(p => p.Products).FirstOrDefault(p => p.Id == categories[categoryChoice].Id);                                            

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

        public bool RenameCategory_Back(string oldName, ref string newName) //Переименновать категорию
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    string categoryNameOld = oldName;
                    string categoryNameNew = newName;

                    if (!data.Categories.Any(p => p.Name == categoryNameNew))
                    {
                        data.Categories.FirstOrDefault(p => p.Name == categoryNameOld).Name = categoryNameNew;
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

        public bool RemoveCategory_Back(Category category)
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    for (int i = 0; i < category.Products.Count(); i++)
                    {
                        category.Products[i].ProductCategory = null;
                    }

                    category.Products.Clear();
                    data.Categories.Remove(category);
                    data.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }




        public bool AddNewProduct_Back(int categoryChoice, string productName, ref string productCategoryName, int productPrice, int key) //Добавить новый товар
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories.Include(p => p.Products).ToList();
                    if (key == 0)
                    {
                        if (data.Categories.Any(p => p.Id == categories[categoryChoice].Id))
                        {
                            productCategoryName = data.Categories.FirstOrDefault(p => p.Id == categories[categoryChoice].Id).Name;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        Category category = data.Categories.Include(p => p.Products).FirstOrDefault(с => с.Name == data.Categories.FirstOrDefault(p => p.Id == categories[categoryChoice].Id).Name);
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
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        public void EditProduct_Back(int choice, ref Product product) //Редактировать товар
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    Console.Clear();
                    product = data.Warehouse.Include(p => p.ProductCategory).FirstOrDefault(p => p.Id == choice);
                    product.ProductInfo();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool EditProductRename_Back(string oldName, ref string newName) //Переименновать товар
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
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        public bool EditProductCategory_Back(int newCategId, string productCategOld, ref Category category) //Поменять категорию товара
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories.Include(p => p.Products).ToList();

                    if (data.Categories.Any(p => p.Id == categories[newCategId].Id))
                    {
                        category = data.Categories.Include(c => c.Products).FirstOrDefault(p => p.Id == categories[newCategId].Id);
                        data.Warehouse.Include(c => c.ProductCategory).FirstOrDefault(p => p.ProductCategory.Name == productCategOld).ProductCategory = category;
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

        public void EditProductPrice_Back(Product product, int productPriceNew) //Поменять цену товара
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    data.Warehouse.FirstOrDefault(p => p.Name == product.Name).Price = productPriceNew;
                    data.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public bool RemoveProduct_Back(int productChoice, ref Product deletedProduct) //Удалить товар
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
                Console.WriteLine(ex.Message);
            }

            return true;
        }



        public bool OrdersView_Back(int orderNumber, ref Order order) // Просмотр заказа
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var orders = data.Orders.Include(p => p.User).ToList();
                    order = orders[orderNumber];

                    return data.Orders.Any(p => p.Id == orders[orderNumber].Id);
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        public bool OrderRemove_Back(int orderId, ref Order order) //Удалить заказ
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var orders = data.Orders.Include(p => p.User).ToList();

                    if (data.Orders.Any(p => p.Id == orders[orderId].Id))
                    {
                        order = orders[orderId];
                        data.Orders.Remove(data.Orders.FirstOrDefault(p => p.Id == orders[orderId].Id));
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



        public bool UserRemove_Back(int userId, ref Customer customer) //Удалить пользователя
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var customers = data.Users.Include(p => p.Basket).ToList();

                    if (data.Users.Any(p => p.Id == customers[userId].Id))
                    {
                        var userOrders = data.Users.Include(p => p.Order).Where(u => u.Order.User == customers[userId]).ToList();
                        customer = data.Users.Include(p => p.Order).FirstOrDefault(u => u.Id == customers[userId].Id);

                        for (int i = 0; i < userOrders.Count; i++)
                        {
                            userOrders[i].Order.OrderProducts.Clear();
                            data.Orders.Remove(userOrders[i].Order);
                        }

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
                Console.WriteLine(ex.Message);
            }

            return true;
        }



        public bool AddModer_Back(string moderLogin, string moderPassword, int key) //Добавить модератора
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Moders.Any(p => p.Login == moderLogin))
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
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        public bool ModerRemove_Back(int moderId, ref Moderator moder) //Удалить модератора
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var moders = data.Moders.Where(p => p.Login != "Peygy").ToList();

                    if (data.Moders.Any(p => p.Id == moders[moderId].Id))
                    {
                        moder = moders[moderId];
                        data.Moders.Remove(data.Moders.FirstOrDefault(p => p.Id == moders[moderId].Id));
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



        public bool AddAdmin_Back(string adminLogin, string adminPassword, int key) //Добавить админа
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Moders.Any(p => p.Login == adminLogin))
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
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        public void ViewAllModers_Back() // Посмотреть всех модераторов
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                var moderators = data.Moders.Where(p => p.Login != "Peygy").ToList();

                for (int i = 0; i < moderators.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {moderators[i].Login} => Пароль: {moderators[i].Password}, Спец. ключ: {moderators[i].SpecialKey}");
                }
            }
        }
    }
}
