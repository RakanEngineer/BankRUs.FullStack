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

        public async Task<PagedResponseDto<CustomerItemDto>> Handle(GetCustomerQuery query)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize > _maxPageSize ? _maxPageSize : query.PageSize;

            var customerRoleId = await _context.Roles
                .Where(r => r.Name == Roles.Customer)
                .Select(r => r.Id)
                .SingleAsync();

            var customersQuery =
                from user in _context.Users
                join userRole in _context.UserRoles on user.Id equals userRole.UserId
                where userRole.RoleId == customerRoleId
                select user;

            if (!string.IsNullOrWhiteSpace(query.Ssn))
            {
                customersQuery = customersQuery
                    .Where(u => u.SocialSecurityNumber.StartsWith(query.Ssn));
            }

            var totalItems = await customersQuery.CountAsync();

            var customers = await customersQuery
                .OrderBy(u => u.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new CustomerItemDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                })
                .ToListAsync();

            return new PagedResponseDto<CustomerItemDto>
            {
                Data = customers,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }
    }
}