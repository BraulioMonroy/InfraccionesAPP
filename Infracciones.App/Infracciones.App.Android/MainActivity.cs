using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Infracciones.Infrastructure.ZebraPrintStation.Services;
using Plugin.CurrentActivity;
using System;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Infracciones.App.Droid
{
    [Activity(Label = "SEDEMA: Sanciones y Cobros", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation), 
        IntentFilter(new[] { "android.nfc.action.NDEF_DISCOVERED" }, DataHost = "zebra.com", DataPath = "/apps/r/nfc", DataScheme = "http", Categories = new[] { "android.intent.category.DEFAULT" }),
        IntentFilter(new[] { "android.nfc.action.NDEF_DISCOVERED" }, DataHost = "www.zebra.com", DataPath = "/apps/r/nfc", DataScheme = "http", Categories = new[] { "android.intent.category.DEFAULT" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 0;
        const int AccessCoarseLocationPermissionRequestCode = 0;

        public NfcAdapter nfcAdapter; // = ((NfcManager)Application.Context.GetSystemService(NfcService)).DefaultAdapter;
        public NfcManagerImplementation nfcManagerImplementation;
        bool isNfcAvailable = false;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

   
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.SetFlags("CollectionView_Experimental");
            Forms.SetFlags("RadioButton_Experimental");
            Forms.SetFlags("CarouselView_Experimental");

            //Getting access to current activity
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            //Loading and caching images
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            //Popupviews contol
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            //Datagrid control
            Xamarin.Forms.DataGrid.DataGridComponent.Init();

            //Xamarin Essentials controls
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);


            nfcAdapter = NfcAdapter.GetDefaultAdapter(this);

            //Init Xamarin.Forms
            Forms.Init(this, savedInstanceState);

            //NFC Adapter
           
            DependencyService.Register<INfcManager, NfcManagerImplementation>();
            nfcManagerImplementation = DependencyService.Get<INfcManager>() as NfcManagerImplementation;
            isNfcAvailable = nfcManagerImplementation.IsNfcAvailable();

            LoadApplication(new App());

            // Change the status bar color: white. In Resources/Values
            this.SetStatusBarColor(Android.Graphics.Color.Transparent);         

            // Enable scrolling to the page when the keyboard is enabled
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            
            //Enable Xamarin.Forms.Maps
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            GetAccessCoarseLocationPermission();
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null) System.Diagnostics.Debugger.Break();
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            if (e.Exception != null) System.Diagnostics.Debugger.Break();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (isNfcAvailable)
            {
                if (NfcAdapter.ActionNdefDiscovered.Equals(Intent.Action))
                {
                    nfcManagerImplementation.OnNewIntent(this, Intent);
                }

                if (nfcAdapter != null)
                {
                    Intent intent = new Intent(this, GetType()).AddFlags(ActivityFlags.SingleTop);
                    PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

                    nfcAdapter.EnableForegroundDispatch(this, pendingIntent, null, null);
                }
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            if (isNfcAvailable)
            {
                nfcAdapter.DisableForegroundDispatch(this);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (isNfcAvailable)
            {
                nfcManagerImplementation.OnNewIntent(this, intent);
            }
        }

        private void GetAccessCoarseLocationPermission()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
            {
                return;
            }

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessCoarseLocation))
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Permission Required")
                    .SetMessage("Print Station requires permission to access your location in order to perform Bluetooth discovery. Please accept this permission to allow Bluetooth discovery to function properly.")
                    .SetPositiveButton("OK", OnPermissionRequiredDialogOkClicked)
                    .SetCancelable(false)
                    .Show();

                return;
            }

            RequestAccessCoarseLocationPermission();
        }

        private void OnPermissionRequiredDialogOkClicked(object sender, DialogClickEventArgs e)
        {
            RequestAccessCoarseLocationPermission();
        }

        private void RequestAccessCoarseLocationPermission()
        {
            ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessCoarseLocation }, AccessCoarseLocationPermissionRequestCode);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
            }
        }
    }
}