namespace BankRUs.Api.UseCases.Transactions
{
    public class TransactionItemDto
    {
        public Guid TransactionId { get; set; }
        public string Type { get; set; } = default!;
        public decimal Amount { get; set; }
        public string? Reference { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal BalanceAfter { get; set; }
    }
}
