using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class MainWindow
    {
        UserEntry entry;
        Customer user;

        public MainWindow() // Начальный экран
        {           
            entry = new UserEntry();
            AppStart();
        }

        public void AppStart()
        {
            bool exit = false;

            while(!exit)
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать в наш интернет-магазин!");
                Console.WriteLine();
                Console.WriteLine("Выберите вариант входа: ");
                Console.WriteLine("1. У меня есть аккаунт");
                Console.WriteLine("2. Я впервые на этом сайте");
                Console.WriteLine("3. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            user = entry.SingIn();
                            if (user.SpecialKey == "00")
                            {
                                UserMenu menu = new UserMenu(user);
                                menu.Showcase();
                            }
                            else
                            {
                                ModerMenu menu = new ModerMenu();
                                menu.ModerShowcase();
                            }
                            exit = true;
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            user = entry.SingUp();
                            UserMenu menu = new UserMenu(user);
                            menu.Showcase();
                            exit = true;
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            Console.WriteLine("Приходите ещё!");
                            exit = true;
                            break;
                        }
                    //case "whitefountain":
                    case "w":
                        {
                            Console.Clear();
                            user = entry.SingIn();
                            if (user.SpecialKey != "01" && user.SpecialKey != "00")
                            {
                                AdminMenu menu = new AdminMenu();
                                menu.AdmShowcase();
                            }
                            exit = true;
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("Такого номера нет в меню!");
                            Console.ReadLine();
                            break;
                        }
                }
            }
        }
    }
}
