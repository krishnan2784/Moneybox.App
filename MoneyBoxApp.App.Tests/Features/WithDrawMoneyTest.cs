using System;
using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using NUnit.Framework;

namespace MoneyBoxApp.App.Tests.Features
{
    [TestFixture]
    public class WithDrawMoneyTest
    {
        WithdrawMoney sut;
        Mock<IAccountRepository> mockAccountRepository;
        Mock<INotificationService> mockNotificationService;

        Guid fromAccountId = Guid.NewGuid();
        Guid fromUserId = Guid.NewGuid();

        User fromUser;

        
    }
}