using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using WebShopApp;

namespace WebShopTests
{
    [TestFixture]
    class ModerToolsTests // Тестирование инструментов(реализации) модерации / Testing Moderation Tools (Implementation)
    {
        private bool checking;

        private Category testCategory;
        private Product testProduct;
        private Order testOrder;
        private Customer testCustomer;

        private string testName;
        private int testNumber;
        private int testNameForOrder;


        [SetUp]
        public void Setup() // Настройка тестов / Test setup
        {
            testName = "test";
            testNumber = 0;
            testNameForOrder = 000000;
        }


        public void Create_Test_Category() // Создать тестовую категорию / Create test category
        {
            using (TestDataContext data = new TestDataContext())
            {
                testCategory = new Category { Name = testName };
                data.Categories.Add(testCategory);
                data.SaveChanges();
            }
        }

        public void Create_Test_Product() // Создать тестовый продукт / Create a test product
        {
            using (TestDataContext data = new TestDataContext())
            {
                Create_Test_Category();

                testProduct = new Product { Name = testName, ProductCategory = testCategory, Price = testNumber };
                data.Warehouse.Add(testProduct);
                testCategory.Products.Add(testProduct);
                data.SaveChanges();
            }
        }

        public void Create_Test_Order() // Создать тестовый заказ / Create test order
        {
            using (TestDataContext data = new TestDataContext())
            {
                testOrder = new Order { OrderNum = testNameForOrder };
                data.Orders.Add(testOrder);
                data.SaveChanges();
            }
        }

        public void Create_Test_Customer() // Создать тестового пользователя / Create test user
        {
            using (TestDataContext data = new TestDataContext())
            {
                testCustomer = new Customer { Login = testName, Password = testName };
                data.Users.Add(testCustomer);
                data.SaveChanges();
            }
        }



        [Test]
        public void ViewAllCategories_Check() // Тест просмотра всех категорий / Test of viewing all categories
        {
            checking = false;
            Create_Test_Category();

            using (TestDataContext data = new TestDataContext())
            {
                var categories = data.Categories.Include(c => c.Products).ToList();

                foreach (Category category in categories)
                {
                    checking = true;
                }

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void ViewAllProducts_Check() // Тест просмотра всех товаров / Test of viewing of all products
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                var products = data.Warehouse.Include(p => p.ProductCategory).ToList();

                for (int i = 0; i < products.Count; i++)
                {
                    string category = products[i].ProductCategory == null ? "Пусто" : $"{products[i].ProductCategory.Name}";

                    checking = true;
                }

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void ViewAllOrders_Check() // Тест просмотра всех заказов / Test of viewing all orders
        {
            checking = false;
            Create_Test_Order();

            using (TestDataContext data = new TestDataContext())
            {
                var orders = data.Orders.Include(o => o.User).ToList();

                for (int i = 0; i < orders.Count; i++)
                {
                    checking = true;
                }

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void ViewAllUsers_Check() // Тест просмотра всех пользователей / Test veiwing for all users
        {
            checking = false;
            Create_Test_Customer();

            using (TestDataContext data = new TestDataContext())
            {
                var customers = data.Users.Include(u => u.Basket).ToList();

                for (int i = 0; i < customers.Count; i++)
                {
                    checking = true;
                }

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void EditCategory_Check() // Тест редактирования категории / Category edit test
        { 
            checking = false;
            Create_Test_Category();

            using (TestDataContext data = new TestDataContext())
            {
                var categories = data.Categories.Include(p => p.Products).ToList();

                if (data.Categories.Any(p => p.Id == categories[testNumber].Id))
                {
                    testCategory = categories.FirstOrDefault(p => p.Id == categories[testNumber].Id);

                    checking = true;
                }
                else
                {
                    checking = false;
                }

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void AddProduct_Check() // Тест добавления товара / Product adding test
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                var products = data.Warehouse.Where(p => p.ProductCategory == null).ToList();

                if (data.Warehouse.Any(p => p.Name == products[testNumber].Name))
                {
                    testProduct = data.Warehouse.Include(p => p.ProductCategory).FirstOrDefault(p => p.Id == products[testNumber].Id);
                    testProduct.ProductCategory.Products.Add(testProduct);
                    data.SaveChanges();

                    checking = true;
                }
                else
                {
                    checking = false;
                }

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void RemoveProduct_Check() // Тест удаления товара / Product removing test
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                if (data.Warehouse.Any(p => p.Name == testCategory.Products[testNumber].Name))
                {
                    testProduct = data.Warehouse.Include(p => p.ProductCategory).FirstOrDefault(p => p.Id == testNumber);
                    testProduct.ProductCategory = null;
                    testCategory.Products.Remove(testProduct);
                    data.SaveChanges();

                    checking = true;
                }
                else
                {
                    checking = false;
                }

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }
    }
}
