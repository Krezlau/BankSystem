using BankSystem.Api.Middleware;
using BankSystem.Data.Models;
using BankSystem.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BankApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login-check")]
    public async Task<IActionResult> LoginCheck([FromBody] LoginCheckRequestModel model)
    {
        return Ok(ApiResponseHelper.Success(await _authService.LoginCheckAsync(model)));
    }

    [HttpPost("login")]
    [LogSignInAttempts]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        return Ok(ApiResponseHelper.Success(await _authService.LoginAsync(model)));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
    {
        return Ok(ApiResponseHelper.Success(await _authService.RegisterAsync(model)));
    }
    
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
    {
        var userId = GetUserId(Request);
        return Ok(ApiResponseHelper.Success(await _authService.ChangePasswordAsync(model, userId)));
    }
}