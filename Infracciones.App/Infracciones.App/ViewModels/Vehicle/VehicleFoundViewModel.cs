using Android.Runtime;
using Infracciones.App.Models;
using Infracciones.Helpers;
using System;
using System.ComponentModel;

namespace Infracciones.App.ViewModels.Vehicle
{
    [Preserve(AllMembers = true)]
    public class VehicleFoundViewModel : BaseViewModel
    {
        #region Fields
        #endregion

        #region Property

        public VehicleModel Vehicle { get; set; }
        public Field<string> Responsible { get; set; }
        public bool IsValid { get; set; }
        public bool HasCompleteInformation { get; set; }

        #endregion

        #region Constructor

        public VehicleFoundViewModel(VehicleModel model)
        {
            Vehicle = model;
            Responsible = new Field<string> { Value = model.Responsible };
            IsValid = false;
            HasCompleteInformation = CheckForCompleteInformation(model);

            Validate(nameof(Responsible));
            Responsible.PropertyChanged += Responsible_PropertyChanged;
        }

        private bool CheckForCompleteInformation(VehicleModel model)
        {
            return model.Brand != null &&
                model.Subbrand != null &&
                model.Origin != null &&
                model.VehicleType != null &&
                model.Model != 0 &&
                !string.IsNullOrWhiteSpace(model.Plates) &&
                !string.IsNullOrWhiteSpace(model.Responsible);
        }

        private void Responsible_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Responsible.Value))
            {
                Vehicle.Responsible = Responsible.Value;
                Validate(nameof(Responsible));
            }
        }

        public void Validate(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Responsible):
                    {
                        if (string.IsNullOrEmpty(Responsible.Value))
                        {
                            Responsible.IsNotValid = true;
                            Responsible.NotValidMessageError = "Obligatorio.";
                        }
                        else
                        {
                            Responsible.IsNotValid = false;
                            Responsible.NotValidMessageError = "";
                        }
                        break;
                    }
                default: break;
            }

            if (Responsible.IsNotValid) IsValid = false;
            else IsValid = true;
        }

        #endregion
    }
}
