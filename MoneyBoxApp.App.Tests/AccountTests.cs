using System;
using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace MoneyBoxApp.App.Tests
{
    public class AccountTests
    {
        private readonly Guid _toAccountId = Guid.NewGuid();
        private Mock<IAccountRepository> _accountRepository;
        private Account _account1;
        private Mock<INotificationService> _notificationService;
        private TransferMoney _transferMoney;
        private const decimal Account1Balance = 6000;
        
        [SetUp]
        public void Setup()
        {
            _account1 = new Account(_toAccountId, Account1Balance) {User = new User(), Paid = 0, PaidIn = 0};
            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository.Setup(a => a.GetAccountById(_toAccountId)).Returns(_account1);
            _notificationService = new Mock<INotificationService>();
            _transferMoney = new TransferMoney(_accountRepository.Object, _notificationService.Object);
        }

        [Test]
        public void CreditMoneyToAccountShould()
        {
            var amount = 1000;
            var account1CurrentBalance = _account1.Balance + amount;
            _account1.Credit(amount);
            
            Assert.That(_account1.Balance, Is.EqualTo(account1CurrentBalance));
        }

        [Test]
        public void CanPayInMoneyShould()
        {
            var amount = 4001;
            Assert.That(_account1.CanCredit(amount), Is.EqualTo(false));
        }

        [Test]
        public void CannotCreditShould()
        {
            const int amount = 4001;
            Assert.That(_account1.CanCredit(amount), Is.EqualTo(false));
        }

        [Test]
        
        public void CanCreditShould()
        {
            const int amount = 4000;
            Assert.That(_account1.CanCredit(amount), Is.EqualTo(true));
        }

        [Test]
        public void CanCreditPaidInShould()
        {
            var amount = 4000;
            _account1.Credit(amount);
            Assert.That(_account1.PaidIn, Is.EqualTo(amount));
        }
    }
    
}