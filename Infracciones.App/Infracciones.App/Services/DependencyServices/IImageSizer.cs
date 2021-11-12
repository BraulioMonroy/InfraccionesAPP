using Android.Util;
using System.Threading.Tasks;

namespace Infracciones.App.Services.DependencyServices
{
    public interface IImageSizer
    {
        Task<Size> GetSizeForImage(string filename);
    }
}
