using Infracciones.App.Models;

namespace Infracciones.App.Services.Endpoints
{
    public class VehicleTypeEndpoints : EndpointService<VehicleTypeModel>
    {
        public VehicleTypeEndpoints()
        {
            GetByIdUrl = UriFactory.VehicleType.GetById();
            GetAllUrl = UriFactory.VehicleType.GetAll();
        }
    }
}
