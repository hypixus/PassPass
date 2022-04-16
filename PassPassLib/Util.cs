using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;

namespace PassPassLib;

public static class Util
{
    // Utilities class. All static methods and constants are supposed to be kept here in the final version.
    public const int AesKeySizeBytes = 256 / 8;
    public const int AesIVSizeBytes = 256 / 16;
    public const int ArgonSaltSize = 16;

    /// <summary>
    ///     Encrypts a string with provided key and IV.
    /// </summary>
    /// <param name="plainText">String to encrypt.</param>
    /// <param name="key">Encryption key.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Encrypted array of bytes representing the original string.</returns>
    /// <exception cref="ArgumentNullException">One or more of arguments provided are null.</exception>
    public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        return aes.EncryptCbc(Encoding.UTF8.GetBytes(plainText), iv);
    }

    /// <summary>
    ///     Decrypts an array of bytes using provided key and IV.
    /// </summary>
    /// <param name="cipherText">Encrypted string.</param>
    /// <param name="key">Encryption key.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Decrypted string.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        return Encoding.UTF8.GetString(aes.DecryptCbc(cipherText, iv));
    }

    /// <summary>
    ///     Encrypts a string with provided string and IV.
    /// </summary>
    /// <param name="plainText">String to encrypt.</param>
    /// <param name="dbPassword">Encryption string.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Encrypted array of bytes representing the original string.</returns>
    public static byte[] EncryptStringToBytes_Aes(string plainText, string dbPassword, byte[] salt, byte[] iv)
    {
        return EncryptStringToBytes_Aes(plainText, Argon2FromPassword(dbPassword, salt), iv);
    }


    /// <summary>
    ///     Decrypts an array of bytes using provided key and IV.
    /// </summary>
    /// <param name="cipherText">Encrypted string.</param>
    /// <param name="dbPassword">Encryption key.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Decrypted string.</returns>
    public static string DecryptStringFromBytes_Aes(byte[] cipherText, string dbPassword, byte[] salt, byte[] iv)
    {
        return DecryptStringFromBytes_Aes(cipherText, Argon2FromPassword(dbPassword, salt), iv);
    }

    public static byte[] Argon2FromPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var config = new Argon2Config
        {
            Type = Argon2Type.HybridAddressing, // Argon2id
            Version = Argon2Version.Nineteen,
            TimeCost = 10,
            MemoryCost = 32768, // 32 MB
            Lanes = 4,
            Threads = 4, // sensible minimum for nowadays platforms
            Password = passwordBytes,
            Salt = salt,
            HashLength = AesKeySizeBytes // AES 256 key length
        };
        var argon2A = new Argon2(config);
        var outArray = argon2A.Hash();
        return outArray.Buffer;
    }

    #region TrueRNG

    public static byte[] GenerateIv()
    {
        return RandomNumberGenerator.GetBytes(AesIVSizeBytes);
    }

    public static byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(ArgonSaltSize);
    }

    #endregion
}