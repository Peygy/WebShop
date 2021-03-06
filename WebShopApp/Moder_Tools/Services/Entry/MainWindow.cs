using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class MainWindow // Начальный экран
                     // Home screen
    {
        UserEntryFront entry;
        Customer user;


        public MainWindow()
        {
            entry = new UserEntryFront();
            AppStart();
        }

        public void AppStart() // Метод запуска приложения
                               // App launch method
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать в наш интернет-магазин!");
                Console.WriteLine();
                Console.WriteLine("Выберите вариант входа: ");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1. У меня есть аккаунт");
                Console.WriteLine("2. Я впервые на этом сайте");
                Console.WriteLine("3. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            entry.SingIn(ref user);

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
                            entry.SingUp(ref user);

                            UserMenu menu = new UserMenu(user);
                            menu.Showcase();

                            exit = true;
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("До Свидания! Приходите ещё!)");

                            exit = true;
                            break;
                        }
                    case "fountain":
                        {
                            Console.Clear();
                            entry.SingInA(ref user);

                            if (user != null && user.SpecialKey != "01" && user.SpecialKey != "00")
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
                            Console.WriteLine();
                            Console.WriteLine("Такого номера нет в меню!");
                            Console.ReadLine();
                            break;
                        }
                }
            }
        }
    }
}
