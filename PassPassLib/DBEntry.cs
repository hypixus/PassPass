namespace PassPassLib;

public class DbEntry
{
    private byte[] _login;
    private byte[] _password;
    public string Description;
    public string Name;

    public DbEntry()
    {
        Name = string.Empty;
        Description = string.Empty;
        Iv = Util.GenerateIv();
        _login = new byte[256];
        _password = new byte[256];
    }

    public DbEntry(string login, string password, string dbPassword)
    {
        Name = string.Empty;
        Description = string.Empty;
        Iv = Util.GenerateIv();
        _login = EncryptField(login, dbPassword);
        _password = EncryptField(password, dbPassword);
    }

    public byte[] Iv { get; }

    public void SetPassword(string newPassword, string dbPassword)
    {
        _password = EncryptField(newPassword, dbPassword);
    }

    public void SetLogin(string newLogin, string dbPassword)
    {
        _login = EncryptField(newLogin, dbPassword);
    }

    public string DecryptPassword(string dbPassword)
    {
        return Util.DecryptStringFromBytes_Aes(_password, dbPassword, Iv);
    }

    public string DecryptLogin(string dbPassword)
    {
        return Util.DecryptStringFromBytes_Aes(_login, dbPassword, Iv);
    }

    private byte[] EncryptField(string toEncrypt, string dbPassword)
    {
        return Util.EncryptStringToBytes_Aes(toEncrypt, dbPassword, Iv);
    }
}