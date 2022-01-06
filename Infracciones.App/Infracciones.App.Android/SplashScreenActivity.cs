using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Infracciones.App.Droid
{
    [Activity(Theme = "@style/SplashTheme",
             MainLauncher = true,
             NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {

            // Icon = "@drawable/Icon"
            base.OnCreate(bundle);
            // System.Threading.Thread.Sleep(600);
           // Window.DecorView.SystemUiVisibility = (StatusBarVisibility)((int)Window.DecorView.SystemUiVisibility ^ (int)SystemUiFlags.LayoutStable ^ (int)SystemUiFlags.LayoutFullscreen);
           // Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            //Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            StartActivity(new Android.Content.Intent(Application.Context, typeof(MainActivity)));
           
            
         
           // this.StartActivity(typeof(MainActivity));
        }
    }
}