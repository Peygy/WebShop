using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class AdminFrontTools
    {        
        ModerFrontTools moderAct = new ModerFrontTools();
        AdminBackTools adminAct = new AdminBackTools();

        Category category;
        Product product;
        Order order;
        Customer customer;
        Moderator moder;

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

                if (adminAct.AddNewCategory_Back(categoryName))
                {
                    Console.Clear();
                    Console.WriteLine($"Категория '{categoryName}' добавлена");
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

        public void EditCategory() //Редактировать категорию
        {
            bool exit = false;

            while (!exit)
            {
                moderAct.ViewAllCategories();

                Console.WriteLine();
                Console.Write("Категория для редактирования: ");
                string choice = Console.ReadLine();
                Console.Clear();

                int.TryParse(choice, out int categoryChoice);
                categoryChoice -= 1;

                if (choice == "menu")
                {
                    return;
                }

                if (adminAct.EditCategory_Back(categoryChoice, ref category))
                {
                    Console.WriteLine($"Категория '{category.Name}':");
                    Console.WriteLine();

                    for (int i = 0; i < category.Products.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {category.Products[i].Name} =>  Цена: {category.Products[i].Price}");
                    }

                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Такой категории нет");
                    Console.ReadLine();
                    Console.Clear();
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

                if (adminAct.RenameCategory_Back(categoryNameOld, ref categoryNameNew))
                {
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
                adminAct.RemoveCategory_Back(category);

                Console.Clear();
                Console.WriteLine($"Категория '{category.Name}' удалена");
                Console.ReadLine();
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
                int productPrice = 0;
                int categoryChoice = 0;
                string productCategoryName = "";

                Console.Write("Название товара: ");
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
                    Console.Write("Категория товара: ");
                    int.TryParse(Console.ReadLine(), out categoryChoice);
                    categoryChoice -= 1;

                    if (adminAct.AddNewProduct_Back(categoryChoice, productName, ref productCategoryName, productPrice, 0))
                    {                      
                        categoryCheck = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Такой категории нет");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }

                Console.Clear();
                Console.Write("Цена товара: ");
                int.TryParse(Console.ReadLine(), out productPrice);

                Console.Clear();
                Console.WriteLine($"Название товара: {productName}");
                Console.WriteLine($"Категория товара: {productCategoryName}");
                Console.WriteLine($"Цена товара: {productPrice} рублей");
                Console.WriteLine();
                Console.WriteLine("1. Подтвердить");
                Console.WriteLine("2. Составить заново");

                if (Console.ReadLine() == "1")
                {
                    adminAct.AddNewProduct_Back(categoryChoice, productName, ref productCategoryName, productPrice, 1);
                    exit = true;
                }
                else
                {
                    Console.Clear();
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

            while (!exit)
            {
                adminAct.EditProduct_Back(choice, ref product);

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

                if (adminAct.EditProductRename_Back(productNameOld, ref productNameNew))
                {
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
            category = product.ProductCategory;
            string productCategOld = category.Name;            

            while (!exit)
            {
                moderAct.ViewAllCategories();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"Текущая категория товара: {productCategOld}");
                Console.WriteLine();

                Console.Write("Введите новую категорию товара: ");
                string productCategNew = Console.ReadLine();
                int.TryParse(productCategNew, out int newCategId);
                newCategId -= 1;

                if (productCategNew == "menu")
                {
                    return;
                }

                if (adminAct.EditProductCategory_Back(newCategId, productCategOld, ref category))
                {                   
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

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Старая категория товара: {productCategOld}");
            Console.WriteLine();
            Console.WriteLine($"Новая категория товара: {category.Name}");
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
                int.TryParse(priceNew, out productPriceNew);

                if (priceNew == "menu")
                {
                    return;
                }

                if (productPriceNew >= 0)
                {
                    adminAct.EditProductPrice_Back(product, productPriceNew);
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

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Старая цена товара: {productPriceOld}");
            Console.WriteLine();
            Console.WriteLine($"Новая цена товара: {productPriceNew}");
            Console.ReadLine();
        }


        public void RemoveProduct() //Удалить товар
        {
            bool exit = false;

            while (!exit)
            {
                moderAct.ViewAllProducts();

                Console.WriteLine();
                Console.Write("Товар для удаления: ");
                string removeProduct = Console.ReadLine();
                int.TryParse(removeProduct, out int productChoice);
                productChoice -= 1;

                if (removeProduct == "menu")
                {
                    return;
                }

                if (adminAct.RemoveProduct_Back(productChoice, ref product))
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine($"Товар '{product.Name}' успешно удален");
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
            orderNumber -= 1;

            if (orderForView == "menu")
            {
                return;
            }

            if (adminAct.OrdersView_Back(orderNumber, ref order))
            {
                Console.WriteLine($"Номер заказа: {order.OrderNum}");
                Console.WriteLine($"Пользователь: {order.User}");
                Console.WriteLine($"Статус: {order.Status}");
                Console.WriteLine();
                Console.WriteLine("В заказе:");

                for (int i = 0; i < order.OrderProducts.Count; i++)
                {
                    Console.WriteLine($"Название: {order.OrderProducts[i].Name} => Цена: {order.OrderProducts[i].Price} рублей");
                }
                Console.ReadLine();
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

        public void OrderRemove() //Удалить заказ
        {
            bool exit = false;

            while (!exit)
            {
                moderAct.ViewAllOrders();

                Console.WriteLine();
                Console.Write("Введите заказ для удаления: ");
                int.TryParse(Console.ReadLine(), out int orderId);
                orderId -= 1;

                if (adminAct.OrderRemove_Back(orderId, ref order))
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine($"Заказ №{order.OrderNum} успешно удален");
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
                userId -= 1;

                if (adminAct.UserRemove_Back(userId, ref customer))
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine($"Пользователь '{customer.Login}' успешно удален");
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
                string moderPassword = "";

                while (!loginCheck)
                {
                    Console.WriteLine();
                    Console.Write("Логин модератора: ");
                    moderLogin = Console.ReadLine();

                    if (moderLogin == "menu")
                    {
                        return;
                    }

                    if (adminAct.AddModer_Back(moderLogin, moderPassword, 0))
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
                moderPassword = Console.ReadLine();

                Console.Clear();
                Console.WriteLine($"Логин модератора: {moderLogin}");
                Console.WriteLine($"Пароль модератор: {moderPassword}");
                Console.WriteLine();
                Console.WriteLine("1. Подтвердить");
                Console.WriteLine("2. Пересоздать");

                if (Console.ReadLine() == "1")
                {
                    exit = adminAct.AddModer_Back(moderLogin, moderPassword, 1);
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
                moderId -= 1;

                if (adminAct.ModerRemove_Back(moderId, ref moder))
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine($"Модератор '{moder.Login}' успешно удален");
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




        public void AddAdmin() //Добавить админа
        {
            bool exit = false;

            while (!exit)
            {
                bool loginCheck = false;
                string adminLogin = "";
                string adminPassword = "";

                while (!loginCheck)
                {
                    Console.WriteLine();
                    Console.Write("Логин админа: ");
                    adminLogin = Console.ReadLine();

                    if (adminLogin == "menu")
                    {
                        return;
                    }

                    if (adminAct.AddAdmin_Back(adminLogin, adminPassword, 0))
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
                adminPassword = Console.ReadLine();

                Console.Clear();
                Console.WriteLine($"Логин модератора: {adminLogin}");
                Console.WriteLine($"Пароль модератор: {adminPassword}");
                Console.WriteLine();
                Console.WriteLine("1. Подтвердить");
                Console.WriteLine("2. Пересоздать");

                if (Console.ReadLine() == "1")
                {
                    adminAct.AddAdmin_Back(adminLogin, adminPassword, 1);
                    exit = true;
                }
            }
        }

        public void ViewAllModers() // Посмотреть всех модераторов
        {
            Console.WriteLine("Все модераторы:");
            Console.WriteLine();

            adminAct.ViewAllModers_Back();
        }
    }
}
