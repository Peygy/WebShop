using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class MainWindow
    {
        UserEntry entry;
        Customer user;

        public MainWindow()
        {           
            entry = new UserEntry();
            AppStart();
        }

        public void AppStart()
        {
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
                        if(user.SpecialKey == 00)
                        {
                            UserMenu menu = new UserMenu(user);
                            menu.Showcase();
                        }
                        else
                        {
                            Moderator moderator = new Moderator { Login = user.Login, Password = user.Password, SpecialKey = user.SpecialKey};
                            ModerMenu menu = new ModerMenu(moderator);
                        }
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        user = entry.SingUp();
                        UserMenu menu = new UserMenu(user);
                        menu.Showcase();
                        break;
                    }
                case "3":
                    {
                        Console.Clear();
                        Console.WriteLine("Приходите ещё!");
                        break;
                    }
            }
        }
    }
}
