﻿using System;
using Moneybox.App;

namespace MoneyBoxApp.App.Tests.Helpers
{
    public class AccountHelper
    {
        private Guid _id = Guid.NewGuid();
        private User _user = new User(Guid.NewGuid(), "", "");
        private readonly AccountSettings _accountSettings = new AccountSettings(500m, 4000m, 500m);
        private decimal _balance;
        private decimal _withdrawn;
        private decimal _paidIn;

        public static implicit operator Account(AccountHelper instance)
        {
            return instance.Build();
        }

        private Account Build()
        {
            return new Account(_id, _user, _balance, _withdrawn, _paidIn, _accountSettings);
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