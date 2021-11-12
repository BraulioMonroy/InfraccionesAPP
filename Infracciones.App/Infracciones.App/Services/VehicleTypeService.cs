using Infracciones.App.Models;
using Infracciones.App.Services.Endpoints;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infracciones.App.Services
{
    public static class VehicleTypeService
    {
        private static readonly VehicleTypeEndpoints _endpoint = new VehicleTypeEndpoints();

        public static async Task<List<VehicleTypeModel>> GetAll()
        {
            return await _endpoint.GetAll();
        }

        public static async Task<VehicleTypeModel> GetById(int id)
        {
            return await _endpoint.GetById(id);
        }
    }
}
