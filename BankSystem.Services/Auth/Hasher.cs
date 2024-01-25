using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BankSystem.Services.Auth;

public static class Hasher
{
    public static byte[] HashPassword(byte[] password)
    {
        const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA256;
        const int Pbkdf2IterCount = 100_000;
        const int Pbkdf2SubkeyLength = 256 / 8;
        const int SaltSize = 128 / 8;

        byte[] salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        byte[] subkey = KeyDerivation.Pbkdf2(Encoding.UTF8.GetString(password), salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

        var outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
        outputBytes[0] = 0x00; 
        Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
        Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
        return outputBytes;
    }
    
     public static bool VerifyHashedPasswordV2(byte[] hashedPassword, byte[] password)
     {
         const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA256;
         const int Pbkdf2IterCount = 100_000; 
         const int Pbkdf2SubkeyLength = 256 / 8;
         const int SaltSize = 128 / 8; 
 
         if (hashedPassword.Length != 1 + SaltSize + Pbkdf2SubkeyLength)
         {
             return false;
         }
 
         byte[] salt = new byte[SaltSize];
         Buffer.BlockCopy(hashedPassword, 1, salt, 0, salt.Length);
 
         byte[] expectedSubkey = new byte[Pbkdf2SubkeyLength];
         Buffer.BlockCopy(hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);
 
         byte[] actualSubkey = KeyDerivation.Pbkdf2(Encoding.UTF8.GetString(password), salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);
         return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
     }
    
}