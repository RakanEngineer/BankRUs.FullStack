namespace BankRUs.Api.UseCases.Customers
{
    public class UpdateAccountDetailsCommand
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? SocialSecurityNumber { get; init; }
    }
}
