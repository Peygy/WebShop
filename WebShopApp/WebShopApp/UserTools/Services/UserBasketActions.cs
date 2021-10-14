using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class UserBasketActions // Класс взаимодействий с корзиной
    {
        Random gener;

        public void OrderRegistr(Customer user) // Оформление заказа
        {
            gener = new Random();
            int orderNumber = gener.Next(100000, 1000000);

            Console.WriteLine("Корзина:");
            Console.WriteLine();
            user.BasketInfo();
            Console.WriteLine("1. Оформить заказ");
            Console.WriteLine("2. Выйти в меню");

            if(Console.ReadLine() == "1")
            {
                if(user.Basket.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Ваша корзина пуста!");
                    Console.ReadLine();
                    return;
                }

                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("ВНИМАНИЕ! Оплата онлайн временно не доступна! Автоматически выбрана: Оплата при получении");
                Console.WriteLine();
                Console.ReadLine();
                Console.Clear();

                Console.WriteLine($"Ваш заказ номер: {orderNumber}");
                Console.WriteLine();
                Console.WriteLine("В вашем заказе:");
                user.BasketInfo();
                Console.WriteLine("1. Подтвердить заказ");
                Console.WriteLine("2. Выйти в меню");
                
                if(Console.ReadLine() == "1")
                {
                    using (ShopDataContext data = new ShopDataContext())
                    {
                        Order order = new Order { OrderNum = orderNumber, User = user, Status = "На складе"};
                        user.UserOrder.Add(order);
                        data.Orders.Add(order);
                        data.SaveChanges();
                    }
                    Console.WriteLine("Заказ успешно оформлен, следите за его статусом!");
                    user.Basket.Clear();
                    Console.ReadLine();                   
                }
            }
        }


        public void OrdersInfo(Customer user) // Вывод заказов
        {
            bool accept = false;

            while(!accept)
            {
                string input;
                int OrderInput;

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

                using (ShopDataContext data = new ShopDataContext())
                {
                    var orders = data.Orders.Where(u => u.User == user).ToList();

                    foreach (Order order in orders)
                    {
                        Console.WriteLine($"{order.Id}. {order.OrderNum}");
                    }

                    Console.WriteLine();
                    input = Console.ReadLine();
                    if (input.Contains("menu"))
                    {
                        return;
                    }

                    int.TryParse(input, out OrderInput);
                    if (OrderInput <= orders.Count && OrderInput > 0)
                    {
                        Console.Clear();
                        Console.WriteLine("*Для выхода в меню введите - menu");
                        Console.WriteLine($"Заказ номер: {orders[OrderInput - 1].OrderNum}");
                        Console.WriteLine($"Статус заказа: {orders[OrderInput - 1].Status}");
                        Console.WriteLine();
                        Console.WriteLine("У Вас в заказе:");

                        user.BasketInfo();
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Такого номера категории не существует! Нажмите Enter и введите другой номер!");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }
        }
    }
}
