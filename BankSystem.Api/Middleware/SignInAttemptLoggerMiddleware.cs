using System.Text;
using System.Text.Json;
using BankSystem.Data;
using BankSystem.Data.Entities;
using BankSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BankSystem.Api.Middleware;

public class SignInAttemptLoggerMiddleware
{
    private readonly RequestDelegate _next;

    public SignInAttemptLoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, BankDbContext dbContext)
    {
        var logSignInAttemptsAttribute = context.GetEndpoint()?.Metadata.GetMetadata<LogSignInAttemptsAttribute>();
        if (logSignInAttemptsAttribute is null)
        {
            await _next(context);
            return;
        }
        var email = await ReadEmailFromBodyAsync(context.Request); 
        if (email is null)
        {
            await _next(context);
            return;
        }
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            await _next(context);
            return;
        }
        
        var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var requestTime = DateTime.UtcNow;
        
        var signInAttempt = new Login() 
        {
            UserId = user.Id,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            Timestamp = requestTime,
            Successful = false,
        };
        
        dbContext.Logins.Add(signInAttempt);
        await dbContext.SaveChangesAsync();
        
        await _next(context);
        
        signInAttempt.Successful = true;
        dbContext.Update(signInAttempt);
        await dbContext.SaveChangesAsync();
    }
    
    private async Task<string?> ReadEmailFromBodyAsync(HttpRequest request)
    {
        using var streamReader = new StreamReader(request.Body, Encoding.UTF8, true);
        var body = await streamReader.ReadToEndAsync();
        var loginRequest = JsonSerializer.Deserialize<LoginRequestModel>(body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(body));
        request.Body = stream;
        return loginRequest?.Email;
    }
}