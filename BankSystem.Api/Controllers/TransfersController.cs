using BankSystem.Data.Models;
using BankSystem.Services.Transfers;
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
    public async Task<IActionResult> Create()
    {
        var userId = GetUserId(Request);
        return Ok(ApiResponseHelper.Success());
    }
    
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var userId = GetUserId(Request);
        return Ok(ApiResponseHelper.Success());
    }
}