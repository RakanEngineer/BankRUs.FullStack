using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Domain.Entities
{
    public class Transaction
    {
        // Parameterless constructor for EF Core
        private Transaction() { }

        // Constructor for creating new transactions
        public Transaction(Guid bankAccountId, string type, decimal amount, string? reference, decimal balanceAfter)
        {
            Id = Guid.NewGuid();
            BankAccountId = bankAccountId;
            Type = type;
            Amount = amount;
            Reference = reference;
            BalanceAfter = balanceAfter;
            CreatedAt = DateTime.UtcNow;
            Currency = "SEK";
        }

        public Guid Id { get; private set; }
        public Guid BankAccountId { get; private set; }
        public BankAccount BankAccount { get; private set; } = null!;
        public string Type { get; private set; } = string.Empty;
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "SEK";
        public string? Reference { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal BalanceAfter { get; private set; }
        
    }
}
