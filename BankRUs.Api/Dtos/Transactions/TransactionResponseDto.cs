using System.ComponentModel.DataAnnotations;

namespace BankRUs.Api.Dtos.Transactions
{    
    public class TransactionResponseDto
    {
        public Guid TransactionId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "SEK";

        public string? Reference { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal BalanceAfter { get; set; }
    }
    public class TransactionListItemDto
    {
        public Guid TransactionId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Reference { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal BalanceAfter { get; set; }
    }
}
