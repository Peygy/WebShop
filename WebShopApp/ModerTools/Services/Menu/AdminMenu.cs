using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class AdminMenu // Меню Админа
    {
        
        AdminFrontTools adminAct;

        public AdminMenu()
        {
            adminAct = new AdminFrontTools();
        }

        public void AdmShowcase() //Витрина админа
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать, администратор");
                Console.WriteLine();
                Console.WriteLine("Меню админа:");
                Console.WriteLine("1. Добавить/Удалить/Редактировать категорию");
                Console.WriteLine("2. Добавить/Удалить/Редактировать товар");
                Console.WriteLine("3. Посмотреть/Удалить заказы");
                Console.WriteLine("4. Посмотреть/Удалить пользователей");
                Console.WriteLine("5. Добавить/Удалить модератора");
                Console.WriteLine("6. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            adminAct.CategorySetUp();
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            adminAct.ProductSetUp();
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            adminAct.OrdersSetUp();
                            break;
                        }
                    case "4":
                        {
                            Console.Clear();
                            adminAct.UsersSetUp();
                            break;
                        }
                    case "5":
                        {
                            Console.Clear();
                            adminAct.ModersSetUp();
                            break;
                        }
                    case "6":
                        {
                            Console.Clear();
                            Console.WriteLine("Выход выполнен успешно, До Свидания!");
                            exit = true;
                            break;
                        }
                    case "addfountain":
                        {
                            Console.Clear();
                            adminAct.AddAdmin();
                            break;
                        }
                }
            }
        }
    }
}
