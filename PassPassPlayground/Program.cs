// See https://aka.ms/new-console-template for more information

// Test environment for the library. All code is to be tested here to ensure library is working perfectly before
// creating any GUI.

using System.Diagnostics;
using PassPassLib;

Console.WriteLine("PassPassPlayground");

var dbPassword = "test1test2";
var dbCollection = new DBCollection(0);
dbCollection.Entries.Add(new DbEntry("test123@gmail.com", "secure123password", dbPassword));
var watch = Stopwatch.StartNew();
for (var i = 0; i < 1000000; i++)
{
    dbCollection.Entries[0].DecryptLogin(dbPassword);
    dbCollection.Entries[0].DecryptPassword(dbPassword);
}

watch.Stop();
Console.WriteLine("Decrypted login: " + dbCollection.Entries[0].DecryptLogin(dbPassword));
Console.WriteLine("Decrypted password: " + dbCollection.Entries[0].DecryptPassword(dbPassword));
var elapsedMs = watch.ElapsedMilliseconds;
Console.WriteLine(elapsedMs + "ms for enc/dec loop.");

var testdb = new Database("testdatabase");
testdb.SetDescription("description test");
testdb.Collections.Add(dbCollection);
testdb.ExportToFile("testdb.bin", dbPassword);