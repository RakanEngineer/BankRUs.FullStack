using BankRUs.Api.UseCases.Customers;

namespace BankRUs.Api.Persistance
{
    public interface ICustomerRepository
    {
        Task<CustomerItemDto?> GetByIdAsync(Guid id);
        Task Update(CustomerItemDto customer);

    }
}
