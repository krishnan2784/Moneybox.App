using System;

namespace Moneybox.App
{
    public class User
    {
       

        public User(Guid id, string name, string email)
        {
            Name = name;
            Email = email;
            Id = id;
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
