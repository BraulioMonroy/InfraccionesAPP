using Infracciones.App.Models;
using Infracciones.Services.Token;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infracciones.App.Services.Endpoints
{
    public class CashClosingEndpoints
    {
        private string GetByIdUrl { get; set; }
        private string AddUrl { get; set; }

        public CashClosingEndpoints()
        {
            GetByIdUrl = UriFactory.CashClosing.GetById();
            AddUrl = UriFactory.CashClosing.Add();
        }

        public async Task<CashClosingModel> GetById(int id)
        {
            var _accessToken = await TokenService.GetToken();

            CashClosingModel result = new CashClosingModel();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = httpClient.GetAsync(new Uri($"{GetByIdUrl}/{id}")).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<CashClosingModel>(x.Result);
                });
            }

            return result;
        }

        public async Task<CashClosingModel> Add(CashClosingDtoModel entity)
        {
            var _accessToken = await TokenService.GetToken();

            CashClosingModel result = new CashClosingModel();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(entity);
                var response = await httpClient.PostAsync(new Uri(AddUrl), new StringContent(content, Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new Exception("No cuenta con los permisos para realizar esta acción.");
                else if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<CashClosingModel>(x.Result);
                });
            }

            return result;
        }
    }
}
