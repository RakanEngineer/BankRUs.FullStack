namespace BankRUs.Api.UseCases.Transactions
{
    public class TransactionListResponseDto
    {
        public Guid AccountId { get; set; }
        public string Currency { get; set; } = "SEK";
        public decimal Balance { get; set; }

        public PagingDto Paging { get; set; } = new();

        public List<TransactionItemDto> Items { get; set; } = new();
    }
}
