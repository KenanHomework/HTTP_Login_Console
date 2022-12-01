using Common.İnterfaces;
using Common.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.DerivedClasses
{
    public class Admin : Human, ICheckable
    {
        private string authCode = string.Empty;

        public string AuthCode { get => authCode; set => authCode = value; }

        public Admin(int id, string name, string password) : base(id, name, password) { }

        public Admin() : base()
        {

        }

        public string LoggedIn()
        {
            authCode = Guid.NewGuid().ToString().Substring(0, 6);

            return AuthCode;
        }

        public string LoginRequestUri()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("&");
            builder.Append("role=admin");

            builder.Append("&");
            builder.Append($"id={Id}");

            builder.Append("&");
            builder.Append($"name={Name}");

            builder.Append("&");
            builder.Append($"password={Password}");

            return builder.ToString();
        }

        // -1 for get All Users
        public string ViewUsersUri(int count = -1)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("&");
            builder.Append($"role=admin");

            builder.Append("&");
            builder.Append($"authCode={AuthCode}");

            builder.Append("&");
            builder.Append($"count={count}");

            return builder.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj is Admin other)
                return (other.Name == this.Name) && (other.Password == this.Password) && (other.Id == other.Id);

            return base.Equals(obj);
        }

        public bool Check()
            => base.Check() &&
               (!string.IsNullOrWhiteSpace(AuthCode));
    }
}
