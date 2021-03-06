using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class UserFrontTools // Класс взаимодействия пользователя с магазином
                         // User interaction class with the store
    {       
        UserBackTools userAct = new UserBackTools();
        OutputModelsFront output = new OutputModelsFront();

        Random gener;
        Category category;
        Product product;
        Order order;


        public void CategoriesOutput(Customer user) // Категории для покупки товаров
                                                    // Categories for buying products 
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("*Для выхода в меню введите - menu");

                output.ViewAllCategories();

                Console.Write("Выберите нужную вам категорию: ");
                string categoryChoice = Console.ReadLine();
                int.TryParse(categoryChoice, out int categoryInput);
                categoryInput -= 1;                

                if (categoryChoice.Contains("menu"))
                {
                    return;
                }

                if (userAct.CategoriesOutput_Back(categoryInput, ref category))
                {
                    Console.Clear();
                    ProductOutput(category, user);

                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Такого номера категории не существует! " +
                        "Нажмите Enter и введите другой номер!");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        public void ProductOutput(Category category, Customer user) // Товары для покупки и добваления в корзину
                                                                    // Products to buy and adding to the basket
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("*Для выхода в меню введите - menu");
                Console.WriteLine();
                Console.WriteLine($"Все продукты категории '{category.Name}':");
                Console.WriteLine();

                for (int i = 0; i < category.Products.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {category.Products[i].Name} => " +
                        $"Цена: {category.Products[i].Price} Руб.");
                }


                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Выберите нужный вам товар: ");
                string productChoice = Console.ReadLine();
                int.TryParse(productChoice, out int productNum);
                productNum -= 1;

                if (productChoice.Contains("menu"))
                {
                    return;
                }

                if (category.Products.Count > productNum)
                {
                    Console.Clear();
                    category.Products[productNum].ProductInfo();

                    Console.WriteLine("1. Добавить в корзину");
                    Console.WriteLine("2. Вернуться в меню");
                    Console.WriteLine();

                    if (Console.ReadLine() == "1")
                    {
                        Product product = category.Products[productNum];
                        userAct.AddToBasket_Back(product, user);

                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine($"Товар '{category.Products[productNum].Name}' " +
                            $"успешно добавлен в корзину! Нажмите Enter");
                        Console.ReadLine();
                    }

                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Такого номера товара не существует! " +
                        "Нажмите Enter и введите другой номер!");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }



        public void UserBasket(Customer user) // Действия с корзиной
                                              // Basket actions
        {
            gener = new Random();
            bool exit = false;
            int orderNumber = 0;
            int generalCost = 0;

            if (userAct.BasketEmptyCheck_Back(user))
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Ваша корзина пуста!");
                Console.ReadLine();

                return;
            }
           
            while (!exit)
            {
                orderNumber = gener.Next(100000, 1000000);

                if (userAct.OrderExist_Back(orderNumber))
                {
                    exit = true;
                }
            }

            Console.WriteLine("Корзина:");
            Console.WriteLine();
            userAct.BasketInfo_Back(ref user);

            for (int i = 0; i < user.Basket.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Название: {user.Basket[i].Name}; " +
                    $"Цена: {user.Basket[i].Price} Руб.");

                generalCost = generalCost + user.Basket[i].Price;
            }

            Console.WriteLine();
            Console.WriteLine($"Итого: {user.Basket.Count} " +
                $"товар(а) на {generalCost} Руб.");


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("1. Оформить заказ");
            Console.WriteLine("2. Удалить товары");
            Console.WriteLine("3. Выйти в меню");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        RegistrationOrder(user, orderNumber);
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        ProductRemoveFromBasket(user);
                        break;
                    }
                case "3":
                    {
                        return;
                    }
            }
        }


        public void RegistrationOrder(Customer user, int orderNumber) // Оформить заказ
                                                                      // Checkout
        {
            int generalCost = 0;

            if (userAct.BasketEmptyCheck_Back(user))
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Ваша корзина пуста!");
                Console.ReadLine();

                return;
            }

            Console.WriteLine("ВНИМАНИЕ! Оплата онлайн временно не доступна! " +
                "Автоматически выбрана: Оплата при получении");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine($"Ваш заказ номер: {orderNumber}");
            Console.WriteLine();
            Console.WriteLine("В вашем заказе:");
            userAct.BasketInfo_Back(ref user);

            for (int i = 0; i < user.Basket.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Название: {user.Basket[i].Name}; " +
                    $"Цена: {user.Basket[i].Price} Руб.");

                generalCost = generalCost + user.Basket[i].Price;
            }

            Console.WriteLine();
            Console.WriteLine($"Общая стоимость: {generalCost} Руб.");
            Console.WriteLine();


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("1. Подтвердить заказ");
            Console.WriteLine("2. Выйти в меню");

            if (Console.ReadLine() == "1")
            {
                userAct.RegistrationOrder_Back(user, orderNumber);

                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Заказ успешно оформлен, следите за его статусом!");
                Console.ReadLine();
            }
        }

        public void ProductRemoveFromBasket(Customer user) // Удалить товар из корзины
                                                           // Remove product from basket
        {
            int generalCost = 0;

            if (userAct.BasketEmptyCheck_Back(user))
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Ваша корзина пуста!");
                Console.ReadLine();

                return;
            }


            Console.WriteLine("Корзина:");
            Console.WriteLine();
            userAct.BasketInfo_Back(ref user);

            for (int i = 0; i < user.Basket.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Название: {user.Basket[i].Name}; " +
                    $"Цена: {user.Basket[i].Price} ₽");

                generalCost = generalCost + user.Basket[i].Price;
            }

            Console.WriteLine();
            Console.WriteLine($"Общая стоимость: {generalCost}");
            Console.WriteLine();


            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Какой хотите удалить товар: ");
            int.TryParse(Console.ReadLine(), out int prodNum);
            userAct.ProductRemoveFromBasket_Back(user, ref product, prodNum);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Товар {product.Name} удален из корзины!");
            Console.ReadLine();
        }

        public void OrdersInfo(Customer user) // Заказы и удаление заказа
                                              // Orders and order remove
        {
            var products = new List<Product>();
            int OrderInput = 0;
            bool exit = false;

            while (!exit)
            {
                if (userAct.OrdersInfo_Back(ref user, OrderInput, ref order, 0))
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("У Вас нет заказов!");
                    Console.ReadLine();

                    return;
                }

                Console.WriteLine("*Для выхода в меню введите - menu");
                Console.WriteLine();
                Console.WriteLine("Ваши заказы: ");
                Console.WriteLine();

                for (int i = 0; i < user.Orders.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {user.Orders[i].OrderNum}");
                }

                Console.WriteLine();
                string input = Console.ReadLine();
                int.TryParse(input, out OrderInput);
                OrderInput -= 1;

                if (input.Contains("menu"))
                {
                    return;
                }

                if (userAct.OrdersInfo_Back(ref user, OrderInput, ref order, 1))
                {
                    Console.Clear();
                    Console.WriteLine("*Для выхода в меню заказов введите - menu");
                    Console.WriteLine();
                    Console.WriteLine($"Заказ номер: {order.OrderNum}");
                    Console.WriteLine($"Статус заказа: {order.Status}");
                    Console.WriteLine();
                    Console.WriteLine("У Вас в заказе:");

                    for (int i = 0; i < products.Count; i++)
                    {
                        Console.WriteLine($"Название: {order.OrderProducts[i].Name} => " +
                            $"Цена: {order.OrderProducts[i].Price} Руб.");
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("1. Удалить заказ");

                    if (Console.ReadLine() == "1")
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Вы точно хотите удалить заказ?");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("1. Да");
                        Console.WriteLine("2. Нет");
                        Console.WriteLine();

                        if (Console.ReadLine() == "1")
                        {
                            userAct.OrdersInfo_Back(ref user, OrderInput, ref order, 2);
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Такого номера заказа не существует! " +
                        "Нажмите Enter и введите другой!");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }


        public void AccountRemove(Customer user) // Удалить аккаунт
                                                 // Remove account
        {
            Console.WriteLine();
            Console.WriteLine("Вы точно хотите удалить Ваш аккаунт?");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("1. Да, точно");
            Console.WriteLine("2. Нет, вернуться в меню");

            if (Console.ReadLine() == "1")
            {
                if(userAct.AccountRemove_Back(user))
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Ваш аккаунт удален!");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("WARNING! Ваш аккаунт НЕ удален!");
                    Console.ReadLine();
                }
            }
        }
    }
}
