using System.Text;
using System.Text.Json;

namespace PassPassLib;

[Serializable]
public class Database
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Version { get; private set; }
    public string Path { get; }
    public List<DBCollection> Collections { get; private set; }

    #region constructors

    public Database(string filepath, string password)
    {
        //TODO
        ImportFromFile(filepath, password);
    }

    public Database(string name)
    {
        Name = name;
        Description = string.Empty;
        Path = string.Empty;
        Version = 1;
        Collections = new List<DBCollection>();
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

    public void AddCollection(DBCollection collection)
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

    private const int IVSize = 16;
    private const int KeySize = 32;

    private void ImportFromFile(string filePath, string password)
    {
        //TODO
        // test if it actually works

        // Sizes for AES-256
        // IV = 16 bytes
        // Key = 32 bytes

        try
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // read IV from first bytes of file.
            var iv = new byte[IVSize];
            fs.Read(iv, 0, IVSize);
            // ingest rest of data in the file.
            var toDecrypt = new byte[fs.Length - IVSize];
            fs.Read(toDecrypt, IVSize, toDecrypt.Length);
            // attempt decryption of JSON structure into a string.
            var decrypted = Util.DecryptStringFromBytes_Aes(toDecrypt, password, iv);

            var resultDatabase =
                JsonSerializer.Deserialize<Database>(new MemoryStream(Encoding.UTF8.GetBytes(decrypted)));

            Name = resultDatabase.Name;
            Description = resultDatabase.Description;
            Version = resultDatabase.Version;
            Collections = resultDatabase.Collections;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
#if DEBUG
            throw;
#endif
        }
    }

    public void ExportToFile(string filepath, string password)
    {
        try
        {
            //TODO
            // test if it actually works
            var serialized = JsonSerializer.Serialize(this);
            var iv = Util.GenerateIv();
            using var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
            // Write first 16 bytes of iv, then the encrypted JSON
            fs.Write(iv);
            fs.Write(Encoding.UTF8.GetBytes(serialized));
            fs.Flush();
            fs.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
#if DEBUG
            throw;
#endif
        }

        #endregion
    }
}