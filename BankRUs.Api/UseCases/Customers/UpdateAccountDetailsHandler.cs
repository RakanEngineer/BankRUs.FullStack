using BankRUs.Intrastructure.Identity;
using BankRUs.Intrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Api.UseCases.Customers
{
    public class UpdateAccountDetailsHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateAccountDetailsHandler(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Handle(UpdateAccountDetailsCommand command)
        {
            var userId = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                .Value;

            if (userId == null)
                return false;

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return false;

            if (command.FirstName != null)
                user.FirstName = command.FirstName;

            if (command.LastName != null)
                user.LastName = command.LastName;

            if (command.Email != null)
                user.Email = command.Email;

            if (command.SocialSecurityNumber != null)
                user.SocialSecurityNumber = command.SocialSecurityNumber;

            //await _context.SaveChangesAsync();
            await _userManager.UpdateAsync(user);


            return true;
        }
    }
}
