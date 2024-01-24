using BankSystem.Api.Middleware;
using BankSystem.Data.Models;
using BankSystem.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : BankApiController 
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("myaccount")]
    [LogRequests]
    public async Task<IActionResult> GetMyAccount()
    {
        var userId = GetUserId(Request);
        return Ok(ApiResponseHelper.Success(await _userService.GetMyAccountAsync(userId)));
    }
    
    [HttpGet("mySensitiveData")]
    [LogRequests]
    public async Task<IActionResult> GetMySensitiveData()
    {
        var userId = GetUserId(Request);
        return Ok(ApiResponseHelper.Success(await _userService.GetMySensitiveDataAsync(userId)));
    }
}