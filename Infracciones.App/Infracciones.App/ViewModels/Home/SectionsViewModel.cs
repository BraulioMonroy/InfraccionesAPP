using Infracciones.App.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Infracciones.App.ViewModels.Home
{
    /// <summary>
    /// ViewModel of Home (Sections) template
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SectionsViewModel : BaseViewModel
    {
        #region Fields

        private string productDescription;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="T:Infracciones.App.ViewModels.Home.SectionsViewModel"/> class.
        /// </summary>
        public SectionsViewModel()
        {
            this.productDescription = "Plataforma de Sanciones y Cobros";
            //this.productIcon = App.BaseImageUrl + "Icon.png";
            //this.productVersion = "1.0";

            this.SectionsDetails = new ObservableCollection<HomeModel>
            {               
                new HomeModel
                {
                    SectionName = "Sanciones",
                    Image = "icnSancionesWhiteBorder.png",
                    Description = "Sanciones",
                    Sectionkey = "Sanction"
                },
                new HomeModel
                {
                    SectionName = "Información",
                    Image = "icnCarroLupaWhite.png",
                    Description = "Información del vehículo",
                    Sectionkey = "Vehicle"
                },
                new HomeModel
                {
                    SectionName = "Pago",
                    Image = "icnPagoWhite.png",
                    Description = "Pago",
                    Sectionkey = "Payment"
                },
                new HomeModel
                {
                    SectionName = "Corte",
                    Image = "icnCajaWhite.png",
                    Description = "Corte de caja",
                    Sectionkey = "CashClosing"
                },
                new HomeModel
                {
                    SectionName = "Consulta",
                    Image = "icnConsultasWhite.png",
                    Description = "Consulta REPUVE",
                    Sectionkey = "Repuve"
                },
                new HomeModel
                {
                    SectionName = "Calidad del Aire",
                    Image = "icnCalidadDelAireWhite.png",
                    Description = "Calidad del Aire",
                    Sectionkey = "AirQuality"
                }
            };

            this.ItemSelectedCommand = new Command(this.ItemSelected);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the description of the app
        /// </summary>
        /// <value>The product description.</value>
        public string ProductDescription
        {
            get
            {
                return this.productDescription;
            }

            set
            {
                this.productDescription = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the sections details.
        /// </summary>
        /// <value>The section details.</value>
        public ObservableCollection<HomeModel> SectionsDetails { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        private void ItemSelected(object selectedItem)
        {
            //do something
        }


        #endregion
    }
}