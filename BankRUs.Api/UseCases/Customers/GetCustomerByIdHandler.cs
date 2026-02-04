using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Api.UseCases.Customers
{
    public class GetCustomerByIdHandler
    {
        private readonly ApplicationDbContext _context;
        public GetCustomerByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CustomerDetailsDto?> Handle(string customerId)
        {            
            var customer = await _context.Users
                .Where(u => u.Id == customerId)
                .Select(u => new CustomerDetailsDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    BankAccounts = u.BankAccounts.Select(ba => new CustomerBankAccountDto
                    {
                        Id = ba.Id,
                        Balance = ba.Balance
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            return customer;
        }
    }
}
