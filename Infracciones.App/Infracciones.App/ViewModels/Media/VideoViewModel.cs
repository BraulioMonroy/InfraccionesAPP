using Infracciones.Services.Media;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Infracciones.ViewModels.Media
{
    public class VideoViewModel //: BaseMediaViewModel
    {
       // private readonly CameraService _cameraService;

     //   private string _videoURL;

        public string VideoURL
        {
            get; set;
            //get { return _videoURL; }
            //set { _videoURL = value; 
            //    OnPropertyChanged(); 
            //}
        }

        //public ICommand TakeVideoCommand { get; private set; }
        //public ICommand PickVideoCommand { get; private set; }

        //public VideoViewModel()
        //{
        //    _cameraService = new CameraService();
        //    Task.Run(async () => await _cameraService.Init());

        //    TakeVideoCommand = new Command(async () => await TakeVideo());
        //    PickVideoCommand = new Command(async () => await PickVideo());
        //}

        //private async Task TakeVideo()
        //{
        //    var file = await _cameraService.TakeVideo();

        //    if (file != null)
        //        VideoURL = file.Path;
        //}

        //private async Task PickVideo()
        //{
        //    var file = await _cameraService.PickVideo();

        //    if (file != null)
        //        VideoURL = file.Path;
        //}
    }
}
