namespace PassPassLib;

public class DBCollection
{
    public string Name;
    public string Description;
    public int CollectionID;
    public List<DBEntry> Entries;

    public DBCollection(int collectionID)
    {
        Name = string.Empty;
        Description = string.Empty;
        CollectionID = collectionID;
    }
}