using Android.Runtime;
using Infracciones.Helpers;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Infracciones.App.ViewModels.Vehicle
{
    [Preserve(AllMembers = true)]
    public class SearchVehicleViewModel : BaseViewModel
    {
        #region Fields
        #endregion

        #region Property

        public Field<string> Plates { get; set; }
        public Field<string> Responsible { get; set; }
        public Field<object> Brand { get; set; }
        public Field<object> SubBrand { get; set; }
        public Field<object> Model { get; set; }
        public Field<object> Origin { get; set; }
        public Field<object> VehicleType { get; set; }
        public Field<object> CheckRepuve { get; set; }
        public bool IsValid { get; set; }

        #endregion

        #region Constructor

        public SearchVehicleViewModel()
        {
            Plates = new Field<string>();
            Responsible = new Field<string>();
            Brand = new Field<object>();
            SubBrand = new Field<object>();
            Model = new Field<object>();
            Origin = new Field<object>();
            VehicleType = new Field<object>();
            CheckRepuve = new Field<object> { Value = false };
            IsValid = false;

            Validate(nameof(Responsible));
            Validate(nameof(Plates));
            Validate(nameof(Model));
            Responsible.PropertyChanged += Responsible_PropertyChanged;
            Plates.PropertyChanged += Plates_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Responsible_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Responsible.Value)) Validate(nameof(Responsible));
        }

        private void Plates_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Plates.Value)) Validate(nameof(Plates));
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Model.Value)) Validate(nameof(Model));
        }

        public void Validate(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Responsible):
                    if (string.IsNullOrEmpty(Responsible.Value))
                    {
                        Responsible.IsNotValid = true;
                        Responsible.NotValidMessageError = "Obligatorio.";
                    }
                    else if (!Regex.Match(Responsible.Value, @"^[a-zA-Z]+$").Success || Responsible.Value.Length > 50)
                    {
                        Responsible.IsNotValid = true;
                        Responsible.NotValidMessageError = "No debe contener carácteres especiales y numéricos y/o debe ser menor a 50 carácteres";
                    }
                    else
                    {
                        Responsible.IsNotValid = false;
                        Responsible.NotValidMessageError = "";
                    }
                    break;
                case nameof(Plates):
                    {
                        if (string.IsNullOrEmpty(Plates.Value))
                        {
                            Plates.IsNotValid = true;
                            Plates.NotValidMessageError = "Obligatorio.";
                        }
                        else if (!Regex.Match(Plates.Value, @"^[a-zA-Z0-9]+$").Success || Plates.Value.Length > 7)
                        {
                            Plates.IsNotValid = true;
                            Plates.NotValidMessageError = "No debe contener carácteres especiales y/o debe ser menor a 7 carácteres";
                        }
                        else
                        {
                            Plates.IsNotValid = false;
                            Plates.NotValidMessageError = "";
                        }
                        break;
                    }
                case nameof(Model):
                    {
                         if (Model.Value != null)
                        {
                            var outParse = 0;
                            if (!int.TryParse(Model.Value.ToString(), out outParse))
                            {
                                    Model.IsNotValid = true;
                                    Model.NotValidMessageError = "Solo se permiten números enteros.";
                               
                            }
                            else if(int.Parse(Model.Value.ToString()) < 1950 || int.Parse(Model.Value.ToString()) > DateTime.Now.Year + 1)
                            {
                                Model.IsNotValid = true;
                                Model.NotValidMessageError = $"La placa debe ser mayor a 1950 y menor {DateTime.Now.Year + 1}";

                            }
                            else
                            {

                                Model.IsNotValid = false;
                                Model.NotValidMessageError = "";
                            }
                        } else
                        {
                            Model.IsNotValid = true;
                            Model.NotValidMessageError = "Obligatorio.";
                        }
                        break;
                    }
                default: break;
            }

            if (Plates.IsNotValid || Responsible.IsNotValid || Model.IsNotValid) 
                IsValid = false;
            else 
                IsValid = true;
        }

        #endregion
    }
}
