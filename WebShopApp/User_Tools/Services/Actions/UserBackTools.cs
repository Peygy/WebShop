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
                    var categories = data.Categories
                        .Include(c => c.Products).ToList();

                    if (categories.Any(c => c == categories[categoryInput]))
                    {
                        category = categories
                            .FirstOrDefault(c => c == categories[categoryInput]);

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
                    product = data.Warehouse
                        .Include(p => p.Orders)
                        .Include(p => p.Customers)
                        .FirstOrDefault(p => p == product);

                    user = data.Users
                        .Include(u => u.Basket)
                        .Include(u => u.Orders)
                        .FirstOrDefault(u => u == user);

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
                    user = data.Users
                        .Include(u => u.Basket)
                        .Include(u => u.Orders)
                        .FirstOrDefault(u => u == user);

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

        public bool ProductRemoveFromBasket_Back(Customer user, ref Product product,int prodNum) // Удаление товара из корзины / Product removing from basket
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    user = data.Users
                        .Include(u => u.Basket)
                        .Include(u => u.Orders)
                        .FirstOrDefault(u => u == user);

                    product = data.Warehouse
                        .Include(p => p.Customers)
                        .Include(p => p.Orders)
                        .FirstOrDefault(p => p.Id == prodNum);

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

        public bool OrdersInfo_Back(ref Customer user, int OrderInput, ref Order order, int key) // Вывод заказов и удаление заказа / Output orders and removing order
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    Customer customer = user;

                    user = data.Users
                        .Include(u => u.Basket)
                        .Include(u => u.Orders)
                        .FirstOrDefault(u => u == customer);                   


                    if (key == 0) // // 'orders' emptyCheck
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

                    if (key == 1) // 'order' Output
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

                    if(key == 2) // 'order' Removing
                    {
                        Order orderForRemove = order;

                        orderForRemove = data.Orders
                            .Include(o => o.OrderProducts)
                            .FirstOrDefault(o => o == orderForRemove);

                        orderForRemove.OrderProducts.Clear();
                        data.Orders.Remove(orderForRemove);
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
                    user = data.Users
                        .Include(u => u.Basket)
                        .Include(u => u.Orders)
                        .FirstOrDefault(u => u == user);

                    for (int i = 0; i < user.Orders.Count; i++)
                    {
                        data.Orders.Remove(user.Orders[i]);
                    }

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


        public void BasketInfo_Back(ref Customer user) // Вывод корзины покупателя / Output user's basket
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    Customer customer = user;

                    user = data.Users
                        .Include(u => u.Basket)
                        .Include(u => u.Orders)
                        .FirstOrDefault(u => u == customer);                                    
                }
            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        public bool BasketEmptyCheck_Back(Customer user)
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    user = data.Users
                        .Include(u => u.Basket)
                        .Include(u => u.Orders)
                        .FirstOrDefault(u => u == user);

                    if (user.Basket.Count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return false;
        }
    }
}
