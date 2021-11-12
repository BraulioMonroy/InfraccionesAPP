using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.Services.DTO;
using Infracciones.App.ViewModels.Vehicle;
using Infracciones.App.Views.Home;
using Infracciones.App.Views.Identity;
using Infracciones.App.Views.Shared;
using Infracciones.Helpers;
using Infracciones.Models;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Vehicle
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchVehiclePage : ContentPage
    {
        private readonly ObservableCollection<VehicleTypeModel> vehicleTypeCollection;
        private readonly ObservableCollection<BrandModel> brandCollection;
        private readonly ObservableCollection<SubBrandModel> subBrandCollection;
        private readonly ObservableCollection<StateModel> statesCollection;

        public SearchVehiclePage()
        {
            InitializeComponent();
            vehicleTypeCollection = new ObservableCollection<VehicleTypeModel>();
            brandCollection = new ObservableCollection<BrandModel>();
            subBrandCollection = new ObservableCollection<SubBrandModel>();
            statesCollection = new ObservableCollection<StateModel>();

            BindVehicleTypes();
            BindBrands();
            BindOrigins();
        }

        private async void BindVehicleTypes()
        {
            var list = await VehicleTypeService.GetAll();

            if (list == null)
                return;

            foreach (var item in list)
                vehicleTypeCollection.Add(item);

            EntVehicleType.ItemsSource = vehicleTypeCollection;
        }

        private async void BindBrands()
        {
            var list = await BrandService.GetAll();

            if (list == null)
                return;

            foreach (var item in list)
                brandCollection.Add(item);

            EntBrand.ItemsSource = brandCollection;
        }

        private async void BindSubrands(int brandId)
        {
            EntSubbrand.ItemsSource = null;
            subBrandCollection.Clear();

            var list = await SubBrandService.GetByBrandId(brandId);

            if (list == null)
                return;

            foreach (var item in list)
                subBrandCollection.Add(item);

            EntSubbrand.ItemsSource = subBrandCollection;

        }

        private async void BindOrigins()
        {
            // TODO: 1 is Mexico. See how to give sanctions to foreign vehicles
            // TODO: 1 is a hardcode and it is bad, check the best way to remove hardcode.
            var list = await StateService.GetByCountryId(1);

            if (list == null)
                return;

            foreach (var item in list)
                statesCollection.Add(item);

            EntOrigin.ItemsSource = statesCollection;
        }

        private void EntBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (sender as Picker);

            if (picker == null)
                return;

            if (picker.SelectedItem == null)
            {
                EntSubbrand.ItemsSource = null;
                return;
            }

            var brand = (BrandModel)picker.SelectedItem;
            BindSubrands(brand.Id);
        }

        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            try
            {
                var vm = BindingContext as SearchVehicleViewModel;
                vm.Brand.Value = EntBrand.SelectedIndex > -1 ? (BrandModel)EntBrand.SelectedItem : null;
                vm.SubBrand.Value = EntSubbrand.SelectedIndex > -1 ? (SubBrandModel)EntSubbrand.SelectedItem : null;
                vm.Origin.Value = EntOrigin.SelectedIndex > -1 ? (StateModel)EntOrigin.SelectedItem : null;
                vm.VehicleType.Value = EntVehicleType.SelectedIndex > -1 ? (VehicleTypeModel)EntVehicleType.SelectedItem : null;

                var loading = new LoadingPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = "Buscando...",
                        TextStyle = "PopupContentLabelStyle"
                    }
                };

                await Navigation.PushPopupAsync(loading);

                var searchParameters = new SanctionSearchDTO
                {
                    TodaySanctions = false,
                    StartDate = null,
                    EndDate = null,
                    Officer = string.Empty,
                    SanctionReason = 0,
                    Plates = vm.Plates.Value,
                    Model = vm.Model.Value != null ? Convert.ToInt32(vm.Model.Value) : 0,
                    Brand = vm.Brand.Value != null ? ((BrandModel)vm.Brand.Value).Id : 0,
                    SubBrand = vm.SubBrand.Value != null ? ((SubBrandModel)vm.SubBrand.Value).Id : 0,
                    Origin = vm.Origin.Value != null ? ((StateModel)vm.Origin.Value).Id : 0,
                    VehicleType = vm.VehicleType.Value != null ? ((VehicleTypeModel)vm.VehicleType.Value).Id : 0
                };

                var vehicleModel = new VehicleModel();
               // var sanctions = (await SanctionService.Search(searchParameters)).OrderBy(x => x.DateCreated).ToList();

                searchParameters.Status = PaymentStatus.NoPagado.ToString();
                var pendingSanctions = (await SanctionService.Search(searchParameters)).OrderBy(x => x.DateCreated).ToList();

                if (pendingSanctions.Count > 0)
                {
                    var sanction = pendingSanctions.First();

                    searchParameters.Status = PaymentStatus.NoPagado.ToString();
                 

                    vehicleModel = new VehicleModel
                    {
                        Plates = sanction.Plates,
                        Responsible = sanction.Responsible,
                        Brand = sanction.Brand,
                        Subbrand = sanction.SubBrand,
                        Origin = sanction.State,
                        VehicleType = sanction.VehicleType,
                        Model = sanction.Model,
                        CheckRepuveString = (bool)vm.CheckRepuve.Value ? "Si" : "No",

                        Sanctions = pendingSanctions
                    };
                }
                else
                {
                    vehicleModel = new VehicleModel
                    {
                        Plates = vm.Plates.Value,
                        Responsible = vm.Responsible.Value,
                        Brand = vm.Brand.Value != null && vm.Brand.Value is BrandModel ? (BrandModel)vm.Brand.Value : null,
                        Subbrand = vm.SubBrand.Value != null && vm.SubBrand.Value is SubBrandModel ? (SubBrandModel)vm.SubBrand.Value : null,
                        Origin = vm.Origin.Value != null && vm.Origin.Value is StateModel ? (StateModel)vm.Origin.Value : null,
                        VehicleType = vm.VehicleType.Value != null && vm.VehicleType.Value is VehicleTypeModel ? (VehicleTypeModel)vm.VehicleType.Value : null,
                        Model = vm.Model.Value != null ? Convert.ToInt32(vm.Model.Value) : 0,
                        CheckRepuveString = (bool)vm.CheckRepuve.Value ? "Si" : "No",

                        Sanctions = new List<SanctionModel>()
                    };
                }

                var details = new VehicleFoundViewModel(vehicleModel);

                var vehicleFoundPage = new VehicleFoundPage
                {
                    BindingContext = details
                };

                await Navigation.PopPopupAsync(true);
                await Navigation.PushAsync(vehicleFoundPage, true);

            }
            catch (Exception ex)
            {
                CloseAllPopup();

                var errorPopupView = new ErrorPopupView()
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = ex.Message,
                        Image = "Error.png"
                    }
                };

                await Navigation.PushPopupAsync(errorPopupView);
            }
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }
    }
}