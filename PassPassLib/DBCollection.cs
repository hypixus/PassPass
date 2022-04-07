namespace PassPassLib;

public class DBCollection
{
    public int CollectionId;
    public string Description;
    public List<DbEntry> Entries;
    public string Name;

    public DBCollection(int collectionId)
    {
        Name = string.Empty;
        Description = string.Empty;
        CollectionId = collectionId;
        Entries = new List<DbEntry>();
    }
}