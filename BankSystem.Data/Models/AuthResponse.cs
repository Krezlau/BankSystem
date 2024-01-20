namespace BankSystem.Data.Models;

public class AuthResponse
{
    public bool Success { get; set; }
    
    public string? Token { get; set; }
    
    public string? Message { get; set; }
    
    public string? TryCountMessage { get; set; }

}

public static class AuthResponseHelper
{
    public static AuthResponse Success(string token) => new() { Success = true, Token = token };
    
    public static AuthResponse Failed(int tryCount) => new()
    {
        Success = false,
        Message = "Invalid email or password.",
        TryCountMessage = $"You have typed wrong password {tryCount} times. After 3 times your account will be locked."
    }; 
}