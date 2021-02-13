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
    public class TransferMoneyTest
    {
        private TransferMoney _sut;
        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<INotificationService> _mockNotificationService;

        private readonly Guid _fromAccountId = Guid.NewGuid();
        private readonly Guid _toAccountId = Guid.NewGuid();
        private readonly Guid _fromUserId = Guid.NewGuid();
        private readonly Guid _toUserId = Guid.NewGuid();

        private User _fromUser;
        private User _toUser;

        private AccountHelper _fromAccount;
        private AccountHelper _toAccount;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _fromUser = new UserHelper().WithId(_fromUserId).WithEmail("from@testaccount.com");
            _toUser = new UserHelper().WithId(_toUserId).WithEmail("to@testaccount.com");
        }

        [SetUp]
        public void SetUp()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockNotificationService = new Mock<INotificationService>();

            _fromAccount= new AccountHelper().WithId(_fromAccountId).WithUser(_fromUser);
            _toAccount = new AccountHelper().WithId(_toAccountId).WithUser(_toUser);

            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(_fromAccount);
            _mockAccountRepository.Setup(m => m.GetAccountById(_toAccountId)).Returns(_toAccount);

            _sut = new TransferMoney(_mockAccountRepository.Object, _mockNotificationService.Object);
        }

        [Test]
        public void ShouldGetFromAccount()
        {
            _sut.Execute(_fromAccountId, _toAccountId, 0m);

            _mockAccountRepository.Verify(m => m.GetAccountById(_fromAccountId), Times.Once());
        }

        [Test]
        public void ShouldGetToAccount()
        {
            _sut.Execute(_fromAccountId, _toAccountId, 0m);

            _mockAccountRepository.Verify(m => m.GetAccountById(_toAccountId), Times.Once());
        }

        [Test]
        public void ShouldThrowExceptionWhenSufficientFundsAreNotAvailable()
        {
            Assert.Throws<InvalidOperationException>(() => { _sut.Execute(_fromAccountId, _toAccountId, 100m); });
        }

        [Test]
        public void ShouldNotifyWhenFundsAreLow()
        {
            Account fromAccount = _fromAccount.WithBalance(1000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);

            _sut.Execute(_fromAccountId, _toAccountId, 750m);

            _mockNotificationService.Verify(m => m.NotifyFundsLow(_fromUser.Email), Times.Once());
        }

        [Test]
        public void ShouldNotNotifyWhenFundsAreNotLow()
        {
            Account fromAccount = _fromAccount.WithBalance(1000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);

            _sut.Execute(_fromAccountId, _toAccountId, 100m);

            _mockNotificationService.Verify(m => m.NotifyFundsLow(_fromUser.Email), Times.Never());
        }

        [Test]
        public void ShouldThrowExceptionWhenPayInLimitIsExceeded()
        {
            Account fromAccount = _fromAccount.WithBalance(5000m);
            Account toAccount = _toAccount.WithPaidIn(3000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);
            _mockAccountRepository.Setup(m => m.GetAccountById(_toAccountId)).Returns(toAccount);

            Assert.Throws<InvalidOperationException>(() => { _sut.Execute(_fromAccountId, _toAccountId, 1500m); });
        }

        [Test]
        public void ShouldNotifyWhenApproachingPayInLimit()
        {
            Account fromAccount = _fromAccount.WithBalance(5000m);
            Account toAccount = _toAccount.WithPaidIn(3000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);
            _mockAccountRepository.Setup(m => m.GetAccountById(_toAccountId)).Returns(toAccount);

            _sut.Execute(_fromAccountId, _toAccountId, 750m);

            _mockNotificationService.Verify(m => m.NotifyApproachingPayInLimit(_toUser.Email), Times.Once());
        }

        [Test]
        public void ShouldNotNotifyWhenNotApproachingPayInLimit()
        {
            Account fromAccount = _fromAccount.WithBalance(5000m);
            Account toAccount = _toAccount.WithPaidIn(3000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);
            _mockAccountRepository.Setup(m => m.GetAccountById(_toAccountId)).Returns(toAccount);

            _sut.Execute(_fromAccountId, _toAccountId, 250m);

            _mockNotificationService.Verify(m => m.NotifyApproachingPayInLimit(_toUser.Email), Times.Never());
        }

        [Test]
        public void ShouldDebitFromAccount()
        {
            Account fromAccount = _fromAccount.WithBalance(5000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);

            _sut.Execute(_fromAccountId, _toAccountId, 250m);

            Assert.AreEqual(4750m, fromAccount.Balance);
            Assert.AreEqual(-250m, fromAccount.Withdrawn);
        }

        [Test]
        public void ShouldCreditToAccount()
        {
            Account fromAccount = _fromAccount.WithBalance(5000m);
            Account toAccount = _toAccount.WithBalance(3000m).WithPaidIn(3000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);
            _mockAccountRepository.Setup(m => m.GetAccountById(_toAccountId)).Returns(toAccount);

            _sut.Execute(_fromAccountId, _toAccountId, 250m);

            Assert.AreEqual(3250m, toAccount.Balance);
            Assert.AreEqual(3250m, toAccount.PaidIn);
        }

        [Test]
        public void ShouldUpdateFromAccount()
        {
            Account fromAccount = _fromAccount.WithBalance(5000m);
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);

            _sut.Execute(_fromAccountId, _toAccountId, 250m);

            _mockAccountRepository.Verify(m => m.Update(fromAccount), Times.Once());
        }

        [Test]
        public void ShouldUpdateToAccount()
        {
            Account fromAccount = _fromAccount.WithBalance(5000m);
            Account toAccount = _toAccount;
            _mockAccountRepository.Setup(m => m.GetAccountById(_fromAccountId)).Returns(fromAccount);
            _mockAccountRepository.Setup(m => m.GetAccountById(_toAccountId)).Returns(toAccount);

            _sut.Execute(_fromAccountId, _toAccountId, 250m);

            _mockAccountRepository.Verify(m => m.Update(toAccount), Times.Once());
        }
    }

}
