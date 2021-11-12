using Infracciones.App.Services.Endpoints;
using Infracciones.Models;
using Infracciones.Services.Token;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infracciones.Services.Endpoints
{
    public class PanicButtonEndpoints : EndpointService<PanicButtonResponseModel>
    {
        private string ReleaseUrl;

        public PanicButtonEndpoints()
        {
            AddUrl = UriFactory.PanicButtonRequest.Add();
            ReleaseUrl = UriFactory.PanicButtonRequest.Release();
        }

        public async Task<PanicButtonResponseModel> Add(PanicButtonRequestModel entity)
        {
            var _accessToken = await TokenService.GetToken();

            PanicButtonResponseModel result = default;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(entity);
                var response = await httpClient.PostAsync(new Uri(AddUrl), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<PanicButtonResponseModel>(x.Result);
                });
            }

            return result;
        }

        public async Task<PanicButtonRequestModel> Release(PanicButtonRequestModel entity)
        {
            var _accessToken = await TokenService.GetToken();

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;

            PanicButtonRequestModel result = default;
            using (var httpClient = new HttpClient(handler))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(entity);
                var response = await httpClient.PostAsync(new Uri(ReleaseUrl), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<PanicButtonRequestModel>(x.Result);
                });
            }

            return result;
        }
    }
}
