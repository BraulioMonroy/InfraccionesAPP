using FFImageLoading;
using Infracciones.App.Models;
using Infracciones.Models;
using Infracciones.Services.Sqlite.MediaService;
using Infracciones.ViewModels.Media;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Sanctions.Media
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoPopupView : PopupPage
    {
        private static SqlMediaService _sqliteService = new SqlMediaService();

        public PhotoPopupView()
        {
            InitializeComponent();
            FrameContainer.BackgroundColor = new Color(0, 0, 0, 0.5);
        }

        private async void ButtonSelectImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                var vm = (PhotoViewModel)BindingContext;
                //StreamImageSource streamImageSource = (StreamImageSource)vm.Photo;
                //CancellationToken cancellationToken = CancellationToken.None;
                //Task<Stream> task = streamImageSource.Stream(cancellationToken);
                //Stream stream = task.Result;

                var image = new MediaModel();
                image.MediaType = "Image";
                image.OfficialId = Preferences.Get("userId", string.Empty);
                image.SanctionReasonId = (int)vm.TypeSanction.SanctionReason;
                //image.Image = stream.ToByteArray();
                image.ImagePath = vm.PhotoPath;

                await _sqliteService.Insert(image);
                await PopupNavigation.Instance.PopAsync(true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Continuar");
                await PopupNavigation.Instance.PopAsync(true);
            }
        }

        private async void ButtonCanel_Clicked(object sender, EventArgs e)
        {
            //Delete image from app folder
            var vm = (PhotoViewModel)BindingContext;
            if (File.Exists(vm.PhotoPath))
                File.Delete(vm.PhotoPath);

            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}