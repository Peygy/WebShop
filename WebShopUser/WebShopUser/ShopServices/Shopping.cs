using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebShopUser
{
    class Shopping // Покупка продуктов
    {
        List<string> ProductCategories;
        List<string> Products;

        public Shopping()
        {
            ProductCategories = new List<string>();
            Products = new List<string>();
        }

        public void CategoriesOutput() // Вывод категорий 
        {
            bool accept = false;

            using (ShopDataContext data = new ShopDataContext())
            {
                var categories = data.Categories.ToList();

                foreach (Category category in categories)
                {
                    ProductCategories.Add(category.CategoryName);
                }
            }

            while (!accept)
            {
                Console.WriteLine("*Для выхода в меню введите - menu");
                Console.WriteLine();
                Console.WriteLine("КАТЕГОРИИ ПРОДУКТОВ");
                Console.WriteLine("Выберите нужную вам категорию");
                Console.WriteLine();

                for (int i = 0; i < ProductCategories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {ProductCategories[i]}");
                }

                Console.WriteLine();
                string input = Console.ReadLine();
                int.TryParse(input, out int CategoryInput);
                if(input.Contains("menu"))
                {
                    return;
                }
                else if (CategoryInput <= ProductCategories.Count && CategoryInput > 0)
                {
                    ProductOutput(ProductCategories[CategoryInput - 1]);
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

        public void ProductOutput(string NameCategory) // Вывод продуктов
        {
            bool accept = false;

            using (ShopDataContext data = new ShopDataContext())
            {
                //data.Warehouse.Include(u => u.ProductCategory).ToList();
                var products = data.Warehouse.Where(p => p.ProductCategory.CategoryName == $"{NameCategory}");

                foreach (Product product in products)
                {
                    Products.Add(product.Name);
                }
            }

            while (!accept)
            {
                Console.WriteLine("*Для выхода в меню введите - menu");
                Console.WriteLine();
                Console.WriteLine("ПРОДУКТЫ");
                Console.WriteLine("Выберите нужный вам товар");
                Console.WriteLine();

                for (int i = 0; i < ProductCategories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {ProductCategories[i]}");
                }

                string input = Console.ReadLine();
                int.TryParse(input, out int ProductInput);
                if (input.Contains("menu"))
                {
                    return;
                }
                if (ProductInput <= ProductCategories.Count && ProductInput > 0)
                {
                    ProductOutput(ProductCategories[ProductInput - 1]);
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

        public void Buying() // Процесс покупки
        { 
            
        }
    }
}
