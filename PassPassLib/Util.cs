using System.Security.Cryptography;
using System.Text;

namespace PassPassLib;

public static class Util
{
    // Utilities class. All static methods and constants are supposed to be kept here in the final version.
    public const int AesKeySize = 256 / 8;
    public const int AesIVSize = 256 / 16;

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
    public static byte[] EncryptStringToBytes_Aes(string plainText, string dbPassword, byte[] iv)
        => EncryptStringToBytes_Aes(plainText, GenerateKeyFromString(dbPassword), iv);
    

    /// <summary>
    ///     Decrypts an array of bytes using provided key and IV.
    /// </summary>
    /// <param name="cipherText">Encrypted string.</param>
    /// <param name="dbPassword">Encryption key.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Decrypted string.</returns>
    public static string DecryptStringFromBytes_Aes(byte[] cipherText, string dbPassword, byte[] iv)
        => DecryptStringFromBytes_Aes(cipherText, GenerateKeyFromString(dbPassword), iv);

    /// <summary>
    ///     Generates AES key from a UTF8 string.
    /// </summary>
    /// <param name="password">String to create byte array from. Notice if size is exceeded, spare bytes are omitted.</param>
    /// <returns>Byte representation of the string provided.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static byte[] GenerateKeyFromString(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        if (passwordBytes.Length is > AesKeySize or 0) throw new ArgumentOutOfRangeException(nameof(password));
        var keyBytes = new byte[AesKeySize];
        var passLen = passwordBytes.Length;
        // generates key byte array by repeating bytes of the key
        for (var i = 0; i < AesKeySize; i++) keyBytes[i] = passwordBytes[i % passLen];

        return keyBytes;
    }

    /// <summary>
    ///     Generate a random initialization vector.
    /// </summary>
    /// <returns>Randomized initialization vector.</returns>
    public static byte[] GenerateIv()
    {
        using var aes = Aes.Create();
        aes.GenerateIV();
        return aes.IV;
    }
}