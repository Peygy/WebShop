using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class UserBackTools // Класс инструментов для взаимодействия пользователя с магазином / Class of tools for user interaction with the store
    {
        ModerFrontTools moderAct = new ModerFrontTools();


        public bool CategoriesOutput_Back(int categoryInput, ref Category category) // Вывод категорий для покупки товаров / Output categories for buying products
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    var categories = data.Categories.ToList();

                    if (categories.Any(p => p.Id == categories[categoryInput].Id))
                    {
                        category = categories.FirstOrDefault(p => p.Id == categories[categoryInput].Id);

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
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool ProductOutput_Back(Product product, Customer user) // Вывод товаров для покупки / Output products to buy
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
                Console.WriteLine(ex.Message);
            }

            return false;
        }



        public bool UserBasket_Back(int orderNumber) // Действия с корзиной / Basket actions
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    if (!data.Orders.Any(p => p.OrderNum == orderNumber))
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
                Console.WriteLine(ex.Message);
            }

            return false;
        }


        public bool RegistrationOrder_Back(Customer user, int orderNumber) // Оформление заказа / Checkout
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    Order order = new Order { OrderNum = orderNumber, User = user, Status = "На складе" };
                    data.Orders.Add(order);
                    user.Basket.Clear();
                    data.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool ProductRemoveFromBasket_Back(Customer user, int prodNum) // Удаление товара / Product removing
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    user.Basket.RemoveAt(prodNum);
                    data.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool OrdersInfo_Back(Customer user, int OrderInput, ref Order order, int key) // Вывод заказов и удаление заказа / Output orders and removing order
        {
            try
            {
                using (UserDataContext data = new UserDataContext())
                {
                    var userWithOrders = data.Users.Include(p => p.Order).Where(u => u.Order.User == user).ToList();

                    if(key == 0)
                    {
                        if (userWithOrders.Count == 0)
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
                        for (int i = 0; i < userWithOrders.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {userWithOrders[i].Order.OrderNum}");
                        }

                        return true;
                    }

                    if (key == 2)
                    {
                        if(OrderInput <= userWithOrders.Count && OrderInput > 0)
                        {
                            order = userWithOrders[OrderInput].Order;

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
                Console.WriteLine(ex.Message);
            }

            return false;
        }


        public bool AccountRemove_Back(Customer user) // Удаление аккаунта / Account removing
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

                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
