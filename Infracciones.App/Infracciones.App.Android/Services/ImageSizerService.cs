using Android.Graphics;
using Infracciones.App.Droid.Services;
using Infracciones.App.Services.DependencyServices;
using Java.Net;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageSizerService))]
namespace Infracciones.App.Droid.Services
{
    public class ImageSizerService : IImageSizer
    {
        public async Task<Android.Util.Size> GetSizeForImage(string filename)
        {
            string fileNameStripped = System.IO.Path.GetFileNameWithoutExtension(filename);
            var resId = Android.App.Application.Context.Resources.GetIdentifier(fileNameStripped, "drawable", Android.App.Application.Context.PackageName);
            if (resId == 0 && filename.Contains("http"))
            {
                try
                {
                    URL url = new URL(filename);
                    Bitmap bmp = await Task.Run(() => BitmapFactory.DecodeStream(url.OpenConnection().InputStream));
                    return new Android.Util.Size(bmp.Width, bmp.Height);
                }
                catch (Exception)
                {
                    return default(Android.Util.Size);
                }
            }
            else
            {
                var options = new BitmapFactory.Options
                {
                    InJustDecodeBounds = true
                };
                BitmapFactory.DecodeResource(Android.App.Application.Context.Resources, resId, options);
                return new Android.Util.Size(options.OutWidth, options.OutHeight);
            }
        }
    }
}