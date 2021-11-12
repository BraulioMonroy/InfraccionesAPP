using Infracciones.App.Models;
using Infracciones.App.Services.Endpoints;
using Infracciones.Services.DTO;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infracciones.Services.Endpoints
{
    public class IdentityEndpoints : EndpointService<LoginModel>
    {
        private string LogInUrl { get; set; }
        private string LogOutUrl { get; set; }
        private string RecoverPasswordUrl { get; set; }

        public IdentityEndpoints()
        {
            LogInUrl = UriFactory.Identity.SignIn();
            LogOutUrl = UriFactory.Identity.SignOut();
            RecoverPasswordUrl = UriFactory.Identity.RecoverPassword();
        }

        public async Task<Tuple<TokenResult, Cookie>> Login(LoginModel login)
        {
            try
            {
               


                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.UseCookies = true;
                handler.UseDefaultCredentials = true;
                //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                TokenResult token = default;
                Cookie cookie = default;
                using (var httpClient = new HttpClient(handler))
                {
                    var content = JsonConvert.SerializeObject(login);
                    var response = await httpClient.PostAsync(new Uri($"{LogInUrl}"), new StringContent(content, Encoding.UTF8, "application/json"));

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(await response.Content.ReadAsStringAsync());

                    var result = await response.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<TokenResult>(result);

                    //await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    //{
                    //    if (x.IsFaulted)
                    //        throw x.Exception;

                    //    result = JsonConvert.DeserializeObject<Token>(x.Result);
                    //});

                    cookie = handler.CookieContainer.GetCookies(new Uri(App.App.BaseUri))["refreshToken"];
                }

                return new Tuple<TokenResult, Cookie>(token, cookie);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RecoverPassword(RecoverPasswordDTO recoverPassword)
        {
            var result = true;

            using (var httpClient = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(recoverPassword);
                var response = await httpClient.PostAsync(new Uri($"{RecoverPasswordUrl}"), new StringContent(content, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        result = false;
                    //throw x.Exception;                   
                });
            }

            return result;
        }
    }
}
