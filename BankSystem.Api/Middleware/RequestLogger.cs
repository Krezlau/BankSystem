using System.Text;
using System.Text.Json;
using BankSystem.Data;
using BankSystem.Data.Entities;
using BankSystem.Data.Models;
using BankSystem.Services.Auth;

namespace BankSystem.Api.Middleware;

public class RequestLogger
{
    private readonly RequestDelegate _next;

    public RequestLogger(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, BankDbContext dbContext)
    {
        var logSignInAttemptsAttribute = context.GetEndpoint()?.Metadata.GetMetadata<LogRequestsAttribute>();
        if (logSignInAttemptsAttribute is null)
        {
            await _next(context);
            return;
        }
        
        var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? string.Empty;
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var authHeader = context.Request.Headers["Authorization"];
        Guid? userId = null;
        if (authHeader.Count != 0)
        {
            var jwt = authHeader.ToString().Replace("Bearer ", "");
            userId = JwtService.GetUserIdFromJwtToken(jwt);
        }
        
        var url = context.Request.Path.Value;
        
        var request = new Log() 
        {
            UserId = userId,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            Successful = false,
            Url = url ?? string.Empty
        };
        
        dbContext.Logins.Add(request);
        await dbContext.SaveChangesAsync();
        
        await _next(context);
        
        if (context.Response.StatusCode < 400)
        {
            request.Successful = true;
            dbContext.Update(request);
            await dbContext.SaveChangesAsync();
        }
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