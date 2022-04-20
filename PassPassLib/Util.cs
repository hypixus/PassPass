using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using NaCl.Core;

namespace PassPassLib;

public static class Util
{
    // Utilities class. All static methods and constants are supposed to be kept here in the final version.

    // Byte array sizes for used algorithms.
    public const int AesKeySizeBytes = 256 / 8; // 32 bytes
    public const int AesIVSizeBytes = 128 / 8; // 16 bytes
    public const int ArgonSaltSizeBytes = 128 / 8; // 16 bytes
    public const int XChaCha20Poly1305KeySizeBytes = 256 / 8; // 32 bytes
    public const int XChaCha20Poly1305NonceSizeBytes = 192 / 8; // 24 bytes;
    public const int XChaCha20Poly1305TagSizeBytes = 128 / 8; // 16 bytes;
    public const int ArgonMemLimit = 65536;
    public const int ArgonOpsLimit = 20;

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

    /// <summary>
    /// Generates a key based off user provided password and salt.
    /// </summary>
    /// <param name="password">Password string from user.</param>
    /// <param name="salt">Randomly generated salt. Must be 16 bytes long.</param>
    /// <returns></returns>
    public static byte[] Argon2FromPassword(byte[] password, byte[] salt)
    {
        if (salt is not {Length: SodiumInterop.Argon2id_SALTBYTES} || password == null)
            throw new CryptographicException("Incorrect input parameters for Argon2id algorithm.");
        return SodiumInteropArgon2(password, salt);
    }

    public static byte[] Argon2FromPassword(string password, byte[] salt)
    {
        return Argon2FromPassword(Encoding.UTF8.GetBytes(password), salt);
    }

    private static unsafe byte[] SodiumInteropArgon2(byte[] password, byte[] salt)
    {
        var output = new Span<byte>(new byte[Util.XChaCha20Poly1305KeySizeBytes]);
        var passwordSpan = new ReadOnlySpan<byte>(password);
        var saltSpan = new ReadOnlySpan<byte>(salt);
        fixed (byte* outPtr = output)
        fixed (byte* saltPtr = saltSpan)
        fixed (byte* inputPtr = passwordSpan)
        {
            var error = SodiumInterop.crypto_pwhash_argon2id(
                outPtr, XChaCha20Poly1305KeySizeBytes, (sbyte*) inputPtr, (ulong) password.Length,
                saltPtr, ArgonOpsLimit, (nuint)ArgonMemLimit * 1024, SodiumInterop.Argon2id_ARGON2ID13);
            if (error != 0) throw new CryptographicException("Sodium Interop call failed with error code " + error);
        }

        return output.ToArray();
    }


    /// <summary>
    /// Encrypts data provided using XChaCha20Poly1305 algorithm.
    /// </summary>
    /// <param name="plainText">Data to be encrypted.</param>
    /// <param name="key">Key for encryption. Must be 32 bytes.</param>
    /// <param name="nonce">Nonce utilized by the algorithm. Must be 24 bytes.</param>
    /// <returns>Tuple of encrypted data and tag, in that order.</returns>
    public static (byte[], byte[]) EncryptDataXCC(byte[] plainText, byte[] key, byte[] nonce)
    {
        var xChaCha = new XChaCha20Poly1305(new ReadOnlyMemory<byte>(key));
        var cipherText = new byte[plainText.Length];
        var tag = new byte[XChaCha20Poly1305TagSizeBytes];
        xChaCha.Encrypt(nonce, plainText, cipherText, tag);
        return (cipherText, tag);
    }

    /// <summary>
    /// Decrypts data provided using XChaCha20Poly1305 algorithm.
    /// </summary>
    /// <param name="cipherText">Encrypted data to decode.</param>
    /// <param name="key">Key used to encrypt the data. Must be 32 bytes.</param>
    /// <param name="nonce">Nonce used while encrypting data. Must be 24 bytes.</param>
    /// <param name="tag"></param>
    /// <returns>Decrypted data.</returns>
    public static byte[] DecryptDataXCC(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag)
    {
        var xChaCha = new XChaCha20Poly1305(new ReadOnlyMemory<byte>(key));
        var plainText = new byte[cipherText.Length];
        xChaCha.Decrypt(nonce, cipherText, tag, plainText);
        return plainText;
    }

    /// <summary>
    /// Encrypts a string provided using XChaCha20Poly1305 algorithm.
    /// </summary>
    /// <param name="plainText">String to be encrypted.</param>
    /// <param name="key">Key for encryption. Must be 32 bytes.</param>
    /// <param name="nonce">Nonce utilized by the algorithm. Must be 24 bytes.</param>
    /// <returns>Tuple of encrypted data and tag, in that order.</returns>
    public static (byte[], byte[]) EncryptStringXCC(string plainText, byte[] key, byte[] nonce)
        => EncryptDataXCC(Encoding.UTF8.GetBytes(plainText), key, nonce);

    /// <summary>
    /// Decrypts data provided using XChaCha20Poly1305 algorithm into a UTF8 string.
    /// </summary>
    /// <param name="cipherText">Encrypted data to decode.</param>
    /// <param name="key">Key used to encrypt the data. Must be 32 bytes.</param>
    /// <param name="nonce">Nonce used while encrypting data. Must be 24 bytes.</param>
    /// <param name="tag"></param>
    /// <returns>Decrypted string.</returns>
    public static string DecryptStringXCC(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag)
        => Encoding.UTF8.GetString(DecryptDataXCC(cipherText, key, nonce, tag));


    #region TrueRNG

    public static byte[] GenerateIv()
    {
        return RandomNumberGenerator.GetBytes(AesIVSizeBytes);
    }

    public static byte[] GenerateArgon2idSalt()
    {
        return RandomNumberGenerator.GetBytes(ArgonSaltSizeBytes);
    }

    public static byte[] GenerateXCCNonce()
    {
        return RandomNumberGenerator.GetBytes(XChaCha20Poly1305NonceSizeBytes);
    }

    #endregion
}