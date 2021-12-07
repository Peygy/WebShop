using NUnit.Framework;
using System.Linq;
using WebShopApp;

namespace WebShopTests
{
    
    [TestFixture]
    class UserToolsTests // Тестирование инструментов для взаимодействия пользователя с магазином
                         // Testing tools for user interaction with the store
    {
        UserBackTools toolsUser = new UserBackTools();
        AdminBackTools toolsAdmin = new AdminBackTools();

        private Category testCategory;
        private Product testProduct;
        private Order testOrder;
        private Customer testUser;

        private bool checking;
        private string testPassword;
        private string testCategoryName;
        private string testProductName;
        private string testUserName;
        private int testId;
        private int testNumber;
        private int testNameForOrder;


        [SetUp]
        public void Setup() // Настройка тестов / Test setup
        {
            checking = false;

            testPassword = "newPass";
            testCategoryName = "testCategory";
            testProductName = "testProduct";
            testUserName = "testUser";

            testId = 1;
            testNumber = 0;
            testNameForOrder = 000000;
        }



        [Test]
        public void CategoriesOutput_Check() // Тест вывода категории для покупки товаров
                                             // Test of displaying category for purchasing products
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);
                toolsUser.CategoriesOutput_Back(testNumber, ref testCategory);

                if (data.Categories.Any(c => c.Id == testId && c == testCategory))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void AddToBasket_Check() // Тест добавления товаров в корзину
                                        // Test of adding products to basket
        {
            using (TestDataContext data = new TestDataContext())
            {
                testUser = new Customer { Login = testUserName, Password = testPassword };
                testProduct = new Product { Name = testProductName };

                data.Warehouse.Add(testProduct);
                data.Users.Add(testUser);
                data.SaveChanges();

                toolsUser.AddToBasket_Back(testProduct, testUser);


                if (data.Users.Any(u => u.Id == testId && u.Basket.Contains(testProduct)))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void UserBasket_Check() // Тест проверки наличия заказа в базе данных
                                       // Testing of checking the existance of an order in the database
        {
            using (TestDataContext data = new TestDataContext())
            {
                if(toolsUser.OrderExist_Back(testNameForOrder))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void RegistrationOrder_Check() // Тест оформления заказа
                                              // Checkout test
        {
            using (TestDataContext data = new TestDataContext())
            {
                testUser = new Customer { Login = testUserName, Password = testPassword };

                data.Users.Add(testUser);
                data.SaveChanges();

                toolsUser.RegistrationOrder_Back(testUser, testNameForOrder);


                if (data.Orders.Any(o => o.Id == testId && o.OrderNum == testNameForOrder))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void ProductRemoveFromBasket_Check() // Тест удаления товара из корзины
                                                    // Test for removing product from the basket
        {
            using (TestDataContext data = new TestDataContext())
            {
                testUser = new Customer { Login = testUserName, Password = testPassword };
                testProduct = new Product { Name = testProductName };

                testUser.Basket.Add(testProduct);
                data.Warehouse.Add(testProduct);
                data.Users.Add(testUser);
                data.SaveChanges();

                toolsUser.ProductRemoveFromBasket_Back(testUser, ref testProduct, testId);


                if (data.Users.Any(u => u.Id == testId && !u.Basket.Contains(testProduct)))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void OrdersInfo_Check() // Тест вывода заказов и удаление заказа
                                       // Test output of orders and remove an order
        {
            int count = 0;

            using (TestDataContext data = new TestDataContext())
            {
                testUser = new Customer { Login = testUserName, Password = testPassword };

                data.Users.Add(testUser);
                data.SaveChanges();

                if(toolsUser.OrdersInfo_Back(ref testUser, testNumber, ref testOrder, 0))
                {
                    count += 1;
                }


                toolsUser.RegistrationOrder_Back(testUser, testNameForOrder);
                
                if(toolsUser.OrdersInfo_Back(ref testUser, testNumber, ref testOrder, 1))
                {
                    count += 1;
                }

                toolsUser.OrdersInfo_Back(ref testUser, testNumber, ref testOrder, 2);


                if (!data.Orders.Any(o => o.Id == testId && o == testOrder))
                {
                    count += 1;
                }
            }

            Assert.AreEqual(3, count);
        }


        [Test]
        public void AccountRemove_Check() // Тест удаления аккаунта
                                          // Account removing test
        {
            using (TestDataContext data = new TestDataContext())
            {
                testUser = new Customer { Login = testUserName, Password = testPassword };

                data.Users.Add(testUser);
                data.SaveChanges();
                toolsUser.RegistrationOrder_Back(testUser, testNameForOrder);

                toolsUser.AccountRemove_Back(testUser);

                if (!data.Users.Any(u => u.Id == testId && u == testUser))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }
    }
}
