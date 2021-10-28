using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class AdminTools // Инструменты админа
    {
        ModerTools moderAct = new ModerTools();
        Category category;
        Product product;

        public void CategorySetUp() //Настройка категории
        {
            moderAct.ViewAllCategories();
            Console.WriteLine();
            Console.WriteLine("1. Добавить новую категорию");
            Console.WriteLine("2. Редактировать категорию");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        AddNewCategory();
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        EditCategory();
                        break;
                    }
                case "menu":
                    {
                        return;
                    }
            }
        }

        public void AddNewCategory() //Добавить новую категорию
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine();
                Console.Write("Название категории: ");
                string categoryName = Console.ReadLine();

                if (categoryName == "menu")
                {
                    return;
                }

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        if (!data.Categories.Any(p => p.Name == categoryName))
                        {
                            Category category = new Category { Name = categoryName };
                            data.Categories.Add(category);
                            data.SaveChanges();

                            Console.Clear();
                            Console.WriteLine($"Категория '{category.Name}' добавлена");
                            Console.ReadLine();
                            exit = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Такая категория существует!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch(DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }    
            }
        }

        public void EditCategory() //Редактировать категорию
        {
            moderAct.ViewAllCategories();
            Console.WriteLine();
            Console.Write("Категория для редактирования: ");
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
                        Console.Clear();
                        category = data.Categories.Include(p => p.Products).FirstOrDefault(p => p.Id == categoryChoice);
                        Console.WriteLine($"Категория '{category.Name}':");
                        Console.WriteLine();

                        for (int i = 0; i < category.Products.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {category.Products[i].Name} =>  Цена: {category.Products[i].Price}");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Такой категории нет");
                        Console.ReadLine();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            Console.WriteLine();
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");            
            Console.WriteLine("3. Переименовать категорию");
            Console.WriteLine("4. Удалить категорию");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        moderAct.AddProduct(category);
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        moderAct.RemoveProduct(category);
                        break;
                    }
                case "3":
                    {
                        Console.Clear();
                        RenameCategory(category);
                        break;
                    }
                case "4":
                    {
                        Console.Clear();
                        RemoveCategory(category);
                        break;
                    }
                case "menu":
                    {
                        return;
                    }
            }
        }          

        public void RenameCategory(Category category) //Переименновать категорию
        {
            bool exit = false;
            string categoryNameOld = category.Name;
            string categoryNameNew = "";

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine($"Текущее название категории: {categoryNameOld}");

                Console.Write("Введите новое название категории: ");
                categoryNameNew = Console.ReadLine();

                if (categoryNameNew == "menu")
                {
                    return;
                }

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        if (!data.Categories.Any(p => p.Name == categoryNameNew))
                        {
                            data.Categories.FirstOrDefault(p => p.Name == categoryNameOld).Name = categoryNameNew;
                            data.SaveChanges();
                            exit = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Такая категория существует!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }               
            }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Старое название категории: {categoryNameOld}");
            Console.WriteLine();
            Console.WriteLine($"Новое название категории: {categoryNameNew}");
            Console.ReadLine();
        }

        public void RemoveCategory(Category category)
        {
            Console.WriteLine();
            Console.WriteLine($"Вы точно хотите удалить категорию '{category.Name}'?");
            Console.WriteLine();
            Console.WriteLine("1. Да");
            Console.WriteLine("2. Нет");

            if (Console.ReadLine() == "1")
            {
                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        category.Products.Clear();
                        data.Categories.Remove(category);
                        data.SaveChanges();

                        Console.Clear();
                        Console.WriteLine($"Категория '{category.Name}' удалена");
                        Console.ReadLine();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        


        public void ProductSetUp() // Добавить/Удалить/Редактировать товары
        {
            moderAct.ViewAllProducts();

            Console.WriteLine();
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Редактировать товар");
            Console.WriteLine("3. Удалить товар");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        AddNewProduct();
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        EditProduct();
                        break;
                    }
                case "3":
                    {
                        Console.Clear();
                        RemoveProduct();
                        break;
                    }
                case "menu":
                    {
                        return;
                    }
            }
        } 
        
        public void AddNewProduct() //Добавить новый товар
        {
            bool exit = false;

            while (!exit)
            {
                bool categoryCheck = false;
                int categoryChoice;
                string productCategoryName = "";

                Console.Write("Название товара: ");
                string productName = Console.ReadLine();

                if (productName == "menu")
                {
                    return;
                }

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        while (!categoryCheck)
                        {
                            Console.Clear();
                            moderAct.ViewAllCategories();

                            Console.WriteLine();
                            Console.Write("Категория товара: ");
                            int.TryParse(Console.ReadLine(), out categoryChoice);

                            if (data.Categories.Any(p => p.Id == categoryChoice))
                            {
                                productCategoryName = data.Categories.FirstOrDefault(p => p.Id == categoryChoice).Name;
                                categoryCheck = true;
                            }
                        }

                        Console.Clear();
                        Console.Write("Цена товара: ");
                        int.TryParse(Console.ReadLine(), out int productPrice);

                        Console.Clear();
                        Console.WriteLine($"Название товара: {productName}");
                        Console.WriteLine($"Категория товара: {productCategoryName}");
                        Console.WriteLine($"Цена товара: {productPrice} рублей");
                        Console.WriteLine();
                        Console.WriteLine("1. Подтвердить");
                        Console.WriteLine("2. Составить заново");

                        if (Console.ReadLine() == "1")
                        {
                            category = data.Categories.Include(p => p.Products).FirstOrDefault(с => с.Name == productCategoryName);
                            Product newProduct = new Product { Name = productName, ProductCategory = category, Price = productPrice };
                            data.Warehouse.Add(newProduct);
                            category.Products.Add(newProduct);
                            data.SaveChanges();
                            exit = true;
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }       
            }
        }

        public void EditProduct() //Редактировать товар
        {
            bool exit = false;

            moderAct.ViewAllProducts();
            Console.WriteLine();
            Console.Write("Выберите товар для редактирования: ");
            string editProduct = Console.ReadLine();
            int.TryParse(editProduct, out int choice);

            if (editProduct == "menu")
            {
                return;
            }

            while(!exit)
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


                Console.WriteLine("1. Изменить название товара");
                Console.WriteLine("2. Изменить категорию товара");
                Console.WriteLine("3. Изменить стоимость товара");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            EditProductRename(product);
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            EditProductCategory(product);
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            EditProductPrice(product);
                            break;
                        }
                    case "menu":
                        {
                            exit = true;
                            break;
                        }
                }
            }  
        }

        public void RemoveProduct() //Удалить товар
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    bool exit = false;
                    var products = data.Warehouse.Include(p => p.ProductCategory).ToList();

                    while (!exit)
                    {
                        moderAct.ViewAllProducts();

                        Console.WriteLine();
                        Console.Write("Товар для удаления: ");
                        string removeProduct = Console.ReadLine();
                        int.TryParse(removeProduct, out int productChoice);
                        ;
                        if (removeProduct == "menu")
                        {
                            return;
                        }

                        if (data.Warehouse.Any(p => p.Id == products[productChoice - 1].Id))
                        {
                            Product deletedProduct = products[productChoice - 1];

                            category = deletedProduct.ProductCategory;
                            category.Products.Remove(deletedProduct);
                            data.Warehouse.Remove(deletedProduct);
                            data.SaveChanges();

                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine($"Товар '{deletedProduct.Name}' успешно удален");
                            Console.ReadLine();
                            exit = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Такого товара не существует!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }        
        }

        public void EditProductRename(Product product) //Переименновать товар
        {
            bool exit = false;
            string productNameOld = product.Name;
            string productNameNew = "";

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine($"Текущее название товара: {productNameOld}");
                Console.WriteLine();

                Console.Write("Введите новое название товара: ");
                productNameNew = Console.ReadLine();

                if (productNameNew == "menu")
                {
                    return;
                }

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        if (!data.Warehouse.Any(p => p.Name == productNameNew))
                        {
                            data.Warehouse.FirstOrDefault(p => p.Name == productNameOld).Name = productNameNew;
                            data.SaveChanges();
                            exit = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Такой товар существует!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }                
            }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Старое название товара: {productNameOld}");
            Console.WriteLine();
            Console.WriteLine($"Новое название товара: {productNameNew}");
            Console.ReadLine();
        }

        public void EditProductCategory(Product product) //Поменять категорию товара
        {
            bool exit = false;
            string productCategOld = product.ProductCategory.Name;
            string productCategNew = "";
            string newCategName = "";

            while(!exit)
            {
                moderAct.ViewAllCategories();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"Текущая категория товара: {productCategOld}");
                Console.WriteLine();

                Console.Write("Введите новую категорию товара: ");
                productCategNew = Console.ReadLine();
                int.TryParse(productCategNew, out int newCategId);

                if (productCategNew == "menu")
                {
                    return;
                }

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        if (data.Categories.Any(p => p.Id == newCategId))
                        {
                            category = data.Categories.Include(c => c.Products).FirstOrDefault(p => p.Id == newCategId);
                            data.Warehouse.Include(c => c.ProductCategory).FirstOrDefault(p => p.ProductCategory.Name == productCategOld).ProductCategory = category;
                            data.SaveChanges();
                            newCategName = data.Categories.FirstOrDefault(p => p.Id == newCategId).Name;
                            exit = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Такой категории не существует!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }               
            }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Старая категория товара: {productCategOld}");
            Console.WriteLine();
            Console.WriteLine($"Новая категория товара: {newCategName}");
            Console.ReadLine();
        }

        public void EditProductPrice(Product product) //Поменять цену товара
        {
            bool exit = false;
            int productPriceOld = product.Price;
            int productPriceNew = 0;

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine($"Текущая цена товара: {productPriceOld}");
                Console.WriteLine();

                Console.Write("Введите новую цену товара: ");
                string priceNew = Console.ReadLine();

                if (priceNew == "menu")
                {
                    return;
                }

                int.TryParse(priceNew, out productPriceNew);

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        if (productPriceNew >= 0)
                        {
                            data.Warehouse.FirstOrDefault(p => p.Name == product.Name).Price = productPriceNew;
                            data.SaveChanges();
                            exit = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Цена не может быть отрицательной!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }             
            }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Старая цена товара: {productPriceOld}");
            Console.WriteLine();
            Console.WriteLine($"Новая цена товара: {productPriceNew}");
            Console.ReadLine();
        }




        public void OrdersSetUp() //Настройка заказов
        {
            moderAct.ViewAllOrders();

            Console.WriteLine();
            Console.WriteLine("1. Просмотреть заказ");
            Console.WriteLine("2. Удалить заказ");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        OrdersView();
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        OrderRemove();
                        break;
                    }
                case "menu":
                    {
                        return;
                    }
            }
        }

        public void OrdersView() // Просмотр заказа
        {
            moderAct.ViewAllOrders();            

            Console.WriteLine();
            Console.Write("Введите заказ для просмотра: ");
            string orderForView = Console.ReadLine();
            int.TryParse(orderForView, out int orderNumber);

            if (orderForView == "menu")
            {
                return;
            }

            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var orders = data.Orders.Include(p => p.User).ToList();

                    if (data.Orders.Any(p => p.Id == orders[orderNumber - 1].Id))
                    {
                        Console.WriteLine($"Номер заказа: {orders[orderNumber - 1].OrderNum}");
                        Console.WriteLine($"Пользователь: {orders[orderNumber - 1].User.Login}");
                        Console.WriteLine($"Статус: {orders[orderNumber - 1].Status}");
                        Console.WriteLine();
                        Console.WriteLine("В заказе:");

                        for (int i = 0; i < orders[orderNumber - 1].OrderProducts.Count; i++)
                        {
                            Console.WriteLine($"Название: {orders[orderNumber - 1].OrderProducts[i].Name} => Цена: {orders[orderNumber - 1].OrderProducts[i].Price} рублей");
                        }
                        Console.ReadLine();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        public void OrderRemove() //Удалить заказ
        {
            bool exit = false;

            while(!exit)
            {
                moderAct.ViewAllOrders();

                Console.WriteLine();
                Console.Write("Введите заказ для удаления: ");
                int.TryParse(Console.ReadLine(), out int orderId);

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        var orders = data.Orders.Include(p => p.User).ToList();

                        if (data.Orders.Any(p => p.Id == orders[orderId - 1].Id))
                        {
                            data.Orders.Remove(data.Orders.FirstOrDefault(p => p.Id == orders[orderId - 1].Id));
                            data.SaveChanges();

                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine($"Заказ №{orders[orderId - 1].OrderNum} успешно удален");
                            Console.ReadLine();
                            exit = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Такого заказа не существует!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }             
            }            
        }




        public void UsersSetUp() //Настройка пользователей
        {
            moderAct.ViewAllOrders();

            Console.WriteLine();
            Console.WriteLine("1. Удалить пользователя");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        UserRemove();
                        break;
                    }
                case "menu":
                    {
                        return;
                    }
            }
        }

        public void UserRemove() //Удалить пользователя
        {
            bool exit = false;

            while (!exit)
            {
                moderAct.ViewAllUsers();

                Console.WriteLine();
                Console.Write("Введите пользователя для удаления: ");
                int.TryParse(Console.ReadLine(), out int userId);

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        var customers = data.Users.Include(p => p.Basket).ToList();

                        if (data.Users.Any(p => p.Id == customers[userId - 1].Id))
                        {
                            data.Users.Remove(data.Users.FirstOrDefault(p => p.Id == customers[userId - 1].Id));
                            data.SaveChanges();

                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine($"Пользователь '{customers[userId - 1].Login}' успешно удален");
                            Console.ReadLine();
                            exit = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Такого пользователя не существует!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }      
            }
        }


        

        public void ModersSetUp() //Настройка модераторов
        {
            ViewAllModers();

            Console.WriteLine();
            Console.WriteLine("1. Добавить модератора");
            Console.WriteLine("2. Удалить модератора");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        AddModer();
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        ModerRemove();
                        break;
                    }
                case "menu":
                    {
                        return;
                    }
            }
        }

        public void AddModer() //Добавить модератора
        {
            bool exit = false;

            while (!exit)
            {
                bool loginCheck = false;
                string moderLogin = "";

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        while (!loginCheck)
                        {
                            Console.WriteLine();
                            Console.Write("Логин модератора: ");
                            moderLogin = Console.ReadLine();

                            if (moderLogin == "menu")
                            {
                                return;
                            }

                            if (!data.Moders.Any(p => p.Login == moderLogin))
                            {
                                loginCheck = true;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine();
                                Console.WriteLine("Такой модератор существует!");
                                Console.ReadLine();
                                Console.Clear();
                            }
                        }

                        Console.WriteLine();
                        Console.Write("Пароль модератора: ");
                        string moderPassword = Console.ReadLine();

                        Console.Clear();
                        Console.WriteLine($"Логин модератора: {moderLogin}");
                        Console.WriteLine($"Пароль модератор: {moderPassword}");
                        Console.WriteLine();
                        Console.WriteLine("1. Подтвердить");
                        Console.WriteLine("2. Пересоздать");

                        if (Console.ReadLine() == "1")
                        {
                            Moderator moderator = new Moderator { Login = moderLogin, Password = moderPassword, SpecialKey = "01" };
                            data.Moders.Add(moderator);
                            data.SaveChanges();
                            exit = true;
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }        
            }
        }

        public void ModerRemove() //Удалить модератора
        {
            bool exit = false;

            while (!exit)
            {
                ViewAllModers();

                Console.WriteLine();
                Console.Write("Введите модератора для удаления: ");
                int.TryParse(Console.ReadLine(), out int moderId);

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        var moders = data.Moders.Where(p => p.Login != "Peygy").ToList();

                        if (data.Moders.Any(p => p.Id == moders[moderId - 1].Id))
                        {
                            data.Moders.Remove(data.Moders.FirstOrDefault(p => p.Id == moders[moderId - 1].Id));
                            data.SaveChanges();

                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine($"Модератор '{moders[moderId - 1].Login}' успешно удален");
                            Console.ReadLine();
                            exit = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Такого модератора не существует!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }




        public void AddAdmin() //Добавить админа
        {
            bool exit = false;

            while (!exit)
            {
                bool loginCheck = false;
                string adminLogin = "";

                try
                {
                    using (AdminDataContext data = new AdminDataContext())
                    {
                        while (!loginCheck)
                        {
                            Console.WriteLine();
                            Console.Write("Логин админа: ");
                            adminLogin = Console.ReadLine();

                            if (adminLogin == "menu")
                            {
                                return;
                            }

                            if (!data.Moders.Any(p => p.Login == adminLogin))
                            {
                                loginCheck = true;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine();
                                Console.WriteLine("Такой админ существует!");
                                Console.ReadLine();
                                Console.Clear();
                            }
                        }

                        Console.WriteLine();
                        Console.Write("Пароль админа: ");
                        string adminPassword = Console.ReadLine();

                        Console.Clear();
                        Console.WriteLine($"Логин модератора: {adminLogin}");
                        Console.WriteLine($"Пароль модератор: {adminPassword}");
                        Console.WriteLine();
                        Console.WriteLine("1. Подтвердить");
                        Console.WriteLine("2. Пересоздать");

                        if (Console.ReadLine() == "1")
                        {
                            Admin moderator = new Admin { Login = adminLogin, Password = adminPassword, SpecialKey = "011" };
                            data.Moders.Add(moderator);
                            data.SaveChanges();
                            exit = true;
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }            
            }
        }

        public void ViewAllModers() // Посмотреть всех модераторов
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                Console.WriteLine("Все модераторы:");
                Console.WriteLine();

                foreach (Moderator moderator in data.Moders.Where(p => p.Login != "Peygy"))
                {
                    Console.WriteLine($"{moderator.Id}. {moderator.Login} => Пароль: {moderator.Password}, Спец. ключ: {moderator.SpecialKey}");
                }
            }
        }
    }
}
