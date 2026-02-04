namespace BankRUs.Api.UseCases.Customers
{
    public class CustomerDetailsDto
    {
        public string Id { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<CustomerBankAccountDto> BankAccounts { get; set; } = new();
    }
    public class CustomerBankAccountDto
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
    }
}
