using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    public class UserEntryBack // Авторизация и Регистрация
                               // Authorization and Registration
    {
        public bool SingIn_Back(string login, string password, ref Customer user) // Вход в аккаунт
                                                                                  // Login to your account
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if (data.Users.Any(u => u.Login == login && u.Password == password))
                    {
                        user = data.Users
                            .Include(u => u.Basket)
                            .Include(u => u.Orders)
                            .FirstOrDefault(u => u.Login == login && u.Password == password);

                        return true;
                    }

                    if (data.Moders.Any(m => m.Login == login && m.Password == password))
                    {
                        #if RELEASE
                            Console.WriteLine();
                            Console.Write("Ключ идентификации: ");
                            string key = Console.ReadLine();
                        #endif

                        #if DEBUG
                            string key = "01";
                        #endif


                        if (key == data.Moders.FirstOrDefault(m => m.Login == login && m.Password == password).SpecialKey)
                        {
                            int userId = data.Moders
                                .FirstOrDefault(m => m.Login == login && m.Password == password && m.SpecialKey == key).Id;

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
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }


        public bool SingUp_Back(string login, string password, ref Customer user, int key) // Регистрация нового аккаунта
                                                                                           // Registering a new account
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    if(key == 0)
                    {
                        if (!data.Users.Any(u => u.Login == login))
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

                        user = data.Users
                            .Include(u => u.Basket)
                            .Include(u => u.Orders)
                            .FirstOrDefault(u => u.Login == login && u.Password == password && u.SpecialKey == "00");

                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }
    }
}
