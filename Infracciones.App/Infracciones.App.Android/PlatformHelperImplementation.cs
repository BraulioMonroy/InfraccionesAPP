using Infracciones.App.Droid;
using Infracciones.Infrastructure.ZebraPrintStation.Services;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformHelperImplementation))]
namespace Infracciones.App.Droid
{
    public class PlatformHelperImplementation : IPlatformHelper
    {
        public string GetIOSBundleIdentifier()
        {
            throw new NotImplementedException();
        }

        public bool IsWindows10()
        {
            return false;
        }
    }
}