using Android.Preferences;
using Infracciones.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Essentials;

namespace Infracciones.ViewModels.Shared
{
    public class ProfileViewModel : BaseViewModel
    {
        #region Fields

        private string _date;
        private string _time;
        private string _officer;
        private string _Key;
        private string _shift;
        private string _sector;

        #endregion

        #region Properties

        public string Date
        {
            get { return _date; }
            set { _date = value; NotifyPropertyChanged(); }
        }

        public string Time
        {
            get { return _time; }
            set { _time = value; NotifyPropertyChanged(); }
        }

        public string Officer
        {
            get { return _officer; }
            set { _officer = value; NotifyPropertyChanged(); }
        }

        public string Key
        {
            get { return _Key; }
            set { _Key = value; NotifyPropertyChanged(); }
        }

        public string Shift
        {
            get { return _shift; }
            set { _shift = value; NotifyPropertyChanged(); }
        }

        public string Sector
        {
            get { return _sector; }
            set { _sector = value; NotifyPropertyChanged(); }
        }

        #endregion

        #region Constructor
        
        public ProfileViewModel()
        {
            Date = DateTime.Now.ToString("D", new CultureInfo("es-mx"));
            Time = DateTime.Now.ToString("HH:mm");
            Officer = Preferences.Get("Officer", string.Empty);
            Key = Preferences.Get("Key", string.Empty);
            Shift = Preferences.Get("Shift", string.Empty);
            Sector = Preferences.Get("Sector", string.Empty);
        }

        #endregion
    }
}
