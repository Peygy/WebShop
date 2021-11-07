using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class ModerFrontTools
    {
        ModerBackTools moderAct = new ModerBackTools();
        Category category;
        Product product;

        public void ViewAllCategories() // Посмотреть все категории
        {
            Console.WriteLine("Все категории:");
            Console.WriteLine();
            moderAct.ViewAllCategories_Back();
        }

        public void ViewAllProducts() // Посмотреть все товары
        {
            Console.WriteLine("Все товары:");
            Console.WriteLine();
            moderAct.ViewAllProducts_Back();
        }

        public void ViewAllOrders() // Посмотреть все заказы
        {
            Console.WriteLine("Все заказы:");
            Console.WriteLine();
            moderAct.ViewAllOrders_Back();
        }

        public void ViewAllUsers() // Посмотреть всех пользователей
        {
            Console.WriteLine("Все пользователи:");
            Console.WriteLine();
            moderAct.ViewAllUsers_Back();
        }



        public void EditCategory() // Редактировать категорию
        {
            bool exit = false;

            while(!exit)
            {
                ViewAllCategories();

                Console.WriteLine();
                Console.Write("Выберите категорию: ");
                string choice = Console.ReadLine();
                int.TryParse(choice, out int categoryChoice);
                categoryChoice -= 1;

                if (choice == "menu")
                {
                    return;
                }

                if (moderAct.EditCategory_Back(categoryChoice, ref category))
                {
                    Console.Clear();
                    Console.WriteLine($"Категория '{category.Name}':");
                    Console.WriteLine();

                    for (int i = 0; i < category.Products.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {category.Products[i].Name} =>  Цена: {category.Products[i].Price} рублей");
                    }

                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Такой категории не существует!");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            Console.WriteLine();
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        AddProduct(category);
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        RemoveProduct(category);
                        break;
                    }
            }
        }

        public void AddProduct(Category category) // Добавить товар
        {
            bool exit = false;

            while(!exit)
            {
                Console.WriteLine($"Доступные товары для добавления в категорию '{category.Name}' :");
                Console.WriteLine();

                moderAct.AddProduct_Back(0, ref product, category, 0);

                Console.WriteLine();
                Console.Write("Введите товар для добавления: ");
                string addChoice = Console.ReadLine();
                int.TryParse(addChoice, out int choice);
                choice -= 1;

                if (addChoice == "menu")
                {
                    return;
                }

                if (moderAct.AddProduct_Back(choice, ref product, category, 1))
                {
                    Console.Clear();
                    Console.WriteLine($"Товар {product.Name} добавлен в '{category.Name}'");
                    Console.ReadLine();

                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Такого товара нет");
                    Console.ReadLine();
                }
            }
        }

        public void RemoveProduct(Category category) // Удалить товар
        {
            bool exit = false;

            while(!exit)
            {
                Console.WriteLine($"Категория '{category.Name}':");
                Console.WriteLine();

                for (int i = 0; i < category.Products.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {category.Products[i].Name} => Цена: {category.Products[i].Price} рублей");
                }

                Console.WriteLine();
                Console.Write("Введите товар для удаления: ");
                string removeChoice = Console.ReadLine();
                int.TryParse(Console.ReadLine(), out int choice);
                choice -= 1;

                if (removeChoice == "menu")
                {
                    return;
                }

                if (moderAct.RemoveProduct_Back(choice, category))
                {
                    Console.Clear();
                    Console.WriteLine($"Товар {category.Products[choice].Name} удален из '{category.Name}'");
                    Console.ReadLine();

                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Такого товара нет");
                    Console.ReadLine();
                }
            }
        }
    }
}
