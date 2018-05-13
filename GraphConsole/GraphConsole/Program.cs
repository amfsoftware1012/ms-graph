using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GraphConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var token = AuthenticationHelper.GetTokenForUserAsync().Result;
            Console.WriteLine($"Your access token is: {token}");
            Console.ReadLine();
            Console.WriteLine($"Call to get all files in one drive");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var filesJsonResponse = httpClient.GetStringAsync("https://graph.microsoft.com/v1.0/me/drive/root/children").Result;
                Console.WriteLine(filesJsonResponse);
            }
            Console.ReadLine();

        }
    }
}
