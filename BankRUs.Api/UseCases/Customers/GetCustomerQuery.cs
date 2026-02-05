namespace BankRUs.Api.UseCases.Customers
{
    public record GetCustomerQuery(int Page, int PageSize, string? Ssn);

}
