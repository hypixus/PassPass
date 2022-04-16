using Newtonsoft.Json;

namespace PassPassLib;

[JsonObject(MemberSerialization.Fields)]
public class DbEntry
{
    [JsonProperty("IV")] private byte[] _iv;

    [JsonProperty("Login")] private byte[] _login;

    [JsonProperty("Password")] private byte[] _password;

    [JsonProperty("Salt")] private byte[] _salt;

    [JsonProperty("Description")] public string Description;

    [JsonProperty("Name")] public string Name;

    public DbEntry()
    {
        Name = string.Empty;
        Description = string.Empty;
        _iv = Util.GenerateIv();
        _salt = Util.GenerateSalt();
        _login = new byte[256];
        _password = new byte[256];
    }

    public DbEntry(string login, string password, string dbPassword)
    {
        Name = string.Empty;
        Description = string.Empty;
        _iv = Util.GenerateIv();
        _salt = Util.GenerateSalt();
        _login = EncryptField(login, dbPassword);
        _password = EncryptField(password, dbPassword);
    }


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
        return Util.DecryptStringFromBytes_Aes(_password, dbPassword, _salt, _iv);
    }

    public string DecryptLogin(string dbPassword)
    {
        return Util.DecryptStringFromBytes_Aes(_login, dbPassword, _salt, _iv);
    }

    private byte[] EncryptField(string toEncrypt, string dbPassword)
    {
        return Util.EncryptStringToBytes_Aes(toEncrypt, dbPassword, _salt, _iv);
    }
}