using Infracciones.App.Models;
using Infracciones.App.Services.Endpoints;
using Infracciones.Helpers;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnixTimeStamp;
using Xamarin.Essentials;

namespace Infracciones.Services.Token
{
    public static class TokenService
    {
        public static async Task<string> GetToken()
        {
            var accessToken = Preferences.Get("accessToken", string.Empty);

            if (string.IsNullOrWhiteSpace(accessToken))
                return await RefreshToken();

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(accessToken);
            if (DateTime.UtcNow > jwt.ValidTo)
                return await RefreshToken();

            return accessToken;
        }

        public static async Task<string> RefreshToken()
        {
            var refreshToken = Preferences.Get("refreshToken", string.Empty);
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new InvalidSesionException("La sesión expiró.");

            var refreshTokenExpirationTime = Preferences.Get("refreshTokenExpirationTime", 0);
            if (refreshTokenExpirationTime == 0)
                throw new InvalidSesionException("La sesión expiró.");

            if (UnixTime.GetCurrentTime() > refreshTokenExpirationTime)
                throw new InvalidSesionException("La sesión expiró.");

            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.UseCookies = true;
                handler.UseDefaultCredentials = true;

                TokenResult result = new TokenResult();
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("refreshToken", refreshToken);

                    var response = await httpClient.PostAsync(new Uri(UriFactory.Identity.RefreshToken()), null);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new Exception("No cuenta con los permisos para realizar esta acción.");
                    else if (!response.IsSuccessStatusCode)
                        throw new Exception(await response.Content.ReadAsStringAsync());

                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                            throw x.Exception;

                        result = JsonConvert.DeserializeObject<TokenResult>(x.Result);
                    });

                    if (result != null)
                    {
                        // Storing token and data
                        Preferences.Set("accessToken", result.AccessToken);
                        Preferences.Set("userId", result.UserId);
                        Preferences.Set("tokenExpirationTime", result.ExpirationTime ?? 0);
                    }

                    var returnedCookie = handler.CookieContainer.GetCookies(new Uri(App.App.BaseUri))["refreshToken"];
                    if (returnedCookie != null)
                    {
                        // Storing refresh token
                        Preferences.Set("refreshToken", returnedCookie.Value);
                        Preferences.Set("refreshTokenExpirationTime", GetUnixTime(returnedCookie.Expires));
                    }
                }

                return result.AccessToken;
            }
            catch (Exception ex)
            {
                throw new InvalidSesionException(ex.Message);
            }
        }

        private static int GetUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
