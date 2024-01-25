using BankSystem.Api.Middleware;
using BankSystem.Data.Models;
using BankSystem.Data.Models.Transfers;
using BankSystem.Services.Transfers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransfersController : BankApiController
{
    private readonly ITransferService _transferService;

    public TransfersController(ITransferService transferService)
    {
        _transferService = transferService;
    }
    
    [HttpPost("send")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [LogRequests]
    public async Task<IActionResult> Create([FromBody] TransferSendModel model)
    {
        var userId = GetUserId(Request);
        return Ok(ApiResponseHelper.Success(await _transferService.SendAsync(userId, model)));
    }
    
    [HttpGet("history")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [LogRequests]
    public async Task<IActionResult> GetHistory()
    {
        var userId = GetUserId(Request);
        return Ok(ApiResponseHelper.Success(await _transferService.GetHistoryAsync(userId)));
    }
}