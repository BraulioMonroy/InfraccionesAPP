using Infracciones.App.Models;
using Infracciones.Models;
using Infracciones.Services.Sqlite.MediaService;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Sanctions.Media
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarouselMediaGalleryPopupView : PopupPage
    {
        private static SqlMediaService _sqlMediaService = new SqlMediaService();
        //private static MediaService _mediaService = new MediaService();
        private readonly ObservableCollection<ImageModel> images;

        public CarouselMediaGalleryPopupView()
        {
            InitializeComponent();                  
        }

        public CarouselMediaGalleryPopupView(TypeSanctionModel typeSanction)
        {
            InitializeComponent();
            images = new ObservableCollection<ImageModel>();
            BindingContext  = typeSanction;
            BindingCarousel(typeSanction);
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        private async void BindingCarousel(TypeSanctionModel typeSanction)
        {
            try
            {
                var results = await _sqlMediaService.GetImages(typeSanction.SanctionReason);

                if (results.Count > 0)
                {
                    foreach (var image in results)
                    {
                        var memoryStream = new MemoryStream(image.Image);

                        var aux = new ImageModel();
                        aux.Id = image.Id;
                        aux.ImageSource = ImageSource.FromStream(() => memoryStream);

                        images.Add(aux);                      
                    }

                    CarouselGallery.ItemsSource = images;
                }
            }
            catch(Exception ex)
            {                
                await DisplayAlert("Error", ex.Message, "Continuar");
            }
        }

        private async void ButtonCanel_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var button = (sender as ImageButton);
               
                var media = new MediaModel
                {
                    Id = Convert.ToInt32(button.ClassId)
                };

                await _sqlMediaService.Remove(media);
                CloseAllPopup();
            }
            catch(Exception ex)
            {
                CloseAllPopup();
                await DisplayAlert("Error", string.Concat("No fue posible remover el elemento multimedia. ", ex.Message) ,"Continuar");
            }
        }
    }
}