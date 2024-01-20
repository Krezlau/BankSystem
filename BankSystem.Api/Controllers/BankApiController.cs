using BankSystem.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

public abstract class BankApiController : ControllerBase
{
    protected Guid GetUserId(HttpRequest request)
    {
        var token = request.Headers["Authorization"].ToString().Split(" ")[1];

        return JwtService.GetUserIdFromJwtToken(token);
    }
}