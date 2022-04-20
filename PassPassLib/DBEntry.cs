using Newtonsoft.Json;

namespace PassPassLib;

[JsonObject(MemberSerialization.Fields)]
public class DbEntry
{

    [JsonProperty("LoginNonce")] private byte[] _loginNonce;

    [JsonProperty("LoginTag")] private byte[] _loginTag;

    [JsonProperty("LoginSalt")] private byte[] _loginSalt;

    [JsonProperty("Login")] private byte[] _login;

    [JsonProperty("PassNonce")] private byte[] _passNonce;

    [JsonProperty("PassTag")] private byte[] _passTag;

    [JsonProperty("PassSalt")] private byte[] _passSalt;

    [JsonProperty("Password")] private byte[] _password;

    [JsonProperty("Description")] public string Description;

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
        (_login, _loginTag) = Util.EncryptStringXCC(login, Util.Argon2FromPassword(dbPassword, _loginSalt), _loginNonce);
        _passSalt = Util.GenerateArgon2idSalt();
        _passNonce = Util.GenerateXCCNonce();
        (_password, _passTag) = Util.EncryptStringXCC(password, Util.Argon2FromPassword(dbPassword, _passSalt), _passNonce);
    }

    public void SetLogin(string newLogin, string dbPassword)
    {
        _loginSalt = Util.GenerateArgon2idSalt();
        _loginNonce = Util.GenerateXCCNonce();
        (_login, _loginTag) = Util.EncryptStringXCC(newLogin, Util.Argon2FromPassword(dbPassword, _loginSalt), _loginNonce);
    }

    public void SetPassword(string newPassword, string dbPassword)
    {
        _passSalt = Util.GenerateArgon2idSalt();
        _passNonce = Util.GenerateXCCNonce();
        (_password, _passTag) = Util.EncryptStringXCC(newPassword, Util.Argon2FromPassword(dbPassword, _passSalt), _passNonce);
    }

    public string DecryptLogin(string dbPassword)
    {
        return Util.DecryptStringXCC(_login, Util.Argon2FromPassword(dbPassword, _loginSalt), _loginNonce, _loginTag);
    }

    public string DecryptPassword(string dbPassword)
    {
        return Util.DecryptStringXCC(_password, Util.Argon2FromPassword(dbPassword, _passSalt), _passNonce, _passTag);
    }

    private bool disposed;

    public void ForceClearArrays()
    {
        if (disposed) throw new ObjectDisposedException("Method was already called.");
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
        disposed = true;
    }

    private void ClearArray(ref byte[] target)
    {
        for (var i = 0; i < target.Length; i++)
        {
            target[i] = byte.MinValue;
        }
    }

}