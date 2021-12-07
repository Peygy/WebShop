using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace WebShopApp
{
    public class ModerBackTools // Класс инструментов(реализации) модерации
                                // Moderation tool(realization) class
    {     
        public bool EditCategory_Back(int categoryChoice, ref Category category) // Редактирование категории
                                                                                 // Editing category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var categories = data.Categories
                        .Include(c => c.Products).ToList();

                    if (data.Categories.Any(c => c == categories[categoryChoice]))
                    {
                        category = data.Categories
                            .Include(c => c.Products)
                            .FirstOrDefault(c => c == categories[categoryChoice]);

                        return true;
                    }
                    else
                    {
                        return false;
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

        public bool AddProductIntoCategory_Back(int choice, Category category, ref Product product, int key) // Добавление товара в категорию
                                                                                                             // Adding product into category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    var products = data.Warehouse
                        .Where(p => p.ProductCategory == null).ToList();

                    if(key == 0) // Output 'products' without category
                    {
                        for (int i = 0; i < products.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {products[i].Name} => Цена: {products[i].Price} рублей");
                        }

                        return true;
                    }
                    else // Adding 'product' to category
                    {
                        if (data.Warehouse.Any(p => p == products[choice]))
                        {
                            category = data.Categories
                                .Include(c => c.Products)
                                .FirstOrDefault(c => c == category);

                            product = data.Warehouse
                                .Include(p => p.ProductCategory)
                                .FirstOrDefault(p => p == products[choice]);

                            category.Products.Add(product);
                            data.SaveChanges();

                            return true;
                        }
                        else
                        {
                            return false;
                        }
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

        public bool RemoveProductFromCategory_Back(int choice, ref Product product, Category category) // Удаление товара из категории
                                                                                                       // Removing product from category
        {
            try
            {
                using (AdminDataContext data = new AdminDataContext())
                {
                    category = data.Categories
                        .Include(c => c.Products)
                        .FirstOrDefault(c => c == category);

                    if (category.Products.Count >= choice)
                    {                       
                        product = data.Warehouse
                            .Include(p => p.ProductCategory)
                            .FirstOrDefault(p => p.Id == choice);

                        product.ProductCategory = null;
                        category.Products.Remove(product);
                        data.SaveChanges();

                        return true;
                    }
                    else
                    {
                        return false;
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
    }
}
