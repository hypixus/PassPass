using System.Runtime.InteropServices;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace PassPassPlayground;

internal static partial class SodiumInterop
{
    internal const int crypto_aead_xchacha20poly1305_ietf_ABYTES = 16;
    internal const int crypto_aead_xchacha20poly1305_ietf_KEYBYTES = 32;
    internal const int crypto_aead_xchacha20poly1305_ietf_NPUBBYTES = 24;
    internal const int crypto_aead_xchacha20poly1305_ietf_NSECBYTES = 0;

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_aead_xchacha20poly1305_ietf_abytes();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_aead_xchacha20poly1305_ietf_keybytes();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_aead_xchacha20poly1305_ietf_npubbytes();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_aead_xchacha20poly1305_ietf_nsecbytes();

    // NOTES ON TYPES (YES THOSE ARE ALL GUESSES)
    // C TYPE -> C# TYPE
    // unsigned char* -> byte*
    // const unsigned char -> byte*
    // unsigned long long* -> out ulong
    // unsigned long long -> ulong

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe int crypto_aead_xchacha20poly1305_ietf_encrypt_detached(
        byte* c, // cipher text RESULT
        byte* mac, // tag
        out ulong maclen_p, // tag length
        byte* m, // plain text
        ulong mlen, // plain text length
        byte* ad, // additional data
        ulong adlen, // additional data length
        byte* nsec, // unused, should be NULL
        byte* npub, // nonce
        byte* k // key
    );

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe int crypto_aead_xchacha20poly1305_ietf_decrypt_detached(
        byte* m, // plain text RESULT
        byte* nsec, // unused, should be NULL
        byte* c, // cipher text
        ulong clen, // cipher text length
        byte* mac, // tag
        byte* ad, // additional data
        ulong adlen, // additional data length
        byte* npub, // nonce
        byte* k // key
    );
}