namespace BankRUs.Application.UseCases.OpenAccount;

// POST https://localhost:8000/api/accounts

public class OpenAccountHandler
{
    //private readonly IEmailSender _emailSender;
    //public OpenAccountHandler(IEmailSender emailSender)
    //{
    //    _emailSender = emailSender;
    //}

    public async Task<OpenAccountResult> HandleAsync(OpenAccountCommand command)
    {
        // TODO: Skapa användarkonto (ASP.NET Core Identity)
        //      Delegera till infrastructure
        // TODO: Skapa bankkonto
        //      Delegera till infrastructure
        // TODO: Skicka välkomstmail till kund
        //      Delegera till infrastructure
        // _emailSender.Send("Ditt bankkonto är nu redo!");

        return new OpenAccountResult { 
            CustomerId = 1 
        };
    }
}