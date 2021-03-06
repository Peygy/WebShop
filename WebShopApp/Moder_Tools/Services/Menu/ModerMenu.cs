using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class ModerMenu // Меню Модерации
                    // Moderation Menu
    {
        ModerFrontTools moderAct = new ModerFrontTools();
        OutputModelsFront output = new OutputModelsFront();


        public void ModerShowcase() // Вывод меню модератора
                                    // Output moderation menu
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Меню модерации магазина:");
                Console.WriteLine();
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
                            moderAct.EditCategory_Menu();
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            output.ViewAllProducts();
                            Console.ReadLine();
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            output.ViewAllOrders();
                            Console.ReadLine();
                            break;
                        }
                    case "4":
                        {
                            Console.Clear();
                            output.ViewAllUsers();
                            Console.ReadLine();
                            break;
                        }
                    case "5":
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Выход выполнен успешно, До Свидания!");

                            exit = true;
                            break;
                        }
                }
            }
        }
    }
}
