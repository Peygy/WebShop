using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopApp
{
    class UserShopping // Покупка продуктов
    {
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
                string CatChoice; // Для выбора категории
                int CategoryInput;

                Console.WriteLine("*Для выхода в меню введите - menu");
                Console.WriteLine();
                Console.WriteLine("КАТЕГОРИИ ПРОДУКТОВ");
                Console.WriteLine("Выберите нужную вам категорию");
                Console.WriteLine();

                using (ShopDataContext data = new ShopDataContext())
                {
                    var categories = data.Categories.ToList();

                    foreach (Category category in categories)
                    {
                        Console.WriteLine($"{category.Id + 1}. {category.Name}");                       
                    }

                    Console.WriteLine();
                    CatChoice = Console.ReadLine();
                    if (CatChoice.Contains("menu"))
                    {
                        return;
                    }

                    int.TryParse(CatChoice, out CategoryInput);
                    if (CategoryInput <= categories.Count && CategoryInput > 0)
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

        public void ProductOutput(int categoryId) // Вывод продуктов
        {
            bool accept = false;
            
            while (!accept)
            {
                string PrChoice; // Для выбора продукта
                int ProductInput;

                Console.WriteLine("*Для выхода в меню введите - menu");
                Console.WriteLine();
                Console.WriteLine("ПРОДУКТЫ");
                Console.WriteLine("Выберите нужный вам товар");
                Console.WriteLine();

                using (ShopDataContext data = new ShopDataContext())
                {
                    var products = data.Warehouse.Where(p => p.ProductCategory.Id == categoryId).ToList();

                    foreach (Product product in products)
                    {
                        Console.WriteLine($"{product.Id + 1}. {product.Name}");
                    }

                    Console.WriteLine();
                    PrChoice = Console.ReadLine();
                    if (PrChoice.Contains("menu"))
                    {
                        return;
                    }

                    int.TryParse(PrChoice, out ProductInput);
                    if (ProductInput <= products.Count && ProductInput > 0)
                    {
                        Console.Clear();
                        products[ProductInput - 1].ProductInfo();
                        Console.WriteLine("1. Добавить в корзину");
                        Console.WriteLine("2. Вернуться в меню");
                        Console.WriteLine();

                        if (Console.ReadLine() == "1")
                        {
                            user.Basket.Add(products[ProductInput - 1]);
                            Console.Clear();
                            Console.WriteLine("Товар успешно добавлен в корзину! Нажмите Enter");
                            Console.ReadLine();
                            accept = true;
                        }
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
