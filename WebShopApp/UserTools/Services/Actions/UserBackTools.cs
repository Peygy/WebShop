using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class UserBackTools // Класс взаимодействий пользователя
    {
        Random gener;
        ModerFrontTools moderAct = new ModerFrontTools();


        public void CategoriesOutput(Customer user) // Вывод категорий для покупки!!!
        {
            bool accept = false;

            while (!accept)
            {
                Console.WriteLine("*Для выхода в меню введите - menu");
                Console.WriteLine();
                moderAct.ViewAllCategories();
                Console.WriteLine();
                Console.Write("Выберите нужную вам категорию: ");
                string categoryChoice = Console.ReadLine();
                int.TryParse(categoryChoice, out int categoryInput);
                categoryInput -= 1;

                using (UserDataContext data = new UserDataContext())
                {
                    var categories = data.Categories.ToList();

                    if (categoryChoice.Contains("menu"))
                    {
                        return;
                    }

                    if (categories.Any(p => p.Id == categories[categoryInput].Id))
                    {
                        Console.Clear();
                        ProductOutput(categories[categoryInput].Id, user);
                        accept = true;
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

        public void ProductOutput(int categoryId, Customer user) // Вывод товаров для покупки!!!
        {
            bool accept = false;

            while (!accept)
            {
                Console.WriteLine("*Для выхода в меню введите - menu");
                Console.WriteLine();

                try
                {
                    using (UserDataContext data = new UserDataContext())
                    {
                        var products = data.Warehouse.Include(p => p.ProductCategory).Where(p => p.ProductCategory.Id == categoryId).ToList();

                        Console.WriteLine($"Все продукты категории '{data.Categories.FirstOrDefault(p => p.Id == categoryId).Name}':");
                        Console.WriteLine();
                        for (int i = 0; i < products.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {products[i].Name} => Цена: {products[i].Price} рублей");
                        }

                        Console.WriteLine();
                        Console.Write("Выберите нужный вам товар: ");
                        string productChoice = Console.ReadLine();
                        int.TryParse(productChoice, out int productNum);
                        productNum -= 1;

                        if (productChoice.Contains("menu"))
                        {
                            return;
                        }

                        if (products.Count > productNum)
                        {
                            Console.Clear();
                            products[productNum].ProductInfo();
                            Console.WriteLine("1. Добавить в корзину");
                            Console.WriteLine("2. Вернуться в меню");
                            Console.WriteLine();

                            if (Console.ReadLine() == "1")
                            {
                                user.Basket.Add(products[productNum]);
                                data.SaveChanges();
                                Console.Clear();
                                Console.WriteLine($"Товар '{products[productNum].Name}' успешно добавлен в корзину! Нажмите Enter");
                                Console.ReadLine();
                            }
                            accept = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Такого номера товара не существует! Нажмите Enter и введите другой номер!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }



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
                while (!exit)
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

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        RegistrationOrder(user, orderNumber);
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
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

            Console.WriteLine();
            Console.WriteLine("ВНИМАНИЕ! Оплата онлайн временно не доступна! Автоматически выбрана: Оплата при получении");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine($"Ваш заказ номер: {orderNumber}");
            Console.WriteLine();
            Console.WriteLine("В вашем заказе:");
            user.BasketInfo();
            Console.WriteLine();
            Console.WriteLine("1. Подтвердить заказ");
            Console.WriteLine("2. Выйти в меню");

            if (Console.ReadLine() == "1")
            {
                try
                {
                    using (UserDataContext data = new UserDataContext())
                    {
                        Order order = new Order { OrderNum = orderNumber, User = user, Status = "На складе" };
                        data.Orders.Add(order);
                        user.Basket.Clear();
                        data.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.Clear();
                Console.WriteLine("Заказ успешно оформлен, следите за его статусом!");
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
            Console.WriteLine();
            Console.Write("Какой хотите удалить товар: ");
            int.TryParse(Console.ReadLine(), out int prodNum);
            prodNum -= 1;

            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    user.Basket.RemoveAt(prodNum);
                    data.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Clear();
            Console.WriteLine("Товар удален из корзины!");
            Console.ReadLine();
        }

        public void OrdersInfo(Customer user) // Вывод заказов и удаление заказа
        {
            bool accept = false;

            while (!accept)
            {
                try
                {
                    using (UserDataContext data = new UserDataContext())
                    {
                        var userWithOrders = data.Users.Include(p => p.Order).Where(u => u.Order.User == user).ToList();

                        if (userWithOrders.Count == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("У Вас нет заказов!");
                            Console.ReadLine();
                            return;
                        }

                        Console.WriteLine("*Для выхода в меню введите - menu");
                        Console.WriteLine("Ваши заказы: ");
                        Console.WriteLine();

                        for (int i = 0; i < userWithOrders.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {userWithOrders[i].Order.OrderNum}");
                        }

                        Console.WriteLine();
                        string input = Console.ReadLine();
                        int.TryParse(input, out int OrderInput);
                        OrderInput -= 1;

                        if (input.Contains("menu"))
                        {
                            return;
                        }

                        if (OrderInput <= userWithOrders.Count && OrderInput > 0)
                        {
                            Console.Clear();
                            Console.WriteLine("*Для выхода в меню заказов введите - menu");
                            Console.WriteLine($"Заказ номер: {userWithOrders[OrderInput].Order.OrderNum}");
                            Console.WriteLine($"Статус заказа: {userWithOrders[OrderInput].Order.Status}");
                            Console.WriteLine();
                            Console.WriteLine("У Вас в заказе:");

                            for (int i = 0; i < userWithOrders[OrderInput].Order.OrderProducts.Count; i++)
                            {
                                Console.WriteLine($"Название: {userWithOrders[OrderInput].Order.OrderProducts[i].Name} => Цена: {userWithOrders[OrderInput].Order.OrderProducts[i].Price} рублей");
                            }

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
                                    userWithOrders[OrderInput].Order.OrderProducts.Clear();
                                    data.Orders.Remove(userWithOrders[OrderInput].Order);
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
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        public void AccountRemove(Customer user) // Удалить аккаунт
        {
            Console.WriteLine("Вы точно хотите удалить Ваш аккаунт?");
            Console.WriteLine("1. Да, точно");
            Console.WriteLine("2. Нет, вернуться в меню");

            if (Console.ReadLine() == "1")
            {
                try
                {
                    using (UserDataContext data = new UserDataContext())
                    {
                        var userOrders = data.Users.Include(p => p.Order).Where(u => u.Order.User == user).ToList();
                        var userForRemove = data.Users.Include(p => p.Order).FirstOrDefault(u => u.Id == user.Id);

                        for (int i = 0; i < userOrders.Count; i++)
                        {
                            userOrders[i].Order.OrderProducts.Clear();
                            data.Orders.Remove(userOrders[i].Order);
                        }

                        userForRemove.Basket.Clear();
                        data.Users.Remove(userForRemove);
                        data.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.Clear();
                Console.WriteLine("Ваш аккаунт удален!");
                Console.ReadLine();
            }
        }
    }
}
