using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using WebShopApp;

namespace WebShopTests
{
    [TestFixture]
    class UserToolsTests
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
            using (TestDataContext data = new TestDataContext())
            {
                data.RemoveRange(data);
                data.SaveChanges();
            }

            testName = "test";
            testNumber = 0;
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
        public void CategoriesOutput_Check()
        {
            checking = false;
            Create_Test_Category();

            using (TestDataContext data = new TestDataContext())
            {
                var categories = data.Categories.Include(c => c.Products).ToList();

                if (categories.Any(c => c.Id == categories[testNumber].Id))
                {
                    testCategory = categories.FirstOrDefault(c => c.Id == categories[testNumber].Id);

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
        public void AddToBaskett_Check()
        {
            checking = false;
            Create_Test_Product();
            Create_Test_Customer();

            using (TestDataContext data = new TestDataContext())
            {
                testCustomer.Basket.Add(testProduct);
                data.SaveChanges();

                checking = true;

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }
    }
}
