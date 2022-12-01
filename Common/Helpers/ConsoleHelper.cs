using Common.Models.DerivedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class ConsoleHelper
    {

        public static void ShowUser(User user)
        {
            Console.WriteLine();

            ShowProperty("Id",user.Id.ToString());
            ShowProperty("Name",user.Name);
            ShowProperty("Password",user.Password);
            ShowProperty("Balance",user.Balance.ToString() + "₼", ConsoleColor.DarkYellow);

        }

        public static void ShowProperty(string propName, string value,ConsoleColor valueColor = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{propName}: ");
            Console.ForegroundColor = valueColor;
            Console.WriteLine(value);
            Console.ResetColor();
        }


    }
}
