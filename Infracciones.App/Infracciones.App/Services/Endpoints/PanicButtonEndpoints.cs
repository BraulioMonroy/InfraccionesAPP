using Infracciones.App.Services.Endpoints;
using Infracciones.Models;
using Infracciones.Services.Token;
using Newtonsoft.Json;
using System;
using System.Collections;
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

        public async Task<ArrayList> Add(PanicButtonFocus entity)
        {
            var _accessToken = await TokenService.GetToken();


           // HttpClientHandler handler = new HttpClientHandler();
           // handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };


            ArrayList toJSON = new ArrayList();
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(entity);
                var response = await httpClient.PostAsync(new Uri("http://54.69.118.16:8088/reporteAlarma"), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                string Result = await response.Content.ReadAsStringAsync();
                toJSON = JsonConvert.DeserializeObject<ArrayList>(Result);

                /*await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<PanicButtonResponseModel>(x.Result);
                });*/
            }

            return toJSON;
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
