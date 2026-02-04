using BankRUs.Application.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BankRUs.Intrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        // Seeda data applikation behöver för att fungera
        // däribland roller (Customer, CustomerService, Admin, ...)
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        await SeedRolesAsync(roleManager);

        // Seed CustomerService User
        await SeedCustomerServiceUserAsync(userManager);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        // "Customer", "CustomerService"
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create role '{role}': {errors}");
                }
            }
        }
    }

    private static async Task SeedCustomerServiceUserAsync(UserManager<ApplicationUser> userManager)
    {
        string email = "customerservice@bank.com";
        string password = "Secret#1";

        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser != null)
            return;

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = email,
            Email = email,
            FirstName = "Customer",
            LastName = "Service",
            SocialSecurityNumber = "19800101-9999"
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Failed to create CustomerService user: {errors}");
        }

        // Assign Role
        await userManager.AddToRoleAsync(user, Roles.CustomerService);
    }
}
