using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using WebShopApp;

namespace WebShopTests
{
    [TestFixture]
    class AdminToolsTests // Тестирование инструментов(реализации) модерации для админов
                          // Testing tools (implementation) of moderation for admins
    {
        AdminBackTools toolsAdmin = new AdminBackTools();

        private Category testCategory;
        private Product testProduct;
        private Order testOrder;
        private Customer testUser;
        private Moderator testModer;

        private bool checking;
        private string testNameNew;
        private string testPassword;
        private string testCategoryName;
        private string testProductName;
        private string testUserName;
        private string testModerName;
        private int testId;
        private int testNumber;
        private int testNameForOrder;


        [SetUp]
        public void Setup() // Настройка тестов / Test setup
        {
            checking = false;

            testPassword = "newPass";
            testNameNew = "newName";
            testCategoryName = "testCategory";
            testProductName = "testProduct";
            testUserName = "testUser";
            testModerName = "testModer";

            testId = 1;
            testNumber = 0;
            testNameForOrder = 000000;
        }



        [Test]
        public void AddingNewCategory_Check() // Тест добавления новой категории
                                              // Test for adding a new category
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);

                if (data.Categories.Any(c => c.Id == testId && c.Name == testCategoryName))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void RenameCategory_Check() // Тест переименования категории
                                           // Category rename test
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);
                toolsAdmin.RenameCategory_Back(testCategoryName, testNameNew);

                if (data.Categories.Any(c => c.Id == testId && c.Name == testNameNew))
                {
                    checking = true;
                }
            }
            Assert.AreEqual(true, checking);
        }

        [Test]
        public void RemoveCategory_Check() // Тест удаления категории
                                           // Category removing test
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);

                testCategory = data.Categories
                            .Include(c => c.Products)
                            .FirstOrDefault(c => c.Name == testCategoryName);

                toolsAdmin.RemoveCategory_Back(testCategory);


                if (!data.Categories.Any(c => c.Id == testId && c.Name == testCategoryName))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }



        [Test]
        public void AddNewProduct_Check() // Тест добавления нового товара
                                          // Test of adding a new product
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);
                toolsAdmin.AddNewProduct_Back(testNumber, testProductName, ref testCategoryName, testNumber, 0);
                toolsAdmin.AddNewProduct_Back(testNumber, testProductName, ref testCategoryName, testNumber, 1);

                if (data.Warehouse.Any(p => p.Id == testId && p.Name == testProductName))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void EditProduct_Check() // Тест редактирования товара
                                        // Product edit test
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);
                toolsAdmin.AddNewProduct_Back(testNumber, testProductName, ref testCategoryName, testNumber, 1);
                toolsAdmin.EditProduct_Back(testNumber, ref testProduct);

                if (data.Warehouse.Any(p => p.Id == testId && p.Name == testProductName))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void EditProductRename_Check() // Тест переименования товара
                                              // Product rename test
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);
                toolsAdmin.AddNewProduct_Back(testNumber, testProductName, ref testCategoryName, testNumber, 1);
                toolsAdmin.EditProductRename_Back(testProductName, testNameNew);

                if (data.Warehouse.Any(p => p.Id == testId && p.Name == testNameNew))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void EditProductCategory_Check() // Тест изменения категории товара
                                                // Product category change test
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);
                toolsAdmin.AddNewCategory_Back(testNameNew);
                toolsAdmin.AddNewProduct_Back(testNumber, testProductName, ref testCategoryName, testNumber, 1);

                testProduct = data.Warehouse
                            .Include(p => p.ProductCategory)
                            .FirstOrDefault(p => p.Id == testId);

                toolsAdmin.EditProductCategory_Back(testId, testProduct, ref testCategory);


                if (data.Warehouse.Any(p => p.Id == testId && p.ProductCategory == testCategory))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void EditProductPrice_Check() // Тест изменения цены товара
                                             // Product price change test
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);
                toolsAdmin.AddNewProduct_Back(testNumber, testProductName, ref testCategoryName, testNumber, 1);

                testProduct = data.Warehouse
                            .Include(p => p.ProductCategory)
                            .FirstOrDefault(p => p.Id == testId);

                toolsAdmin.EditProductPrice_Back(testProduct, testNumber + 1);


                if (data.Warehouse.Any(p => p.Id == testId && p.Price == testNumber + 1))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void RemoveProduct_Check() // Тест удаления товара
                                          // Product removing test
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);
                toolsAdmin.AddNewProduct_Back(testNumber, testProductName, ref testCategoryName, testNumber, 1);
                toolsAdmin.RemoveProduct_Back(testNumber, ref testProduct);

                if (!data.Warehouse.Any(p => p.Id == testId && p == testProduct))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void OrdersView_Check() // Тест просмотра заказа
                                       // Order view test
        {
            using (TestDataContext data = new TestDataContext())
            {
                testUser = new Customer { Login = testUserName, Password = testNameNew };
                testOrder = new Order { OrderNum = testNameForOrder, User = testUser };

                data.Orders.Add(testOrder);
                data.SaveChanges();

                toolsAdmin.OrdersView_Back(testNumber, ref testOrder);


                if (data.Orders.Any(o => o.Id == testId && o == testOrder))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void OrderRemove_Check() // Тест удаления заказа
                                        // Order removing test
        {
            using (TestDataContext data = new TestDataContext())
            {
                testUser = new Customer { Login = testUserName, Password = testPassword };
                testOrder = new Order { OrderNum = testNameForOrder, User = testUser };

                data.Users.Add(testUser);
                data.Orders.Add(testOrder);
                data.SaveChanges();

                toolsAdmin.OrderRemove_Back(testNumber, ref testOrder);


                if (!data.Orders.Any(o => o.Id == testId && o == testOrder))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void UserRemove_Check() // Тест удаления пользователя
                                       // User removing test
        {
            using (TestDataContext data = new TestDataContext())
            {
                testUser = new Customer { Login = testUserName, Password = testPassword };
                
                data.Users.Add(testUser);
                data.SaveChanges();

                toolsAdmin.UserRemove_Back(testNumber, ref testUser);


                if (!data.Users.Any(u => u.Id == testId && u == testUser))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void AddModer_Check() // Тест добавления модератора
                                     // Add moderator test
        {
            using (TestDataContext data = new TestDataContext())
            {
                testModer = new Moderator { Login = testModerName, Password = testPassword };

                data.Moders.Add(testModer);
                data.SaveChanges();

                toolsAdmin.AddModer_Back(testModerName, testPassword, 1);


                if (data.Moders.Any(m => m.Id == testId && m.Login == testModerName))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void ModerRemove_Check() // Тест удаления модератора
                                        // Moderator removing test
        {
            using (TestDataContext data = new TestDataContext())
            {
                testModer = new Moderator { Login = testModerName, Password = testPassword };
                
                data.Moders.Add(testModer);
                data.SaveChanges();

                toolsAdmin.ModerRemove_Back(testNumber, ref testModer);


                if (!data.Moders.Any(m => m.Id == testId && m == testModer))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void AddAdmin_Check() // Тест добавления админа
                                     // Admin add test
        {
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddAdmin_Back(testModerName, testPassword, 1);

                if (data.Moders.Any(a => a.Id == testId && a.Login == testModerName))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }
    }
}
