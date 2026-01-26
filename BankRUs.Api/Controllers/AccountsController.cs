using BankRUs.Api.Dtos.Accounts;
using BankRUs.Application.UseCases.OpenAccount;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BankRUs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly OpenAccountHandler _openAccountHandler;

    public AccountsController(OpenAccountHandler openAccountHandler)
    {
        _openAccountHandler = openAccountHandler;
    }

    // POST /api/accounts (Endpoint /  API endpoint)
    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountRequestDto requestDto)
    {
        var openAccountResult = await _openAccountHandler.HandleAsync(
            new OpenAccountCommand(
                FirstName: requestDto.FirstName,
                LastName: requestDto.LastName,
                SocialSecurityNumber: requestDto.SocialSecurityNumber,
                Email: requestDto.Email));

        var response = new CreateAccountResponseDto(openAccountResult.UserId);

        // Returnera 201 Created
        return Created(string.Empty, response);
    }
}
