using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using BankSystem.Data.Entities;
using SecretSharingDotNet.Cryptography;
using SecretSharingDotNet.Math;

namespace BankSystem.Services.Auth;

public static class PasswordService 
{
    public static (string hashedSecret, List<PasswordKey> keys) CreatePassword(string plaintextPassword)
    {
        var keys = CreateKeyHashes(plaintextPassword);
        
        var gcd = new ExtendedEuclideanAlgorithm<BigInteger>();
        var shamir = new ShamirsSecretSharing<BigInteger>(gcd);
        var shares = shamir.MakeShares(5, plaintextPassword.Length,  10000);
        if (shares.OriginalSecret is null) throw new Exception("Original secret is null");
        var secret = shares.OriginalSecret.Value;
        
        var success = TryEncryptShares(shares, keys, out var encrypted);
        if (!success) throw new Exception("Failed to encrypt shares");
        
        var hashedSecret = Convert.ToBase64String(Hasher.HashPassword(secret));
        
        var passwordKeys = new List<PasswordKey>();
        for (int i = 0; i < shares.Count; i++)
        {
            passwordKeys.Add(new PasswordKey()
            {
                IV = Convert.ToBase64String(keys[i].iv),
                EncryptedShare = Convert.ToBase64String(encrypted[i]),
                Salt = Convert.ToBase64String(keys[i].salt),
            });
        }

        return (hashedSecret, passwordKeys);
    }

    public static bool VerifyPassword(List<(int pos, char c)> passwordChars, string hashedSecret, List<PasswordKey> keys)
    {
        var success = TryDecryptShares(passwordChars, keys, out var shares);
        if (!success) return false;
        
        var gcd = new ExtendedEuclideanAlgorithm<BigInteger>();
        var shamir = new ShamirsSecretSharing<BigInteger>(gcd);
        var secret = shamir.Reconstruction(shares.ToArray());
        return Hasher.VerifyHashedPasswordV2(Convert.FromBase64String(hashedSecret), secret);
    }

    # region private methods

    private static List<(byte[] iv, byte[] keyHash, byte[] salt)> CreateKeyHashes(string plaintextPassword)
    {
        var keys = new List<(byte[] iv, byte[] keyHash, byte[] salt)>();
        foreach (var t in plaintextPassword)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var iv = RandomNumberGenerator.GetBytes(16);
            var letterBytes = Encoding.UTF8.GetBytes(t.ToString());
            var key = Rfc2898DeriveBytes.Pbkdf2(letterBytes, salt, 100_000, HashAlgorithmName.SHA256, 32); 
            keys.Add((iv, key, salt));
        }

        return keys;
    }

    private static bool TryEncryptShares(Shares<BigInteger> shares, List<(byte[] iv, byte[] keyHash, byte[] salt)> keys, out List<byte[]> encrypted)
    {
        encrypted = new List<byte[]>();
        for (int i = 0; i < shares.Count; i++)
        {
            using var aes = Aes.Create();
            aes.Key = keys[i].keyHash;
            aes.IV = keys[i].iv;
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;

            try
            {
                var ct = aes.EncryptCbc(shares[i].Y.Value.ToByteArray(), keys[i].iv, PaddingMode.PKCS7);
                encrypted.Add(ct);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        return true;
    }

    private static bool TryDecryptShares(List<(int pos, char c)> passwordChars, List<PasswordKey> keys, out List<string> shares)
    {
        shares = new List<string>();
        foreach (var (pos, c) in passwordChars)
        {
            var key = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(c.ToString()), Convert.FromBase64String(keys[pos].Salt), 100_000, HashAlgorithmName.SHA256, 32);
            using var aes = Aes.Create();
            aes.IV = Convert.FromBase64String(keys[pos].IV);
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Key = key;
            
            try
            {
                var y = aes.DecryptCbc(Convert.FromBase64String(keys[pos].EncryptedShare), Convert.FromBase64String(keys[pos].IV), PaddingMode.PKCS7);
                shares.Add(MakeShare(pos + 1, y));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        return true;
    }
    
    private static string MakeShare(int pos, byte[] y)
    {
        StringBuilder sb = new();
        sb.Append(pos.ToString("X2"));
        sb.Append('-');
        sb.Append(ConvertYToHex(y));
        
        return sb.ToString();
    }

    private static string ConvertYToHex(byte[] y)
    {
        var hexRepresentation = new StringBuilder(y.Length * 2);
        foreach (byte b in y)
        {
            const string hexAlphabet = "0123456789ABCDEF";
            hexRepresentation.Append(hexAlphabet[b >> 4]).Append(hexAlphabet[b & 0xF]);
        }
        return hexRepresentation.ToString();
    }
    
    # endregion
}