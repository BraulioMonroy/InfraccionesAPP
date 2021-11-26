using Infracciones.App.Models;
using Infracciones.Services.Geolocation;
using Infracciones.Services.Media;
using Infracciones.ViewModels.Media;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Sanctions.Media
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectMediaTypePopupView : PopupPage
    {
        private static readonly MediaService _mediaService = new MediaService();
        public bool IsGettingMedia { get; set; } = false;

        public SelectMediaTypePopupView()
        {
            InitializeComponent();
        }

        private void ImageButtonClose_Clicked(object sender, System.EventArgs e)
        {
            CloseAllPopup();
        }
        
        private async void ImageButtonTakePhoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (IsGettingMedia) return;

                IsGettingMedia = true;
                var photo = await _mediaService.TakePhoto();

                if (photo != null)
                {
                    var photoViewModel = new PhotoViewModel
                    {
                        Photo = photo.Photo,
                        PhotoPath = photo.PhotoPath,
                        TypeSanction = (TypeSanctionModel)this.BindingContext
                    };

                    if (photoViewModel.Photo != null)
                    {
                        var popupView = new PhotoPopupView();
                        popupView.BindingContext = photoViewModel;
                        await Navigation.PushPopupAsync(popupView);
                    }
                }

                IsGettingMedia = false;
            }
            catch (Exception ex)
            {
                IsGettingMedia = false;
                CloseAllPopup();
                await DisplayAlert("Error", ex.Message, "Continuar");
            }
        }      

        private async void ImageButtonTakeVideo_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (IsGettingMedia) return;

                IsGettingMedia = true;
                var mediaFile = await _mediaService.TakeVideo();

                if (mediaFile != null)
                {


                    var videoViewModel = new VideoViewModel
                    {
                        VideoURL = mediaFile.Path,
                        VideoPath = mediaFile.Path,
                        TypeSanction = (TypeSanctionModel)this.BindingContext
                    };

                    var popupView = new VideoPopupView
                    {
                        BindingContext = videoViewModel
                    };

                    await Navigation.PushPopupAsync(popupView);
                }

                IsGettingMedia = false;
            }
            catch (Exception ex)
            {
                IsGettingMedia = false;
                CloseAllPopup();
                await DisplayAlert("Error", ex.Message, "Continuar");
            }
        }

        private async void ImageButtonGallery_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (IsGettingMedia) return;

                IsGettingMedia = true;
                var photos = await _mediaService.PickMultiplePhotos();

                foreach (var photo in photos)
                {
                    var photoViewModel = new PhotoViewModel
                    {
                        Photo = photo.Photo,
                        PhotoPath = photo.PhotoPath,
                        TypeSanction = (TypeSanctionModel)this.BindingContext
                    };

                    if (photoViewModel.Photo != null)
                    {
                        var popupView = new PhotoPopupView();
                        popupView.BindingContext = photoViewModel;
                        await Navigation.PushPopupAsync(popupView);
                    }
                }

                IsGettingMedia = false;
            }
            catch (Exception ex)
            {
                IsGettingMedia = false;
                CloseAllPopup();
                await DisplayAlert("Error", ex.Message, "Continuar");
            }
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}