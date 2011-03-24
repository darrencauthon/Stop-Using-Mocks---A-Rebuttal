using AutoMoq;
using Moq;
using NUnit.Framework;
using Should;

namespace ClassLibrary3
{
    public class DarrenCauthon
    {
        private AutoMoqer mocker;

        [SetUp]
        public void Setup()
        {
            mocker = new AutoMoqer();

            mocker.GetMock<IEncryption>()
                .Setup(x => x.CheckPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
        }

        [Test]
        public void Returns_a_user_when_the_username_and_password_are_valid()
        {
            var expectedUser = new User { Password = "My Encrypted Password" };

            ThisUsernameReturnsThisUser("Username", expectedUser);
            ThisEncryptedAndDecryptedPasswordWillMatch("My Encrypted Password", "Password");

            var result = GetTheUser("Username", "Password");

            result.ShouldBeSameAs(expectedUser);
        }

        [Test]
        public void Returns_null_when_the_password_does_not_match()
        {
            var expectedUser = new User { Password = "another encrypted password" };

            ThisUsernameReturnsThisUser("un", expectedUser);
            ThisEncryptedAndDecryptedPasswordDoNotMatch("another encrypted password", "pw");

            var result = GetTheUser("un", "pw");

            result.ShouldBeNull();
        }

        [Test]
        public void Returns_null_when_the_username_is_not_valid()
        {
            ThisUsernameReturnsThisUser("I don't exist.", null);

            var result = GetTheUser("I don't exist.", "x");

            result.ShouldBeNull();
        }

        private void ThisEncryptedAndDecryptedPasswordDoNotMatch(string encryptedPassword, string decryptedPassword)
        {
            mocker.GetMock<IEncryption>()
                .Setup(x => x.CheckPassword(encryptedPassword, decryptedPassword))
                .Returns(false);
        }

        private User GetTheUser(string username, string password)
        {
            var userRepository = mocker.Resolve<UserRepository>();
            return userRepository.FindByCredentials(username, password);
        }

        private void ThisUsernameReturnsThisUser(string username, User user)
        {
            mocker.GetMock<IDataStore>()
                .Setup(x => x.FindOneByNamedQuery("FindUserByUserName", username))
                .Returns(user);
        }

        private void ThisEncryptedAndDecryptedPasswordWillMatch(string encryptedPassword, string decryptedPassword)
        {
            mocker.GetMock<IEncryption>()
                .Setup(x => x.CheckPassword(encryptedPassword, decryptedPassword))
                .Returns(true);
        }
    }
}