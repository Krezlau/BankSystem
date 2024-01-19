using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using BankSystem.Data.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SecretSharingDotNet.Cryptography;
using SecretSharingDotNet.Math;

namespace BankSystem.Services.Auth;

public static class PasswordService 
{
    public static (string hashedSecret, List<PasswordKey> keys) CreatePassword(string plaintextPassword)
    {
        var keys = new List<(byte[] iv, byte[] key, byte[] salt)>();
        // kdf on every character
        foreach (var t in plaintextPassword)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var iv = RandomNumberGenerator.GetBytes(16);
            var letterBytes = Encoding.UTF8.GetBytes(t.ToString());
            var key = Rfc2898DeriveBytes.Pbkdf2(letterBytes, salt, 100_000, HashAlgorithmName.SHA256, 32); 
            keys.Add((iv, key, salt));
        }
        
        // make shamir's shares
        // var (secret, shares) = Shamir.MakeRandomShares(5, plaintextPassword.Length);
        var gcd = new ExtendedEuclideanAlgorithm<BigInteger>();
        var split = new ShamirsSecretSharing<BigInteger>(gcd);
        var shares = split.MakeShares(5, plaintextPassword.Length,  10000);
        if (shares.OriginalSecret is null) throw new Exception("Original secret is null");
        var secret = shares.OriginalSecret.Value;
        
        var encrypted = new List<string>();
        // encrypt shares with keys using cbc
        for (int i = 0; i < shares.Count; i++)
        {
            using var aes = Aes.Create();
            aes.Key = keys[i].key;
            aes.IV = keys[i].iv;
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            
            var ct = aes.EncryptCbc(shares[i].Y.Value.ToByteArray(), keys[i].iv);
            var bytes = shares[i].Y.Value.ToByteArray();
            var str = Convert.ToBase64String(bytes);
            encrypted.Add(Convert.ToBase64String(ct));
        }
        
        // hash secret
        var hashedSecret = Convert.ToBase64String(HashPassword(secret));
        
        var passwordKeys = new List<PasswordKey>();
        for (int i = 0; i < shares.Count; i++)
        {
            passwordKeys.Add(new PasswordKey()
            {
                IV = keys[i].iv,
                Key = Convert.FromBase64String(encrypted[i]),
                Salt = keys[i].salt,
            });
        }

        return (hashedSecret, passwordKeys);
    }

    public static bool VerifyPassword(List<(int pos, char c)> passwordChars, string hashedSecret, List<PasswordKey> keys)
    {
        var shares = new List<string>();
        foreach ((int pos, char c) in passwordChars)
        {
            var key = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(c.ToString()), keys[pos].Salt, 100_000, HashAlgorithmName.SHA256, 32);
            using var aes = Aes.Create();
            aes.IV = keys[pos].IV;
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;

            var y = aes.DecryptCbc(keys[pos].Key, keys[pos].IV);
            shares.Add(MakeShare(pos + 1, y ));
        }
        var gcd = new ExtendedEuclideanAlgorithm<BigInteger>();
        var split = new ShamirsSecretSharing<BigInteger>(gcd);
        var secret = split.Reconstruction(shares.ToArray());
        return VerifyHashedPasswordV2(Convert.FromBase64String(hashedSecret), secret);
    }

    private static string MakeShare(int pos, byte[] y)
    {
        StringBuilder sb = new();
        if (pos < 10 ) sb.Append('0');
        sb.Append(pos);
        sb.Append('-');
        var hexRepresentation = new StringBuilder(y.Length * 2);
        foreach (byte b in y)
        {
            const string hexAlphabet = "0123456789ABCDEF";
            hexRepresentation.Append(hexAlphabet[b >> 4]).Append(hexAlphabet[b & 0xF]);
        }

        sb.Append(hexRepresentation);
        return sb.ToString();
    }
    
    private static byte[] HashPassword(byte[] password)
    {
        const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
        const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
        const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
        const int SaltSize = 128 / 8; // 128 bits

        byte[] salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        byte[] subkey = KeyDerivation.Pbkdf2(Encoding.UTF8.GetString(password), salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

        var outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
        outputBytes[0] = 0x00; // format marker
        Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
        Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
        return outputBytes;
    }
    
      private static bool VerifyHashedPasswordV2(byte[] hashedPassword, byte[] password)
     {
         const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
         const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
         const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
         const int SaltSize = 128 / 8; // 128 bits
 
         // We know ahead of time the exact length of a valid hashed password payload.
         if (hashedPassword.Length != 1 + SaltSize + Pbkdf2SubkeyLength)
         {
             return false; // bad size
         }
 
         byte[] salt = new byte[SaltSize];
         Buffer.BlockCopy(hashedPassword, 1, salt, 0, salt.Length);
 
         byte[] expectedSubkey = new byte[Pbkdf2SubkeyLength];
         Buffer.BlockCopy(hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);
 
         // Hash the incoming password and verify it
         byte[] actualSubkey = KeyDerivation.Pbkdf2(Encoding.UTF8.GetString(password), salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);
         return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
     }
}