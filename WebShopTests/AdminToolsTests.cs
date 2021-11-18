using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using WebShopApp;

namespace WebShopTests
{
    [TestFixture]
    class AdminToolsTests
    {
        private bool checking;

        private AdminBackTools adminTools;
        private Category category;
        private Product product;

        private string testName;
        private string testNameNew;
        private int testNumber;

        [SetUp]
        public void Setup()
        {
            adminTools = new AdminBackTools();
            category = new Category();

            testName = "test";
            testNameNew = "newTest";
            testNumber = 0;
        }


        public void Create_Test_Category()
        {
            using (TestDataContext data = new TestDataContext())
            {
                category = new Category { Name = testName };
                data.Categories.Add(category);
                data.SaveChanges();
            }
        }

        public void Create_Test_Product()
        {
            using (TestDataContext data = new TestDataContext())
            {
                category = new Category { Name = testName };
                data.Categories.Add(category);

                product = new Product { Name = testName, ProductCategory = category, Price = testNumber };
                data.Warehouse.Add(product);
                category.Products.Add(product);
                data.SaveChanges();
            }
        }


        [Test]
        public void Adding_New_Category_Check()
        {
            using (TestDataContext data = new TestDataContext())
            {
                if (!data.Categories.Any(p => p.Name == testName))
                {
                    Create_Test_Category();
                    data.Categories.RemoveRange(data.Categories);
                    data.SaveChanges();

                    checking = true;
                }
                else
                {
                    checking = false;
                }    
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void EditCategory_Check()
        {
            using (TestDataContext data = new TestDataContext())
            {
                Create_Test_Category();
                var categories = data.Categories.Include(p => p.Products).ToList();

                if (data.Categories.Any(p => p.Id == categories[testNumber].Id))
                {
                    category = categories.FirstOrDefault(p => p.Id == categories[testNumber].Id);

                    checking = true;
                }
                else
                {
                    checking = false;
                }

                data.Categories.RemoveRange(data.Categories);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void RenameCategory_Check()
        {
            using (TestDataContext data = new TestDataContext())
            {
                Create_Test_Category();

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

                data.Categories.RemoveRange(data.Categories);
                data.SaveChanges();
            }

            Assert.AreEqual(true, checking);
        }

        [Test]
        public void RemoveCategory_Back()
        {
            Create_Test_Category();

            using (TestDataContext data = new TestDataContext())
            {
                foreach (Product product in category.Products)
                {
                    product.ProductCategory = null;
                }

                category.Products.Clear();
                data.Categories.Remove(category);
                data.SaveChanges();

                Assert.Pass();
            }
        }

        [Test]
        public void AddNewProduct_Back() 
        {
            using (TestDataContext data = new TestDataContext())
            {
                var categories = data.Categories.Include(p => p.Products).ToList();

                Category category = data.Categories.Include(p => p.Products).FirstOrDefault(с => с.Name == data.Categories.FirstOrDefault(p => p.Id == categories[testNumber].Id).Name);
                Product newProduct = new Product { Name = testName, ProductCategory = category, Price = testNumber };

                data.Warehouse.Add(newProduct);
                category.Products.Add(newProduct);
                data.SaveChanges();

                data.Warehouse.RemoveRange(data.Warehouse);
                data.SaveChanges();

                Assert.Pass();
            }
        }
    }
}
