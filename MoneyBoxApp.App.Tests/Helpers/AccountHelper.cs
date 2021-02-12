using System;
using Moneybox.App;

namespace MoneyBoxApp.App.Tests
{
    public class AccountHelper
    {
        private Guid _id = Guid.NewGuid();
        private User _user = new User(Guid.NewGuid(), "", "");
        private decimal _balance = 0;
        private decimal _withdrawn = 0;
        private decimal _paidIn = 0;

        public static implicit operator Account(AccountHelper instance)
        {
            return instance.Build();
        }

        private Account Build()
        {
            return new Account(_id, _user, _balance, _withdrawn, _paidIn);
        }

        public AccountHelper WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public AccountHelper WithUser(User user)
        {
            _user = user;
            return this;
        }

        public AccountHelper WithBalance(decimal balance)
        {
            _balance = balance;
            return this;
        }

        public AccountHelper WithWithdrawn(decimal withdrawn)
        {
            _withdrawn = withdrawn;
            return this;
        }

        public AccountHelper WithPaidIn(decimal paidIn)
        {
            _paidIn = paidIn;
            return this;
        }
    }
}