// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the bottom of this file.


using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Identity.Client;

namespace Ms.Graph.UwpSample
{
    public class AuthenticationHelper
    {

        static AuthenticationHelper()
        {
            
        }


        public static PublicClientApplication IdentityClientApp = new PublicClientApplication(App.Current.Resources["ida:ClientID"].ToString())
        {
            RedirectUri = App.Current.Resources["ida:ReturnUrl"].ToString()
        };

        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        public static async Task<string> GetTokenForUserAsync()
        {
            AuthenticationResult authResult = null;
            var scopes = new string[]
            {
                "https://graph.microsoft.com/User.Read",
                "https://graph.microsoft.com/User.ReadWrite",
                "https://graph.microsoft.com/User.ReadBasic.All",
                "https://graph.microsoft.com/Mail.Send",
                "https://graph.microsoft.com/Calendars.ReadWrite",
                "https://graph.microsoft.com/Mail.ReadWrite",
                "https://graph.microsoft.com/Files.ReadWrite",
             };

            try
            {
                authResult = await IdentityClientApp.AcquireTokenSilentAsync(scopes, IdentityClientApp.Users.FirstOrDefault());
                
            }
            catch (MsalUiRequiredException)
            {
                authResult = await IdentityClientApp.AcquireTokenAsync(scopes);                
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }

            return authResult?.AccessToken;
        }

        /// <summary>
        /// Signs out the user.
        /// </summary>
        public static void SignOut()
        {
            foreach (var user in IdentityClientApp.Users)
            {
                IdentityClientApp.Remove(user);
            }            
        }

    }
}
