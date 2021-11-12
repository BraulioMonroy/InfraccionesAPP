using Infracciones.App.Models;
using Infracciones.App.Services;
using System.Collections.Generic;

namespace Infracciones.App.ViewModels.Vehicle
{
    public class BrandViewModel : BaseViewModel
    {
        public List<BrandModel> ListBrands { get; set; }

        public BrandViewModel()
        {
            BindBrands();
        }

        private BrandModel _selectedBrand;

        public BrandModel SelectedBrand
        {
            get { return _selectedBrand; }
            set {
                SetProperty(ref _selectedBrand, value);
                Description = _selectedBrand.Description;
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private async void BindBrands()
        {
            ListBrands = await BrandService.GetAll();
        }
    }
}
