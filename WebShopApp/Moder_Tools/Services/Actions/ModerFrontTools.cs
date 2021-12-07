using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class ModerFrontTools // Класс управления модерации
                          // Moderation control class
    {
        ModerBackTools moderAct = new ModerBackTools();
        OutputModelsFront output = new OutputModelsFront();

        Category category;
        Product product;       


        public void EditCategory(ref Category category, ref int key) // Редактировать категорию
                                                                     // Edit category
        {
            bool exit = false;

            while (!exit)
            {
                output.ViewAllCategories();


                Console.Write("Категория для редактирования: ");
                string choice = Console.ReadLine();                
                int.TryParse(choice, out int categoryChoice);
                categoryChoice -= 1;

                if (choice == "menu")
                {
                    key = 1;
                    return;
                }

                if (moderAct.EditCategory_Back(categoryChoice, ref category)) // Из инструментов модератора
                                                                              // From moderator tools
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine($"Категория '{category.Name}':");
                    Console.WriteLine();

                    for (int i = 0; i < category.Products.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {category.Products[i].Name} => " +
                            $"Цена: {category.Products[i].Price} ₽");
                    }

                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Такой категории не существует");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        public void EditCategory_Menu() // Меню выбора редактирования категории
                                        // Category edit selection menu
        {
            int key = 0;
            EditCategory(ref category, ref key);

            if (key == 1)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.Clear();
                        AddProductIntoCategory(category);
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        RemoveProductFromCategory(category);
                        break;
                    }
                case "menu":
                    {
                        return;
                    }
            }
        }

        public void AddProductIntoCategory(Category category) // Добавить продукт в категорию
                                                              // Add product into category
        {
            bool exit = false;

            while(!exit)
            {
                Console.WriteLine();
                Console.WriteLine($"Доступные товары для добавления в категорию '{category.Name}' :");
                Console.WriteLine();

                moderAct.AddProductIntoCategory_Back(0, category, ref product, 0);

                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Введите товар для добавления: ");
                string addChoice = Console.ReadLine();
                int.TryParse(addChoice, out int choice);
                choice -= 1;

                if (addChoice == "menu")
                {
                    return;
                }

                if (moderAct.AddProductIntoCategory_Back(choice, category, ref product, 1))
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine($"Товар {product.Name} добавлен в '{category.Name}'");
                    Console.ReadLine();
                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Такого товара нет");
                    Console.ReadLine();
                }
            }
        }

        public void RemoveProductFromCategory(Category category) // Удалить товар из категории
                                                                 // Remove product from category
        {
            bool exit = false;

            while(!exit)
            {
                Console.WriteLine();
                Console.WriteLine($"Категория '{category.Name}':");
                Console.WriteLine();

                for (int i = 0; i < category.Products.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {category.Products[i].Name} => " +
                        $"Цена: {category.Products[i].Price} ₽");
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Введите товар для удаления: ");
                string removeChoice = Console.ReadLine();
                int.TryParse(Console.ReadLine(), out int choice);

                if (removeChoice == "menu")
                {
                    return;
                }

                if (moderAct.RemoveProductFromCategory_Back(choice, ref product, category))
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine($"Товар {product.Name} удален из '{category.Name}'");
                    Console.ReadLine();
                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Такого товара нет");
                    Console.ReadLine();
                }
            }
        }
    }
}
