using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebShopUser
{
    class AuthorizationUser // Авторизации и Регистрация
    {
        Customer user;

        public void Authorization()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("*Для пользования программой надо пройти регистрацию или авторизоваться!");
                Console.WriteLine();
                Console.WriteLine("1. Вход в профиль");
                Console.WriteLine("2. Регистрация");
                Console.WriteLine("3. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            SingIn();
                            exit = true;
                            break;
                        }
                    case "2":
                        {
                            SingUp();
                            exit = true;
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            Console.WriteLine("Спасибо за покупки, приходите ещё!");
                            Environment.Exit(0);
                            break;
                        }
                }
            }
        }


        public void SingIn() // Вход
        {
            bool EndEntry = false;

            while(!EndEntry)
            {
                Console.Clear();
                Console.Write("Введите Ваш Логин: ");
                string login = Console.ReadLine();
                Console.Write("Введите Ваш Пароль: ");
                string password = Console.ReadLine();
            }
        }


        public void SingUp() // Регистрация
        {
            bool EndRegistration = false;

            while (!EndRegistration)
            {
                Console.Clear();
                Console.Write("Введите Логин: ");
                string login = Console.ReadLine();
                Console.Write("Введите Пароль: ");
                string password = Console.ReadLine();

                Console.Clear();
                Console.WriteLine("Подтвердите регистрацию");
                Console.WriteLine();
                Console.WriteLine($"Логин: {login}");
                Console.WriteLine($"Пароль: {password}");
                Console.WriteLine();
                Console.WriteLine("1. Подтвердить");
                Console.WriteLine("2. Перезаполнить данные");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            using (ShopDataContext data = new ShopDataContext())
                            {
                                user = new Customer { Login = login, Password = password };
                                data.Users.Add(user);
                                data.SaveChanges();
                            }
                            EndRegistration = true;
                            break;
                        }
                    case "2":
                        {
                            break;
                        }
                }
            }
        }
    }
}
