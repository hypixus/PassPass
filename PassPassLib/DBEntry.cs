using Newtonsoft.Json;

namespace PassPassLib;

[JsonObject(MemberSerialization.Fields)]
public class DbEntry
{
    [JsonProperty("Login")] private byte[] _login;

    [JsonProperty("LoginNonce")] private byte[] _loginNonce;

    [JsonProperty("LoginSalt")] private byte[] _loginSalt;

    [JsonProperty("LoginTag")] private byte[] _loginTag;

    [JsonProperty("PassNonce")] private byte[] _passNonce;

    [JsonProperty("PassSalt")] private byte[] _passSalt;

    [JsonProperty("PassTag")] private byte[] _passTag;

    [JsonProperty("Password")] private byte[] _password;

    [JsonProperty("Description")] public string Description;

    private bool _disposed;

    [JsonProperty("Name")] public string Name;

    public DbEntry()
    {
        Name = string.Empty;
        Description = string.Empty;

        _loginNonce = Util.GenerateXCCNonce();
        _loginTag = new byte[1];
        _loginSalt = new byte[1];
        _login = new byte[1];

        _passNonce = Util.GenerateXCCNonce();
        _passTag = new byte[Util.XChaCha20Poly1305TagSizeBytes];
        _passSalt = new byte[1];
        _password = new byte[1];
    }

    public DbEntry(string login, string password, string dbPassword)
    {
        Name = string.Empty;
        Description = string.Empty;
        _loginSalt = Util.GenerateArgon2idSalt();
        _loginNonce = Util.GenerateXCCNonce();
        (_login, _loginTag) =
            Util.EncryptStringXcc(login, Util.Argon2FromPassword(dbPassword, _loginSalt), _loginNonce);
        _passSalt = Util.GenerateArgon2idSalt();
        _passNonce = Util.GenerateXCCNonce();
        (_password, _passTag) =
            Util.EncryptStringXcc(password, Util.Argon2FromPassword(dbPassword, _passSalt), _passNonce);
    }

    public void SetLogin(string newLogin, string dbPassword)
    {
        _loginSalt = Util.GenerateArgon2idSalt();
        _loginNonce = Util.GenerateXCCNonce();
        (_login, _loginTag) =
            Util.EncryptStringXcc(newLogin, Util.Argon2FromPassword(dbPassword, _loginSalt), _loginNonce);
    }

    public void SetPassword(string newPassword, string dbPassword)
    {
        _passSalt = Util.GenerateArgon2idSalt();
        _passNonce = Util.GenerateXCCNonce();
        (_password, _passTag) =
            Util.EncryptStringXcc(newPassword, Util.Argon2FromPassword(dbPassword, _passSalt), _passNonce);
    }

    public string DecryptLogin(string dbPassword)
    {
        return Util.DecryptStringXcc(_login, Util.Argon2FromPassword(dbPassword, _loginSalt), _loginNonce, _loginTag);
    }

    public string DecryptPassword(string dbPassword)
    {
        return Util.DecryptStringXcc(_password, Util.Argon2FromPassword(dbPassword, _passSalt), _passNonce, _passTag);
    }

    public void SecureDispose()
    {
        if (_disposed) throw new ObjectDisposedException("Method was already called.");
        ClearArray(ref _loginNonce);
        ClearArray(ref _loginTag);
        ClearArray(ref _loginSalt);
        ClearArray(ref _login);
        ClearArray(ref _passNonce);
        ClearArray(ref _passTag);
        ClearArray(ref _passSalt);
        ClearArray(ref _password);
        Description = string.Empty;
        Name = string.Empty;
        _disposed = true;
    }

    private static void ClearArray(ref byte[] target)
    {
        for (var i = 0; i < target.Length; i++) target[i] = byte.MinValue;
    }
}