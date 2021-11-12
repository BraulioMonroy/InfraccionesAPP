using Infracciones.Services.Geolocation;
using Infracciones.ViewModels.Media;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Infracciones.Services.Media
{
    //TODO: Optimize with MVVC Pattern 
    // See https://www.luisbeltran.mx/2019/11/20/curso-basico-de-xamarin-utilizando-la-camara-en-xamarin-forms/
    // See https://medium.com/swlh/select-multiple-images-from-the-gallery-in-xamarin-forms-df2e037be572
    public class MediaService
    {
        private static readonly GeolocationService _geolocationService = new GeolocationService();

        public async Task<bool> RequestCameraAndGalleryPermissions()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            var photosStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);

            if (cameraStatus != PermissionStatus.Granted ||
                    storageStatus != PermissionStatus.Granted ||
                        photosStatus != PermissionStatus.Granted)
            {
                var permissionRequestResult = await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] {
                    Permission.Camera,
                    Permission.Storage,
                    Permission.Photos
                });

                var cameraResult = permissionRequestResult[Permission.Camera];
                var storageResult = permissionRequestResult[Permission.Storage];
                var photosResults = permissionRequestResult[Permission.Photos];

                return (cameraResult != PermissionStatus.Denied &&
                            storageResult != PermissionStatus.Denied &&
                                photosResults != PermissionStatus.Denied);
            }

            return true;
        }

        public async Task<PhotoViewModel> TakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                throw new Exception("Su equipo no cuenta con una camara disponible.");

            var isPermissionGranted = await RequestCameraAndGalleryPermissions();

            if (!isPermissionGranted)
                return null;

            var fullLocation = await _geolocationService.GetFullCurrentLocation();
            var location = new Location();

            if (fullLocation != null)
            {
                location.Latitude = fullLocation.Latitude;
                location.Longitude = fullLocation.Longitude;
                location.Altitude = Convert.ToDouble(fullLocation.Altitude);
                location.Timestamp = DateTime.UtcNow;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sanciones",
                SaveToAlbum = true,
                CompressionQuality = 92,
                PhotoSize = PhotoSize.Medium,
                CustomPhotoSize = 90,
                MaxWidthHeight = 1000,
                Location = location ?? null
            });

            if (file == null)
                return null;

            var imageSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            //TODO: Try to return MediaFile object and with MVVC pattern convert to ImageSource an then show it

            var photoViewModel = new PhotoViewModel
            {
                Photo = imageSource,
                PhotoPath = file.Path
            };

            return photoViewModel;
        }

        public async Task<PhotoViewModel> PickPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
                throw new Exception("Permisos no concedidos para acceder a las fotos.");

            var isPermissionGranted = await RequestCameraAndGalleryPermissions();

            if (!isPermissionGranted)
                return null;

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92,
                CustomPhotoSize = 90
            });

            if (file == null)
                return null;

            var imageSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            var photoViewModel = new PhotoViewModel
            {
                Photo = imageSource,
                PhotoPath = file.Path
            };

            return photoViewModel;
        }

        public async Task<List<PhotoViewModel>> PickMultiplePhotos()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
                throw new Exception("Permisos no concedidos para acceder a las fotos.");

            var isPermissionGranted = await RequestCameraAndGalleryPermissions();

            if (!isPermissionGranted)
                return null;

            //PickMediaOptions doesn't have a 'Directory' property, files are stored in a folder named 'temp'
            var files = await CrossMedia.Current.PickPhotosAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92,
                CustomPhotoSize = 90
            });

            List<PhotoViewModel> imagesSelected = new List<PhotoViewModel>();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    var imageSource = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });

                    if (imageSource != null)
                    {
                        var photoViewModel = new PhotoViewModel
                        {
                            Photo = imageSource,
                            PhotoPath = file.Path
                        };

                        imagesSelected.Add(photoViewModel);
                    }
                }
            }

            return imagesSelected;
        }

        public async Task<MediaFile> TakeVideo()
        {

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                throw new Exception("Su equipo no cuenta con una camara disponible.");

            var isPermissionGranted = await RequestCameraAndGalleryPermissions();

            if (!isPermissionGranted)
                return null;

            var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
            {
                Directory = "Sanciones",
                SaveToAlbum = true,
                CompressionQuality = 75
            });

            return file;
        }

        public async Task<MediaFile> PickVideo()
        {
            if (CrossMedia.Current.IsPickVideoSupported)
            {
                var file = await CrossMedia.Current.PickVideoAsync();
                return file;
            }

            return null;
        }
    }
}
