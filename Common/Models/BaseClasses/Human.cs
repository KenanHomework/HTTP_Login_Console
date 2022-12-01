using Common.İnterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.BaseClasses
{
    public class Human : ICheckable
    {
        public Human(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        public Human()
        {

        }

        public int Id { get; set; } = -1;

        public string Name { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool Check()
            => (Id >= 0) &&
               (!string.IsNullOrWhiteSpace(Name)) &&
               (!string.IsNullOrWhiteSpace(Password));
    }
}
