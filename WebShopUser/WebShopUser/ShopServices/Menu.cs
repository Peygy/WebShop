using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopUser
{
    class Menu // Меню
    {
        AuthorizationUser Entry;
        Shopping buying;

        public Menu()
        {
            Entry = new AuthorizationUser();
            Showcase();
        }

        public void Showcase()
        {
            Entry.Authorization();
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Выберите номер пункта из меню:");
                Console.WriteLine("1. Начать покупку");
                Console.WriteLine("2. Посмотреть корзину");
                Console.WriteLine("3. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            buying = new Shopping();
                            buying.CategoriesOutput();
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            
                            break;
                        }
                    case "3":
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
