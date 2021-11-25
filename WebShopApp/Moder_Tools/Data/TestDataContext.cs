using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    public class TestDataContext : DbContext // Класс контекста для тестов / Context class for tests
    {
        public DbSet<Customer> Users { get; set; }
        public DbSet<Product> Warehouse { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Moderator> Moders { get; set; }

        public TestDataContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestData;Trusted_Connection=True;");
        }
    }
}
