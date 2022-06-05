using PassPassLib;

// ReSharper disable StringLiteralTypo

namespace PassPassLibTests;

[TestFixture]
public class PassPassTests
{
    [SetUp]
    public void Setup()
    {
        Console.WriteLine("Initializing tests.");
        testCollection = new DbCollection(0);
        testCollection.Entries.Add(new DbEntry(DbEntryLogin, DbEntryPassword, DbPassword));
        testDatabase = new Database("testDatabase");
        testDatabase.AddCollection(testCollection);
    }

    private DbCollection testCollection;
    private Database testDatabase;
    private const string DbPassword = "test1test2";
    private const string DbEntryLogin = "test123@gmail.com";
    private const string DbEntryPassword = "secure123password";

    [Test]
    [Timeout(10000)]
    public void TestEntryEncryptionSpeed()
    {
        var dbEntry = new DbEntry("test123@gmail.com", "secure123password", DbPassword);
        testCollection.Entries.Add(dbEntry);
        Assert.That(dbEntry, Is.EqualTo(testCollection.Entries[1]));
    }

    [Test]
    [Timeout(10000)]
    public void TestEntryDecryptionSpeed()
    {
        var login = testCollection.Entries[0].DecryptLogin(DbPassword);
        var password = testCollection.Entries[0].DecryptPassword(DbPassword);
        Assert.That(login, Is.EqualTo(DbEntryLogin));
        Assert.That(password, Is.EqualTo(DbEntryPassword));
    }

    [Test]
    [Timeout(10000)]
    [TestCase("piraci@karaiby.pl", "poszedłkoteknapłotek")]
    [TestCase("dzban@glinaipartnerzy.com", "definitywnie123skomplikowane")]
    [TestCase("Placek&&!!225", "bardzo$%^&*()#dobreAAAAAsło")]
    public void TestEncryptionDecryption_IfCorrect(string login, string password)
    {
        var testEntry = new DbEntry(login, password, DbPassword);
        Assert.That(login, Is.EqualTo(testEntry.DecryptLogin(DbPassword)));
        Assert.That(password, Is.EqualTo(testEntry.DecryptPassword(DbPassword)));
    }

    [Test]
    [Timeout(20000)]
    [Repeat(3)]
    [TestCase("testdb.bin")]
    public void TestDatabaseSavingAndReading(string filepath)
    {
        testDatabase.ExportToFile(filepath, DbPassword);
        var readDb = new Database(filepath, DbPassword);
        Assert.That(readDb.Collections[0].Entries[0].DecryptPassword(DbPassword), Is.EqualTo(DbEntryPassword));
    }

    [Test]
    [Timeout(10000)]
    [Repeat(3)]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    [TestCase(@"C:\testdb.bin")]
    public void TestDatabaseSavingAndReading_ThrowsNoAccess(string filepath)
    {
        testDatabase.ExportToFile(filepath, DbPassword);
        _ = new Database(filepath, DbPassword);
    }

    [Test]
    [Timeout(5000)]
    public void TestSecureDisposing_Database()
    {
        testDatabase.SecureDispose();
        Assert.That(testDatabase.Collections.Count, Is.EqualTo(0));
        Assert.That(testDatabase.Name, Is.EqualTo(string.Empty));
        Assert.That(testDatabase.Description, Is.EqualTo(string.Empty));
        Assert.That(testDatabase.Path, Is.EqualTo(string.Empty));
        Assert.That(testDatabase.Version, Is.EqualTo(-1));
    }

    [Test]
    [Timeout(5000)]
    public void TestSecureDisposing_Collection()
    {
        testCollection.SecureDispose();
        Assert.That(testCollection.Entries.Count, Is.EqualTo(0));
        Assert.That(testCollection.Description, Is.EqualTo(string.Empty));
        Assert.That(testCollection.Name, Is.EqualTo(string.Empty));
        Assert.That(testCollection.CollectionId, Is.EqualTo(-1));
    }
}