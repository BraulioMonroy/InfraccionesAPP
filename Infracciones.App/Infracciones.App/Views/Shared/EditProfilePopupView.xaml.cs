using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePopupView : PopupPage
    {
        public EditProfilePopupView()
        {
            InitializeComponent();
        }

        private async void ToggleButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}