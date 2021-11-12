using Infracciones.App.Models;
using Infracciones.App.Services.Endpoints;
using Infracciones.Services.Logging;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Infracciones.App.Services
{
    public static class CashClosingService
    {
        private static readonly CashClosingEndpoints _endpoint = new CashClosingEndpoints();
        private static readonly LogService _logService = new LogService();

        public static async Task<CashClosingModel> Add(CashClosingDtoModel entity)
        {
            try
            {
                var log = await _logService.CreateLog();
                entity.Log = log;
                entity.Latitude = entity.Log.Latitude;
                entity.Longitude = entity.Log.Longitude;
                entity.Payments.ForEach(x => x.Log = log);
                return await _endpoint.Add(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<CashClosingModel> GetById(int id)
        {
            try
            {
                return await _endpoint.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
