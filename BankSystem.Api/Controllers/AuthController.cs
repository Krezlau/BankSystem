using BankSystem.Api.Middleware;
using BankSystem.Data.Models;
using BankSystem.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [LogSignInAttempts]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        return Ok(await _authService.LoginAsync(model));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
    {
        return Ok(await _authService.RegisterAsync(model));
    }
    
}