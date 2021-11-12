using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.CashClosing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CashClosingSendingPopupView : PopupPage
    {
        //float maxValue = 1;
        //float progressmax = 3;
        //bool istimerRunning = true;
        //float progress = 0;
        //int counter = 1;

        public CashClosingSendingPopupView()
        {
            InitializeComponent();

            //Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            //{
            //    if (progress >= 1)
            //    {
            //        istimerRunning = false;
            //        Navigation.PopPopupAsync();
            //    }
            //    else
            //    {
            //        progress += maxValue / progressmax;
            //        //progressbar.ProgressTo(progress, 500, Easing.Linear);
            //        progressLabel.Text = $"Enviando corte...";
            //        //counter += 1;
            //    }

            //    return istimerRunning;
            //});
        }
    }
}