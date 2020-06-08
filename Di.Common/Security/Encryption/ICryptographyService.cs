namespace Di.Common.Security.Encryption
{
    public interface ICryptographyService
    {
        string EncryptString(string value, string key, string iv);
        string DecryptString(string value, string key, string iv);
    }
}