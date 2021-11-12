using Infracciones.App.Models;
using Infracciones.App.Services.DependencyServices;
using Infracciones.App.ViewModels;
using Infracciones.Models;
using Infracciones.Services.Sqlite.MediaService;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Infracciones.ViewModels.Media
{
    public class CarouselViewModel : BaseViewModel
    {
        private static SqlMediaService _sqlMediaService = new SqlMediaService();

        public ObservableCollection<ImageModel> Images { get; set; }
        public TypeSanctionModel TypeSanction { get; set; } = new TypeSanctionModel();
        public Command RemoveMediaCommand { get; set; }

        public CarouselViewModel()
        {

        }

        public CarouselViewModel(TypeSanctionModel typeSanction)
        {
            TypeSanction = typeSanction;
            Images = new ObservableCollection<ImageModel>();
            RemoveMediaCommand = new Command<int>(async (id) => await RemoveMedia(id));
            Task.Run(async () => await BindingCarousel());
        }

        private async Task BindingCarousel()
        {
            try
            {
                var results = await _sqlMediaService.GetImages(TypeSanction.SanctionReason);

                if (results.Count > 0)
                {
                    foreach (var image in results)
                    {
                        //var memoryStream = new MemoryStream(image.Image);

                        var aux = new ImageModel();
                        aux.Id = image.Id;
                        //aux.ImageSource = ImageSource.FromStream(() => memoryStream);
                        aux.ImagePath = image.ImagePath;
                        aux.ImageSource = ImageSource.FromFile(aux.ImagePath);

                        if(aux.ImageSource != null)
                        {
                            Images.Add(aux);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task RemoveMedia(int id)
        {
            try
            {
                var media = await _sqlMediaService.GetById(id);
                if (media == null) throw new Exception("No se encontro el elemento multimedia. ");

                if (File.Exists(media.ImagePath))
                    File.Delete(media.ImagePath);

                await _sqlMediaService.Remove(media);

                var imageToRemove = Images.FirstOrDefault(x => x.Id == media.Id);
                Images.Remove(imageToRemove);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("No fue posible remover el elemento multimedia. ", ex.Message));
            }
        }
    }
}
