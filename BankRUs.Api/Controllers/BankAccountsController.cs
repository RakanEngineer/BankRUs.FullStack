using BankRUs.Api.Dtos.BankAccounts;
using BankRUs.Api.Dtos.Transactions;
using BankRUs.Api.UseCases.Deposits;
using BankRUs.Api.UseCases.Withdrawals;
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

    // POST /api/bank-accounts/{bankAccountId}/deposits
    [HttpPost("{bankAccountId}/deposits")]
    public async Task<IActionResult> Deposit(Guid bankAccountId, [FromBody] DepositRequestDto request,
        [FromServices] CreateDepositHandler handler)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await handler.Handle(bankAccountId, request);

        if (result == null)
            return NotFound();

        return Created("", result);
    }

    [HttpPost("{bankAccountId}/withdrawals")]
    public async Task<IActionResult> Withdraw(Guid bankAccountId, CreateWithdrawalRequestDto dto,
        [FromServices] CreateWithdrawalHandler handler)
    {
        try
        {
            var result = await handler.Handle(bankAccountId, dto);

            if (result == null)
                return NotFound();

            return Created("", result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Insufficient funds",
                Status = 409,
                Detail = ex.Message
            });
        }
    }
}
