namespace BankRUs.Api.UseCases.Withdrawals
{
    public class CreateWithdrawalRequestDto
    {
        public decimal Amount { get; set; }
        public string? Reference { get; set; }
    }
}
