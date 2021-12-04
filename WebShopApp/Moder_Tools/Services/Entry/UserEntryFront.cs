using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class UserEntryFront // Авторизация и Регистрация / Authorization and Registration
    {
        UserEntryBack entryBack = new UserEntryBack();

        public void SingIn(ref Customer user) // Вход в аккаунт / Login to your account
        {
            bool EndEntry = false;

            while (!EndEntry)
            {
                Console.WriteLine();
                Console.Write("Введите Ваш Логин: ");
                string login = Console.ReadLine();

                Console.WriteLine();
                Console.Write("Введите Ваш Пароль: ");
                string password = Console.ReadLine();


                if (entryBack.SingIn_Back(login, password, ref user))
                {
                    EndEntry = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Неверный Логин или Пароль, или Вы забанены!");
                    Console.WriteLine();
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }


        public void SingUp(ref Customer user) // Регистрация нового аккаунта / Registering a new account
        {
            bool EndRegistration = false;

            while (!EndRegistration)
            {
                string password = null;
                string login = null;
                bool exit = false;

                while (!exit)
                {
                    Console.WriteLine();
                    Console.Write("Введите Логин: ");
                    login = Console.ReadLine();

                    if (entryBack.SingUp_Back(login, password, ref user, 0))
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Такой логин занят!");
                        Console.WriteLine();
                        Console.ReadLine();
                        Console.Clear();
                    }
                }

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
                Console.WriteLine();
                Console.WriteLine("Подтвердите регистрацию");
                Console.WriteLine();
                Console.WriteLine($"Логин: {login}");
                Console.WriteLine($"Пароль: {password}");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1. Подтвердить");
                Console.WriteLine("2. Перезаполнить данные");


                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            entryBack.SingUp_Back(login, password, ref user, 1);

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
        }

        public void SingInA(ref Customer user) // Вход в спец. аккаунт / Login to spec. account
        {
            bool EndEntry = false;

            while (!EndEntry)
            {
                Console.WriteLine();
                Console.Write("Логин: ");
                string login = Console.ReadLine();

                Console.WriteLine();
                Console.Write("Пароль: ");
                string password = Console.ReadLine();


                if (entryBack.SingIn_Back(login, password, ref user))
                {
                    EndEntry = true;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
