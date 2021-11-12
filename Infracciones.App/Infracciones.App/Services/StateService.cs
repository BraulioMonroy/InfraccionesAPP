using Infracciones.App.Models;
using Infracciones.App.Services.Endpoints;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infracciones.App.Services
{
    public static class StateService
    {
        private static readonly StateEndpoints _endpoint = new StateEndpoints();

        public static async Task<List<StateModel>> GetByCountryId(int brandId)
        {
            return await _endpoint.GetByCountryId(brandId);
        }

        public static async Task<StateModel> GetById(int id)
        {
            return await _endpoint.GetById(id);
        }
    }
}
