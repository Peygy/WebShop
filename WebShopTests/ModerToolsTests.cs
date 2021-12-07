using NUnit.Framework;
using System.Linq;
using WebShopApp;

namespace WebShopTests
{    
    [TestFixture]
    class ModerToolsTests // Тестирование инструментов(реализации) модерации
                          // Testing Moderation Tools (Implementation)
    {
        ModerBackTools toolsModer = new ModerBackTools();
        AdminBackTools toolsAdmin = new AdminBackTools();

        private Category testCategory;
        private Product testProduct;

        private bool checking;
        private string testCategoryName;
        private string testProductName;
        private int testId;
        private int testNumber;


        [SetUp]
        public void Setup() // Настройка тестов / Test setup
        {
            checking = false;
           
            testCategoryName = "testCategory";
            testProductName = "testProduct";

            testId = 1;
            testNumber = 0;
        }



        [Test]
        public void EditCategory_Check() // Тест редактирования категории
                                         // Category edit test
        { 
            using (TestDataContext data = new TestDataContext())
            {
                toolsAdmin.AddNewCategory_Back(testCategoryName);
                toolsAdmin.AddNewProduct_Back(testNumber, testProductName, ref testCategoryName, testNumber, 1);

                toolsModer.EditCategory_Back(testNumber, ref testCategory);


                if (data.Categories.Any(c => c.Id == testId && c == testCategory))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }


        [Test]
        public void AddProductIntoCategory_Check() // Тест добавления товара
                                                   // Product adding test
        {
            using (TestDataContext data = new TestDataContext())
            {
                testCategory = new Category { Name = testCategoryName };
                testProduct = new Product { Name = testProductName };

                data.Categories.Add(testCategory);
                data.Warehouse.Add(testProduct);
                data.SaveChanges();

                toolsModer.AddProductIntoCategory_Back(testNumber, testCategory, ref testProduct, 1);


                if (data.Categories.Any(c => c.Id == testId && c.Products.Contains(testProduct)))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void RemoveProductFromCategory_Check() // Тест удаления товара
                                                      // Product removing test
        {
            using (TestDataContext data = new TestDataContext())
            {
                testCategory = new Category { Name = testCategoryName };
                testProduct = new Product { Name = testProductName };

                data.Categories.Add(testCategory);
                data.Warehouse.Add(testProduct);
                data.SaveChanges();

                toolsModer.RemoveProductFromCategory_Back(testId, ref testProduct, testCategory);


                if (data.Categories.Any(c => c.Id == testId && !c.Products.Contains(testProduct)))
                {
                    checking = true;
                }
            }

            Assert.AreEqual(true, checking);
        }
    }
}
