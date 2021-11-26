using Infracciones.Models;
using Infracciones.Services.Sqlite.MediaService;
using Infracciones.ViewModels.Media;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Sanctions.Media
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPopupView : PopupPage
    {

        private static SqlMediaService _sqliteService = new SqlMediaService();
        public VideoPopupView()
        {
            InitializeComponent();
        }

        private async void ButtonCanel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void ButtonSelectVideo_Clicked(object sender, EventArgs e)
        {
            try
            {
                var vm = (VideoViewModel)BindingContext;
                //StreamImageSource streamImageSource = (StreamImageSource)vm.Photo;
                //CancellationToken cancellationToken = CancellationToken.None;
                //Task<Stream> task = streamImageSource.Stream(cancellationToken);
                //Stream stream = task.Result;

                var video = new MediaModel();
                video.MediaType = "Video";
                video.isImage = false;
                video.OfficialId = Preferences.Get("userId", string.Empty);
                video.SanctionReasonId = (int)vm.TypeSanction.SanctionReason;
                video.MediaPath = vm.VideoPath;

                await _sqliteService.Insert(video);
                await PopupNavigation.Instance.PopAsync(true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Continuar");
                await PopupNavigation.Instance.PopAsync(true);
            }

        }

}
}