using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class UserMenu // Меню Пользователя
                   // User's menu
    {       
        UserFrontTools actions = new UserFrontTools();

        Customer user;


        public UserMenu(Customer UserInput)
        {
            user = UserInput;
        }

        public void Showcase() // Вывод меню пользователя
                               // Output user's menu
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Добро Пожаловать в наш интернет-магазин!");
                Console.WriteLine("Выберите номер пункта из меню:");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1. Начать покупку");
                Console.WriteLine("2. Посмотреть корзину");
                Console.WriteLine("3. Посмотреть заказы");
                Console.WriteLine("4. Удалить аккаунт");
                Console.WriteLine("5. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            actions.CategoriesOutput(user);
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            actions.UserBasket(user);
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            actions.OrdersInfo(user);
                            break;
                        }
                    case "4":
                        {
                            Console.Clear();
                            actions.AccountRemove(user);
                            exit = true;
                            break;
                        }
                    case "5":
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Спасибо за покупки, приходите ещё!");

                            exit = true;
                            break;
                        }
                }
            }
        }
    }
}
