namespace ClassLibrary3
{
    public interface IEncryption
    {
        bool CheckPassword(string encryptedPassword, string unencryptedPassword);
    }
}