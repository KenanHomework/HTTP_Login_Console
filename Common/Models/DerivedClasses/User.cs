using Common.İnterfaces;
using Common.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.DerivedClasses
{
    public class User : Human, ICheckable
    {

        public float Balance { get; set; } = 0.0f;

        public User() : base() { }

        public User(int id, string name, string password) : base(id, name, password) { }


        public User(int id, string name, string password, float balance) : base(id, name, password) => Balance = balance;

        public string LoginRequestUri()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("&");
            builder.Append($"role=user");

            builder.Append("&");
            builder.Append($"id={Id}");

            builder.Append("&");
            builder.Append($"name={Name}");

            builder.Append("&");
            builder.Append($"password={Password}");

            return builder.ToString();
        }

        public string BalanceRequestUri()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("&");
            builder.Append($"role=user");

            builder.Append("&");
            builder.Append($"id={Id}");

            builder.Append("&");
            builder.Append($"balance");

            return builder.ToString();
        }

        public override string ToString() => $"\nId: {Id} ~~~~Name: {Name} ~~~Password: {Password} ~~~Balance: {Balance}";

        public override bool Equals(object? obj)
        {
            if (obj is not User other)
                return false;

            return other.Name == Name;
        }

        public bool Check()
            => base.Check();

    }
}
