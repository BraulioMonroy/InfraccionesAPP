namespace Infracciones.Infrastructure.ZebraPrintStation.Services
{
    public interface IPlatformHelper
    {
        string GetIOSBundleIdentifier();

        bool IsWindows10();
    }
}
