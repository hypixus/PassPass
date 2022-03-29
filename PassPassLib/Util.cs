using System.Security.Cryptography;
using System.Text;

namespace PassPassLib;

public static class Util
{
    private const int AesKeySize = 256 / 8;

    /// <summary>
    /// Encrypts a string with provided key and IV.
    /// </summary>
    /// <param name="plainText">String to encrypt.</param>
    /// <param name="key">Encryption key.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Encrypted array of bytes representing the original string.</returns>
    /// <exception cref="ArgumentNullException">One or more of arguments provided are null.</exception>
    public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] iv)
    {
        // Check arguments.
        if (plainText is not {Length: > 0})
            throw new ArgumentNullException(nameof(plainText));
        if (key is not {Length: > 0})
            throw new ArgumentNullException(nameof(key));
        if (iv is not {Length: > 0})
            throw new ArgumentNullException(nameof(iv));

        // Create an Aes instance with data provided.
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;

        // Create an encryptor to perform the stream transform.
        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


        byte[] encrypted;
        // Create the streams used for encryption.
        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using var swEncrypt = new StreamWriter(csEncrypt);
            //Write all data to the stream.
            swEncrypt.Write(plainText);
        }

        encrypted = msEncrypt.ToArray();

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    /// <summary>
    /// Decrypts an array of bytes using provided key and IV.
    /// </summary>
    /// <param name="cipherText">Encrypted string.</param>
    /// <param name="key">Encryption key.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Decrypted string.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key, byte[] iv)
    {
        // Check arguments.
        if (cipherText is not {Length: > 0})
            throw new ArgumentNullException(nameof(cipherText));
        if (key is not {Length: > 0})
            throw new ArgumentNullException(nameof(key));
        if (iv is not {Length: > 0})
            throw new ArgumentNullException(nameof(iv));

        // Create an Aes object with the specified key and IV.
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;

        // Create a decryptor to perform the stream transform.
        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for decryption.
        using var msDecrypt = new MemoryStream(cipherText);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        // Read the decrypted bytes from the decrypting stream
        // and place them in a string.
        var plaintext = srDecrypt.ReadToEnd();

        return plaintext;
    }

    /// <summary>
    /// Encrypts a string with provided string and IV.
    /// </summary>
    /// <param name="plainText">String to encrypt.</param>
    /// <param name="dbPassword">Encryption string.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Encrypted array of bytes representing the original string.</returns>
    /// 
    public static byte[] EncryptStringToBytes_Aes(string plainText, string dbPassword, byte[] iv)
        => EncryptStringToBytes_Aes(plainText, GenerateKeyFromString(dbPassword), iv);

    /// <summary>
    /// Decrypts an array of bytes using provided key and IV.
    /// </summary>
    /// <param name="cipherText">Encrypted string.</param>
    /// <param name="key">Encryption key.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Decrypted string.</returns>
    public static string DecryptStringFromBytes_Aes(byte[] cipherText, string dbPassword, byte[] iv)
        => DecryptStringFromBytes_Aes(cipherText, GenerateKeyFromString(dbPassword), iv);

    private static byte[] GenerateKeyFromString(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        if (passwordBytes.Length is > AesKeySize or 0)
        {
            throw new ArgumentOutOfRangeException(nameof(password));
        }
        var keyBytes = new byte[AesKeySize];
        var passLen = passwordBytes.Length;
        // generates key byte array by repeating bytes of the key
        for (var i = 0; i < AesKeySize; i++)
        {
            keyBytes[i] = passwordBytes[i % passLen];
        }

        return keyBytes;
    }
    
    public static byte[] GenerateIv()
    {
        using var aes = Aes.Create();
        aes.GenerateIV();
        return aes.IV;
    }
}