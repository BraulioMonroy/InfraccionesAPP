using Infracciones.App.Models;
using Infracciones.App.Services.Endpoints;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infracciones.App.Services
{
    public static class BrandService
    {
        private static readonly BrandEndpoints _endpoint = new BrandEndpoints();

        public static async Task<List<BrandModel>> GetAll()
        {
            //TokenService.RefreshToken();
            return await _endpoint.GetAll();
        }

        public static async Task<BrandModel> GetById(int id)
        {
            return await _endpoint.GetById(id);
        }
    }
}
