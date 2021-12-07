using NUnit.Framework;
using WebShopApp;

namespace WebShopTests 
{
    [TestFixture]
    class UserEntryTests // Тестирование авторизации и регистрации
                         // Testing authorization and registration
    {
        UserEntryBack toolsEntry = new UserEntryBack();

        private Customer testUser;
        private Moderator testModer;

        private string testPassword;
        private string testUserName;
        private string testModerName;


        [SetUp]
        public void Setup() // Настройка тестов / Test setup
        {
            testPassword = "newPass";
            testUserName = "testUser";
            testModerName = "testModer";
        }

        [Test]
        public void SingIn_Check() // Тест входа в аккаунт
                                   // Account login test
        {
            int count = 0;

            using (TestDataContext data = new TestDataContext())
            {
                testModer = new Moderator { Login = testModerName, Password = testPassword, SpecialKey = "01" };
                testUser = new Customer { Login = testUserName, Password = testPassword };

                data.Moders.Add(testModer);
                data.Users.Add(testUser);
                data.SaveChanges();


                if (toolsEntry.SingIn_Back(testUserName, testPassword, ref testUser))
                {
                    count += 1;
                }   

                if (toolsEntry.SingIn_Back(testModerName, testPassword, ref testUser))
                {
                    count += 1;
                }
            }

            Assert.AreEqual(2, count);
        }

        [Test]
        public void SingUp_Check() // Тест регистрации нового аккаунта
                                   // Testing of registration a new account
        {
            int count = 0;

            using (TestDataContext data = new TestDataContext())
            {
                if (toolsEntry.SingUp_Back(testUserName, testPassword, ref testUser, 0))
                {
                    count += 1;
                }

                if (toolsEntry.SingUp_Back(testUserName, testPassword, ref testUser, 1))
                {
                    count += 1;
                }
            }

            Assert.AreEqual(2, count);
        }
    }
}
