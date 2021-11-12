using Infracciones.App.Droid.Services;
using Infracciones.Services.Sqlite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(PathService))]
namespace Infracciones.App.Droid.Services
{
    public class PathService : IPathService
    {
        public string GetDatabasePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.Combine(path, App.DatabaseName);
        }

        public string GetInternalFolder()
        {
            return Android.App.Application.Context.FilesDir.AbsolutePath;
        }

        public string GetPrivateExternalFolder()
        {
            return Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
        }

        public string GetPublicExternalFolder()
        {
            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        }
    }
}