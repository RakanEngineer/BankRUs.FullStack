using BankRUs.Api.Dtos.BankAccounts;
using BankRUs.Api.Dtos.Transactions;
using BankRUs.Api.UseCases.Deposits;
using BankRUs.Api.UseCases.Transactions;
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

    // GET /api/bank-accounts/{bankAccountId}/transactions
    [HttpGet("{bankAccountId}/transactions")]
    public async Task<IActionResult> GetTransactions(
        Guid bankAccountId,
        [FromServices] GetTransactionsHandler handler,        
        int page = 1,
        int pageSize = 20,
        string sort = "desc",
        string? type = null,
        DateTime? from = null,
        DateTime? to = null)
    {
        var result = await handler.Handle(
            bankAccountId,
            page,
            pageSize,
            sort,
            type,
            from,
            to);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

}
