using Infracciones.Models;
using Infracciones.Services.Endpoints;
using System.Threading.Tasks;
using Infracciones.Services.Logging;
using System;
using System.Collections;

namespace Infracciones.Services.PanicButton
{
    public class PanicButtonService
    {
        private static readonly PanicButtonEndpoints _endpoint = new PanicButtonEndpoints();
        private static readonly LogService _logService = new LogService();

        public static async Task<ArrayList> Add(PanicButtonFocus panicButtonRequestFocus)
        {
            try
            {
               // panicButtonRequest.Log = await _logService.CreateLog();
                return await _endpoint.Add(panicButtonRequestFocus);
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
