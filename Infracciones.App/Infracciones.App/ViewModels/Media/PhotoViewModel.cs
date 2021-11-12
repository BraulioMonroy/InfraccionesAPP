using Infracciones.App.Models;
using Xamarin.Forms;

namespace Infracciones.ViewModels.Media
{
    public class PhotoViewModel //: BaseMediaViewModel
    {
        //private readonly CameraService _cameraService;

        //private ImageSource _photo;

        public ImageSource Photo
        { 
            get; set;
            //get { return _photo; }
            //set { _photo = value; 
            //    OnPropertyChanged(); 
            //}
        }

        public string PhotoPath { get; set; }
        public int SanctionId { get; set; }

        public TypeSanctionModel TypeSanction { get; set; }

        //public ICommand TakePhotoCommand { get; private set; }
        //public ICommand PickPhotoCommand { get; private set; }

        //public PhotoViewModel()
        //{
        //    _cameraService = new CameraService();
        //    Task.Run(async () => await _cameraService.Init());

        //    TakePhotoCommand = new Command(async () => await TakePhoto());
        //    PickPhotoCommand = new Command(async () => await PickPhoto());
        //}

        //public async Task TakePhoto()
        //{
        //    var file = await _cameraService.TakePhoto();

        //    if (file != null)
        //        Photo = ImageSource.FromStream(() => file.GetStream());
        //}

        //public async Task PickPhoto()
        //{
        //    var file = await _cameraService.PickPhoto();

        //    if (file != null)
        //        Photo = ImageSource.FromStream(() => file.GetStream());
        //}
    }
}
