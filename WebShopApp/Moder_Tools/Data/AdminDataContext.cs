using System;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    class AdminDataContext : DbContext // Класс контекста для сотрудников
                                       // Context class for staff
    {
        public DbSet<Customer> Users { get; set; }
        public DbSet<Product> Warehouse { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Moderator> Moders { get; set; }


        public AdminDataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #if RELEASE
                optionsBuilder
                    .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ShopData;Trusted_Connection=True;");                    
            #endif

            #if DEBUG
                optionsBuilder
                    .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestData;Trusted_Connection=True;");                  
            #endif
        }
    }
}
