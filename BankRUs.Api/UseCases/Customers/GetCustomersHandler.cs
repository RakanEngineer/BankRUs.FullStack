using BankRUs.Application.Identity;
using BankRUs.Intrastructure.Identity;
using BankRUs.Intrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BankRUs.Api.UseCases.Customers
{
    public class GetCustomersHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly int _maxPageSize;
        public GetCustomersHandler(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _maxPageSize = configuration.GetValue<int>("Pagination:MaxPageSize");
        }

        public async Task<CustomerListResponseDto> Handle(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize > _maxPageSize ? _maxPageSize : pageSize;

            // All 
            //var query = _context.Users.AsQueryable();
            //query = query.Where(u => u.Email != "customerservice@bank.com");           

            //var totalItems = await query.CountAsync();
            //var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            //var customers = await query
            //    .OrderBy(x => x.FirstName)
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToListAsync();

            ////////////////////////////////////
            // Only customers (Role Filter)
            var customersInRole = await _userManager.GetUsersInRoleAsync(Roles.Customer);

            var totalItems = customersInRole.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var customers = customersInRole
                .OrderBy(x => x.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new CustomerListResponseDto
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,

                Data = customers.Select(c => new CustomerItemDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email
                }).ToList()
            };
        }
    }
}