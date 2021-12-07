using System;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    class UserDataContext : DbContext // Класс контекста для пользователя
                                      // Context class for user
    {
        public DbSet<Product> Warehouse { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        public UserDataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #if RELEASE
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ShopData;Trusted_Connection=True;");
            #endif

            #if DEBUG
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestData;Trusted_Connection=True;");
            #endif
        }
    }
}
