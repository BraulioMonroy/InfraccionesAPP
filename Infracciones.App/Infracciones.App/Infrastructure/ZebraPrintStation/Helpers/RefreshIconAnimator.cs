using System.Threading.Tasks;
using Xamarin.Forms;

namespace Infracciones.Infrastructure.ZebraPrintStation.Helpers
{
    public static class RefreshIconAnimator
    {
        public static async Task AnimateOneRotationAsync(VisualElement element)
        {
            await element.RotateTo(360, 2000, Easing.SinInOut);
            element.Rotation = 0;
        }
    }
}
