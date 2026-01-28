using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.Intrastructure.Identity;
using BankRUs.Intrastructure.Persistance;
using BankRUs.Intrastructure.Repositories;
using BankRUs.Intrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IEmailSender = BankRUs.Application.Services.IEmailSender;

var builder = WebApplication.CreateBuilder(args);

//// Samma instans av CustomerService ska vara tillgänglig
//// för samtliga klasser inom ett anrop.
//// Varje request får sin egna instans av CustomerService
//builder.Services.AddScoped<CustomerService>();

// Det finns enbart en instans av CustomerService som delas
// av alla komponenter i applikationen, över applikations livstid.

//// Varje enskild komponent som begär en CustomerService får sin egna
//// instans av denna.
//builder.Services.AddTransient<CustomerService>();

// Registrera ApplicationDbContext i DI-containern
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();

// Command/Query handlers
builder.Services.AddScoped<OpenAccountHandler>();
builder.Services.AddScoped<OpenBankAccountHandler>();

// Services
builder.Services.AddScoped<IIdentityService, IdentityService>();

if (builder.Environment.IsDevelopment())
{
    // Utveckling
    builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
}
else
{
    // Produktion
    builder.Services.AddScoped<IEmailSender, EmailSender>();
}

// Repositories
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();

// 3 typer av livslängder på objekt
// - singleton = ett och samma objekt delas mellan alla andra under hela applikations livslängd
// - scoped = varje HTTP-reqeust får sin egen isntans som sen delas av alla objekt inom denna request
// - transitent = varje objekt får alltid sin egna instans av typen

builder.Services
  .AddIdentity<ApplicationUser, IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();

    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
