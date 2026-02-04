namespace BankRUs.Api.UseCases.Withdrawals
{
    public class WithdrawalResponseDto
    {
        public Guid TransactionId { get; set; }
        public string AccountId { get; set; } = default!;
        public string Type { get; set; } = "withdrawal";
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "SEK";
        public string? Reference { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal BalanceAfter { get; set; }
    }
}
