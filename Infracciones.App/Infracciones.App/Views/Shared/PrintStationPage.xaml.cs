using Infracciones.App.Views.Home;
using Infracciones.Infrastructure.ZebraPrintStation.Helpers;
using Infracciones.Infrastructure.ZebraPrintStation.Models;
using Infracciones.Infrastructure.ZebraPrintStation.Services;
using Infracciones.Infrastructure.ZebraPrintStation.Sqlite;
using Infracciones.Infrastructure.ZebraPrintStation.ViewModels;
using Infracciones.ViewModels.Sanction;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrintStationPage : ContentPage
    {
        private const string IsAppInitializedPropertyName = "IsAppInitialized";
        public PrintStationPageViewModel ViewModel { get; } = new PrintStationPageViewModel();
        private readonly ListSanctionViewModel sanctions;
        private readonly FormatType formatType;

        public PrintStationPage(ListSanctionViewModel sanctions, FormatType formatType)
        {
            InitializeComponent();

            BindingContext = ViewModel;
            this.sanctions = sanctions;
            this.formatType = formatType;

            try
            {
                if (DependencyService.Get<INfcManager>().IsNfcAvailable())
                {
                    DependencyService.Get<INfcManager>().TagScanned += async (object sender, string nfcData) => {
                        Device.BeginInvokeOnMainThread(async () => {
                            await Navigation.PopToRootAsync(); // Must call Navigation.PopToRootAsync() on UI thread on Windows tablets
                        });

                        await Task.Factory.StartNew(() => {
                            ViewModel.IsSelectingNfcPrinter = true;
                            ViewModel.SelectedPrinter = null;

                            try
                            {
                                ViewModel.StoredFormatList.Clear();
                            }
                            catch (NotImplementedException)
                            {
                                ViewModel.StoredFormatList.Clear(); // Workaround for Xamarin.Forms.Platform.WPF issue: https://github.com/xamarin/Xamarin.Forms/issues/3648
                            }

                            try
                            {
                                ViewModel.PrinterFormatList.Clear();
                            }
                            catch (NotImplementedException)
                            {
                                ViewModel.PrinterFormatList.Clear(); // Workaround for Xamarin.Forms.Platform.WPF issue: https://github.com/xamarin/Xamarin.Forms/issues/3648
                            }
                        });

                        Connection connection = null;
                        DiscoveredPrinter printer = null;
                        bool success = false;

                        try
                        {
                            await Task.Factory.StartNew(() => {
                                printer = NfcDiscoverer.DiscoverPrinter(nfcData);

                                connection = printer.GetConnection();
                                try
                                {
                                    connection.Open(); // Test connection
                                }
                                catch (Exception e)
                                {
                                    throw new ConnectionException("No fue posible abrir la conexión. Por favor, revise que la impresora este encendida y que el servicio de Bluetooth del dispositivo esté habilitado. ", e);
                                }

                                success = true;
                            });
                        }
                        catch (Exception e)
                        {
                            await DisplayError(e.Message);
                        }
                        finally
                        {
                            await Task.Factory.StartNew(() => {
                                try
                                {
                                    connection?.Close();
                                }
                                catch (Exception) { }

                                if (success)
                                {
                                    ViewModel.SelectedPrinter = printer;
                                    ViewModel.IsSelectingNfcPrinter = false;
                                    RefreshFormatLists();
                                }
                                else
                                {
                                    ViewModel.IsSelectingNfcPrinter = false;
                                }
                            });
                        }
                    };
                }
            }
            catch (TypeLoadException) { } // NFC is not supported on Windows 7

            InitializeStoredFormats();
        }

        private async Task AnimateStoredFormatsRefreshIconAsync()
        {
            await RefreshIconAnimator.AnimateOneRotationAsync(StoredFormatsRefreshIcon);

            if (ViewModel.IsStoredFormatListRefreshing)
            {
                await AnimateStoredFormatsRefreshIconAsync();
            }
        }

        private async Task AnimatePrinterFormatsRefreshIconAsync()
        {
            await RefreshIconAnimator.AnimateOneRotationAsync(PrinterFormatsRefreshIcon);

            if (ViewModel.IsPrinterFormatListRefreshing)
            {
                await AnimatePrinterFormatsRefreshIconAsync();
            }
        }

        private string GetStoredFormatDirectory()
        {
            string directoryName;

            try
            {
                directoryName = DependencyService.Get<IPlatformHelper>().GetIOSBundleIdentifier();
            }
            catch (NotImplementedException)
            {
                directoryName = "Print Station";
            }

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), directoryName);
        }

        private void InitializeStoredFormats()
        {
            if (!Application.Current.Properties.ContainsKey(IsAppInitializedPropertyName))
            {
                Task.Factory.StartNew(async () => {
                    await App.App.Database.SaveStoredFormatAsync(new PrintStationDatabase.StoredFormat
                    {
                        DriveLetter = "E",
                        Name = "SANCTIONRECEIPT",
                        Extension = "ZPL",
                        Content = GetZplFormat.SanctionReceipt()
                    });

                    await App.App.Database.SaveStoredFormatAsync(new PrintStationDatabase.StoredFormat
                    {
                        DriveLetter = "E",
                        Name = "PAYMENTRECEIPT",
                        Extension = "ZPL",
                        Content = GetZplFormat.PaymentReceipt()
                    });

                });

                Application.Current.Properties.Add(IsAppInitializedPropertyName, true);
                Application.Current.SavePropertiesAsync();
            }
        }

        private async Task RefreshStoredFormatListAsync()
        {
            await Task.Factory.StartNew(() => {
                try
                {
                    ViewModel.StoredFormatList.Clear();
                }
                catch (NotImplementedException)
                {
                    ViewModel.StoredFormatList.Clear(); // Workaround for Xamarin.Forms.Platform.WPF issue: https://github.com/xamarin/Xamarin.Forms/issues/3648
                }
            });

            if (ViewModel.SelectedPrinter != null)
            {
                await Task.Factory.StartNew(() => {
                    ViewModel.IsStoredFormatListRefreshing = true;
                });

                Device.BeginInvokeOnMainThread(async () => {
                    await AnimateStoredFormatsRefreshIconAsync();
                });

                await Task.Factory.StartNew(() => {
                    Thread.Sleep(1000); // Give the user some time to read the progress message to confirm that refresh is occurring
                });

                await AddStoredFormatsToFormatListAsync();

                await Task.Factory.StartNew(() => {
                    ViewModel.IsStoredFormatListRefreshing = false;
                });

                ViewExtensions.CancelAnimations(StoredFormatsRefreshIcon);
            }
        }

        private async Task RefreshPrinterFormatListAsync()
        {
            await Task.Factory.StartNew(() => {
                try
                {
                    ViewModel.PrinterFormatList.Clear();
                }
                catch (NotImplementedException)
                {
                    ViewModel.PrinterFormatList.Clear(); // Workaround for Xamarin.Forms.Platform.WPF issue: https://github.com/xamarin/Xamarin.Forms/issues/3648
                }
            });

            if (ViewModel.SelectedPrinter != null)
            {
                await Task.Factory.StartNew(() => {
                    ViewModel.IsPrinterFormatListRefreshing = true;
                });

                Device.BeginInvokeOnMainThread(async () => {
                    await AnimatePrinterFormatsRefreshIconAsync();
                });

                await AddPrinterFormatsToFormatListAsync();

                await Task.Factory.StartNew(() => {
                    ViewModel.IsPrinterFormatListRefreshing = false;
                });

                //ViewExtensions.CancelAnimations(PrinterFormatsRefreshIcon);
            }
        }

        public void RefreshFormatLists()
        {
            Device.BeginInvokeOnMainThread(async () => {
                await RefreshStoredFormatListAsync();
            });

            Device.BeginInvokeOnMainThread(async () => {
                await RefreshPrinterFormatListAsync();
            });
        }

        private async Task AddStoredFormatsToFormatListAsync()
        {
            List<FormatViewModel> formats = new List<FormatViewModel>();

            await Task.Factory.StartNew(async () => {
                List<PrintStationDatabase.StoredFormat> storedFormats = await App.App.Database.GetStoredFormatsAsync();
                foreach (PrintStationDatabase.StoredFormat storedFormat in storedFormats)
                {
                    if (string.Equals(storedFormat.Extension, "zpl", StringComparison.OrdinalIgnoreCase))
                    {
                        formats.Add(new FormatViewModel
                        {
                            DatabaseId = storedFormat.Id,
                            DriveLetter = storedFormat.DriveLetter,
                            Name = storedFormat.Name,
                            Extension = storedFormat.Extension,
                            Content = storedFormat.Content,
                            Source = FormatSource.LocalStorage,
                            OnDeleteButtonClicked = new Command(async (object parameter) => {
                                if (!ViewModel.IsPrinterFormatListRefreshing && !ViewModel.IsSavingFormat)
                                {
                                    FormatViewModel format = parameter as FormatViewModel;

                                    bool deleteFormat = await DisplayAlert("Delete Format", $"Are you sure you want to delete the stored format '{format.PrinterPath}'?", "Delete", "Cancel");
                                    if (deleteFormat)
                                    {
                                        await Task.Factory.StartNew(() => {
                                            ViewModel.IsDeletingFormat = true;
                                            format.IsDeleting = true;
                                        });

                                        await App.App.Database.DeleteStoredFormatByIdAsync(format.DatabaseId);

                                        await Task.Factory.StartNew(() => {
                                            format.IsDeleting = false;
                                            ViewModel.IsDeletingFormat = false;
                                        });

                                        await RefreshStoredFormatListAsync();
                                    }
                                }
                            }),
                            OnPrintButtonClicked = new Command(async (object parameter) => {
                                if (!ViewModel.IsPrinterFormatListRefreshing && !ViewModel.IsSavingFormat)
                                {
                                    //FormatViewModel format = parameter as FormatViewModel;
                                    //await PushPageAsync(new PrintFormatPage(ViewModel.SelectedPrinter, format));
                                }
                            })
                        });
                    }
                }

                formats.ForEach(ViewModel.StoredFormatList.Add);
            });
        }

        private async Task<string> RetrieveFormatContentAsync(FormatViewModel format)
        {
            string formatContent = null;
            Connection connection = null;

            try
            {
                await Task.Factory.StartNew(() => {
                    connection = ConnectionCreator.Create(ViewModel.SelectedPrinter);
                    connection.Open();

                    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
                    ZebraPrinterLinkOs linkOsPrinter = ZebraPrinterFactory.CreateLinkOsPrinter(printer);

                    formatContent = Encoding.UTF8.GetString(printer.RetrieveFormatFromPrinter(format.PrinterPath));
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

            return formatContent;
        }

        private async Task SavePrinterFormatAsync(FormatViewModel format)
        {
            PrintStationDatabase.StoredFormat storedFormat = null;
            Connection connection = null;
            bool fileWriteAttempted = false;
            bool linePrintEnabled = false;

            try
            {
                await Task.Factory.StartNew(async () => {
                    connection = ConnectionCreator.Create(ViewModel.SelectedPrinter);
                    connection.Open();

                    string originalPrinterLanguage = SGD.GET(PrintFormatPage.DeviceLanguagesSgd, connection);
                    linePrintEnabled = "line_print".Equals(originalPrinterLanguage, StringComparison.OrdinalIgnoreCase);

                    if (linePrintEnabled)
                    {
                        SGD.SET(PrintFormatPage.DeviceLanguagesSgd, "zpl", connection);
                    }

                    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
                    ZebraPrinterLinkOs linkOsPrinter = ZebraPrinterFactory.CreateLinkOsPrinter(printer);

                    string formatContent = Encoding.UTF8.GetString(printer.RetrieveFormatFromPrinter(format.PrinterPath));

                    fileWriteAttempted = true;
                    storedFormat = new PrintStationDatabase.StoredFormat
                    {
                        DriveLetter = format.DriveLetter,
                        Name = format.Name,
                        Extension = format.Extension,
                        Content = formatContent
                    };
                    await App.App.Database.SaveStoredFormatAsync(storedFormat);
                });
            }
            catch (Exception e)
            {
                await Task.Factory.StartNew(async () => {
                    if (fileWriteAttempted && storedFormat != null)
                    {
                        await App.App.Database.DeleteStoredFormatByIdAsync(storedFormat.Id);
                    }
                });

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
            }
            finally
            {
                if (linePrintEnabled)
                {
                    await Task.Factory.StartNew(() => {
                        try
                        {
                            connection?.Open();
                            SGD.SET(PrintFormatPage.DeviceLanguagesSgd, "line_print", connection);
                        }
                        catch (ConnectionException) { }
                    });
                }

                await Task.Factory.StartNew(() => {
                    try
                    {
                        connection?.Close();
                    }
                    catch (ConnectionException) { }
                });
            }
        }

        private async Task AddPrinterFormatsToFormatListAsync()
        {
            Connection connection = null;

            try
            {
                List<FormatViewModel> formats = new List<FormatViewModel>();

                await Task.Factory.StartNew(() => {
                    connection = ConnectionCreator.Create(ViewModel.SelectedPrinter);
                    connection.Open();

                    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
                    ZebraPrinterLinkOs linkOsPrinter = ZebraPrinterFactory.CreateLinkOsPrinter(printer);

                    string[] formatPrinterPaths = printer.RetrieveFileNames(new string[] { "ZPL" });
                    foreach (string formatPrinterPath in formatPrinterPaths)
                    {
                        int colonIndex = formatPrinterPath.IndexOf(":");
                        string driveLetter = formatPrinterPath.Substring(0, colonIndex);
                        string name = formatPrinterPath.Substring(colonIndex + 1, formatPrinterPath.LastIndexOf(".") - colonIndex - 1);
                        string extension = formatPrinterPath.Substring(formatPrinterPath.LastIndexOf(".") + 1);

                        if (!string.Equals(driveLetter, "Z", StringComparison.OrdinalIgnoreCase))
                        { // Ignore all formats stored on printer's Z drive
                            formats.Add(new FormatViewModel
                            {
                                DriveLetter = driveLetter,
                                Name = name,
                                Extension = extension,
                                Content = null, // Populate this later, upon navigating to print format page
                                Source = FormatSource.Printer,
                                OnSaveButtonClicked = new Command(async (object parameter) => {
                                    if (!ViewModel.IsSavingFormat && !ViewModel.IsDeletingFormat && !ViewModel.IsStoredFormatListRefreshing)
                                    {
                                        FormatViewModel format = parameter as FormatViewModel;

                                        await Task.Factory.StartNew(() => {
                                            ViewModel.IsSavingFormat = true;
                                            format.IsSaving = true;
                                        });

                                        await SavePrinterFormatAsync(format);

                                        await Task.Factory.StartNew(() => {
                                            format.IsSaving = false;
                                            ViewModel.IsSavingFormat = false;
                                        });

                                        await RefreshStoredFormatListAsync();
                                    }
                                }),
                                OnPrintButtonClicked = new Command(async (object parameter) => {
                                    if (!ViewModel.IsSavingFormat)
                                    {
                                        //FormatViewModel format = parameter as FormatViewModel;
                                        //await PushPageAsync(new PrintFormatPage(ViewModel.SelectedPrinter, format));
                                    }
                                })
                            });
                        }
                    }

                    formats.ForEach(ViewModel.PrinterFormatList.Add);
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
        }

        private async Task PushPageAsync(Page page)
        {
            if (Navigation.NavigationStack.Count == 0 || 
                    Navigation.NavigationStack.Last().GetType() == typeof(PrintStationPage))
            {
                await Navigation.PushAsync(page);
            }
        }

        private async Task PushModalAsync(Page page)
        {
            if (Navigation.ModalStack.Count == 0)
            {
                await Navigation.PushModalAsync(page);
            }
        }

        private async void StoredFormatsRefreshIcon_Tapped(object sender, EventArgs e)
        {
            if (!ViewModel.IsStoredFormatListRefreshing)
            {
                await RefreshStoredFormatListAsync();
            }
        }

        private async void PrinterFormatsRefreshIcon_Tapped(object sender, EventArgs e)
        {
            if (!ViewModel.IsPrinterFormatListRefreshing)
            {
                await RefreshPrinterFormatListAsync();
            }
        }

        private async void SelectPrinter_Clicked(object sender, EventArgs e)
        {
            await PushPageAsync(new SelectPrinterPage(this));
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            var cancelPopupView = new ErrorPopupView
            {
                BindingContext = new PopupViewModel()
                {
                    Text = "¡Se ha cancelado el proceso!",
                    Image = "icnBasuraRed.png"
                }
            };

            await Navigation.PushPopupAsync(cancelPopupView);

            await Task.Delay(1000);            
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.RemovePopupPageAsync(cancelPopupView);
            await Navigation.PopToRootAsync(true);
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }

        private async void PrintButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!ViewModel.IsSavingFormat)
                {
                    await Loading("Generando formatos...");
                    FormatViewModel format = await GetFormat(formatType);                   
                    CloseAllPopup();                   
                    await PushPageAsync(new PrintFormatPage(ViewModel.SelectedPrinter, format, sanctions));
                }
            }
            catch(Exception ex)
            {
                await DisplayError(ex.Message);
            }
        }

        private async Task<FormatViewModel> GetFormat(FormatType formatType)
        {
            var format = new FormatViewModel();
            format.DriveLetter = "E";
            format.Extension = "ZPL";
            format.Source = FormatSource.Printer;
            format.Type = formatType;

            if (formatType == FormatType.Sanction)
            {
                format.Name = "SANCTIONRECEIPT";
                format.Content = GetZplFormat.SanctionReceipt();
            }
            else if (formatType == FormatType.Payment)
            {
                format.Name = "PAYMENTRECEIPT";
                format.Content = GetZplFormat.PaymentReceipt();
            }

            await SavePrinterFormatAsync(format);

            return format;
        }

        private async Task DisplayError(string message)
        {
            CloseAllPopup();

            var errorPopupView = new ErrorPopupView()
            {
                BindingContext = new PopupViewModel()
                {
                    Text = message,
                    Image = "Error.png"
                }
            };

            await Navigation.PushPopupAsync(errorPopupView);
        }

        private async Task Loading(string message)
        {
            var loading = new LoadingPopupView
            {
                BindingContext = new PopupViewModel()
                {
                    Text = message,
                    TextStyle = "PopupContentLabelStyle"
                }
            };

            await Navigation.PushPopupAsync(loading, true);
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }
    }
}