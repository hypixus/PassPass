namespace PassPassLib;

public class DbEntry
{
    public string Name;
    public string Description;
    public byte[] Iv { get; private set; }
    private byte[] _login;
    private byte[] _password;

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

    public void SetPassword(string newPassword, string dbPassword) => _password = EncryptField(newPassword, dbPassword);

    public void SetLogin(string newLogin, string dbPassword) => _login = EncryptField(newLogin, dbPassword);

    public string DecryptPassword(string dbPassword)
        => Util.DecryptStringFromBytes_Aes(_password, dbPassword, Iv);

    public string DecryptLogin(string dbPassword)
        => Util.DecryptStringFromBytes_Aes(_login, dbPassword, Iv);

    private byte[] EncryptField(string toEncrypt, string dbPassword)
        => Util.EncryptStringToBytes_Aes(toEncrypt, dbPassword, Iv);
}