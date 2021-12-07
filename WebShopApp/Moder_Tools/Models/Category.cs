﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    public class Category // Класс Категории
                          // Category class
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
