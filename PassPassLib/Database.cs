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

    public void SecureDispose()
    {
        Description = string.Empty;
        Name = string.Empty;
        Path = string.Empty;
        Version = -1;
        foreach (var collection in Collections) collection.SecureDispose();
    }

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
        var nonce = new byte[Util.XChaCha20Poly1305NonceSizeBytes];
        var salt = new byte[Util.ArgonSaltSizeBytes];
        var tag = new byte[Util.XChaCha20Poly1305TagSizeBytes];
        var contents = new byte[
            fs.Length - Util.XChaCha20Poly1305NonceSizeBytes - Util.ArgonSaltSizeBytes -
            Util.XChaCha20Poly1305TagSizeBytes];
        if (fs.Read(nonce) != Util.XChaCha20Poly1305NonceSizeBytes
            || fs.Read(salt) != Util.ArgonSaltSizeBytes
            || fs.Read(tag) != Util.XChaCha20Poly1305TagSizeBytes
            || fs.Read(contents) != contents.Length
           )
            throw new IOException("Database file is shorter than expected.");
        fs.Dispose();
        // Do not keep decrypted data in memory unless necessary for debugging purposes.
#if DEBUG
        var decrypted = Util.DecryptStringXcc(contents, Util.Argon2FromPassword(password, salt), nonce, tag);
        Console.WriteLine(decrypted);
        var resultDatabase = JsonConvert.DeserializeObject<Database>(decrypted);
#else
        var resultDatabase =
            JsonConvert.DeserializeObject<Database>(Util.DecryptStringXcc(contents,
                Util.Argon2FromPassword(password, salt), nonce, tag));
#endif
        if (resultDatabase == null) throw new Exception("Database deserialization unsuccessful.");
        Name = resultDatabase.Name;
        Description = resultDatabase.Description;
        Version = resultDatabase.Version;
        Collections = resultDatabase.Collections;
    }

    public void ExportToFile(string filepath, string password)
    {
        var nonce = Util.GenerateXCCNonce();
        var salt = Util.GenerateArgon2idSalt();
        using var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
#if DEBUG
        var serialized = JsonConvert.SerializeObject(this);
        Console.WriteLine(serialized);
        var (encrypted, tag) = Util.EncryptStringXcc(serialized, Util.Argon2FromPassword(password, salt), nonce);
#else
        var (encrypted, tag) =
            Util.EncryptStringXcc(JsonConvert.SerializeObject(this), Util.Argon2FromPassword(password, salt), nonce);
#endif
        // Write in that order - nonce, salt, tag, encrypted.
        fs.Write(nonce);
        fs.Write(salt);
        fs.Write(tag);
        fs.Write(encrypted);
        fs.Flush();
        fs.Dispose();
    }

    #endregion
}