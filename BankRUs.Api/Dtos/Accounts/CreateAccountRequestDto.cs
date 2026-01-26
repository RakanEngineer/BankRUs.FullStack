namespace BankRUs.Api.Dtos.Accounts;

// Använd record när: DTO, Command, Query
// Använd class när: entitet
public record CreateAccountRequestDto(
    string FirstName,
    string LastName,
    string SocialSecurityNumber,
    string Email
);