using FakeItEasy;
using NUnit.Framework;

namespace ClassLibrary3
{
    public class KarlSeguin
    {
        [Test]
        public void ReturnsNullWhenTheUserDoesntExist()
        {
            var store = A.Fake<IDataStore>();

            A.CallTo(() => store.FindOneByNamedQuery(null, null)).WithAnyArguments().Returns(null);
            var user = new UserRepository(store, null).FindByCredentials(null, null);
            Assert.IsNull(user);
        }

        [Test]
        public void ReturnsNullIfThePasswordsDontMatch()
        {
            var store = A.Fake<IDataStore>();
            var encryption = A.Fake<IEncryption>();

            A.CallTo(() => encryption.CheckPassword(null, null)).WithAnyArguments().Returns(false);
            var user = new UserRepository(store, encryption).FindByCredentials(null, null);

            Assert.IsNull(user);
        }

        [Test]
        public void ReturnTheValidUser()
        {
            var store = A.Fake<IDataStore>();
            var encryption = A.Fake<IEncryption>();
            var expected = new User();

            Any.CallTo(store).WithReturnType<User>().Returns(expected);
            Any.CallTo(encryption).WithReturnType<bool>().Returns(true);
            var user = new UserRepository(store, encryption).FindByCredentials(null, null);

            Assert.AreSame(expected, user);
        }

        [Test]
        public void LoadsTheUserFromTheDataStore()
        {
            var store = A.Fake<IDataStore>();
            new UserRepository(store, A.Fake<IEncryption>()).FindByCredentials("Leto", null);

            A.CallTo(() => store.FindOneByNamedQuery("FindUserByUserName", "Leto")).MustHaveHappened();
        }

        [Test]
        public void VerifiesTheSubmittedPasswordAgainstTheStoredOne()
        {
            var store = A.Fake<IDataStore>();
            var encryption = A.Fake<IEncryption>();
            var user = new User {Password = "Ghanima"};

            Any.CallTo(store).WithReturnType<User>().Returns(user);
            new UserRepository(store, encryption).FindByCredentials(null, "Duncan");

            A.CallTo(() => encryption.CheckPassword("Ghanima", "Duncan")).MustHaveHappened();
        }
    }
}