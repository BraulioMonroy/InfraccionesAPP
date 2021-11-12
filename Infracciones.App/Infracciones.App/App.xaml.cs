using DLToolkit.Forms.Controls;
using Infracciones.App.Services;
using Infracciones.App.Views.Home;
using Infracciones.App.Views.Identity;
using Infracciones.Infrastructure.ZebraPrintStation.Sqlite;
using System;
using System.IO;
using UnixTimeStamp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Infracciones.App
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = "https://demosystems.net/sedema/test/Media/";
        public static string BaseUri { get; } = "https://infracciones.promad.com.mx:30443/infraccionesAPP";        
        public static string DatabaseName = "sedema_sanciones.db3";
        public static string PrintStationDatabaseName = "sedema_sanciones_print_station.db3";

        private static PrintStationDatabase database;

        public static PrintStationDatabase Database
        {
            get
            {
                if (database == null)
                {
                    string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Print Station");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    database = new PrintStationDatabase(Path.Combine(directory, PrintStationDatabaseName));
                }
                return database;
            }
        }


        //TODO: Catch exceptions on Endpoints and services
        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        public App()
        {
            InitializeComponent();

            FlowListView.Init();
            Device.SetFlags(new string[] { "RadioButton_Experimental" });
            Plugin.Media.CrossMedia.Current.Initialize();

            var refreshToken = Preferences.Get("refreshToken", string.Empty);
            if (string.IsNullOrEmpty(refreshToken))
            {
                IdentityService.Logout();
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                var refreshTokenExpirationTime = Preferences.Get("refreshTokenExpirationTime", 0);
                if (UnixTime.GetCurrentTime() > refreshTokenExpirationTime)
                {
                    IdentityService.Logout();
                    MainPage = new NavigationPage(new LoginPage());
                }
                else
                    MainPage = new NavigationPage(new HomePage());
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null) System.Diagnostics.Debugger.Break();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
