using Common;
using Common.Helpers;
using Common.Models.DerivedClasses;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Xml.Linq;

namespace Client
{
    public class Program
    {
        public static string BaseUrl = "http://localhost:27001";

        public static HttpClient Client = new HttpClient();

        public static User User = new User();

        public static Admin Admin = new Admin();

        public static string Role = string.Empty;

        static async Task Main(string[] args)
        {

            Console.Clear();

            LoginStartAsync();


            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"\nSuccesfully  Logged as {Role.ToUpper()} !");
            Console.ResetColor();

            Console.ReadKey();
            Console.Clear();

            while (true)
            {
                StartCommand();
            }
        }


        public static async Task AdminViewAsync()
        {
            int count = 1;

            count = AskIntProperty("Count");

            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseUrl}/{HTTPMethods.View}?{Admin.ViewUsersUri(count)}")
            };

            var response = await Client.SendAsync(message);

            List<User> users = JsonSerializer.Deserialize<List<User>>(response.Content.ReadAsStringAsync().Result);

            Console.Clear();
            Console.WriteLine("\n~~~~~User:");

            foreach (var user in users)
            {
                ConsoleHelper.ShowUser(user);
            }

            Console.ReadKey();
        }

        public static async Task UserViewAsync()
        {

            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseUrl}/{HTTPMethods.View}?{User.BalanceRequestUri()}")
            };

            var response = await Client.SendAsync(message);

            float balance = float.Parse(response.Content.ReadAsStringAsync().Result);

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\nBalance of ");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{User.Name}: ");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{balance} ₼");

            Console.ResetColor();
        }

        public static void StartCommandAdmin()
        {
            string command;

            while (true)
            {
                command = AskStringProperty("Command");

                switch (command.ToLower())
                {
                    case HTTPMethods.View:
                        AdminViewAsync();
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invailid Command !");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }

        }

        public static async Task StartCommandUserAsync()
        {
            string command;

            while (true)
            {
                command = AskStringProperty("Command");

                switch (command.ToLower())
                {
                    case HTTPMethods.View:
                        UserViewAsync();
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invailid Command !");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static void StartCommand()
        {
            switch (Role.ToLower())
            {
                case "admin":
                    StartCommandAdmin();
                    break;

                case "user":
                    StartCommandUserAsync();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invailid Role !");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }
        }



        public static async Task StartAdminLoginAsync()
        {
            while (true)
            {

                Admin admin = AskAdmin();

                var message = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseUrl}/{HTTPMethods.Login}?{admin.LoginRequestUri()}")
                };

                var response = await Client.SendAsync(message);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Admin.AuthCode = await response.Content.ReadAsStringAsync();
                    Role = "admin";
                    break;
                }

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Admin not found or inavalid admin data !");
                Console.ResetColor();

                Console.ReadKey();
                Console.Clear();

            }

        }

        public static async Task StartUserLoginAsync()
        {
            while (true)
            {

                User user = AskUser();

                var message = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseUrl}/{HTTPMethods.Login}?{user.LoginRequestUri()}")
                    //RequestUri = new Uri("http://localhost:27001/login")
                };

                Client = new HttpClient();

                var response = await Client.SendAsync(message);

                //await Client.SendAsync(message);
                //var response = Client.GetAsync(@"http://localhost:27001").Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    User = user;
                    Role = "user";
                    break;
                }

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("User not found or inavalid user data !");
                Console.ResetColor();

                Console.ReadKey();
                Console.Clear();

            }
        }

        public static async Task LoginStartAsync()
        {

        LoginStart:
            Console.Clear();
            string role = AskStringProperty("Role (admin & user)");

            switch (role.ToLower())
            {
                case "admin":
                    await StartAdminLoginAsync();
                    break;

                case "user":
                    await StartUserLoginAsync();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"\nRole is not valid !\n");
                    Console.ResetColor();
                    Console.ReadKey();
                    goto LoginStart;
            }
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