using BankRUs.Api.Persistance;
using BankRUs.Api.UseCases.Customers;

namespace BankRUs.Api.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerItemDto?> GetCustomerByIdAsync(Guid id)
      => await _customerRepository.GetByIdAsync(id);

        public async Task UpdateCustomer(CustomerItemDto customer)
        {
            await _customerRepository.Update(customer);
            await _unitOfWork.SaveAsync();
        }
    }
}
