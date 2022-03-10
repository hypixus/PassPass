namespace PassPassLib;

public class DBEntry
{
    public string Name;
    public string Description;
    public string Login;
    private string _password;

    public DBEntry()
    {
        Name = string.Empty;
        Description = string.Empty;
        Login = string.Empty;
        _password = string.Empty;
    }

    public DBEntry(string login, string password, string DBpassword)
    {
        Name = string.Empty;
        Description = string.Empty;
        Login = login;
        _password = EncryptPassword(password, DBpassword);
    }

    public void setPassword(string password, string DBpassword) => _password = EncryptPassword(password, DBpassword);

    public string getPassword(string password, string DBpassword)
    {
        string toreturn = _password;
        throw new NotImplementedException();
    }

    private string EncryptPassword(string password, string DBpassword)
    {
        //TODO
        throw new NotImplementedException();
    }
}