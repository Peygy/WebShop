using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class OutputModelsFront // Класс вывода моделей
                            // Model inference class
    {
        OutputModelsBack output = new OutputModelsBack();

        public void ViewAllCategories() // Посмотреть все категории
                                        // View all categories
        {
            Console.WriteLine();
            Console.WriteLine("Все категории:");
            Console.WriteLine();

            output.ViewAllCategories_Back();

            Console.WriteLine();
            Console.WriteLine();
        }

        public void ViewAllProducts() // Посмотреть все товары
                                      // View all products
        {
            Console.WriteLine();
            Console.WriteLine("Все товары:");
            Console.WriteLine();

            output.ViewAllProducts_Back();

            Console.WriteLine();
            Console.WriteLine();
        }

        public void ViewAllOrders() // Посмотреть все заказы
                                    // View all orders
        {
            Console.WriteLine();
            Console.WriteLine("Все заказы:");
            Console.WriteLine();

            output.ViewAllOrders_Back();

            Console.WriteLine();
            Console.WriteLine();
        }

        public void ViewAllUsers() // Посмотреть всех пользователей
                                   // View all users
        {
            Console.WriteLine();
            Console.WriteLine("Все пользователи:");
            Console.WriteLine();

            output.ViewAllUsers_Back();

            Console.WriteLine();
            Console.WriteLine();
        }

        public void ViewAllModers() // Посмотреть всех модераторов
                                    // View all moderators
        {
            Console.WriteLine();
            Console.WriteLine("Все модераторы:");
            Console.WriteLine();

            output.ViewAllModers_Back();

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
