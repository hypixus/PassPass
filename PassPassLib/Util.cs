using System.Security.Cryptography;
using System.Text;

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


    #region AES256

    /// <summary>
    ///     Encrypts a string with provided key and IV.
    /// </summary>
    /// <param name="plainText">String to encrypt.</param>
    /// <param name="key">Encryption key.</param>
    /// <param name="iv">Initialization Vector for AES algorithm.</param>
    /// <returns>Encrypted array of bytes representing the original string.</returns>
    /// <exception cref="ArgumentNullException">One or more of arguments provided are null.</exception>
    [Obsolete(
        "AES-256-CBC is considered vulnerable. For more information visit https://docs.microsoft.com/en-us/dotnet/standard/security/vulnerabilities-cbc-mode")]
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
    [Obsolete(
        "AES-256-CBC is considered vulnerable. For more information visit https://docs.microsoft.com/en-us/dotnet/standard/security/vulnerabilities-cbc-mode")]
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
    [Obsolete(
        "AES-256-CBC is considered vulnerable. For more information visit https://docs.microsoft.com/en-us/dotnet/standard/security/vulnerabilities-cbc-mode")]
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
    [Obsolete(
        "AES-256-CBC is considered vulnerable. For more information visit https://docs.microsoft.com/en-us/dotnet/standard/security/vulnerabilities-cbc-mode")]
    public static string DecryptStringFromBytes_Aes(byte[] cipherText, string dbPassword, byte[] salt, byte[] iv)
    {
        return DecryptStringFromBytes_Aes(cipherText, Argon2FromPassword(dbPassword, salt), iv);
    }

    #endregion

    #region Argon2id
    /// <summary>
    ///     Generates a key based off provided byte array.
    /// </summary>
    /// <param name="password">Byte array to derive.</param>
    /// <param name="salt">Randomly generated salt. Must be 16 bytes long.</param>
    /// <returns>256 bit key derived from the password.</returns>
    public static byte[] Argon2FromPassword(byte[] password, byte[] salt)
    {
        if (salt is not {Length: SodiumInterop.Argon2id_SALTBYTES} || password == null)
            throw new CryptographicException("Incorrect input parameters for Argon2id algorithm.");
        return SodiumInteropArgon2(password, salt);
    }

    /// <summary>
    ///     Generates a key based off user provided password and salt.
    /// </summary>
    /// <param name="password">UTF-8 encoded password string provided by user.</param>
    /// <param name="salt">Randomly generated salt. Must be 16 bytes long.</param>
    /// <returns>256 bit key derived from the password.</returns>
    public static byte[] Argon2FromPassword(string password, byte[] salt)
        => Argon2FromPassword(Encoding.UTF8.GetBytes(password), salt);
    #endregion

    #region XChaCha20Poly1305

    /// <summary>
    ///     Encrypts data provided using XChaCha20Poly1305 algorithm.
    /// </summary>
    /// <param name="plainText">Data to be encrypted.</param>
    /// <param name="key">Key for encryption. Must be 32 bytes.</param>
    /// <param name="nonce">Nonce utilized by the algorithm. Must be 24 bytes.</param>
    /// <returns>Tuple of encrypted data and tag, in that order.</returns>
    public static (byte[], byte[]) EncryptDataXcc(byte[] plainText, byte[] key, byte[] nonce)
    {
        if (key.Length != SodiumInterop.XCC_KEYBYTES
            || nonce.Length != SodiumInterop.XCC_NPUBBYTES)
            throw new CryptographicException("Incorrect input parameters for XChaCha20-Poly1305 algorithm.");
        return SodiumInteropXccEncrypt(plainText, key, nonce);
    }


    /// <summary>
    ///     Decrypts data provided using XChaCha20Poly1305 algorithm.
    /// </summary>
    /// <param name="cipherText">Encrypted data to decode.</param>
    /// <param name="key">Key used to encrypt the data. Must be 32 bytes.</param>
    /// <param name="nonce">Nonce used while encrypting data. Must be 24 bytes.</param>
    /// <param name="tag"></param>
    /// <returns>Decrypted data.</returns>
    public static byte[] DecryptDataXcc(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag)
    {
        if (key.Length != SodiumInterop.XCC_KEYBYTES
            || nonce.Length != SodiumInterop.XCC_NPUBBYTES
            || tag.Length != SodiumInterop.XCC_ABYTES)
            throw new CryptographicException("Incorrect input parameters for XChaCha20-1305 algorithm.");
        return SodiumInteropXccDecrypt(cipherText, key, nonce, tag);
    }

    /// <summary>
    ///     Encrypts a string provided using XChaCha20Poly1305 algorithm.
    /// </summary>
    /// <param name="plainText">String to be encrypted.</param>
    /// <param name="key">Key for encryption. Must be 32 bytes.</param>
    /// <param name="nonce">Nonce utilized by the algorithm. Must be 24 bytes.</param>
    /// <returns>Tuple of encrypted data and tag, in that order.</returns>
    public static (byte[], byte[]) EncryptStringXcc(string plainText, byte[] key, byte[] nonce)
    {
        return EncryptDataXcc(Encoding.UTF8.GetBytes(plainText), key, nonce);
    }

    /// <summary>
    ///     Decrypts data provided using XChaCha20Poly1305 algorithm into a UTF8 string.
    /// </summary>
    /// <param name="cipherText">Encrypted data to decode.</param>
    /// <param name="key">Key used to encrypt the data. Must be 32 bytes.</param>
    /// <param name="nonce">Nonce used while encrypting data. Must be 24 bytes.</param>
    /// <param name="tag"></param>
    /// <returns>Decrypted string.</returns>
    public static string DecryptStringXcc(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag)
    {
        return Encoding.UTF8.GetString(DecryptDataXcc(cipherText, key, nonce, tag));
    }
    #endregion

    #region Unsafe code

    /// <summary>
    ///     Creates an Argon2id hash from password bytes using libsodium.
    ///     Do not call directly. This function does not do any checks on arguments. Please use Argon2FromPassword() instead.
    /// </summary>
    /// <param name="password">Password bytes to be derived.</param>
    /// <param name="salt">Salt bytes for Argon2id algorithm.</param>
    /// <returns>Argon2id derived password.</returns>
    /// <exception cref="CryptographicException"></exception>
    private static unsafe byte[] SodiumInteropArgon2(byte[] password, byte[] salt)
    {
        var output = new Span<byte>(new byte[XChaCha20Poly1305KeySizeBytes]);
        var passwordSpan = new ReadOnlySpan<byte>(password);
        var saltSpan = new ReadOnlySpan<byte>(salt);
        fixed (byte* outPtr = output)
        fixed (byte* saltPtr = saltSpan)
        fixed (byte* inputPtr = passwordSpan)
        {
            var error = SodiumInterop.crypto_pwhash_argon2id(
                outPtr, XChaCha20Poly1305KeySizeBytes, (sbyte*) inputPtr, (ulong) password.Length,
                saltPtr, ArgonOpsLimit, (nuint) ArgonMemLimit * 1024, SodiumInterop.Argon2id_ARGON2ID13);
            if (error != 0) throw new CryptographicException("Sodium Interop call failed with error code " + error);
        }

        return output.ToArray();
    }

    /// <summary>
    ///     Encrypts data with XChaCha20-Poly1305 using libsodium.
    ///     Do not call directly. This function does not do any checks on arguments. Please use <c>EncryptDataXcc</c> instead.
    /// </summary>
    /// <param name="plainText">Data to be encrypted.</param>
    /// <param name="key">Key for XChaCha20-Poly1305 algorithm.</param>
    /// <param name="nonce">Nonce for XChaCha20-Poly1305 algorithm.</param>
    /// <returns>Tuple of encrypted data and tag, in that order.</returns>
    /// <exception cref="CryptographicException"></exception>
    private static unsafe (byte[], byte[]) SodiumInteropXccEncrypt(byte[] plainText, byte[] key, byte[] nonce)
    {
        var plainTextSpan = new ReadOnlySpan<byte>(plainText);
        var keySpan = new ReadOnlySpan<byte>(key);
        var nonceSpan = new ReadOnlySpan<byte>(nonce);
        var cipherTextSpan = new Span<byte>(new byte[plainText.Length]);
        var tagSpan = new Span<byte>(new byte[XChaCha20Poly1305TagSizeBytes]);
        fixed (byte* c = cipherTextSpan)
        fixed (byte* m = plainTextSpan)
        fixed (byte* npub = nonceSpan)
        fixed (byte* k = keySpan)
        fixed (byte* mac = tagSpan)
        {
            var error = SodiumInterop.crypto_aead_xchacha20poly1305_ietf_encrypt_detached(
                c, mac, out _, m, (ulong) plainText.Length, null, 0, null, npub, k);
            if (error != 0) throw new CryptographicException("Sodium Interop call failed with error code " + error);
            return (cipherTextSpan.ToArray(), tagSpan.ToArray());
        }
    }

    /// <summary>
    ///     Decrypts data encrypted with XChaCha20-Poly1305 using libsodium.
    ///     Do not call directly. This function does not do any checks on arguments. Please use <c>DecryptDataXcc</c> instead.
    /// </summary>
    /// <param name="cipherText">Data to be decrypted.</param>
    /// <param name="key">Key used for encryption.</param>
    /// <param name="nonce">Nonce used for encryption.</param>
    /// <param name="tag">Tag being the output of original data's encryption.</param>
    /// <returns>Decrypted data.</returns>
    /// <exception cref="CryptographicException"></exception>
    private static unsafe byte[] SodiumInteropXccDecrypt(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag)
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

    #endregion

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