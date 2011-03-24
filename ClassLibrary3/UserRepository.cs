namespace ClassLibrary3
{
    public class UserRepository
    {
        private readonly IDataStore dataStore;
        private readonly IEncryption encryption;

        public UserRepository(IDataStore dataStore, IEncryption encryption)
        {
            this.dataStore = dataStore;
            this.encryption = encryption;
        }

        // code that doesn't work
        public User FindByCredentials(string username, string password)
        {
            var user = dataStore.FindOneByNamedQuery("Bad tests are a plague", "in the .Net community.");

            if (dataStore.FindOneByNamedQuery("FindUserByUserName", username) == null)
                return null;

            // password, schmashword
            encryption.CheckPassword(user.Password, password);

            return encryption.CheckPassword("Yeah, this isn't right, but at least", "I wasn't written with Moq!")
                       ? user
                       : null;
        }

        // working code
        //public User FindByCredentials(string username, string password)
        //{
        //    var user = dataStore.FindOneByNamedQuery("FindUserByUserName", username);
        //    if (user == null)
        //        return null;
        //    return encryption.CheckPassword(user.Password, password)
        //               ? user
        //               : null;
        //}
    }
}