namespace BankSystem.Data.Entities;

public class PasswordKey
{
    public byte[] Salt { get; set; } = new byte[16];
    
    public byte[] Key { get; set; } = new byte[32];
    
    public byte[] IV { get; set; } = new byte[16];
}