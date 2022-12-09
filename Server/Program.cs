using Common;
using Common.Models.BaseClasses;
using Common.Models.DerivedClasses;
using Common.Services;
using System.Net;
using System.Text.Json;

namespace Server
{
    public class Program
    {
        public static string BaseUrl = "http://localhost:27001/";

        public static List<User> Users = new List<User>();

        public static Admin Admin = new Admin(0, "Admin", "Admin123");

        public static HttpListenerContext Context;
        public static HttpListenerRequest Request;
        public static HttpListenerResponse Response;

        static async Task Main(string[] args)
        {

            Users.Add(new User()
            {
                Id = 0,
                Name = "kanan",
                Password = "123"
            });

            Users.Add(new User()
            {
                Id = 1,
                Name = "a",
                Password = "a"
            });

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(BaseUrl);
            listener.Start();

            while (true)
            {
                Context = listener.GetContext();
                Request = Context.Request;
                Response = Context.Response;

                CallCommand();
            }
        }

        public static void LoginUser()
        {
            User user = new User();

            user.Id = int.Parse(Request.QueryString["id"]);
            user.Name = Request.QueryString["name"];
            user.Password = Request.QueryString["password"];

            User? findResult = Users.Find(u =>
                                          {
                                              return (u.Id == user.Id) &&
                                                     (u.Name == user.Name) &&
                                                     (u.Password == user.Password);
                                          });

            if (findResult != null &&  findResult.Check())
                Response.StatusCode = 200;
            else
                Response.StatusCode = 404;

            Response.Close();
        }

        public static void LoginAdmin()
        {

            Admin admin = new Admin();

            admin.Id = int.Parse(Request.QueryString["id"]);
            admin.Name = Request.QueryString["name"];
            admin.Password = Request.QueryString["password"];

            if (Admin.Equals(admin))
            {
                Response.StatusCode = 200;
                Admin.LoggedIn();
                StreamWriter writer = new StreamWriter(Response.OutputStream);
                writer.Write(Admin.AuthCode);
                writer.Close();
                return;
            }
            else
                Response.StatusCode = 404;

            Response.Close();

        }

        // 203 Non-Authoritative Information

        public static void LoginCall()
        {
            string role = Request.QueryString["role"];

            switch (role)
            {
                case "user":
                    LoginUser();
                    break;

                case "admin":
                    LoginAdmin();
                    break;

                default:
                    Response.StatusCode = 405;
                    Response.Close();
                    break;
            }

        }

        public static void AdminView()
        {
            int count = int.Parse(Request.QueryString["count"]);
            string autCode = Request.QueryString["authCode"];

            if (autCode != Admin.AuthCode)
            {
                Response.StatusCode = 412;
                Response.Close();
            }

            if (count < 0 || count > Users.Count)
                count = Users.Count;

            StreamWriter writer = new StreamWriter(Response.OutputStream);
            writer.Write(JsonSerializer.Serialize(Users.Take(count)));
            writer.Close();
        }

        public static void UserView()
        {
            int id = int.Parse(Request.QueryString["id"]);

            StreamWriter writer = new StreamWriter(Response.OutputStream);
            float balance = Users.Where((u) => { return u.Id == id; }).Select(u => u.Balance).FirstOrDefault();
            writer.Write(balance);
            writer.Close();
        }

        public static void ViewCall()
        {
            string role = Request.QueryString["role"];

            switch (role)
            {
                case "user":
                    UserView();
                    break;

                case "admin":
                    AdminView();
                    break;

                default:
                    Response.StatusCode = 405;
                    Response.Close();
                    break;
            }
        }

        public static void CallCommand()
        {
            switch (QueryService.GetRequestMethod(Request.RawUrl).ToLower())
            {
                case HTTPMethods.Login:
                    LoginCall();
                    break;
                case HTTPMethods.View:
                    ViewCall();
                    break;

                default:
                    break;
            }
        }


    }
}