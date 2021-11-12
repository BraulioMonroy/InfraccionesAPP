namespace Infracciones.App.Services.Endpoints
{
    public class UriFactory
    {
        private static string BaseUri = App.BaseUri;

        public static class Brand
        {
            public static string GetById() => $"{BaseUri}/api/brands";       
            public static string GetAll() => $"{BaseUri}/api/brands/enabled";
        }

        public static class VehicleType
        {
            public static string GetById() => $"{BaseUri}/api/cartypes";
            public static string GetAll() => $"{BaseUri}/api/cartypes/enabled";
        }

        public static class CashClosing
        {
            public static string GetById() => $"{BaseUri}/api/cashclosings";
            public static string Add() => $"{BaseUri}/api/cashclosings";
        }

        public static class City
        {          
            public static string GetById() => $"{BaseUri}/api/cities";
            public static string GetByState() => $"{BaseUri}/api/cities/state";
        }

        public static class Color
        {           
            public static string GetById() => $"{BaseUri}/api/colors";
            public static string GetAll() => $"{BaseUri}/api/colors/enabled";
        }

        public static class Country
        {          
            public static string GetById() => $"{BaseUri}/api/countries";
            public static string GetAll() => $"{BaseUri}/api/countries/enabled";
        }

        public static class Domain
        {           
            public static string GetById() => $"{BaseUri}/api/domains";
            public static string GetAll() => $"{BaseUri}/api/domains/enabled";
        }

        public static class Identity
        {
            public static string SignIn() => $"{BaseUri}/api/identity/signinmobile";
            public static string SignOut() => $"{BaseUri}/api/identity/signout";
            public static string RecoverPassword() => $"{BaseUri}/api/identity/recoverpassword";
            public static string RefreshToken() => $"{BaseUri}/api/identity/refreshtokenmobile";
        }

        public static class Media
        {
            public static string Add() => $"{BaseUri}/api/media";
        }

        public static class PanicButtonRequest
        {
            public static string Add() => $"{BaseUri}/api/panicButton";
            public static string Release() => $"{BaseUri}/api/panicButton/release";
        }

        public static class Payment
        {
            public static string GetById() => $"{BaseUri}/api/payments";
            public static string GetByStatus() => $"{BaseUri}/api/payments/status";
            public static string GetByOfficer() => $"{BaseUri}/api/payments/opens";
            public static string GetOpenByStatusAndOfficer(bool open, string status, string officerId) => $"{BaseUri}/api/payments/opens/{open}/status/{status}/officer/{officerId}";
            public static string Close() => $"{BaseUri}/api/payments/close";
            public static string Update() => $"{BaseUri}/api/payments";
        }

        public static class SanctionReason
        {           
            public static string GetById() => $"{BaseUri}/api/sanctionreasons";
            public static string GetAll() => $"{BaseUri}/api/sanctionreasons/enabled";
        }

        public static class Sanction
        {
            public static string Add() => $"{BaseUri}/api/sanctions";
            public static string Update() => $"{BaseUri}/api/sanctions";           
            public static string GetById() => $"{BaseUri}/api/sanctions";
            public static string Search() => $"{BaseUri}/api/sanctions/search";
            //TODO: Change URI for "officer"
            public static string GetByOfficer() => $"{BaseUri}/api/sanctions/oficial";                 
        }

        public static class Sector
        {            
            public static string GetById() => $"{BaseUri}/api/sectors";
            public static string GetAll() => $"{BaseUri}/api/sectors/enabled";
        }

        public static class Shift
        {
            public static string GetById() => $"{BaseUri}/api/shifts";
            public static string GetAll() => $"{BaseUri}/api/shifts/enabled";
        }

        public static class State
        {
            public static string GetById() => $"{BaseUri}/api/states";
            public static string GetByCountry() => $"{BaseUri}/api/states/country";
        }

        public static class SubBrand
        {           
            public static string GetById() => $"{BaseUri}/api/subbrands";
            public static string GetByBrand() => $"{BaseUri}/api/subbrands/brand";
        }

        public static class User
        {
            public static string GetById() => $"{BaseUri}/api/users";         
        }
    }
}
