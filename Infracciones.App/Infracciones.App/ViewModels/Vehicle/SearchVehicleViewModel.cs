using Android.Runtime;
using Infracciones.Helpers;
using System.ComponentModel;

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

            Validate(nameof(Plates));
            Validate(nameof(Model));
            Plates.PropertyChanged += Plates_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
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
                            else
                            {
                                Model.IsNotValid = false;
                                Model.NotValidMessageError = "";
                            }
                        }
                        break;
                    }
                default: break;
            }

            if (Plates.IsNotValid || Responsible.IsNotValid || Model.IsNotValid) IsValid = false;
            else IsValid = true;
        }

        #endregion
    }
}
