using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class UserTools // Класс взаимодействий пользователя
    {
        Random gener;

        public void UserBasket(Customer user) // Взаимодействие с корзиной
        {
            gener = new Random();
            int orderNumber = 0;

            if (user.Basket.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Ваша корзина пуста!");
                Console.ReadLine();
                return;
            }

            using (UserDataContext data = new UserDataContext())
            {
                bool exit = false;
                while(!exit)
                {
                    orderNumber = gener.Next(100000, 1000000);
                    if (!data.Orders.Any(p => p.OrderNum == orderNumber))
                    {
                        exit = true;
                    }
                }
            }            

            Console.WriteLine("Корзина:");
            Console.WriteLine();
            user.BasketInfo();
            Console.WriteLine("1. Оформить заказ");
            Console.WriteLine("2. Удалить товары");
            Console.WriteLine("3. Выйти в меню");

            switch(Console.ReadLine())
            {
                case "1":
                    {
                        RegistrationOrder(user, orderNumber);
                        break;
                    }
                case "2":
                    {
                        ProductRemoveFromBasket(user);
                        break;
                    }
                case "3":
                    {
                        return;
                    }
            }
        }


        public void RegistrationOrder(Customer user, int orderNumber) // Оформление заказа
        {
            if (user.Basket.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Ваша корзина пуста!");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("ВНИМАНИЕ! Оплата онлайн временно не доступна! Автоматически выбрана: Оплата при получении");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine($"Ваш заказ номер: {orderNumber}");
            Console.WriteLine();
            Console.WriteLine("В вашем заказе:");
            user.BasketInfo();
            Console.WriteLine("1. Подтвердить заказ");
            Console.WriteLine("2. Выйти в меню");

            if (Console.ReadLine() == "1")
            {
                using (UserDataContext data = new UserDataContext())
                {
                    Order order = new Order { OrderNum = orderNumber, User = user, Status = "На складе" };
                    user.UserOrder.Add(order);
                    data.Orders.Add(order);
                    data.SaveChanges();
                }
                Console.WriteLine("Заказ успешно оформлен, следите за его статусом!");
                user.Basket.Clear();
                Console.ReadLine();
            }
        }

        public void ProductRemoveFromBasket(Customer user) // Удалить товар
        {
            if (user.Basket.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Ваша корзина пуста!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Корзина:");
            Console.WriteLine();
            user.BasketInfo();
            Console.Write("Какой хотите удалить товар: ");
            int.TryParse(Console.ReadLine(), out int prodNum);
            user.Basket.RemoveAt(prodNum - 1);
            using (UserDataContext data = new UserDataContext())
            {
                user.Basket.RemoveAt(prodNum - 1);
                data.SaveChanges();
            }

            Console.Clear();
            Console.WriteLine("Товар удален из корзины!");
            Console.ReadLine();
        }

        public void OrdersInfo(Customer user) // Вывод заказов и удаление заказа
        {
            bool accept = false;

            while(!accept)
            {
                if(user.UserOrder.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("У Вас нет заказов!");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("*Для выхода в меню введите - menu");
                Console.WriteLine("Ваши заказы: ");
                Console.WriteLine();

                using (UserDataContext data = new UserDataContext())
                {
                    var orders = data.Orders.Where(u => u.User == user).ToList();

                    foreach (Order order in orders)
                    {
                        Console.WriteLine($"{order.Id}. {order.OrderNum}");
                    }

                    Console.WriteLine();
                    string input = Console.ReadLine();
                    if (input.Contains("menu"))
                    {
                        return;
                    }

                    int.TryParse(input, out int OrderInput);
                    if (OrderInput <= orders.Count && OrderInput > 0)
                    {
                        Console.Clear();
                        Console.WriteLine("*Для выхода в меню заказов введите - menu");
                        Console.WriteLine($"Заказ номер: {orders[OrderInput - 1].OrderNum}");
                        Console.WriteLine($"Статус заказа: {orders[OrderInput - 1].Status}");
                        Console.WriteLine();
                        Console.WriteLine("У Вас в заказе:");
                        user.BasketInfo();
                        Console.WriteLine("1. Удалить заказ");

                        if (Console.ReadLine() == "1")
                        {
                            Console.Clear();
                            Console.WriteLine("Вы точно хотите удалить заказ?");
                            Console.WriteLine("1. Да");
                            Console.WriteLine("2. Нет");
                            Console.WriteLine();

                            if (Console.ReadLine() == "1")
                            {
                                data.Orders.Remove(orders[OrderInput - 1]);
                                user.UserOrder.RemoveAt(OrderInput - 1);
                                data.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Такого номера заказа не существует! Нажмите Enter и введите другой!");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }
        }


        public bool AccountRemove(Customer user) // Удалить аккаунт
        {
            Console.WriteLine("Вы точно хотите удалить Ваш аккаунт?");
            Console.WriteLine("1. Да, точно");
            Console.WriteLine("2. Нет, вернуться в меню");

            if (Console.ReadLine() == "1")
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    user.Basket.Clear();
                    user.UserOrder.Clear();
                    data.Users.Remove(user);
                    data.SaveChanges();
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
