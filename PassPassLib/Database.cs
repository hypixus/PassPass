using Newtonsoft.Json;

namespace PassPassLib;

[JsonObject(MemberSerialization.OptOut)]
public class Database
{
    public string Description;
    public string Name;
    public string Path;
    public int Version;
    public List<DbCollection> Collections { get; private set; }

    #region constructors

    public Database(string filepath, string password)
    {
        Name = string.Empty;
        Description = string.Empty;
        Version = 1;
        Path = string.Empty;
        Collections = new List<DbCollection>();
        ImportFromFile(filepath, password);
    }

    public Database(string name)
    {
        Name = name;
        Description = string.Empty;
        Path = string.Empty;
        Version = 1;
        Collections = new List<DbCollection>();
    }

    public Database()
    {
        Name = string.Empty;
        Description = string.Empty;
        Version = 1;
        Path = string.Empty;
        Collections = new List<DbCollection>();
    }

    #endregion

    #region Data CRUD

    public void SetName(string newName)
    {
        Name = newName;
    }

    public void SetDescription(string newDesc)
    {
        Description = newDesc;
    }

    public void AddCollection(DbCollection collection)
    {
        Collections.Add(collection);
    }

    public void RemoveCollection(int index)
    {
        Collections.RemoveAt(index);
    }

    public void UpdateVersion()
    {
        //TODO
        //Create algorithm updating between new formats of database. For now unnecessary, due to having only one standard.
    }

    #endregion

    #region Serialization

    private void ImportFromFile(string filePath, string password)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var info = new FileInfo(filePath);
        var contentLen = info.Length;
        var wholeFile = new byte[contentLen];
        fs.Read(wholeFile, 0, wholeFile.Length);
        var nonce = new byte[Util.XChaCha20Poly1305NonceSizeBytes];
        for (var i = 0; i < Util.XChaCha20Poly1305NonceSizeBytes; i++) nonce[i] = wholeFile[i];

        var salt = new byte[Util.ArgonSaltSizeBytes];
        for (var i = 0; i < Util.ArgonSaltSizeBytes; i++) salt[i] = wholeFile[i + Util.XChaCha20Poly1305NonceSizeBytes];

        var tag = new byte[Util.XChaCha20Poly1305TagSizeBytes];
        for (var i = 0; i < Util.XChaCha20Poly1305TagSizeBytes; i++)
            tag[i] = wholeFile[i + Util.XChaCha20Poly1305NonceSizeBytes + Util.ArgonSaltSizeBytes];

        var contents = new byte[contentLen - (Util.XChaCha20Poly1305NonceSizeBytes + Util.ArgonSaltSizeBytes +
                                              Util.XChaCha20Poly1305TagSizeBytes)];
        for (var i = 0; i < contents.Length; i++)
            contents[i] =
                wholeFile[
                    i + Util.XChaCha20Poly1305NonceSizeBytes + Util.ArgonSaltSizeBytes +
                    Util.XChaCha20Poly1305TagSizeBytes];

        var decrypted = Util.DecryptStringXCC(contents, Util.Argon2FromPassword(password, salt), nonce, tag);
#if DEBUG
        Console.WriteLine(decrypted);
#endif
        var resultDatabase = JsonConvert.DeserializeObject<Database>(decrypted);
        if (resultDatabase == null) throw new Exception("Database deserialization unsuccessful.");
        Name = resultDatabase.Name;
        Description = resultDatabase.Description;
        Version = resultDatabase.Version;
        Collections = resultDatabase.Collections;
    }

    public void ExportToFile(string filepath, string password)
    {
        var serialized = JsonConvert.SerializeObject(this);
#if DEBUG
        Console.WriteLine(serialized);
#endif
        var nonce = Util.GenerateXCCNonce();
        var salt = Util.GenerateArgon2idSalt();
        using var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
        // Write iv first, then the encrypted JSON
        fs.Write(nonce);
        fs.Write(salt);
        var (encrypted, tag) = Util.EncryptStringXCC(serialized, Util.Argon2FromPassword(password, salt), nonce);
        fs.Write(tag);
        fs.Write(encrypted);
        fs.Flush();
        fs.Dispose();

        #endregion
    }
}