using Infracciones.App.Services.Endpoints;
using Infracciones.Services.Token;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infracciones.App.Services.Media
{
    public class MediaFileService
    {
        private string AddUrl { get; set; }

        public MediaFileService()
        {
            AddUrl = UriFactory.Media.Add();
        }

        public async Task AddMedia(int sanctionId, IEnumerable<string> filePaths)
        {
            var _accessToken = await TokenService.GetToken();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                var response = new HttpResponseMessage();

                using (var content = new MultipartFormDataContent())
                {
                    foreach (var filePath in filePaths)
                    {
                        var stream = new FileStream(filePath, FileMode.Open);

                        content.Add(new StreamContent(stream), "Files", Path.GetFileName(filePath));
                    }

                    response = await httpClient.PostAsync(new Uri($"{AddUrl}/{sanctionId}"), content);
                }

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;
                });
            }
        }
    }
}
