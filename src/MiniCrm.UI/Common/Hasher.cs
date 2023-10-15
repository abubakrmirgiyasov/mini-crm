using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace MiniCrm.UI.Common;

public class Hasher
{
    public static byte[] GetSalt()
    {
        byte[] salt = new byte[128 / 8];

        using var random = RandomNumberGenerator.Create();
        random.GetBytes(salt);
        return salt;
    }

    public static string GetHash(string password, byte[] salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 1000,
            numBytesRequested: 256 / 8));
    }
}
