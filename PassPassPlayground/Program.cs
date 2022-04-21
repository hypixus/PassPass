// Test environment for the library. All code is to be tested here to ensure library is working perfectly before
// creating any GUI.

// define mode of operation.

#define SPEEDTEST

#if CHACHAALT
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using PassPassLib;
using PassPassPlayground;

const string originalString = "We love dancing since this is an incredibly nice way to spend free time, as well as to socialize.";
Console.WriteLine("To encrypt: " + originalString);
var plainText = Encoding.UTF8.GetBytes(originalString);
var salt = Encoding.UTF8.GetBytes("Doc0k3nv2fTpH1AI");
var key = Util.Argon2FromPassword(plainText, salt);
var nonce = Util.GenerateXCCNonce();
byte[] cipherText, tag, cipherText2, tag2, decrypted, decrypted2;
var stopwatch = new Stopwatch();
stopwatch.Start();
(cipherText, tag) = SodiumInteropXCCEncrypt(plainText, key, nonce);
stopwatch.Stop();
Console.WriteLine("Sodium.cursed encryption: " + stopwatch.ElapsedMilliseconds + "ms.");
stopwatch.Reset();
stopwatch.Start();
decrypted = SodiumInteropXCCDecrypt(cipherText, key, nonce, tag);
stopwatch.Stop();
Console.WriteLine("Sodium.cursed decryption: " + stopwatch.ElapsedMilliseconds + "ms.");
Debug.Assert(string.CompareOrdinal(Encoding.UTF8.GetString(decrypted), Encoding.UTF8.GetString(plainText)) == 0);
Console.WriteLine(Encoding.UTF8.GetString(decrypted));
stopwatch.Reset();
stopwatch.Start();
(cipherText2, tag2) = Util.EncryptDataXCC(plainText, key, nonce);
stopwatch.Stop();
Console.WriteLine("NaCl.Core encryption: " + stopwatch.ElapsedMilliseconds + "ms.");
stopwatch.Reset();
stopwatch.Start();
decrypted2 = Util.DecryptDataXCC(cipherText2, key, nonce, tag2);
stopwatch.Stop();
Console.WriteLine("NaCl.Core decryption: " + stopwatch.ElapsedMilliseconds + "ms.");
Debug.Assert(string.CompareOrdinal(Encoding.UTF8.GetString(decrypted2), Encoding.UTF8.GetString(plainText)) == 0);
Console.WriteLine(Encoding.UTF8.GetString(decrypted2));

unsafe (byte[], byte[]) SodiumInteropXCCEncrypt(byte[] plainText, byte[] key, byte[] nonce)
{
    var plainTextSpan = new ReadOnlySpan<byte>(plainText);
    var keySpan = new ReadOnlySpan<byte>(key);
    var nonceSpan = new ReadOnlySpan<byte>(nonce);
    var cipherTextSpan = new Span<byte>(new byte[plainText.Length]);
    var tagSpan = new Span<byte>(new byte[Util.XChaCha20Poly1305TagSizeBytes]);
    fixed (byte* c = cipherTextSpan)
    fixed (byte* m = plainTextSpan)
    fixed (byte* npub = nonceSpan)
    fixed (byte* k = keySpan)
    fixed (byte* mac = tagSpan)
    {
        var error = SodiumInterop.crypto_aead_xchacha20poly1305_ietf_encrypt_detached(
            c, mac, out var maclen_p, m, (ulong) plainText.Length, null, 0, null, npub, k);
        if (error != 0) throw new CryptographicException("Sodium Interop call failed with error code " + error);
        return (cipherTextSpan.ToArray(), tagSpan.ToArray());
    }
}

unsafe byte[] SodiumInteropXCCDecrypt(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag)
{
    var cipherTextSpan = new ReadOnlySpan<byte>(cipherText);
    var keySpan = new ReadOnlySpan<byte>(key);
    var nonceSpan = new ReadOnlySpan<byte>(nonce);
    var tagSpan = new ReadOnlySpan<byte>(tag);
    var plainTextSpan = new Span<byte>(new byte[cipherText.Length]);
    fixed (byte* c = cipherTextSpan)
    fixed (byte* m = plainTextSpan)
    fixed (byte* npub = nonceSpan)
    fixed (byte* mac = tagSpan)
    fixed (byte* k = keySpan)
    {
        var error = SodiumInterop.crypto_aead_xchacha20poly1305_ietf_decrypt_detached(
            m, null, c, (ulong) cipherText.Length, mac, null, 0, npub, k);
        if (error != 0) throw new CryptographicException("Sodium Interop call failed with error code " + error);
        return plainTextSpan.ToArray();
    }
}
#elif ARGON2ALT
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using NSec.Cryptography;
using PassPassLib;
using PassPassPlayground;
using Sodium;

var stopwatch = new Stopwatch();
var passwordBytes = Encoding.UTF8.GetBytes("testPassword123");
var salt = Encoding.UTF8.GetBytes("Doc0k3nv2fTpH1AI");
// expected result at 16th byte = 89
stopwatch.Start();
// test Isopoh
var result = Util.Argon2FromPassword(passwordBytes, salt);
stopwatch.Stop();
Console.WriteLine("Isopoh: " + stopwatch.ElapsedMilliseconds + " ms.");
Console.WriteLine("Isopoh key length: " + result.Length);
stopwatch.Reset();
stopwatch.Start();
//test NSec
var result2 = NSecArgon2(passwordBytes, salt);
stopwatch.Stop();
Console.WriteLine("NSec: " + stopwatch.ElapsedMilliseconds + " ms.");
Console.WriteLine("NSec key length: " + result2.Length);
stopwatch.Reset();
stopwatch.Start();
//test Sodium.core
var result3 = SodiumCoreArgon2(passwordBytes, salt);
stopwatch.Stop();
Console.WriteLine("Sodium.core: " + stopwatch.ElapsedMilliseconds + " ms.");
Console.WriteLine("Sodium.core key length: " + result3.Length);
stopwatch.Reset();
stopwatch.Start();
//test SodiumInterop
var result4 = SodiumInteropArgon2(passwordBytes, salt);
stopwatch.Stop();
Console.WriteLine("SodiumInterop: " + stopwatch.ElapsedMilliseconds + " ms.");
Console.WriteLine("SodiumInterop key length: " + result4.Length);

Debug.Assert(result2[15] == 0x59);
Debug.Assert(result3[15] == 0x59);
Debug.Assert(result4[15] == 0x59);

Console.WriteLine("Result NSec: " + result2[15] / 16 + result2[15] % 16);
Console.WriteLine("Result Sodium.core: " + result3[15] / 16 + result3[15] % 16);
Console.WriteLine("Result Sodium.cursed: " + result4[15] / 16 + result4[15] % 16);

const int opsLimit = 20;
const int memLimit = 65536; // 64 mb
const int resultLength = 16;

byte[] NSecArgon2(byte[] password, byte[] salt)
{
    var argon2Params = new Argon2Parameters
    {
        DegreeOfParallelism = 1, // library limitation. Once allowed, will change to 4.
        MemorySize = memLimit, // 64 MB
        NumberOfPasses = opsLimit // iterations
    };
    var argon2 = new Argon2id(argon2Params);
    var key = argon2.DeriveBytes(new ReadOnlySpan<byte>(password),
        new ReadOnlySpan<byte>(salt), resultLength);
    return key;
}

byte[] SodiumCoreArgon2(byte[] password, byte[] salt)
{
    return PasswordHash.ArgonHashBinary(password, salt, opsLimit, memLimit * 1024, resultLength,
        PasswordHash.ArgonAlgorithm.Argon_2ID13);
}


unsafe byte[] SodiumInteropArgon2(byte[] password, byte[] salt)
{
    var output = new Span<byte>(new byte[Util.XChaCha20Poly1305KeySizeBytes]);
    var passwordSpan = new ReadOnlySpan<byte>(password);
    var saltSpan = new ReadOnlySpan<byte>(salt);
    fixed (byte* outPtr = output)
    fixed (byte* saltPtr = saltSpan)
    fixed (byte* inputPtr = passwordSpan)
    {
        var error = SodiumInterop.crypto_pwhash_argon2id(
            outPtr, resultLength, (sbyte*) inputPtr, (ulong) password.Length,
            saltPtr, opsLimit, memLimit * 1024, SodiumInterop.crypto_pwhash_argon2id_ALG_ARGON2ID13);
        if (error != 0) throw new CryptographicException("Sodium Interop call failed with error code " + error);
    }

    return output.ToArray();
}

#elif CHACHATEST
using PassPassLib;
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
#endif
if (Environment.OSVersion.Platform == PlatformID.Win32NT) Console.ReadKey();