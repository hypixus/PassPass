// See https://aka.ms/new-console-template for more information

// Test environment for the library. All code is to be tested here to ensure library is working perfectly before
// creating any GUI.

using System.Diagnostics;
using PassPassLib;

Console.WriteLine("PassPassPlayground");

var dbPassword = "test1test2";
var dbCollection = new DbCollection(0);
dbCollection.Entries.Add(new DbEntry("test123@gmail.com", "secure123password", dbPassword));
Console.WriteLine("Login: " + dbCollection.Entries[0].DecryptLogin(dbPassword));
Console.WriteLine("Password: " + dbCollection.Entries[0].DecryptPassword(dbPassword));
var testdb = new Database("testdatabase");
testdb.SetDescription("description test");
testdb.Collections.Add(dbCollection);
testdb.ExportToFile("testdb.bin", dbPassword);
var filedb = new Database("testdb.bin", dbPassword);
Console.WriteLine(filedb.Name);
Console.WriteLine(filedb.Description);
Console.WriteLine("Login: " + filedb.Collections[0].Entries[0].DecryptLogin(dbPassword));
Console.WriteLine("Password: " + filedb.Collections[0].Entries[0].DecryptPassword(dbPassword));