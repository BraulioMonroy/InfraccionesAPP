using Infracciones.Infrastructure.ZebraPrintStation.Helpers;
using Infracciones.Infrastructure.ZebraPrintStation.Services;
using Infracciones.Infrastructure.ZebraPrintStation.ViewModels;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPrinterPage : ContentPage
    {
        private SelectPrinterPageViewModel viewModel = new SelectPrinterPageViewModel();
        private PrintStationPage mainPage;

        public SelectPrinterPage(PrintStationPage mainPage)
        {
            InitializeComponent();

            BindingContext = viewModel;
            this.mainPage = mainPage;

            Device.BeginInvokeOnMainThread(async () => {
                await DiscoverPrintersAsync();
            });
        }

        private async Task ClearDiscoveredPrinterListAsync()
        {
            await Task.Factory.StartNew(() => {
                viewModel.HighlightedPrinter = null;
            });

            Device.BeginInvokeOnMainThread(() => {
                try
                {
                    viewModel.DiscoveredPrinterList.Clear(); // ListView view model operations must be done on UI thread due to iOS issues when clearing list while item is selected: https://forums.xamarin.com/discussion/19114/invalid-number-of-rows-in-section
                }
                catch (NotImplementedException)
                {
                    viewModel.DiscoveredPrinterList.Clear(); // Workaround for Xamarin.Forms.Platform.WPF issue: https://github.com/xamarin/Xamarin.Forms/issues/3648
                }
            });
        }

        private async Task AnimateRefreshIconAsync()
        {
            await RefreshIconAnimator.AnimateOneRotationAsync(RefreshIcon);

            if (viewModel.IsPrinterListRefreshing)
            {
                await AnimateRefreshIconAsync();
            }
        }

        private async Task DiscoverPrintersAsync()
        {
            await ClearDiscoveredPrinterListAsync();

            await Task.Factory.StartNew(() => {
                viewModel.IsPrinterListRefreshing = true;
            });

            Device.BeginInvokeOnMainThread(async () => {
                await AnimateRefreshIconAsync();
            });

            await Task.Factory.StartNew(() => {
                try
                {
                    List<DiscoveredPrinter> usbDriverPrinters = DependencyService.Get<IConnectionManager>().GetZebraUsbDriverPrinters();
                    foreach (DiscoveredPrinter printer in usbDriverPrinters)
                    {
                        Device.BeginInvokeOnMainThread(() => {
                            viewModel.DiscoveredPrinterList.Add(printer); // ListView view model operations must be done on UI thread due to iOS issues when clearing list while item is selected: https://forums.xamarin.com/discussion/19114/invalid-number-of-rows-in-section
                        });
                    }
                }
                catch (Exception)
                {
                    // Do nothing
                }

                try
                {
                    DiscoveryHandlerImplementation usbDiscoveryHandler = new DiscoveryHandlerImplementation(this);
                    DependencyService.Get<IConnectionManager>().GetZebraUsbDirectPrinters(usbDiscoveryHandler);

                    while (!usbDiscoveryHandler.IsFinished)
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (Exception)
                {
                    // Do nothing
                }

                DiscoveryHandlerImplementation networkDiscoveryHandler = new DiscoveryHandlerImplementation(this);
                NetworkDiscoverer.LocalBroadcast(networkDiscoveryHandler);

                while (!networkDiscoveryHandler.IsFinished)
                {
                    Thread.Sleep(100);
                }

                if (Device.RuntimePlatform != Device.WPF || DependencyService.Get<IPlatformHelper>().IsWindows10())
                {
                    try
                    {
                        DiscoveryHandlerImplementation bluetoothDiscoveryHandler = new DiscoveryHandlerImplementation(this);
                        DependencyService.Get<IConnectionManager>().FindBluetoothPrinters(bluetoothDiscoveryHandler);

                        while (!bluetoothDiscoveryHandler.IsFinished)
                        {
                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }
                }
            });

            await Task.Factory.StartNew(() => {
                viewModel.IsPrinterListRefreshing = false;
            });

            ViewExtensions.CancelAnimations(RefreshIcon);
        }

        private async void RefreshIcon_Tapped(object sender, EventArgs e)
        {
            if (!viewModel.IsPrinterListRefreshing)
            {
                await DiscoverPrintersAsync();
            }
        }

        private async void SelectButton_Clicked(object sender, EventArgs eventArgs)
        {
            if (!viewModel.IsSelectingPrinter && Navigation.NavigationStack.Count > 0 && Navigation.NavigationStack.Last().GetType() == typeof(SelectPrinterPage))
            {
                await Task.Factory.StartNew(() => {
                    viewModel.IsSelectingPrinter = true;
                });

                try
                {
                    DiscoveredPrinter selectedPrinter = (DiscoveredPrinter)PrinterList.SelectedItem;
                    Connection connection = null;

                    try
                    {
                        await Task.Factory.StartNew(() => {
                            connection = ConnectionCreator.Create(selectedPrinter);
                            connection.Open();
                        });
                    }
                    catch (Exception e)
                    {
                        CloseAllPopup();

                        var errorPopupView = new ErrorPopupView()
                        {
                            BindingContext = new PopupViewModel()
                            {
                                Text = e.Message,
                                Image = "Error.png"
                            }
                        };

                        await Navigation.PushPopupAsync(errorPopupView);
                        return;
                    }
                    finally
                    {
                        await Task.Factory.StartNew(() => {
                            try
                            {
                                connection?.Close();
                            }
                            catch (ConnectionException) { }
                        });
                    }

                    await Task.Factory.StartNew(() => {
                        mainPage.ViewModel.SelectedPrinter = selectedPrinter;
                    });

                    mainPage.RefreshFormatLists();

                    await Navigation.PopAsync();
                }
                finally
                {
                    await Task.Factory.StartNew(() => {
                        viewModel.IsSelectingPrinter = false;
                    });
                }
            }
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }

        class DiscoveryHandlerImplementation : DiscoveryHandler
        {

            private SelectPrinterPage selectPrinterPage;

            public bool IsFinished { get; private set; } = false;

            public DiscoveryHandlerImplementation(SelectPrinterPage selectPrinterPage)
            {
                this.selectPrinterPage = selectPrinterPage;
            }

            public void DiscoveryError(string message)
            {
                IsFinished = true;
            }

            public void DiscoveryFinished()
            {
                IsFinished = true;
            }

            public void FoundPrinter(DiscoveredPrinter printer)
            {
                Device.BeginInvokeOnMainThread(() => {
                    selectPrinterPage.viewModel.DiscoveredPrinterList.Add(printer); // ListView view model operations must be done on UI thread due to iOS issues when clearing list while item is selected: https://forums.xamarin.com/discussion/19114/invalid-number-of-rows-in-section
                });
            }

          
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }
    }
}