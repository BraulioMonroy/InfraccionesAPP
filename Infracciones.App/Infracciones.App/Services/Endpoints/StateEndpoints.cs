using Infracciones.App.Models;
using Infracciones.Services.Token;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infracciones.App.Services.Endpoints
{

    public class StateEndpoints : EndpointService<StateModel>
    {
        private string GetByCountryIdUrl { get; set; }

        public StateEndpoints()
        {
            GetByIdUrl = UriFactory.State.GetById();
            GetByCountryIdUrl = UriFactory.State.GetByCountry();
        }

        public async Task<List<StateModel>> GetByCountryId(int id)
        {
            var _accessToken = await TokenService.GetToken();

            List<StateModel> result = new List<StateModel>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = httpClient.GetAsync(new Uri($"{GetByCountryIdUrl}/{id}")).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<StateModel>>(x.Result);
                });
            }

            return result;
        }
    }
}
