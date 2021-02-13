using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationService _notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            _accountRepository = accountRepository;
            _notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var from = _accountRepository.GetAccountById(fromAccountId);
            from.EnsureSufficientFundsAreAvailable(amount);

            if (from.IsExceedingLowFundsLimitAmount(amount))
            {
                _notificationService.NotifyFundsLow(from.User.Email);
            }

            from.Debit(amount);
            
            _accountRepository.Update(from);
        }
    }
}
