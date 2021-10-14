using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class UserMenu // Меню для пользователя
    {
        UserShopping buying;
        Customer user;
        UserBasketActions basket;

        public UserMenu(Customer UserInput)
        {
            basket = new UserBasketActions();
            user = UserInput;
        }

        public void Showcase()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Выберите номер пункта из меню:");
                Console.WriteLine("1. Начать покупку");
                Console.WriteLine("2. Посмотреть корзину");
                Console.WriteLine("3. Посмотреть заказы");
                Console.WriteLine("4. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            buying = new UserShopping(user);
                            buying.CategoriesOutput();
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            basket.OrderRegistr(user);
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            basket.OrdersInfo(user);
                            break;
                        }
                    case "4":
                        {
                            Console.Clear();
                            Console.WriteLine("Спасибо за покупки, приходите ещё!");
                            exit = true;
                            break;
                        }
                }
            }
        }
    }
}
