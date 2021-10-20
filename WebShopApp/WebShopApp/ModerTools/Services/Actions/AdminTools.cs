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
            using (AdminDataContext data = new AdminDataContext())
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
                            AddCategory();
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
        }

        public void EditCategory() //Редактировать категорию
        {
            using (AdminDataContext data = new AdminDataContext())
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
                if (data.Categories.Any(p => p.Id == categoryChoice))
                {
                    Console.Clear();
                    category = data.Categories.FirstOrDefault(p => p.Id == categoryChoice);
                    Console.WriteLine($"Категория '{category.Name}':");
                    Console.WriteLine();

                    foreach (Product product in category.Products)
                    {
                        Console.WriteLine($"{product.Id}. {product.Name} =>  Стоимость: {product.Price}");
                    }
                }
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

        public void AddCategory() //Добавить категорию
        {
            bool exit = false;

            while(!exit)
            {
                Console.WriteLine();
                Console.Write("Название категории: ");
                string categoryName = Console.ReadLine();

                if(categoryName == "menu")
                {
                    return;
                }

                using (AdminDataContext data = new AdminDataContext())
                {
                    if (!data.Categories.Any(p => p.Name == categoryName))
                    {
                        Category category = new Category { Name = categoryName };
                        data.Categories.Add(category);
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
        }      

        public void RenameCategory(Category category) //Переименновать категорию
        {
            bool exit = false;
            string categoryNameOld = "";
            string categoryNameNew = "";

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine($"Текущее название категории: {category.Name}");
                categoryNameOld = category.Name;

                Console.Write("Введите новое название категории: ");
                categoryNameNew = Console.ReadLine();

                if (categoryNameNew == "menu")
                {
                    return;
                }

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
                using (AdminDataContext data = new AdminDataContext())
                {
                    category.Products.Clear();
                    data.Categories.Remove(category);
                    data.SaveChanges();
                }
            }
        }

        


        public void ProductSetUp() // Добавить/Удалить/Редактировать продукты
        {
            moderAct.ViewAllProducts();

            Console.WriteLine();
            Console.WriteLine("1. Добавить");
            Console.WriteLine("2. Редактировать");
            Console.WriteLine("3. Удалить");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        AddProduct();
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
        
        public void AddProduct() //Добавить продукт
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                bool exit = false;

                while (!exit)
                {
                    bool categoryCheck = false;
                    int categoryChoice;
                    string productCategoryName = "";

                    Console.Write("Название продукта: ");
                    string productName = Console.ReadLine();

                    if (productName == "menu")
                    {
                        return;
                    }

                    while (!categoryCheck)
                    {
                        Console.Clear();
                        moderAct.ViewAllCategories();

                        Console.WriteLine();
                        Console.Write("Категория продукта: ");
                        int.TryParse(Console.ReadLine(), out categoryChoice);

                        if (data.Categories.Any(p => p.Id == categoryChoice))
                        {
                            productCategoryName = data.Categories.FirstOrDefault(p => p.Id == categoryChoice).Name;
                            categoryCheck = true;
                        }
                    }

                    Console.Clear();
                    Console.Write("Стоимость продукта: ");
                    int.TryParse(Console.ReadLine(), out int productPrice);

                    Console.Clear();
                    Console.WriteLine($"Название продукта: {productName}");
                    Console.WriteLine($"Категория продукта: {productCategoryName}");
                    Console.WriteLine($"Стоимость продукта: {productPrice} рублей");
                    Console.WriteLine();
                    Console.WriteLine("1. Подтвердить");
                    Console.WriteLine("2. Составить заново");

                    if (Console.ReadLine() == "1")
                    {
                        category = data.Categories.FirstOrDefault(p => p.Name == productCategoryName);
                        Product newProduct = new Product { Name = productName, ProductCategory = category, Price = productPrice };
                        data.Warehouse.Add(newProduct);
                        category.Products.Add(newProduct);
                        data.SaveChanges();
                        exit = true;
                    }
                }
            }
        }

        public void EditProduct() //Редактировать продукт
        {
            moderAct.ViewAllProducts();
            Console.WriteLine();
            Console.Write("Выберите продукт для редактирования: ");
            string editProduct = Console.ReadLine();
            int.TryParse(editProduct, out int choice);

            if (editProduct == "menu")
            {
                return;
            }

            using (AdminDataContext data = new AdminDataContext())
            {
                Console.Clear();
                product = data.Warehouse.FirstOrDefault(p => p.Id == choice);
                product.ProductInfo();
            }

            Console.WriteLine("1. Изменить название");
            Console.WriteLine("2. Изменить категорию");
            Console.WriteLine("3. Изменить стоимость");

            switch(Console.ReadLine())
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
                        return;
                    }
            }
        }

        public void RemoveProduct() //Удалить продукт
        {
            using (AdminDataContext data = new AdminDataContext())
            {
                bool exit = false;

                while (!exit)
                {
                    moderAct.ViewAllProducts(); 

                    Console.WriteLine();
                    Console.Write("Продукт для удаления: ");
                    string removeProduct = Console.ReadLine();
                    int.TryParse(removeProduct, out int productChoice);

                    if (removeProduct == "menu")
                    {
                        return;
                    }

                    if (data.Warehouse.Any(p => p.Id == productChoice))
                    {
                        Product deletedProduct = new Product();
                        deletedProduct = data.Warehouse.FirstOrDefault(p => p.Id == productChoice);
                        deletedProduct.ProductCategory.Products.Remove(deletedProduct);
                        data.Warehouse.Remove(deletedProduct);
                        data.SaveChanges();

                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Товар успешно удален");
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

        public void EditProductRename(Product product) //Переименновать продукт
        {
            bool exit = false;
            string productNameOld = "";
            string productNameNew = "";

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine($"Текущее название продукта: {product.Name}");
                productNameOld = product.Name;

                Console.Write("Введите новое название продукта: ");
                productNameNew = Console.ReadLine();

                if (productNameNew == "menu")
                {
                    return;
                }

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
                        Console.WriteLine("Такой продукт существует!");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }
        }

        public void EditProductCategory(Product product) //Поменять категорию продукта
        {
            bool exit = false;
            string productCategOld = "";
            string productCategNew = "";

            while(!exit)
            {
                Console.WriteLine();
                Console.WriteLine($"Текущая категория продукта: {product.ProductCategory.Name}");
                productCategOld = product.ProductCategory.Name;

                Console.Write("Введите новую категорию продукта: ");
                productCategNew = Console.ReadLine();

                if (productCategNew == "menu")
                {
                    return;
                }                

                using (AdminDataContext data = new AdminDataContext())
                {
                    if (data.Categories.Any(p => p.Name == productCategNew))
                    {
                        category = data.Categories.FirstOrDefault(p => p.Name == productCategNew);
                        data.Warehouse.FirstOrDefault(p => p.ProductCategory.Name == productCategOld).ProductCategory = category;
                        data.SaveChanges();
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
        }

        public void EditProductPrice(Product product) //Поменять стоимость продукта
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine($"Текущая стоимость продукта: {product.Price}");
                int productPriceOld = product.Price;

                Console.Write("Введите новую стоимость продукта: ");
                string price = Console.ReadLine();

                if (price == "menu")
                {
                    return;
                }

                int.TryParse(price, out int productPriceNew);

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
                        Console.WriteLine("Стоимость не может быть отрицательной!");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }       
        }




        public void OrdersSetUp() //Настройка заказов
        {
            moderAct.ViewAllOrders();

            Console.WriteLine();
            Console.WriteLine("1. Удалить заказ");

            switch (Console.ReadLine())
            {
                case "1":
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

        public void OrderRemove() //Удалить заказ
        {
            bool exit = false;

            while(!exit)
            {
                moderAct.ViewAllOrders();

                Console.WriteLine();
                Console.Write("Введите заказ для удаления: ");
                int.TryParse(Console.ReadLine(), out int orderId);

                using (AdminDataContext data = new AdminDataContext())
                {
                    if (data.Orders.Any(p => p.Id == orderId))
                    {
                        data.Orders.Remove(data.Orders.FirstOrDefault(p => p.Id == orderId));
                        data.SaveChanges();

                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Товар успешно удален");
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

                using (AdminDataContext data = new AdminDataContext())
                {
                    if (data.Users.Any(p => p.Id == userId))
                    {
                        data.Users.Remove(data.Users.FirstOrDefault(p => p.Id == userId));
                        data.SaveChanges();

                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Пользователь успешно удален");
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

                using (AdminDataContext data = new AdminDataContext())
                {
                    if (data.Moders.Any(p => p.Id == moderId))
                    {
                        data.Moders.Remove(data.Moders.FirstOrDefault(p => p.Id == moderId));
                        data.SaveChanges();

                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Модератор успешно удален");
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
        }




        public void AddAdmin() //Добавить админа
        {
            bool exit = false;

            while (!exit)
            {
                bool loginCheck = false;
                string adminLogin = "";

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
