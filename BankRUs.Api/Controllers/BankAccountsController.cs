using BankRUs.Api.Dtos.BankAccounts;
using BankRUs.Application.UseCases.OpenBankAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers;

[Route("api/bank-accounts")]
[ApiController]
public class BankAccountsController : ControllerBase
{
    private readonly OpenBankAccountHandler _openBankAccountHandler;

    public BankAccountsController(OpenBankAccountHandler openBankAccountHandler)
    {
        _openBankAccountHandler = openBankAccountHandler;
    }

    // POST /api/bank-accounts
    // {
    //    "userId": "",
    //    "bankAccountName": "Semester"
    // }
    [HttpPost]
    public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequestDto request)
    {
        var openBankAccountResult = await _openBankAccountHandler.HandleAsync(
            new OpenBankAccountCommand(UserId: request.UserId));

        // TODO: Hårdkodad information nedan ska komma från 
        // resultatobjektet
        var response = new BankAccountDto(
            Id: openBankAccountResult.Id,
            AccountNumber: "100.200.300",
            Name: "Standardkonto",
            IsLocked: false,
            Balance: 0m,
            UserId: Guid.NewGuid());

        return Created(string.Empty, response);
    }
}
