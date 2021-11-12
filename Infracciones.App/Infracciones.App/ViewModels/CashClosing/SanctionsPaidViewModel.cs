using Infracciones.App.Models;
using Infracciones.App.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Infracciones.App.ViewModels.CashClosing
{
    public class SanctionsPaidViewModel : BaseViewModel
    { 
        #region Constructor

        public SanctionsPaidViewModel(ObservableCollection<SanctionsPaid> sanctionsPaids)
        {
            this.SanctionsPaidDetails = sanctionsPaids; 

            this.ItemSelectedCommand = new Command(this.ItemSelected);
        }       

        #endregion

        #region Properties

        public ObservableCollection<SanctionsPaid> SanctionsPaidDetails { get; set; }

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
