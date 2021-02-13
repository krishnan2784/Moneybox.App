using System;
using Moneybox.App;

namespace MoneyBoxApp.App.Tests
{
    public class UserHelper
    {
        Guid id = Guid.NewGuid();
        string name = "";
        string email = "";

        public static implicit operator User(UserHelper instance)
        {
            return instance.Build();
        }

        public User Build()
        {
            return new User(id, name, email);
        }

        public UserHelper WithId(Guid id)
        {
            this.id = id;
            return this;
        }

        public UserHelper WithName(string name)
        {
            this.name = name;
            return this;
        }

        public UserHelper WithEmail(string email)
        {
            this.email = email;
            return this;
        }
    }
}