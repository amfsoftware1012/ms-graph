using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace GraphConsole
{
    public static class AuthenticationHelper
    {
        private static readonly string ApplicationId = ConfigurationManager.AppSettings["ida:AppId"];

        public static PublicClientApplication IdentityClientApp = new PublicClientApplication(ApplicationId)
        {
            RedirectUri = ConfigurationManager.AppSettings["ida:ReturnUrl"]
        };
        
        public static async Task<string> GetTokenForUserAsync()
        {
            var scopes = new string[]
            {
                "https://graph.microsoft.com/User.ReadBasic.All",
                "https://graph.microsoft.com/Files.ReadWrite"
            };
            var authResult = await IdentityClientApp.AcquireTokenAsync(scopes);
            return authResult.AccessToken;
        }
    }
}
