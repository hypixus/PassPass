using Newtonsoft.Json;

namespace PassPassLib;

[JsonObject(MemberSerialization.Fields)]
public class DbEntry
{

    [JsonProperty("LoginNonce")] private byte[] _loginNonce;

    [JsonProperty("LoginTag")] private byte[] _loginTag;

    [JsonProperty("Login")] private byte[] _login;

    [JsonProperty("PassNonce")] private byte[] _passNonce;

    [JsonProperty("PassTag")] private byte[] _passTag;

    [JsonProperty("Password")] private byte[] _password;

    [JsonProperty("Salt")] private byte[] _salt;

    [JsonProperty("Description")] public string Description;

    [JsonProperty("Name")] public string Name;

    public DbEntry()
    {
        Name = string.Empty;
        Description = string.Empty;
        _loginNonce = Util.GenerateXCCNonce();
        _loginTag = new byte[Util.XChaCha20Poly1305TagSizeBytes];
        _passNonce = Util.GenerateXCCNonce();
        _passTag = new byte[Util.XChaCha20Poly1305TagSizeBytes];
        _salt = Util.GenerateArgon2idSalt();
        _login = new byte[1];
        _password = new byte[1];
    }

    public DbEntry(string login, string password, string dbPassword)
    {
        Name = string.Empty;
        Description = string.Empty;
        _salt = Util.GenerateArgon2idSalt();
        _loginNonce = Util.GenerateXCCNonce();
        (_login, _loginTag) = Util.EncryptStringXCC(login, Util.Argon2FromPassword(dbPassword, _salt), _loginNonce);
        _passNonce = Util.GenerateXCCNonce();
        (_password, _passTag) = Util.EncryptStringXCC(password, Util.Argon2FromPassword(dbPassword, _salt), _passNonce);
    }

    public void SetPassword(string newPassword, string dbPassword)
    {
        _passNonce = Util.GenerateXCCNonce();
        (_password, _passTag) = Util.EncryptStringXCC(newPassword, Util.Argon2FromPassword(dbPassword, _salt), _passNonce);
    }

    public void SetLogin(string newLogin, string dbPassword)
    {
        _loginNonce = Util.GenerateXCCNonce();
        (_login, _loginTag) = Util.EncryptStringXCC(newLogin, Util.Argon2FromPassword(dbPassword, _salt), _loginNonce);
    }

    public string DecryptPassword(string dbPassword)
    {
        return Util.DecryptStringXCC(_password, Util.Argon2FromPassword(dbPassword, _salt), _passNonce, _passTag);
    }

    public string DecryptLogin(string dbPassword)
    {
        return Util.DecryptStringXCC(_login, Util.Argon2FromPassword(dbPassword, _salt), _loginNonce, _loginTag);
    }
}