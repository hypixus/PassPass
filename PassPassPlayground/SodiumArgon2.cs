using System.Runtime.InteropServices;

namespace PassPassPlayground;

internal static partial class SodiumInterop
{
    private const string Sodium = "libsodium";

    internal const int crypto_pwhash_argon2id_ALG_ARGON2ID13 = 2;
    internal const int crypto_pwhash_argon2id_BYTES_MIN = 16;
    internal const long crypto_pwhash_argon2id_MEMLIMIT_MIN = 8192;
    internal const long crypto_pwhash_argon2id_OPSLIMIT_MAX = 4294967295;
    internal const long crypto_pwhash_argon2id_OPSLIMIT_MIN = 1;
    internal const int crypto_pwhash_argon2id_SALTBYTES = 16;

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


    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_pwhash_argon2id_alg_argon2id13();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_bytes_max();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_bytes_min();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_memlimit_max();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_memlimit_min();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_opslimit_max();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_opslimit_min();

    [DllImport(Sodium, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nuint crypto_pwhash_argon2id_saltbytes();
}