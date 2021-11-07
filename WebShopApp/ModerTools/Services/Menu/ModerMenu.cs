using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class ModerMenu // Меню Модерации
    {
        ModerFrontTools moderAct;

        public ModerMenu()
        {
            moderAct = new ModerFrontTools();
        }

        public void ModerShowcase() //Витрина модератора
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Меню модерации магазина:");
                Console.WriteLine();
                Console.WriteLine("1. Просмотреть/Редактировать категории");
                Console.WriteLine("2. Просмотреть товары");
                Console.WriteLine("3. Посмотреть заказы");
                Console.WriteLine("4. Посмотреть пользователей");
                Console.WriteLine("5. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            moderAct.ViewAllCategories();
                            moderAct.EditCategory();
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            moderAct.ViewAllProducts();
                            Console.ReadLine();
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            moderAct.ViewAllOrders();
                            Console.ReadLine();
                            break;
                        }
                    case "4":
                        {
                            Console.Clear();
                            moderAct.ViewAllUsers();
                            Console.ReadLine();
                            break;
                        }
                    case "5":
                        {
                            Console.Clear();
                            Console.WriteLine("Выход выполнен успешно, До Свидания!");
                            exit = true;
                            break;
                        }
                }
            }
        }
    }
}
