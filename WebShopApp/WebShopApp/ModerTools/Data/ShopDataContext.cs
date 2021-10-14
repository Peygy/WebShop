using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    class ShopDataContext : DbContext // Класс контекста с БД
    {
        public DbSet<Customer> Users { get; set; }
        public DbSet<Product> Warehouse { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Moderator> Moders { get; set; }

        public ShopDataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ShopData;Trusted_Connection=True;");
        }
    }
}
