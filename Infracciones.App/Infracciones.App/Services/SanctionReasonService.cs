using Infracciones.App.Models;
using Infracciones.Services.Endpoints;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infracciones.App.Services
{
    public static class SanctionReasonService
    {
        private static readonly SanctionReasonEndpoint _endpoint = new SanctionReasonEndpoint();

        public static async Task<List<SanctionReasonModel>> GetAll()
        {
            return await _endpoint.GetAll();
        }

        public static async Task<SanctionReasonModel> GetById(int id)
        {
            return await _endpoint.GetById(id);
        }
    }
}
