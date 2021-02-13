using System;

namespace Moneybox.App
{
    public class Account
    {
        public Account(Guid id, User user, decimal balance, decimal withdrawn, decimal paidIn, AccountSettings accountSettings)
        {
            Id = id;
            User = user;
            Balance = balance;
            Withdrawn = withdrawn;
            PaidIn = paidIn;
            AccountSettings = accountSettings;
        }

        public AccountSettings AccountSettings { get; set; }
        public decimal Balance { get; set; }
        public Guid Id { get; set; }

        public decimal Paid { get; set; }
        public decimal PaidIn { get; set; }
        public User User { get; set; }
        public decimal Withdrawn { get; set; }
        
        public bool CanCredit(decimal amount)
        {
            return PaidIn + amount <= AccountSettings.PayInLimit;
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
            if (PaidIn + amount > AccountSettings.PayInLimit)
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
            return AccountSettings.PayInLimit - PaidIn - amount < AccountSettings.PayInLimitApproachWarningAmount;
        }
        public bool IsExceedingLowFundsLimitAmount(decimal amount)
        {
            return Balance - amount < AccountSettings.LowFundsWarningAmount;
        }
    }
}