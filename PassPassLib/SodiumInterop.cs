using System.Runtime.InteropServices;

// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace PassPassLib;

/// <summary>
///     Provides direct calls to Sodium library. Do not use outside of the library itself.
/// </summary>
internal static class SodiumInterop
{
    private const string Sodium = "libsodium";

    // NOTES ON TYPES (YES THOSE ARE ALL GUESSES)
    // C TYPE -> C# TYPE
    // const char* -> sbyte*
    // unsigned char* -> byte*
    // const unsigned char -> byte*
    // const unsigned char* -> byte*
    // unsigned long long* -> out ulong
    // unsigned long long -> ulong
    // size_t -> nuint

    #region Argon2id

    // hardcoded values, to reduce amount of calls to sodium itself.

    internal const int Argon2id_ARGON2ID13 = 2;
    internal const int Argon2id_BYTES_MIN = 16;
    internal const long Argon2id_MEMLIMIT_MIN = 8192;
    internal const long Argon2id_OPSLIMIT_MAX = 4294967295;
    internal const long Argon2id_OPSLIMIT_MIN = 1;
    internal const int Argon2id_SALTBYTES = 16;

    /// <summary>
    ///     Generates an Argon2 hash with provided parameters.
    /// </summary>
    /// <param name="out">The output hash of the function.</param>
    /// <param name="outlen">Length of the hash to be generated.</param>
    /// <param name="passwd">Password to be derived.</param>
    /// <param name="passwdlen">Length of the password provided.</param>
    /// <param name="salt">Salt for the Argon2 algorithm.</param>
    /// <param name="opslimit">Maximum amount of computations to perform.</param>
    /// <param name="memlimit">Maximum memory used by algorithm in bytes.</param>
    /// <param name="alg">Version of the algorithm to be used.</param>
    /// <returns>Result code of the execution. 0 if everything went correctly.</returns>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe int crypto_pwhash_argon2id(
        byte* @out, 
        ulong outlen,
        sbyte* passwd,
        ulong passwdlen,
        byte* salt,
        ulong opslimit,
        nuint memlimit,
        int alg);

    /// <summary>
    ///     Version 1.3 of the Argon 2 algorithm identifier.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_pwhash_argon2id_alg_argon2id13();

    /// <summary>
    ///     The maximum length of a key derived with Argon2id.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_bytes_max();

    /// <summary>
    ///     The minimum length of a key derived with Argon2id.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_bytes_min();

    /// <summary>
    ///     The maximum memory assignable for key derivation with Argon2id.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_memlimit_max();

    /// <summary>
    ///     The minimum memory needed for key derivation with Argon2id.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_memlimit_min();

    /// <summary>
    ///     The maximum amount of computations to perform during key derivation with Argon2id.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_opslimit_max();

    /// <summary>
    ///     The minimum amount of computations needed to perform during key derivation with Argon2id.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_opslimit_min();

    /// <summary>
    ///     The salt length required to perform key derivation with Argon2id.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_saltbytes();

    #endregion

    #region XChaCha20-Poly1305

    // hardcoded values, to reduce amount of calls to sodium itself.
    internal const int XCC_ABYTES = 16;
    internal const int XCC_KEYBYTES = 32;
    internal const int XCC_NPUBBYTES = 24;
    internal const int XCC_NSECBYTES = 0;

    /// <summary>
    ///     Length of the authentication tag output by XChaCha20-Poly1305 algorithm.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_aead_xchacha20poly1305_ietf_abytes();

    /// <summary>
    ///     Required length of the key used by XChaCha20-Poly1305 algorithm.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_aead_xchacha20poly1305_ietf_keybytes();

    /// <summary>
    ///     Required length of the nonce used by XChaCha20-Poly1305 algorithm.
    /// </summary>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_aead_xchacha20poly1305_ietf_npubbytes();

    /// <summary>
    ///     Required length of the nsec parameter in XChaCha20-Poly1305 functions. Equals to zero.
    /// </summary>
    /// <returns></returns>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_aead_xchacha20poly1305_ietf_nsecbytes();

    /// <summary>
    ///     Encrypts data provided using XChaCha20-Poly1305 algorithm.
    /// </summary>
    /// <param name="c">The output encrypted data.</param>
    /// <param name="mac">Resulting authentication tag of the XChaCha20-Poly1305 algorithm.</param>
    /// <param name="maclen_p">Length of the tag.</param>
    /// <param name="m">Source data to be encrypted.</param>
    /// <param name="mlen">Length of the data to be encrypted.</param>
    /// <param name="ad">Additional data in XChaCha20-Poly1305 algorithm.</param>
    /// <param name="adlen">Length of additional data.</param>
    /// <param name="nsec">Unused parameter, should always be null.</param>
    /// <param name="npub">Nonce for XChaCha20-Poly1305 algorithm.</param>
    /// <param name="k">Key for XChaCha20-Poly1305 algorithm.</param>
    /// <returns></returns>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe int crypto_aead_xchacha20poly1305_ietf_encrypt_detached(
        byte* c, // cipher text <- RESULT
        byte* mac, // tag <- RESULT
        out ulong maclen_p, // tag length
        byte* m, // plain text
        ulong mlen, // plain text length
        byte* ad, // additional data
        ulong adlen, // additional data length
        byte* nsec, // unused, should be NULL
        byte* npub, // nonce
        byte* k // key
    );

    /// <summary>
    ///     Decrypts the data provided using XChaCha20-Poly1305 algorithm.
    /// </summary>
    /// <param name="m">The output decrypted data.</param>
    /// <param name="nsec">Unused parameter, should always be null.</param>
    /// <param name="c">The input data to be decrypted.</param>
    /// <param name="clen">Length of the input data.</param>
    /// <param name="mac">Authentication tag of data encrypted.</param>
    /// <param name="ad">Additional data in XChaCha20-Poly1305 algorithm.</param>
    /// <param name="adlen">Length of additional data.</param>
    /// <param name="npub">Nonce for XChaCha20-Poly1305 algorithm.</param>
    /// <param name="k">Key for XChaCha20-Poly1305 algorithm.</param>
    /// <returns></returns>
    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe int crypto_aead_xchacha20poly1305_ietf_decrypt_detached(
        byte* m, // plain text <- RESULT
        byte* nsec, // unused, should be NULL
        byte* c, // cipher text
        ulong clen, // cipher text length
        byte* mac, // tag
        byte* ad, // additional data
        ulong adlen, // additional data length
        byte* npub, // nonce
        byte* k // key
    );

    #endregion
}