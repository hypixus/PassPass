// Test environment for the library. All code is to be tested here to ensure library is working perfectly before
// creating any GUI.

// define mode of operation.

#define SPEEDTEST

using System.Diagnostics;
using PassPassLib;

#if CHACHA
var login = "test123@gmail.com";
var password = "secure123password";
var keypassword = "12extremelySecurePwd34";
var argonSalt = Util.GenerateArgon2idSalt();
var chachaSalt = Util.GenerateXCCNonce();
byte[] tag, result;
(result, tag) = Util.EncryptStringXCC(login, Util.Argon2FromPassword(keypassword, argonSalt), chachaSalt);
var original = Util.DecryptDataXCC(result, Util.Argon2FromPassword(keypassword, argonSalt), chachaSalt, tag);
Console.WriteLine(original);
#elif SPEEDTEST
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
#endif
Console.ReadKey();