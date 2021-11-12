using Infracciones.App.Models;
using Infracciones.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Infracciones.App.ViewModels.Sanction
{
    /// <summary>
    /// ViewModel of Type Sanctions template
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SanctionTypeViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="T:Infracciones.App.ViewModels.Home.SanctionTypeViewModel"/> class.
        /// </summary>
        public SanctionTypeViewModel()
        {
            this.TypeSanctionsDetails = new ObservableCollection<TypeSanctionModel>
            {
                new TypeSanctionModel
                {
                    SanctionName = "Emisión de gases",
                    SanctionReason = SanctionReason.EmisionDeGases,
                    Image = "icnEmisionGases.png",                    
                    Description = "Emisión de humo negro, azul o vehículo ostensiblemente contaminante.",
                    SanctionKey = "gasemisions"
                },
                new TypeSanctionModel
                {
                    SanctionName = "Falta de documentos",
                    SanctionReason = SanctionReason.FaltaDeDocumentos,
                    Image = "icnFaltaDocumentos.png",
                    Description = "Holograma o Certificado de Verificación Vehicular Vigente, entre otros.",
                    SanctionKey = "nodocuments"
                },
                new TypeSanctionModel
                {
                    SanctionName = "Circular en horario no correspondiente",
                    SanctionReason = SanctionReason.HorarioNoPermitido,
                    Image = "icnNoCircula.png",
                    Description = "Circular en horario o día restringido",
                    SanctionKey = "scheduletimes"
                }
            };

            this.ItemSelectedCommand = new Command(this.ItemSelected);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the sanctions details.
        /// </summary>
        /// <value>The section details.</value>
        public ObservableCollection<TypeSanctionModel> TypeSanctionsDetails { get; set; }

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
