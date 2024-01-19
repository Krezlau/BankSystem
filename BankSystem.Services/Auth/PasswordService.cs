using System.Security.Cryptography;
using System.Text;

namespace BankSystem.Services.Auth;

public interface IPasswordService
{
    (string hashedSecret, List<PasswordKey> keys) CreatePassword(string plaintextPassword);

    bool VerifyPassword();
}

public class PasswordService : IPasswordService
{
    public (string hashedSecret, List<PasswordKey> keys) CreatePassword(string plaintextPassword)
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
        var (secret, shares) = Shamir.MakeRandomShares(5, plaintextPassword.Length);
        
        var encrypted = new List<byte[]>();
        // encrypt shares with keys using cbc
        for (int i = 0; i < shares.Count; i++)
        {
            using var aes = Aes.Create();
            aes.Key = keys[i].key;
            aes.IV = keys[i].iv;
            aes.Mode = CipherMode.CBC;
            
            var encryptor = aes.CreateEncryptor();
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using var writer = new StreamWriter(cryptoStream);
            writer.Write(shares[i].y.ToString());
            encrypted.Add(memoryStream.ToArray());
        }
        
        // hash secret
        var secretSalt = RandomNumberGenerator.GetBytes(16);
        var hashedSecret = Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(secret.ToByteArray(),
            secretSalt, 100_000, HashAlgorithmName.SHA256, 32));
        
        var passwordKeys = new List<PasswordKey>();
        for (int i = 0; i < shares.Count; i++)
        {
            passwordKeys.Add(new PasswordKey()
            {
                IV = keys[i].iv,
                Key = encrypted[i],
                Salt = keys[i].salt,
            });
        }

        return (hashedSecret, passwordKeys);
    }
}

public class PasswordKey
{
    public byte[] Salt { get; set; } = new byte[16];
    
    public byte[] Key { get; set; } = new byte[32];
    
    public byte[] IV { get; set; } = new byte[16];
}