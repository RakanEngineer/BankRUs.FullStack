using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Api.UseCases.Withdrawals
{
    public class CreateWithdrawalHandler
    {
        private readonly ApplicationDbContext _context;

        public CreateWithdrawalHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WithdrawalResponseDto?> Handle(
        Guid bankAccountId,
        CreateWithdrawalRequestDto dto)
        {
            var account = await _context.BankAccounts
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == bankAccountId);

            if (account == null)
                return null;

            // Withdraw domain logic
            account.Withdraw(dto.Amount, dto.Reference ?? "");

            await _context.SaveChangesAsync();

            var transaction = account.Transactions.Last();

            return new WithdrawalResponseDto
            {
                TransactionId = transaction.Id,
                AccountId = account.UserId,
                Type = transaction.Type,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                Reference = transaction.Reference,
                CreatedAt = transaction.CreatedAt,
                BalanceAfter = transaction.BalanceAfter
            };
        }
    }
}
