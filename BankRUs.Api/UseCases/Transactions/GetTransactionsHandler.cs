using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Api.UseCases.Transactions
{
    public class GetTransactionsHandler
    {
        private readonly ApplicationDbContext _context;
        public GetTransactionsHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TransactionListResponseDto?> Handle(
        Guid bankAccountId,
        int page = 1,
        int pageSize = 20,
        string sort = "desc",
        string? type = null,
        DateTime? from = null,
        DateTime? to = null)
        {
            var account = await _context.BankAccounts
                .FirstOrDefaultAsync(x => x.Id == bankAccountId);

            if (account == null)
                return null;

            var query = _context.Transactions
                .Where(t => t.BankAccountId == bankAccountId);

            // Filter by type
            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(t => t.Type == type);

            // Filter by date
            if (from.HasValue)
                query = query.Where(t => t.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(t => t.CreatedAt <= to.Value);

            // Sorting
            query = sort == "asc"
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt);

            // Pagination
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var transactions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new TransactionListResponseDto
            {
                AccountId = account.Id,
                Balance = account.Balance,
                Currency = "SEK",

                Paging = new PagingDto
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                },

                Items = transactions.Select(t => new TransactionItemDto
                {
                    TransactionId = t.Id,
                    Type = t.Type,
                    Amount = t.Amount,
                    Reference = t.Reference,
                    CreatedAt = t.CreatedAt,
                    BalanceAfter = t.BalanceAfter
                }).ToList()
            };
        }
    }
}
