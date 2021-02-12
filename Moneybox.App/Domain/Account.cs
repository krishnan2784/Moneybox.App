using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal LowFundsWarningAmount = 500m;
        public const decimal PayInLimit = 4000m;
        public const decimal PayInLimitApproachWarningAmount = 500m;
        public Account(Guid id, User user, decimal balance, decimal withdrawn, decimal paidIn)
        {
            Id = id;
            User = user;
            Balance = balance;
            Withdrawn = withdrawn;
            PaidIn = paidIn;
        }

        public decimal Balance { get; set; }
        public Guid Id { get; set; }

        public decimal Paid { get; set; }
        public decimal PaidIn { get; set; }
        public User User { get; set; }
        public decimal Withdrawn { get; set; }
        
        public bool CanCredit(decimal amount)
        {
            return PaidIn + amount <= PayInLimit;
        }

        public void Credit(decimal amount)
        {
            Balance += amount;
            PaidIn += amount;
        }
        public void Debit(decimal amount)
        {
            Balance -= amount;
            Withdrawn -= amount;
        }

        public void EnsurePayInLimitIsNotExceeded(decimal amount)
        {
            if (amount > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }
        }

        public void EnsureSufficientFundsAreAvailable(decimal amount)
        {
            if (Balance - amount < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }
        }

        public bool IsApproachingPayInLimit(decimal amount)
        {
            return PayInLimit - PaidIn - amount < PayInLimitApproachWarningAmount;
        }
        public bool IsExceedingLowFundsLimitAmount(decimal amount)
        {
            return Balance - amount < LowFundsWarningAmount;
        }
    }
}