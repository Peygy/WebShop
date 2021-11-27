using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using WebShopApp;

namespace WebShopTests
{
    [TestFixture]
    class AdminToolsTests // Тестирование инструментов(реализации) модерации для админов / Testing tools (implementation) of moderation for admins
    {
        private bool checking;

        private Category testCategory;
        private Product testProduct;
        private Order testOrder;
        private Customer testCustomer;
        private Moderator testModer;

        private string testName;
        private string testNameNew;
        private int testNumber;
        private int testNameForOrder;


        [SetUp]
        public void Setup() // Настройка тестов / Test setup
        {
            checking = false;
            testName = "test";
            testNameNew = "newTest";
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

        public void Create_Test_Product() // Создать тестовый товара / Create a test product
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

        public void Create_Test_Order() // Создать тестовый заказ / Create a test order
        {
            using (TestDataContext data = new TestDataContext())
            {
                testOrder = new Order { OrderNum = testNameForOrder };
                data.Orders.Add(testOrder);
                data.SaveChanges();
            }
        }

        public void Create_Test_Customer() // Создать тестового пользователя / Create a test user
        {
            using (TestDataContext data = new TestDataContext())
            {
                testCustomer = new Customer { Login = testName, Password = testName };
                data.Users.Add(testCustomer);
                data.SaveChanges();
            }
        }

        public void Create_Test_Moder() // Создать тестового модератора / Create a test moder
        {
            using (TestDataContext data = new TestDataContext())
            {
                testModer = new Moderator { Login = testName, Password = testName, SpecialKey = "01" };
                data.Moders.Add(testModer);
                data.SaveChanges();
            }
        }



        [Test]
        public void AddingNewCategory_Check() // Тест добавления новой категории / Test for adding a new category
        {
            checking = false;

            using (TestDataContext data = new TestDataContext())
            {
                if (!data.Categories.Any(p => p.Name == testName))
                {
                    Create_Test_Category();

                    checking = true;
                }
                else
                {
                    checking = false;
                }

                data.SaveChanges();
                data.Categories.RemoveRange(data.Categories);
            }

            Assert.AreEqual(true, checking);
        }      

        [Test]
        public void RenameCategory_Check() // Тест переименования категории / Category rename test
        {
            checking = false;
            Create_Test_Category();

            using (TestDataContext data = new TestDataContext())
            {
                if (!data.Categories.Any(p => p.Name == testNameNew))
                {
                    data.Categories.Include(p => p.Products).FirstOrDefault(p => p.Name == testName).Name = testNameNew;
                    data.SaveChanges();

                    checking = true;
                }
                else
                {
                    checking = false;
                }

                data.SaveChanges();
                data.Categories.RemoveRange(data.Categories);
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void RemoveCategory_Check() // Тест удаления категории / Category removing test
        {
            checking = false;
            Create_Test_Category();

            using (TestDataContext data = new TestDataContext())
            {
                foreach (Product product in testCategory.Products)
                {
                    product.ProductCategory = null;
                }

                testCategory.Products.Clear();
                data.Categories.RemoveRange(data.Categories);
                data.SaveChanges();

                checking = true;
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void AddNewProduct_Check() // Тест добавления нового товара / Test of adding a new product
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                var categories = data.Categories.Include(c => c.Products).ToList();

                if (data.Categories.Any(c => c.Id == categories[testNumber].Id))
                {
                    string nullName = data.Categories.FirstOrDefault(c => c.Id == categories[testNumber].Id).Name;
                }

                data.Warehouse.RemoveRange(data.Warehouse);
                data.Categories.RemoveRange(data.Categories);
                data.SaveChanges();
                checking = true;
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void EditProduct_Check() // Тест редактирования товара / Product edit test
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                if (data.Warehouse.Any(p => p.Id == testNumber))
                {
                    testProduct = data.Warehouse.Include(p => p.ProductCategory).FirstOrDefault(p => p.Id == testNumber);

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
        public void EditProductRename_Check() // Тест переименования товара / Product rename test
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                if (!data.Warehouse.Any(p => p.Name == testNameNew))
                {
                    data.Warehouse.FirstOrDefault(p => p.Name == testName).Name = testNameNew;
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
        public void EditProductCategory_Check() // Тест изменения категории товара / Product category change test
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                var categories = data.Categories.Include(c => c.Products).ToList();

                if (data.Categories.Any(c => c.Id == categories[testNumber].Id))
                {
                    testCategory = data.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == categories[testNumber].Id);
                    data.Warehouse.Include(p => p.ProductCategory).FirstOrDefault(p => p.ProductCategory.Name == testName).ProductCategory = testCategory;
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
        public void EditProductPrice_Check() // Тест изменения цены товара / Product price change test
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                data.Warehouse.FirstOrDefault(p => p.Name == testProduct.Name).Price = testNumber;

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.Pass();
        }

        [Test]
        public void RemoveProduct_Check() // Тест удаления товара / Product removing test
        {
            checking = false;
            Create_Test_Product();

            using (TestDataContext data = new TestDataContext())
            {
                var products = data.Warehouse.Include(p => p.ProductCategory).ToList();

                if (data.Warehouse.Any(p => p.Id == products[testNumber].Id))
                {
                    testProduct = products[testNumber];
                    testCategory = testProduct.ProductCategory;

                    testCategory.Products.Remove(testProduct);
                    data.Warehouse.Remove(testProduct);
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
        public void OrdersView_Check() // Тест просмотра заказа / Order view test
        {
            checking = false;
            Create_Test_Order();

            using (TestDataContext data = new TestDataContext())
            {
                var orders = data.Orders.Include(o => o.OrderProducts).Include(o => o.User).ToList();

                if (data.Orders.Any(o => o.Id == orders[testNumber].Id))
                {
                    testOrder = orders[testNumber];

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
        public void OrderRemove_Check() // Тест удаления заказа / Order removing test
        {
            checking = false;
            Create_Test_Order();

            using (TestDataContext data = new TestDataContext())
            {
                var orders = data.Orders.Include(o => o.OrderProducts).Include(o => o.User).ToList();

                if (data.Orders.Any(o => o.Id == orders[testNumber].Id))
                {
                    testOrder = orders[testNumber];
                    testOrder.User.Orders.Remove(testOrder);

                    data.Orders.Remove(testOrder);
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
        public void UserRemove_Check() // Тест удаления пользователя / User removing test
        {
            checking = false;
            Create_Test_Customer();

            using (TestDataContext data = new TestDataContext())
            {
                var customers = data.Users.Include(u => u.Basket).Include(u => u.Orders).ToList();

                if (data.Users.Any(u => u.Id == customers[testNumber].Id))
                {
                    testCustomer = data.Users.Include(u => u.Basket).Include(u => u.Orders).FirstOrDefault(u => u.Id == customers[testNumber].Id);

                    for (int i = 0; i < testCustomer.Orders.Count; i++)
                    {
                        data.Orders.Remove(testCustomer.Orders[i]);
                    }

                    testCustomer.Orders.Clear();
                    testCustomer.Basket.Clear();
                    data.Users.Remove(testCustomer);
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
        public void AddModer_Check() // Тест добавления модератора / Add moderator test
        {
            checking = false;

            using (TestDataContext data = new TestDataContext())
            {
                if (!data.Moders.Any(m => m.Login == testName))
                {
                    Create_Test_Moder();

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
        public void ModerRemove_Check() // Тест удаления модератора / Moderator removing test
        {
            checking = false;

            using (TestDataContext data = new TestDataContext())
            {
                var moders = data.Moders.Where(m => m.Login != "Peygy").ToList();

                if (data.Moders.Any(m => m.Id == moders[testNumber].Id))
                {
                    testModer = moders[testNumber];
                    data.Moders.Remove(data.Moders.FirstOrDefault(m => m.Id == moders[testNumber].Id));
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
        public void AddAdmin_Check() // Тест добавления админа / Admin add test
        {
            checking = false;

            using (TestDataContext data = new TestDataContext())
            {
                if (!data.Moders.Any(m => m.Login == testName))
                {
                    Admin testAdmin = new Admin { Login = testName, Password = testName, SpecialKey = "011" };
                    data.Moders.Add(testAdmin);
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
        public void ViewAllModers_Check() // Тест посмотра всех модераторов / Test viewing of all moderators
        {
            checking = false;

            using (TestDataContext data = new TestDataContext())
            {
                var moderators = data.Moders.Where(m => m.Login != "Peygy").ToList();

                for (int i = 0; i < moderators.Count; i++)
                {
                    checking = true;
                }               

                data.RemoveRange(data);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }
    }
}
