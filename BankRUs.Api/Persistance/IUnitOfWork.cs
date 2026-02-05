namespace BankRUs.Api.Persistance
{
    public interface IUnitOfWork
    {
        Task SaveAsync();

    }
}
