using BankRUs.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BankRUs.Intrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string SocialSecurityNumber { get; set; }
    public List<BankAccount> BankAccounts { get; set; }
    public bool IsDeleted { get; private set; }
    public void Delete()
    {
        IsDeleted = true;
    }
}
