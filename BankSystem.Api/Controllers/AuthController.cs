using BankSystem.Api.Middleware;
using BankSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost]
    [LogSignInAttempts]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        return Ok(model);
    }
    
}