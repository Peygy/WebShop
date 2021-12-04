using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebShopApp;

namespace WebShopTests
{
    /*
    [TestFixture]
    class UserToolsTests // Тестирование инструментов для взаимодействия пользователя с магазином / Testing tools for user interaction with the store
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
        public void CategoriesOutput_Check() // Тест вывода категорий для покупки товаров /
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
        public void AddToBasket_Check() // Тест добавления товаров в корзину /
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

        [Test]
        public void UserBasket_Check() // Тест действий с корзиной /
        {
            checking = false;

            using (TestDataContext data = new TestDataContext())
            {
                if (!data.Orders.Any(o => o.OrderNum == testNameForOrder))
                {
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
        public void RegistrationOrder_Check() // Тест оформления заказа /
        {
            checking = false;
            Create_Test_Customer();

            using (TestDataContext data = new TestDataContext())
            {
                Order order = new Order { OrderNum = testNameForOrder, User = testCustomer, OrderProducts = testCustomer.Basket, Status = "На складе" };
                data.Orders.Add(order);
                testCustomer.Orders.Add(order);
                testCustomer.Basket.Clear();
                data.SaveChanges();

                checking = true;

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void ProductRemoveFromBasket_Check() 
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                Product product = data.Warehouse.FirstOrDefault(p => p.Id == testNumber);
                testCustomer.Basket.Remove(product);
                data.SaveChanges();

                checking = true;

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void OrdersInfo_Check()
        {
            checking = false;
            Create_Test_Customer();
            Create_Test_Order();

            using (TestDataContext data = new TestDataContext())
            {
                for (int i = 0; i < testCustomer.Orders.Count; i++)
                {
                    checking = true;
                }

                if (testNumber <= testCustomer.Orders.Count)
                {
                    testOrder = testCustomer.Orders[testNumber];

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
        public void AccountRemove_Check()
        {
            checking = false;
            Create_Test_Customer();

            using (TestDataContext data = new TestDataContext())
            {
                for (int i = 0; i < testCustomer.Orders.Count; i++)
                {
                    data.Orders.Remove(testCustomer.Orders[i]);
                }

                testCustomer.Orders.Clear();
                testCustomer.Basket.Clear();
                data.Users.Remove(testCustomer);
                data.SaveChanges();

                checking = true;

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }
    }
    */
}
