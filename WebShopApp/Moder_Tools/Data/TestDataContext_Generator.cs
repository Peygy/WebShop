using System;
using System.Data.Common;
//using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp
{
    /*
    class TestDataContext_Generator : IDisposable // Класс генерации базы данных для тестов / Data base generation for tests
    {
        private DbConnection connection;
       

        private DbContextOptions<TestDataContext> Create_ContextOptions() // Создание настроек для SQLite в основном контексте тестов
                                                                          // Making settings for SQLite in the main test context
        {
            return new DbContextOptionsBuilder<TestDataContext>()
                .UseSqlite(connection).Options;
        }

        public TestDataContext CreateContext() // Создание базы данных в памяти с SQLite / Creating an in-memory database with SQLite
        {
            if(connection == null)
            {
                connection = new SqliteConnection("DataSource=:memory");
                connection.Open();

                var options = Create_ContextOptions();
                using (var context = new TestDataContext(options))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new TestDataContext(Create_ContextOptions());
        }


        public void Dispose() // Удаление подключения, если не используется / Removing a connection if not in use
        {
            if(connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }
    }
    */
}
