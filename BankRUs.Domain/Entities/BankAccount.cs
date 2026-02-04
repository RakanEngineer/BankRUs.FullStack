
using System.ComponentModel.DataAnnotations;

namespace BankRUs.Domain.Entities;

public class BankAccount
{
    // Parameterless constructor for EF Core
    private BankAccount()
    {
        Transactions = new List<Transaction>();
    }

    public BankAccount(string accountNumber, string name, string userId)
    {
        Id = Guid.NewGuid();
        AccountNumber = accountNumber;
        Name = name;
        UserId = userId;
        Balance = 0;
        IsLocked = false;
        Transactions = new List<Transaction>();
    }

    public Guid Id { get; protected set; }

    [MaxLength(25)]
    public string AccountNumber { get; protected set; }
    
    [MaxLength(25)]
    public string Name { get; protected set; } = string.Empty;

    public bool IsLocked { get; protected set; }

    public decimal Balance { get; protected set; }
    public string UserId { get; protected set; }
    public ICollection<Transaction> Transactions { get; private set; }


    public Transaction Deposit(decimal amount, string reference)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        if (IsLocked)
            throw new InvalidOperationException("Cannot deposit to a locked account.");

        Balance += amount;

        var transaction = new Transaction(
            bankAccountId: Id,
            type: "deposit",
            amount: amount,
            reference: reference,
            balanceAfter: Balance

            );
        
        Transactions.Add(transaction);
        return transaction;
    }
    public void Withdraw(decimal amount, string reference) { }
}

// Konstruktor
// Object initializer-syntax


