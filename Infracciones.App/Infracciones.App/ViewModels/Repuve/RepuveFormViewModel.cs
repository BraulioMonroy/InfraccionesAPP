using Infracciones.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Infracciones.ViewModels.Repuve
{
    public class RepuveFormViewModel : BaseMediaViewModel
    {
        #region Fields
        #endregion

        #region Property

        public Field<string> Plates { get; set; }
        public Field<string> VehicleIdNumber { get; set; }
        public Field<string> RegistrationCertKey { get; set; }
        public Field<string> RegistrationCertNumber { get; set; }
        public bool IsValid { get; set; }
        public Command ClearCommand { get; set; }

        #endregion

        #region Constructor

        public RepuveFormViewModel()
        {
            Plates = new Field<string>();
            VehicleIdNumber = new Field<string>();
            RegistrationCertKey = new Field<string>();
            RegistrationCertNumber = new Field<string>();
            IsValid = false;

            Validate(nameof(Plates));
            Validate(nameof(VehicleIdNumber));
            Plates.PropertyChanged += Plates_PropertyChanged;
            VehicleIdNumber.PropertyChanged += Model_PropertyChanged;

            ClearCommand = new Command(() =>
            {
                Plates.PropertyChanged -= Plates_PropertyChanged;
                VehicleIdNumber.PropertyChanged -= Model_PropertyChanged;

                Plates = new Field<string>();
                VehicleIdNumber = new Field<string>();
                RegistrationCertKey = new Field<string>();
                RegistrationCertNumber = new Field<string>();
                IsValid = false;

                Validate(nameof(Plates));
                Validate(nameof(VehicleIdNumber));
                Plates.PropertyChanged += Plates_PropertyChanged;
                VehicleIdNumber.PropertyChanged += Model_PropertyChanged;
            });
        }

        private void Plates_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Plates.Value)) Validate(nameof(Plates));
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(VehicleIdNumber.Value)) Validate(nameof(VehicleIdNumber));
        }

        public void Validate(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Plates):
                    {
                        if (string.IsNullOrEmpty(Plates.Value))
                        {
                            Plates.IsNotValid = true;
                            Plates.NotValidMessageError = "Obligatorio.";
                        }
                        else
                        {
                            Plates.IsNotValid = false;
                            Plates.NotValidMessageError = "";
                        }
                        break;
                    }
                case nameof(VehicleIdNumber):
                    {
                        if (string.IsNullOrEmpty(VehicleIdNumber.Value))
                        {
                            VehicleIdNumber.IsNotValid = true;
                            VehicleIdNumber.NotValidMessageError = "Obligatorio.";
                        }
                        else
                        {
                            VehicleIdNumber.IsNotValid = false;
                            VehicleIdNumber.NotValidMessageError = "";
                        }
                        break;
                    }
                default: break;
            }

            if (Plates.IsNotValid || VehicleIdNumber.IsNotValid) IsValid = false;
            else IsValid = true;
        }

        #endregion
    }
}
