﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    class UserEntryBack // Авторизации и Регистрация
    {
        Customer user;

        public bool SingIn_Back(string login, string password, ref Customer user) // Вход
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (data.Users.Any(p => p.Login == login && p.Password == password))
                    {
                        int userId = data.Users.FirstOrDefault(p => p.Login == login && p.Password == password).Id;
                        user = new Customer { Id = userId, Login = login, Password = password, SpecialKey = "00" };

                        return true;
                    }
                    else if (data.Moders.Any(p => p.Login == login && p.Password == password))
                    {
                        Console.Write("Ключ идентификации: ");
                        string key = Console.ReadLine();

                        if (key == data.Moders.FirstOrDefault(p => p.Login == login && p.Password == password).SpecialKey)
                        {
                            int userId = data.Moders.FirstOrDefault(p => p.Login == login && p.Password == password && p.SpecialKey == key).Id;
                            user = new Customer { Id = userId, Login = login, Password = password, SpecialKey = key };

                            return true;
                        }
                    }

                    else
                    {
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return true;
        }


        public bool SingUp_Back(string login, string password, ref Customer user,int key) // Регистрация
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if(key == 0)
                    {
                        if (!data.Users.Any(p => p.Login == login))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        user = new Customer { Login = login, Password = password, SpecialKey = "00" };
                        data.Users.Add(user);
                        data.SaveChanges();
                        user = data.Users.FirstOrDefault(p => p.Login == login && p.Password == password && p.SpecialKey == "00");

                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }
    }
}