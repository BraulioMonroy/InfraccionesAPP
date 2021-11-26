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

        public ObservableCollection<MediaModel> Medias { get; set; }
        public TypeSanctionModel TypeSanction { get; set; } = new TypeSanctionModel();
        public Command RemoveMediaCommand { get; set; }

        public CarouselViewModel()
        {

        }

        public CarouselViewModel(TypeSanctionModel typeSanction)
        {
            TypeSanction = typeSanction;
            Medias = new ObservableCollection<MediaModel>();
            RemoveMediaCommand = new Command<int>(async (id) => await RemoveMedia(id));
            Task.Run(async () => await BindingCarousel());
        }

        private async Task BindingCarousel()
        {
            try
            {
                var results = await _sqlMediaService.GetAllVideosAndImages(TypeSanction.SanctionReason);
                

                if (results.Count > 0)
                {
                    foreach (var item in results)
                    {
                        //var memoryStream = new MemoryStream(image.Image);

                        var aux = new MediaModel();
                        aux.Id = item.Id;
                        aux.MediaType = item.MediaType;
                        aux.isImage = item.isImage;
                        //aux.ImageSource = ImageSource.FromStream(() => memoryStream);
                        aux.MediaPath = item.MediaPath;
                      

                        if(aux.MediaPath != null)
                        {
                            Medias.Add(aux);
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

                if (File.Exists(media.MediaPath))
                    File.Delete(media.MediaPath);

                await _sqlMediaService.Remove(media);

                var mediaToRemove = Medias.FirstOrDefault(x => x.Id == media.Id);
                Medias.Remove(mediaToRemove);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("No fue posible remover el elemento multimedia. ", ex.Message));
            }
        }
    }
}
