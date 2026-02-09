namespace BankRUs.Api.UseCases.Customers
{
    public class DeleteCustomer
    {
        public record DeleteCustomerCommand(string UserId);
    }
}