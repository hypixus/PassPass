using Newtonsoft.Json;

namespace PassPassLib;

[JsonObject(MemberSerialization.Fields)]
public class DbEntry
{
    [JsonProperty("Login")] private byte[] _login;

    [JsonProperty("LoginNonce")] private byte[] _loginNonce;

    [JsonProperty("LoginTag")] private byte[] _loginTag;
    
    [JsonProperty("Password")] private byte[] _password;

    [JsonProperty("PassNonce")] private byte[] _passNonce;

    [JsonProperty("PassTag")] private byte[] _passTag;

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
        _login = new byte[256];
        _password = new byte[256];
    }

    public DbEntry(string login, string password, string dbPassword)
    {
        Name = string.Empty;
        Description = string.Empty;
        _loginNonce = Util.GenerateXCCNonce();
        _loginTag = new byte[Util.XChaCha20Poly1305TagSizeBytes];
        _passNonce = Util.GenerateXCCNonce();
        _passTag = new byte[Util.XChaCha20Poly1305TagSizeBytes];
        _salt = Util.GenerateArgon2idSalt();
        SetLogin(login, dbPassword);
        SetPassword(password, dbPassword);
    }

    public void SetPassword(string newPassword, string dbPassword)
    {
        _passNonce = Util.GenerateXCCNonce();
        var (encrypted, tag) =
            Util.EncryptStringXCC(newPassword, Util.Argon2FromPassword(dbPassword, _salt), _passNonce);
        _password = encrypted;
        _passTag = tag;
    }

    public void SetLogin(string newLogin, string dbPassword)
    {
        _loginNonce = Util.GenerateXCCNonce();
        var (encrypted, tag) =
            Util.EncryptStringXCC(newLogin, Util.Argon2FromPassword(dbPassword, _salt), _loginNonce);
        _login = encrypted;
        _loginTag = tag;
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