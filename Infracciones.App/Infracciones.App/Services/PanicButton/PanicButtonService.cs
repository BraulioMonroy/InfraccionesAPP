using Infracciones.Models;
using Infracciones.Services.Endpoints;
using System.Threading.Tasks;
using Infracciones.Services.Logging;
using System;

namespace Infracciones.Services.PanicButton
{
    public class PanicButtonService
    {
        private static readonly PanicButtonEndpoints _endpoint = new PanicButtonEndpoints();
        private static readonly LogService _logService = new LogService();

        public static async Task<PanicButtonResponseModel> Add(PanicButtonRequestModel panicButtonRequest)
        {
            try
            {
                panicButtonRequest.Log = await _logService.CreateLog();
                return await _endpoint.Add(panicButtonRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<PanicButtonRequestModel> Release(PanicButtonRequestModel panicButtonRequest)
        {
            try
            {
                panicButtonRequest.Log = await _logService.CreateLog();
                return await _endpoint.Release(panicButtonRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
