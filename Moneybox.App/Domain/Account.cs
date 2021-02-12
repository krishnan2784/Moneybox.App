using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;

        public Account(Guid id, decimal balance)
        {
            Id = id;
            Balance = balance;
        }
        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }
        public decimal Paid { get; set; }

        public decimal PaidIn { get; set; }


        public decimal Credit(decimal amount)
        {
            if (amount > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }
            Balance += amount;
            PaidIn += amount;
            return Balance;
        }

        public bool CanCredit(decimal amount)
        {
            return PaidIn + amount <= PayInLimit;
        }
    }
}
