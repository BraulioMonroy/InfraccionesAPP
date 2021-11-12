using Infracciones.App.Models;

namespace Infracciones.App.Services.Endpoints
{
    public class BrandEndpoints : EndpointService<BrandModel>
    {
        public BrandEndpoints()
        {
            GetByIdUrl = UriFactory.Brand.GetById();
            GetAllUrl = UriFactory.Brand.GetAll();
        }
    }
}
