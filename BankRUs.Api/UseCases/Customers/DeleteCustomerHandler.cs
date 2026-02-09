using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using static BankRUs.Api.UseCases.Customers.DeleteCustomer;

namespace BankRUs.Api.UseCases.Customers
{
    public class DeleteCustomerHandler
    {
        private readonly ApplicationDbContext _context;

        public DeleteCustomerHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCustomerCommand command)
        {
            var customer = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == command.UserId);

            if (customer == null || customer.IsDeleted)
                return false;

            customer.Delete();

            await _context.SaveChangesAsync();

            return true;
        }
    }
}