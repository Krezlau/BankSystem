namespace BankSystem.Data.Models.Users;

public class BankAccountModel
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    
    public decimal AccountBalance { get; set; }
    
    public string CardNumber { get; set; } = string.Empty;
    
    public string Cvv { get; set; } = string.Empty;
    
    public string ExpirationDate { get; set; } = string.Empty;

}