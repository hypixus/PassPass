using Blake2Fast;

namespace PassPassLib;

public class Agron2
{
    public const int intLength = 4;

    //Inputs:
    //password (P):       Bytes (0..232-1)    Password (or message) to be hashed
    //    salt (S):           Bytes (8..232-1)    Salt (16 bytes recommended for password hashing)
    //parallelism (p):    Number (1..224-1)   Degree of parallelism (i.e. number of threads)
    //tagLength (T):      Number (4..232-1)   Desired number of returned bytes
    //memorySizeKB (m):   Number (8p..232-1)  Amount of memory (in kibibytes) to use
    //iterations (t):     Number (1..232-1)   Number of iterations to perform
    //version (v):        Number (0x13)       The current version is 0x13 (19 decimal)
    //key (K):            Bytes (0..232-1)    Optional key (Errata: PDF says 0..32 bytes, RFC says 0..232 bytes)
    //associatedData (X): Bytes (0..232-1)    Optional arbitrary extra data
    //    hashType (y):       Number (0=Argon2d, 1=Argon2i, 2=Argon2id)
    public static byte[] Argon2Hash
    (
        byte[] password,
        byte[] salt,
        int parallelism,
        int tagLength,
        int memorySizeKB,
        int iterations,
        byte[] key,
        byte[] associatedData,
        int hashType,
        int version = 0x13
    )
    {
        var buffer = new byte[40 + password.Length + salt.Length + key.Length + associatedData.Length];
        // all input integers are treated as little endian 32bit.
        // add ints.
        Buffer.BlockCopy(BitConverter.GetBytes(parallelism), 0, buffer, 0, intLength);
        Buffer.BlockCopy(BitConverter.GetBytes(tagLength), 0, buffer, 4, intLength);
        Buffer.BlockCopy(BitConverter.GetBytes(memorySizeKB), 0, buffer, 8, intLength);
        Buffer.BlockCopy(BitConverter.GetBytes(iterations), 0, buffer, 12, intLength);
        Buffer.BlockCopy(BitConverter.GetBytes(version), 0, buffer, 16, intLength);
        Buffer.BlockCopy(BitConverter.GetBytes(hashType), 0, buffer, 20, intLength);
        //add arrays sizes and themselves.
        Buffer.BlockCopy(BitConverter.GetBytes(password.Length), 0, buffer, 24, intLength);
        Buffer.BlockCopy(password, 0, buffer, 28, password.Length);

        Buffer.BlockCopy(BitConverter.GetBytes(salt.Length), 0, buffer, 28 + password.Length, intLength);
        Buffer.BlockCopy(salt, 0, buffer, 32 + password.Length, salt.Length);

        Buffer.BlockCopy(BitConverter.GetBytes(key.Length), 0, buffer, 32 + password.Length + salt.Length, intLength);
        Buffer.BlockCopy(key, 0, buffer, 36 + password.Length + salt.Length, key.Length);

        Buffer.BlockCopy(BitConverter.GetBytes(associatedData.Length), 0, buffer,
            36 + password.Length + salt.Length + key.Length, intLength);
        Buffer.BlockCopy(associatedData, 0, buffer, 40 + password.Length + salt.Length + key.Length,
            associatedData.Length);

        var h0 = Blake2b.ComputeHash(64, buffer);

        var blockCount = memorySizeKB - memorySizeKB % (4 * parallelism);
        var columnCount = blockCount / parallelism;

        var m = (int) (4 * parallelism * Math.Floor((float) (memorySizeKB / 4 * parallelism)));

        var q = m / parallelism;
        var B = new byte[parallelism, q, 1024];

        return h0;
    }
}