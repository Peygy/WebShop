using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class UserShopping // Покупка товаров
    {
        ModerTools moderAct = new ModerTools();
        Customer user;

        public UserShopping(Customer UserInput)
        {            
            user = UserInput;
        }

        public void CategoriesOutput() // Вывод категорий 
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
                int.TryParse(categoryChoice, out int CategoryInput);

                using (UserDataContext data = new UserDataContext())
                {
                    var categories = data.Categories.ToList();                    

                    if (categoryChoice.Contains("menu"))
                    {
                        return;
                    }
                   
                    if (categories.Any(p => p.Id == categories[CategoryInput - 1].Id))
                    {
                        ProductOutput(categories[CategoryInput - 1].Id);
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

        public void ProductOutput(int categoryId) // Вывод товаров
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

                        if (productChoice.Contains("menu"))
                        {
                            return;
                        }

                        if (products.Count < productNum)
                        {
                            Console.Clear();
                            products[productNum - 1].ProductInfo();
                            Console.WriteLine("1. Добавить в корзину");
                            Console.WriteLine("2. Вернуться в меню");
                            Console.WriteLine();

                            if (Console.ReadLine() == "1")
                            {
                                user.Basket.Add(products[productNum - 1]);
                                data.SaveChanges();
                                Console.Clear();
                                Console.WriteLine("Товар успешно добавлен в корзину! Нажмите Enter");
                                Console.ReadLine();
                            }
                            accept = true;
                        }
                        else
                        {
                            Console.WriteLine("Такого номера товара не существует! Нажмите Enter и введите другой номер!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch(DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                }                
            }
        }
    }
}
