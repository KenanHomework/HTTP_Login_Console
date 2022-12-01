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

        public static Admin AskAdmin()
        {
            Console.Clear();
            int id = AskIntProperty("Id");

            Console.Clear();
            string name = AskStringProperty("name");

            Console.Clear();
            string password = AskStringProperty("password");

            return new Admin(id, name, password);
        }

        public static User AskUser()
        {
            Console.Clear();
            int id = AskIntProperty("Id");

            Console.Clear();
            string name = AskStringProperty("name");

            Console.Clear();
            string password = AskStringProperty("password");

            return new User(id, name, password);
        }

        public static string AskStringProperty(string propName)
        {
            string value = string.Empty;

            while (true)
            {

                Console.Write($"Enter {propName}: ");
                value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value))
                    break;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n{propName} can't be empty !\n");
                Console.ResetColor();
            }

            return value;
        }

        public static int AskIntProperty(string propName)
        {
            int value = 0;
            string temp;

            while (true)
            {

                Console.Write($"Enter {propName}: ");
                temp = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(temp))
                {
                    try
                    {
                        value = int.Parse(temp);
                        break;
                    }
                    catch (Exception) { }
                }

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n{propName} is not valid !\n");
                Console.ResetColor();
            }

            return value;
        }
    }
}
