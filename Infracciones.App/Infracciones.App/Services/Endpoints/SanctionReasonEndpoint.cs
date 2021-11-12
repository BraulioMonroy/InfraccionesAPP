using Infracciones.App.Models;
using Infracciones.App.Services.Endpoints;

namespace Infracciones.Services.Endpoints
{
    public class SanctionReasonEndpoint : EndpointService<SanctionReasonModel>
    {
        public SanctionReasonEndpoint()
        {
            GetAllUrl = UriFactory.SanctionReason.GetAll();
            GetByIdUrl = UriFactory.SanctionReason.GetById();
        }
    }
}
