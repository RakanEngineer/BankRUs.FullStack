using System.ComponentModel.DataAnnotations;

namespace BankRUs.Api.Dtos.Transactions
{
    public class DepositRequestDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [MaxLength(140)]
        public string? Reference { get; set; }
    }
}
