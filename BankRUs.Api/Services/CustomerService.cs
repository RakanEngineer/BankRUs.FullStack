using System.Security.Principal;

namespace BankRUs.Api.Services;

public class CustomerService
{
    private readonly List<Customer> _customers =
    [
        new() { Id = 1, FirstName = "John", LastName = "Doe" },
        new() { Id = 2, FirstName = "Jane", LastName = "Doe" },
        new() { Id = 3, FirstName = "Jim", LastName = "Doe" },
        new() { Id = 4, FirstName = "Jessica", LastName = "Doe" },
    ];

    public IEnumerable<Customer> GetCustomers()
        => _customers;

    public Customer? FindCustomerById(int id)
        => _customers.Find(x => x.Id == id);

    public Customer CreateCustomer(string firstName, string lastName)
    {
        var newCustomer = new Customer
        {
            Id = _customers.Count + 1,
            FirstName = firstName,
            LastName = lastName
        };

        _customers.Add(newCustomer);

        return newCustomer;
    }

    public bool DeleteCustomer(int id)
    {
        var customer = FindCustomerById(id);

        if (customer is null) return false;

        return _customers.Remove(customer);
    }
}
