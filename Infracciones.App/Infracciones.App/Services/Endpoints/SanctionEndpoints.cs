using Infracciones.App.Models;
using Infracciones.App.Services.DTO;
using Infracciones.Services.Token;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infracciones.App.Services.Endpoints
{
    public class SanctionEndpoints : EndpointService<SanctionModel>
    {
        private string SearchUrl { get; set; }
        private string GetByOfficerUrl { get; set; }

        public SanctionEndpoints()
        {
            GetByIdUrl = UriFactory.Sanction.GetById();
            AddUrl = UriFactory.Sanction.Add();
            UpdateUrl = UriFactory.Sanction.Update();
            SearchUrl = UriFactory.Sanction.Search();
            GetByOfficerUrl = UriFactory.Sanction.GetByOfficer();
        }

        public async Task<List<SanctionModel>> Search(SanctionSearchDTO sanctionSearch)
        {
            var _accessToken = await TokenService.GetToken();

            // HttpClientHandler handler = new HttpClientHandler();
            // handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            List<SanctionModel> result = new List<SanctionModel>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(sanctionSearch);
                var response = await httpClient.PostAsync(new Uri($"{SearchUrl}"), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<SanctionModel>>(x.Result);
                });
            }

            return result;
        }

        public async Task<List<SanctionModel>> GetByOfficer(string id)
        {
            var _accessToken = await TokenService.GetToken();

            List<SanctionModel> result = new List<SanctionModel>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = httpClient.GetAsync(new Uri($"{GetByOfficerUrl}/{id}")).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<SanctionModel>>(x.Result);
                });
            }

            return result;
        }

    }
}
