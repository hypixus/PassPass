namespace PassPassLib;

public class DBCollection
{
    public string Name;
    public string Description;
    public int CollectionId;
    public List<DbEntry> Entries;

    public DBCollection(int collectionId)
    {
        Name = string.Empty;
        Description = string.Empty;
        CollectionId = collectionId;
        Entries = new List<DbEntry>();
    }
}