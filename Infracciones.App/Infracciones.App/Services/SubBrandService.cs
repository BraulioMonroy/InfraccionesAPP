using Infracciones.App.Models;
using Infracciones.App.Services.Endpoints;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infracciones.App.Services
{
    public static class SubBrandService
    {
        private static readonly SubBrandEndpoints _endpoint = new SubBrandEndpoints();

        public static async Task<List<SubBrandModel>> GetByBrandId(int brandId)
        {
            return await _endpoint.GetByBrandId(brandId);
        }

        public static async Task<SubBrandModel> GetById(int id)
        {
            return await _endpoint.GetById(id);
        }
    }
}
