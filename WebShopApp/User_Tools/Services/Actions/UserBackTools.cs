using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class UserBackTools // Класс инструментов для взаимодействия пользователя с магазином / Class of tools for user interaction with the store
    {
        public bool CategoriesOutput_Back(int categoryInput, ref Category category) // Вывод категорий для покупки товаров / Output categories for buying products
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    var categories = data.Categories.Include(c => c.Products).ToList();

                    if (categories.Any(c => c.Id == categories[categoryInput].Id))
                    {
                        category = categories.FirstOrDefault(c => c.Id == categories[categoryInput].Id);

                        return true;
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

        public bool AddToBasket_Back(Product product, Customer user) // Добавление товаров в корзину / Adding products to basket
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    user.Basket.Add(product);
                    data.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }



        public bool UserBasket_Back(int orderNumber) // Действия с корзиной / Basket actions
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    if (!data.Orders.Any(o => o.OrderNum == orderNumber))
                    {
                        return true;
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


        public bool RegistrationOrder_Back(Customer user, int orderNumber) // Оформление заказа / Checkout
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    Order order = new Order { OrderNum = orderNumber, User = user, OrderProducts = user.Basket, Status = "На складе" };
                    data.Orders.Add(order);
                    user.Orders.Add(order);
                    user.Basket.Clear();
                    data.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool ProductRemoveFromBasket_Back(Customer user, int prodNum) // Удаление товара из / Product removing
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    Product product = data.Warehouse.FirstOrDefault(p => p.Id == prodNum);
                    user.Basket.Remove(product);
                    data.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }

        public bool OrdersInfo_Back(Customer user, int OrderInput, ref Order order, int key) // Вывод заказов и удаление заказа / Output orders and removing order
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    if(key == 0)
                    {
                        if (user.Orders.Count == 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (key == 1)
                    {
                        for (int i = 0; i < user.Orders.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {user.Orders[i].OrderNum}");
                        }

                        return true;
                    }

                    if (key == 2)
                    {
                        if(OrderInput <= user.Orders.Count && OrderInput > 0)
                        {
                            order = user.Orders[OrderInput];

                            return true;
                        }
                        else
                        {
                            return false;
                        }                        
                    }

                    if(key == 3)
                    {
                        order.OrderProducts.Clear();
                        data.Orders.Remove(order);
                        data.SaveChanges();

                        return true;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }


        public bool AccountRemove_Back(Customer user) // Удаление аккаунта / Account removing
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    for (int i = 0; i < user.Orders.Count; i++)
                    {
                        data.Orders.Remove(user.Orders[i]);
                    }

                    user.Orders.Clear();
                    user.Basket.Clear();
                    data.Users.Remove(user);
                    data.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }
    }
}
