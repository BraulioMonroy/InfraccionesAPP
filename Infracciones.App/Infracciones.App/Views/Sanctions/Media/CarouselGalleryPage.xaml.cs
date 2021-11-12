using Infracciones.App.Models;
using Infracciones.Models;
using Infracciones.Services.Sqlite.MediaService;
using Infracciones.ViewModels.Media;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Sanctions.Media
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarouselGalleryPage : ContentPage
    {
        public CarouselGalleryPage()
        {
            InitializeComponent();
        }

        public CarouselGalleryPage(TypeSanctionModel typeSanction)
        {
            InitializeComponent();

            var vm = new CarouselViewModel(typeSanction);
            BindingContext = vm;
        }

        private async void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            try
            {
                var vm = (CarouselViewModel)this.BindingContext;
                var popupView = new SelectMediaTypePopupView();
                popupView.BindingContext = vm.TypeSanction;
                await Navigation.PushPopupAsync(popupView, true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", string.Concat("No es posible agregar elementos multimedia. ", ex.Message), "Continuar");
            }
        }

        private async void ButtonBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }
    }
}