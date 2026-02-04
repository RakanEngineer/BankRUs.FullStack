using BankRUs.Api.Dtos.Transactions;
using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BankRUs.Api.UseCases.Deposits
{
    public class CreateDepositHandler
    {
        private readonly ApplicationDbContext _context;
        public CreateDepositHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TransactionResponseDto?> Handle(Guid bankAccountId, DepositRequestDto dto)
        {
            var account = await _context.BankAccounts
                .FirstOrDefaultAsync(x => x.Id == bankAccountId);
            
            if (account == null)
                return null;

            // Deposit Method            
            var transaction = account.Deposit(dto.Amount, dto.Reference ?? "");

            await _context.SaveChangesAsync();

            return new TransactionResponseDto
            {
                TransactionId = transaction.Id,
                UserId = account.UserId,
                Type = transaction.Type,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                Reference = transaction.Reference,
                CreatedAt = transaction.CreatedAt,
                BalanceAfter = transaction.BalanceAfter
            };
            //    var account = await _context.BankAccounts
            //        .FirstOrDefaultAsync(x => x.Id == bankAccountId);

            //    if (account == null)
            //        return null;

            //    // Increase balance
            //    account.Balance += dto.Amount;

            //    // Creation Transaction
            //    var transaction = new Transaction
            //    {
            //        BankAccountId = bankAccountId,
            //        Type = "deposit",
            //        Amount = dto.Amount,
            //        Reference = dto.Reference,
            //        BalanceAfter = account.Balance
            //    };

            //    _context.Transactions.Add(transaction);

            //    await _context.SaveChangesAsync();

            //    return new TransactionResponseDto
            //    {
            //        TransactionId = transaction.Id,
            //        UserId = account.UserId,
            //        Type = transaction.Type,
            //        Amount = transaction.Amount,
            //        Currency = transaction.Currency,
            //        Reference = transaction.Reference,
            //        CreatedAt = transaction.CreatedAt,
            //        BalanceAfter = transaction.BalanceAfter
            //    };
            }
        }
}
