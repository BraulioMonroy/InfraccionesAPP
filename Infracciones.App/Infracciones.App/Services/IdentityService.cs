using Infracciones.App.Models;
using Infracciones.Services.DTO;
using Infracciones.Services.Endpoints;
using Infracciones.Services.Logging;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnixTimeStamp;
using Xamarin.Essentials;

namespace Infracciones.App.Services
{
    public static class IdentityService
    {
        private static readonly IdentityEndpoints _endpoint = new IdentityEndpoints();
        private static readonly LogService _logService = new LogService();

        public static async Task<bool> Login(string username, string password)
        {
            try
            {
                var result = await _endpoint.Login(new LoginModel()
                {
                    Username = username,
                    Password = password,
                    Log = await _logService.CreateLog()
                });

                var token = result.Item1;
                var cookie = result.Item2;

                if (token == null || cookie == null)
                    return false;

                // Storing token and data
                Preferences.Set("accessToken", token.AccessToken);
                Preferences.Set("userId", token.UserId);
                Preferences.Set("tokenExpirationTime", token.ExpirationTime ?? 0);
                Preferences.Set("currentTime", UnixTime.GetCurrentTime());
                Preferences.Set("username", username);
                Preferences.Set("password", password);

                // Storing refresh token
                Preferences.Set("refreshToken", cookie.Value);
                Preferences.Set("refreshTokenExpirationTime", GetUnixTime(cookie.Expires));

                Preferences.Set("Officer", token.Fullname);
                Preferences.Set("Role", token.Role);
                Preferences.Set("Key", token.Key);
                Preferences.Set("Shift", token.Shift);
                Preferences.Set("ShiftId", token.ShiftId ?? 0);
                Preferences.Set("Sector", token.Sector);
                Preferences.Set("SectorId", token.SectorId ?? 0);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<bool> RecoverPassword(string email)
        {
            try
            {
                var recoverPassword = new RecoverPasswordDTO()
                {
                    Email = email,
                    Log = await _logService.CreateLog()
                };

                return await _endpoint.RecoverPassword(recoverPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Logout()
        {
            // Token and data
            Preferences.Set("accessToken", string.Empty);
            Preferences.Set("userId", string.Empty);
            Preferences.Set("tokenExpirationTime", 0);
            Preferences.Set("currentTime", 0);
            Preferences.Set("username", string.Empty);
            Preferences.Set("password", string.Empty);

            // Refresh token
            Preferences.Set("refreshToken", string.Empty);
            Preferences.Set("refreshTokenExpirationTime", 0);

            // Officer info
            Preferences.Set("Officer", string.Empty);
            Preferences.Set("Role", string.Empty);
            Preferences.Set("Key", string.Empty);
            Preferences.Set("Shift", string.Empty);
            Preferences.Set("ShiftId", 0);
            Preferences.Set("Sector", string.Empty);
            Preferences.Set("SectorId", 0);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static int GetUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
