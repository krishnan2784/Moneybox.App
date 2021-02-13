using System;
using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using MoneyBoxApp.App.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace MoneyBoxApp.App.Tests.Features
{
    [TestFixture]
    public class WithDrawMoneyTest
    {
        readonly Guid _fromAccountId = Guid.NewGuid();
        readonly Guid _fromUserId = Guid.NewGuid();
        private AccountHelper _accountHelper;
        User _fromUser;
        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<INotificationService> _mockNotificationService;
        private WithdrawMoney _sut;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _fromUser = new UserHelper().WithId(_fromUserId).WithEmail("from@user.com");
        }

        [SetUp]
        public void SetUp()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockNotificationService = new Mock<INotificationService>();

            _accountHelper = new AccountHelper().WithId(_fromAccountId).WithUser(_fromUser);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(_accountHelper);

            _sut = new WithdrawMoney(_mockAccountRepository.Object, _mockNotificationService.Object);
        }

        [Test]
        public void ShouldDebitFromAccount()
        {
            Account fromAccount = _accountHelper.WithBalance(5000m).WithWithdrawn(0m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);

            _sut.Execute(_fromAccountId, 250m);

            Assert.AreEqual(4750m, fromAccount.Balance);
            Assert.AreEqual(-250m, fromAccount.Withdrawn);
        }

        [Test]
        public void ShouldGetFromAccount()
        {
            _sut.Execute(_fromAccountId, 0m);

            _mockAccountRepository.Verify(m => m.GetAccountById(_fromAccountId), Times.Once());
        }

        [Test]
        public void ShouldNotifyWhenFundsAreLow()
        {
            Account fromAccount = _accountHelper.WithBalance(1000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);

            _sut.Execute(_fromAccountId, 750m);

            _mockNotificationService.Verify(m => m.NotifyFundsLow(_fromUser.Email), Times.Once());
        }

        [Test]
        public void ShouldNotNotifyWhenFundsAreNotLow()
        {
            Account fromAccount = _accountHelper.WithBalance(1000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);

            _sut.Execute(_fromAccountId, 100m);

            _mockNotificationService.Verify(m => m.NotifyFundsLow(_fromUser.Email), Times.Never());
        }

        [Test]
        public void ShouldThrowExceptionWhenSufficientFundsAreNotAvailable()
        {
            Assert.Throws<InvalidOperationException>(() => { _sut.Execute(_fromAccountId, 100m); });
        }
        [Test]
        public void ShouldUpdateFromAccount()
        {
            Account fromAccount = _accountHelper.WithBalance(5000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);

            _sut.Execute(_fromAccountId, 250m);

            _mockAccountRepository.Verify(m => m.Update(fromAccount), Times.Once());
        }
    }
}