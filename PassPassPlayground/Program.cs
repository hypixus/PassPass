// See https://aka.ms/new-console-template for more information

using PassPassLib;

Console.WriteLine("PassPassPlayground");

var dbPassword = "test1test2";
var dbCollection = new DBCollection(0);
dbCollection.Entries.Add(new DbEntry("test123@gmail.com", "secure123password", dbPassword));
Console.WriteLine("Decrypted login: " + dbCollection.Entries[0].DecryptLogin(dbPassword));
Console.WriteLine("Decrypted password: " + dbCollection.Entries[0].DecryptPassword(dbPassword));

Console.ReadKey();