namespace BankSystem.Data.Models;

public class AuthResponse
{
    public bool Success { get; set; }
    
    public string? Token { get; set; }
    
    public Guid? UserId { get; set; }
    
    public string? Email { get; set; }
    
    public string? Message { get; set; }
    
    public string? TryCountMessage { get; set; }

}

public static class AuthResponseHelper
{
    public static AuthResponse Success(string token, Guid userId, string email) => new()
    {
        Success = true,
        Token = token,
        UserId = userId,
        Email = email,
    };
    
    public static AuthResponse Failed(int tryCount) => new()
    {
        Success = false,
        Message = "Invalid email or password.",
        TryCountMessage = tryCount >= 3
            ? "Your account has been locked. Please wait one hour before trying again."
            : $"You have typed wrong password {tryCount} times. After 3 times your account will be locked."
    }; 
    
    public static AuthResponse Locked() => new()
    {
        Success = false,
        Message = "Your account has been locked. Please wait one hour before trying again.",
    };
}