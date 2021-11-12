using Infracciones.App.Models;
using Infracciones.Services.DTO;
using Infracciones.Services.Token;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infracciones.App.Services.Endpoints
{
    public class PaymentEndpoints : EndpointService<PaymentModel>
    {
        private string GetByReferenceUrl { get; set; }
        private string GetByOfficerUrl { get; set; }
        private string GetOpenByStatusAndOfficerUrl { get; set; }
        private string PayUrl { get; set; }
        private string GetByStatusUrl { get; set; }
        private string CloseUrl { get; set; }

        public PaymentEndpoints()
        {
            GetByIdUrl = UriFactory.Payment.GetById();
            GetByOfficerUrl = UriFactory.Payment.GetByOfficer();
            GetByStatusUrl = UriFactory.Payment.GetByStatus();
            PayUrl = UriFactory.Payment.Update();
            GetByReferenceUrl = UriFactory.Payment.GetById();
            CloseUrl = UriFactory.Payment.Close();
        }

        public async Task<List<PaymentModel>> GetByOfficer(string officerId)
        {
            var _accessToken = await TokenService.GetToken();

            List<PaymentModel> result = new List<PaymentModel>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = httpClient.GetAsync(new Uri($"{GetByOfficerUrl}/{officerId}")).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<PaymentModel>>(x.Result);
                });
            }

            return result;
        }

        public async Task<List<PaymentModel>> GetOpenByStatusAndOfficer(bool open, string status, string officerId)
        {
            var _accessToken = await TokenService.GetToken();

            GetOpenByStatusAndOfficerUrl = UriFactory.Payment.GetOpenByStatusAndOfficer(open, status, officerId);

            List<PaymentModel> result = new List<PaymentModel>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = httpClient.GetAsync(new Uri(GetOpenByStatusAndOfficerUrl)).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<PaymentModel>>(x.Result);
                });
            }

            return result;

        }
        public async Task<PaymentModel> GetByReference(string paymentReference)
        {
            var _accessToken = await TokenService.GetToken();

            var result = new PaymentModel();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = httpClient.GetAsync(new Uri($"{GetByReferenceUrl}/{paymentReference}")).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<PaymentModel>(x.Result);
                });
            }

            return result;
        }


        //TODO: Declare obsolete and replace by GetByOfficer when production
        public async Task<List<PaymentModel>> GetSanctionsPaid()
        {
            var _accessToken = await TokenService.GetToken();
            var status = "Pagado";

            List<PaymentModel> result = new List<PaymentModel>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = httpClient.GetAsync(new Uri($"{GetByStatusUrl}/{status}")).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<PaymentModel>>(x.Result);
                });
            }

            return result;
        }

        public async Task<PaymentModel> Pay(string paymentReference, PaymentModel entity)
        {
            var _accessToken = await TokenService.GetToken();

            PaymentModel result = default;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(entity);
                var response = await httpClient.PutAsync(new Uri($"{PayUrl}/{paymentReference}"), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<PaymentModel>(x.Result);
                });
            }

            return result;
        }

        public async Task<IEnumerable<PaymentDTO>> Close(IEnumerable<PaymentDTO> payments)
        {
            var _accessToken = await TokenService.GetToken();

            List<PaymentDTO> result = default;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(payments);
                var response = await httpClient.PostAsync(new Uri($"{CloseUrl}"), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<PaymentDTO>>(x.Result);
                });
            }

            return result;
        }

        public async Task<PaymentDTO> Close(int sanctionId, PaymentDTO payment)
        {
            var _accessToken = await TokenService.GetToken();

            PaymentDTO result = default;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(payment);
                var response = await httpClient.PutAsync(new Uri($"{CloseUrl}/{sanctionId}"), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<PaymentDTO>(x.Result);
                });
            }

            return result;
        }
    }
}
