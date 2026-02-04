using BankRUs.Domain.Entities;
using BankRUs.Intrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Intrastructure.Persistance
{
    public static class SeedData
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            if (context.Users.Any()) return;

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                SocialSecurityNumber = "19900101-2013",
                Email = "johndoe@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "Password123!");
            context.Users.Add(user);

            //var account = await context.BankAccounts
            //    .FirstOrDefaultAsync(x => x.UserId == user.Id);

            var bankAccount = new BankAccount(
                accountNumber: "1234567890",
                name: "John Doe Account",
                userId: user.Id
            );

            context.BankAccounts.Add(bankAccount);

            bankAccount.Deposit(500.00m, "Initial Deposit");

            await context.SaveChangesAsync();
        }
    }
}
