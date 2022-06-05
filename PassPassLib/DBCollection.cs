using Newtonsoft.Json;

namespace PassPassLib;

[JsonObject(MemberSerialization.OptOut)]
public class DbCollection
{
    [JsonProperty("ColID")] public int CollectionId;

    [JsonProperty("Description")] public string Description;

    [JsonProperty("Entries")] public List<DbEntry> Entries;

    [JsonProperty("Name")] public string Name;

    public DbCollection(int collectionId)
    {
        Name = string.Empty;
        Description = string.Empty;
        CollectionId = collectionId;
        Entries = new List<DbEntry>();
    }

    public DbCollection()
    {
        Name = string.Empty;
        Description = string.Empty;
        CollectionId = 0;
        Entries = new List<DbEntry>();
    }

    public void SecureDispose()
    {
        CollectionId = -1;
        Description = string.Empty;
        Name = string.Empty;
        foreach (var entry in Entries) entry.SecureDispose();
        Entries = new List<DbEntry>();
    }
}