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
    public class SubBrandEndpoints : EndpointService<SubBrandModel>
    {
        private string GetByBrandIdUrl { get; set; }

        public SubBrandEndpoints()
        {
            GetByIdUrl = UriFactory.SubBrand.GetById();
            GetByBrandIdUrl = UriFactory.SubBrand.GetByBrand();
        }

        public async Task<List<SubBrandModel>> GetByBrandId(int id)
        {
            var _accessToken = await TokenService.GetToken();

            List<SubBrandModel> result = new List<SubBrandModel>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = httpClient.GetAsync(new Uri($"{GetByBrandIdUrl}/{id}")).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<SubBrandModel>>(x.Result);
                });
            }

            return result;
        }
    }
}
