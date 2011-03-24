namespace ClassLibrary3
{
    public interface IDataStore
    {
        User FindOneByNamedQuery(string queryName, string username);
    }
}