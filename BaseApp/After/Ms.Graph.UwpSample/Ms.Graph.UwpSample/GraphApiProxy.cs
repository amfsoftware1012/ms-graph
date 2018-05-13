using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Ms.Graph.UwpSample
{
    public static class GraphApiProxy
    {


        private static async Task<GraphServiceClient> GetGraphClient()
        {
            return new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization =
                            new AuthenticationHeaderValue("bearer", await AuthenticationHelper.GetTokenForUserAsync());
                    }));
        }


        public static async Task<IDriveRecentCollectionPage> GetRecentFiles()
        {
            var client = await GetGraphClient();
            return await client.Me.Drive.Recent().Request().GetAsync();
        }


        public static async Task UploadNewFile(string content,string name)
        {
            var token = await AuthenticationHelper.GetTokenForUserAsync();
            using (var httpClient = new HttpClient())
            {
                var stringContent = new StringContent(content);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var httpResult =await httpClient
                    .PutAsync($"https://graph.microsoft.com/v1.0/me/drive/root:/{name}:/content",
                        stringContent);

                httpResult.EnsureSuccessStatusCode();
            }
        }


        public static async Task<Stream> DownloadFile(string fileName)
        {
            var client = await GetGraphClient();
            var files = await client
                .Me.Drive.Root
                .Children.Request().Filter($"name eq '{fileName}'").GetAsync();

            var file = files.FirstOrDefault();
            if (file == null) return null;

           return await client.Me.Drive.Items[file.Id].Content.Request().GetAsync();
        }


    }
}
