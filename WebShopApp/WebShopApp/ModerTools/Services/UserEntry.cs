using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    class UserEntry // Авторизации и Регистрация
    {
        Customer user;

        public Customer SingIn() // Вход
        {
            bool EndEntry = false;
            
            while (!EndEntry)
            {
                Console.Write("Введите Ваш Логин: ");
                string login = Console.ReadLine();
                Console.Write("Введите Ваш Пароль: ");
                string password = Console.ReadLine();

                using (ShopDataContext data = new ShopDataContext())
                {
                    if(data.Users.Any(p => p.Login == login && p.Password == password && p.SpecialKey == 00))
                    {
                        user = new Customer { Login = login, Password = password };
                        EndEntry = true;
                    }
                    else if(data.Moders.Any(p => p.Login == login && p.Password == password))
                    {
                        user = new Customer { Login = login, Password = password };
                        EndEntry = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Неверный Логин или Пароль!");
                        Console.WriteLine();
                    }
                }
            }
            return user;
        }


        public Customer SingUp() // Регистрация
        {
            bool EndRegistration = false;

            while (!EndRegistration)
            {
                string password = null;
                string login = null;

                Console.Write("Введите Логин: ");
                login = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Введите Пароль (Не менее 8 символов): ");
                password = Console.ReadLine();

                while (password.Length < 8)
                {
                    Console.Clear();
                    Console.WriteLine("Пароль слишком короткий!");
                    Console.WriteLine();

                    Console.Write("Введите Логин: ");
                    Console.WriteLine(login);
                    Console.WriteLine();
                    Console.Write("Введите Пароль (Не менее 8 символов): ");
                    password = Console.ReadLine();
                }

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
                                user = new Customer { Login = login, Password = password, SpecialKey = 00 };
                                data.Users.Add(user);
                                data.SaveChanges();
                            }
                            EndRegistration = true;
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            break;
                        }
                }
            }
            return user;
        }
    }
}
