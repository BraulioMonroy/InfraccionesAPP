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
    public class EndpointService<T> where T : class
    {
        public string GetAllUrl { get; set; }
        public string GetEnabledUrl { get; set; }
        public string GetByIdUrl { get; set; }
        public string AddUrl { get; set; }
        public string UpdateUrl { get; set; }
        public string DeleteUrl { get; set; }

        public async Task<List<T>> GetAll()
        {
            var _accessToken = await TokenService.GetToken();

            List<T> result = new List<T>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await httpClient.GetAsync(new Uri(GetAllUrl));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<T>>(x.Result);
                });
            }

            return result;
        }

        public async Task<List<T>> GetEnabled()
        {
            var _accessToken = await TokenService.GetToken();

            List<T> result = new List<T>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await httpClient.GetAsync(new Uri(GetEnabledUrl));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<List<T>>(x.Result);
                });
            }

            return result;
        }

        public async Task<T> GetById(int id)
        {
            var _accessToken = await TokenService.GetToken();

            T result = default;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await httpClient.GetAsync(new Uri($"{GetByIdUrl}/{id}"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<T>(x.Result);
                });
            }

            return result;
        }

        public async Task<T> Add(T entity)
        {
            var _accessToken = await TokenService.GetToken();

            T result = default;
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

                    result = JsonConvert.DeserializeObject<T>(x.Result);
                });
            }

            return result;
        }

        public async Task<T> Update(int id, T entity)
        {
            var _accessToken = await TokenService.GetToken();

            T result = default;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(entity);
                var response = await httpClient.PutAsync(new Uri($"{UpdateUrl}/{id}"), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<T>(x.Result);
                });
            }

            return result;
        }

        public async Task<T> Update(string id, T entity)
        {
            var _accessToken = await TokenService.GetToken();

            T result = default;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var content = JsonConvert.SerializeObject(entity);
                var response = await httpClient.PutAsync(new Uri($"{UpdateUrl}/{id}"), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<T>(x.Result);
                });
            }

            return result;
        }

        public async Task<T> Delete(int id)
        {
            var _accessToken = await TokenService.GetToken();

            T result = default;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await httpClient.DeleteAsync(new Uri($"{DeleteUrl}/{id}"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;

                    result = JsonConvert.DeserializeObject<T>(x.Result);
                });
            }

            return result;
        }
    }
}
