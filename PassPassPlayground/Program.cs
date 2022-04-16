// See https://aka.ms/new-console-template for more information

// Test environment for the library. All code is to be tested here to ensure library is working perfectly before
// creating any GUI.

using System.Diagnostics;
using PassPassLib;

Console.WriteLine("PassPassPlayground");
var stopwatch = new Stopwatch();
var dbPassword = "test1test2";
var dbCollection = new DbCollection(0);
Console.WriteLine("Adding and encrypting example entry.");
stopwatch.Start();
dbCollection.Entries.Add(new DbEntry("test123@gmail.com", "secure123password", dbPassword));
stopwatch.Stop();
Console.WriteLine("Operation took " + stopwatch.ElapsedMilliseconds + " ms.");
stopwatch.Reset();
Console.WriteLine("Decrypting back...");
stopwatch.Start();
Console.WriteLine("Login: " + dbCollection.Entries[0].DecryptLogin(dbPassword));
Console.WriteLine("Password: " + dbCollection.Entries[0].DecryptPassword(dbPassword));
stopwatch.Stop();
Console.WriteLine("Operation took " + stopwatch.ElapsedMilliseconds + " ms.");
stopwatch.Reset();
Console.WriteLine("Creating example database...");
var testdb = new Database("testdatabase");
testdb.SetDescription("description test");
testdb.Collections.Add(dbCollection);
Console.WriteLine("Exporting database to file...");
stopwatch.Start();
testdb.ExportToFile("testdb.bin", dbPassword);
stopwatch.Stop();
Console.WriteLine("Operation took " + stopwatch.ElapsedMilliseconds + " ms.");
stopwatch.Reset();
Console.WriteLine("Importing database from file...");
stopwatch.Start();
var filedb = new Database("testdb.bin", dbPassword);
stopwatch.Stop();
Console.WriteLine("Operation took " + stopwatch.ElapsedMilliseconds + " ms.");
stopwatch.Reset();
Console.WriteLine("DB Name: " + filedb.Name);
Console.WriteLine("DB Desc: " + filedb.Description);
Console.WriteLine("Login: " + filedb.Collections[0].Entries[0].DecryptLogin(dbPassword));
Console.WriteLine("Password: " + filedb.Collections[0].Entries[0].DecryptPassword(dbPassword));
Console.ReadKey();