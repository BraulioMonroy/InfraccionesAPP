using Infracciones.App.Models;
using Infracciones.App.Services.DTO;
using Infracciones.App.Services.Endpoints;
using Infracciones.Services.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infracciones.App.Services
{
    public static class SanctionService
    {
        private static readonly SanctionEndpoints _endpoint = new SanctionEndpoints();
        private static readonly LogService _logService = new LogService();
        //private static readonly GeolocationService _geolocationService = new GeolocationService();
        //private static readonly IPService _ipService = new IPService();

        public static async Task<List<SanctionModel>> MakeSanctions(SanctionDTO sanctions)
        {
            var doneSanctions = new List<SanctionModel>();

            var userId = Xamarin.Essentials.Preferences.Get("userId", string.Empty);

            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("Por favor, vuelva a iniciar sesión.");

            try
            {
                // Vehicle Info
                var sanction = new SanctionModel();
                sanction.Plates = sanctions.Vehicle.Plates;
                sanction.Model = sanctions.Vehicle.Model;
                sanction.Responsible = sanctions.Vehicle.Responsible;
                sanction.VehicleTypeId = sanctions.Vehicle.VehicleType.Id;
                sanction.BrandId = sanctions.Vehicle.Brand.Id;
                sanction.SubBrandId = sanctions.Vehicle.Subbrand.Id;
                sanction.Origin = sanctions.Vehicle.Origin.Id;
                //TODO: Fix "OfficerId" when user manager be implemented
                sanction.OfficerId = userId;
                sanction.Date = DateTime.Now;

                //TODO: Change when Color field will be available on the UI
                sanction.ColorId = 1;
                sanction.Log = await _logService.CreateLog();

                // Make sanctions
                foreach (var item in sanctions.SanctionReasons)
                {
                    var aux = new SanctionModel();
                    aux = sanction;

                    aux.SanctionReasonId = item.Id;
                    aux.Amount = item.Amount;

                    aux = await Add(aux);

                    doneSanctions.Add(aux);
                }

                return doneSanctions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<SanctionModel>> Search(SanctionSearchDTO sanctionSearch)
        {
            try
            {
                sanctionSearch.Log = await _logService.CreateLog();
                return await _endpoint.Search(sanctionSearch);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<SanctionModel>> GetByOfficer(string id)
        {
            return await _endpoint.GetByOfficer(id);
        }

        public static async Task<SanctionModel> GetById(int id)
        {
            return await _endpoint.GetById(id);
        }

        public static async Task<SanctionModel> Add(SanctionModel sanction)
        {
            return await _endpoint.Add(sanction);
        }

        public static async Task<SanctionModel> Update(int id, SanctionModel sanction)
        {
            return await _endpoint.Update(id, sanction);
        }
    }
}
