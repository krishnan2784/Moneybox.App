using System;
using Moneybox.App;

namespace MoneyBoxApp.App.Tests.Helpers
{
    public class UserHelper
    {
        private string _email = "";
        private Guid _id = Guid.NewGuid();
        private string _name = "";
        public static implicit operator User(UserHelper instance)
        {
            return instance.Build();
        }

        public UserHelper WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public UserHelper WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public UserHelper WithName(string name)
        {
            _name = name;
            return this;
        }

        private User Build()
        {
            return new User(_id, _name, _email);
        }
    }
}