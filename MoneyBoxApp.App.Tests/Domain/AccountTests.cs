using System;
using Moneybox.App;
using NUnit.Framework;

namespace MoneyBoxApp.App.Tests.Domain
{
    public class AccountTests
    {
       [Test]
        public void ShouldCreditMoneyToAccount()
        {
            const decimal baseValue = 10m;
            var account = (Account)new AccountHelper().WithBalance(baseValue);
            const int amount = 1000;
            
            
            var account1CurrentBalance = account.Balance + amount;
            account.Credit(amount);
            
            Assert.That(account.Balance, Is.EqualTo(account1CurrentBalance));
        }

        [Test]
        public void ShouldCanPayInMoney()
        {
            const int baseValue = 0;
            const int amount = 4001;
            var account = (Account)new AccountHelper().WithBalance(baseValue);
            
            Assert.That(account.CanCredit(amount), Is.EqualTo(false));
        }

        [Test]
        public void ShouldCannotCredit()
        {
            const int baseValue = 0;
            const int amount = 4001;
            
            var account = (Account)new AccountHelper().WithBalance(baseValue);
            
            Assert.That(account.CanCredit(amount), Is.EqualTo(false));
        }

        [Test]
        
        public void ShouldCanCredit()
        {
            const int amount = 4000;
            var account = (Account)new AccountHelper().WithBalance(0);
            Assert.That(account.CanCredit(amount), Is.EqualTo(true));
        }

        [Test]
        public void ShouldCanCreditPaidIn()
        {
            const int baseValue = 0;
            const int amount = 4000;
            var account = (Account)new AccountHelper().WithBalance(baseValue);
            account.Credit(amount);
            Assert.That(account.PaidIn, Is.EqualTo(amount));
        }

        [Test]
        public void ShouldThrowExceptionWhenPayInLimitIsExceeded()
        {
            const int amount = 4001;
            var account = (Account)new AccountHelper().WithBalance(0);
            Assert.Throws<InvalidOperationException>(() => { account.EnsurePayInLimitIsNotExceeded(amount); });
        }

        [Test]
        public void ShouldNotThrowExceptionWhenPayInLimitIsExceeded()
        {
            const int amount = 4000;
            var account = (Account)new AccountHelper().WithBalance(4000);
            Assert.DoesNotThrow(() => { account.EnsurePayInLimitIsNotExceeded(amount); });
        }

        [Test]
        public void ShouldReturnTrueWhenApproachingPayInLimit()
        {
            const int baseAmount = 2000;
            var additionalValue = 1750m;
            var account = (Account)new AccountHelper().WithPaidIn(baseAmount);
            

            Assert.IsTrue(account.IsApproachingPayInLimit(additionalValue));
        }

        [Test]
        public void ShouldReturnFalseWhenNotApproachingPayInLimit()
        {
            const int baseAmount = 2000;
            const decimal additionalValue = 1000m;
            
            var account = (Account)new AccountHelper().WithBalance(baseAmount);
            
            Assert.IsFalse(account.IsApproachingPayInLimit(additionalValue));
        }

        [Test]
        public void ShouldThrowExceptionWhenInsufficientFundsAreAvailable()
        {
            const decimal baseValue = 10m;
            const decimal debitValue = 20m;
            var account = (Account)new AccountHelper().WithBalance(baseValue);
            Assert.Throws<InvalidOperationException>(() => { account.EnsureSufficientFundsAreAvailable(debitValue); });
        }

        [Test]
        public void ShouldNotThrowExceptionWhenSufficientFundsAreAvailable()
        {
            const decimal baseValue = 10m;
            const decimal debitValue = 10m;
            var account = (Account)new AccountHelper().WithBalance(baseValue);
            Assert.DoesNotThrow(() => { account.EnsureSufficientFundsAreAvailable(debitValue); });
        }

        [Test]
        public void ShouldReturnTrueWhenWarningAmountIsExceeded()
        {
            const decimal baseValue = 600m;
            const decimal debitValue = 200m;
            var account = (Account)new AccountHelper().WithBalance(baseValue);


            Assert.IsTrue(account.IsExceedingLowFundsLimitAmount(debitValue));
        }

        [Test]
        public void ShouldReturnFalseWhenWarningAmountIsNotBreached()
        {
            const decimal baseValue = 600m;
            var account = (Account)new AccountHelper().WithBalance(baseValue);

            Assert.IsFalse(account.IsExceedingLowFundsLimitAmount(50m));
        }

        [Test]
        public void ShouldDebitBalance()
        {
            const decimal baseValue = 600m;
            const decimal debitValue = 60m;
            const decimal expectedValue = baseValue - debitValue;
            var account = (Account)new AccountHelper().WithBalance(baseValue);
            account.Debit(debitValue);

            Assert.That(account.Balance, Is.EqualTo(expectedValue));
        }

        [Test]
        public void ShouldReduceWithDrawnBalance()
        {
            const decimal baseValue = 600m;
            const decimal debitValue = 60m;
            const decimal expectedValue = baseValue - debitValue;
            var account = (Account)new AccountHelper().WithWithdrawn(baseValue);
            account.Debit(debitValue);

            Assert.That(account.Withdrawn, Is.EqualTo(expectedValue));
        }
    }
    
}