using Infracciones.App.Models;
using Infracciones.Infrastructure.ZebraPrintStation.Exceptions;
using Infracciones.Infrastructure.ZebraPrintStation.Models;
using Infracciones.Infrastructure.ZebraPrintStation.Services;
using Infracciones.Infrastructure.ZebraPrintStation.ViewModels;
using Infracciones.ViewModels.Payment;
using Infracciones.ViewModels.Sanction;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.Payment;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrintFormatPage : ContentPage
    {
        public const string DeviceLanguagesSgd = "device.languages";

        private readonly DiscoveredPrinter selectedPrinter;
        private readonly FormatViewModel format;
        private readonly PrintFormatPageViewModel viewModel = new PrintFormatPageViewModel();

        public PrintFormatPage(DiscoveredPrinter selectedPrinter, FormatViewModel format, ListSanctionViewModel sanctions)
        {
            InitializeComponent();

            this.selectedPrinter = selectedPrinter;
            this.format = format;
            BindingContext = viewModel;

            Device.BeginInvokeOnMainThread(async () => {
                await Print(sanctions);
            });
        }

        private async Task Print(ListSanctionViewModel sanctions)
        {
            var sanctionsPrinted = new List<SanctionModel>();

            foreach (var sanction in sanctions.Sanctions)
            {
                await PrintReceipt(sanction);
                sanctionsPrinted.Add(sanction);
            }

            if (format.Type == FormatType.Sanction)
            {
                var printedSanctions = new PrintedSanctionsPage();
                printedSanctions.BindingContext = new PrintedSanctionsViewModel(sanctionsPrinted);
                await Navigation.PushAsync(printedSanctions, true);
            }
            else if (format.Type == FormatType.Payment)
            {
                var printedPayments = new PrintedSanctionsPayedPage();
                printedPayments.BindingContext = new PrintedSanctionsPayedViewModel(sanctionsPrinted);
                await Navigation.PushAsync(printedPayments, true);
            }
        }

        private async Task PopulateVariableFieldListAsync()
        {
            await Task.Factory.StartNew(() => {
                viewModel.IsVariableFieldListRefreshing = true;

                try
                {
                    viewModel.FormatVariableList.Clear();
                }
                catch (NotImplementedException)
                {
                    viewModel.FormatVariableList.Clear(); // Workaround for Xamarin.Forms.Platform.WPF issue: https://github.com/xamarin/Xamarin.Forms/issues/3648
                }
            });

            Connection connection = null;
            bool linePrintEnabled = false;

            try
            {
                await Task.Factory.StartNew(() => {
                    connection = ConnectionCreator.Create(selectedPrinter);
                    connection.Open();

                    string originalPrinterLanguage = SGD.GET(DeviceLanguagesSgd, connection);
                    linePrintEnabled = "line_print".Equals(originalPrinterLanguage, StringComparison.OrdinalIgnoreCase);

                    if (linePrintEnabled)
                    {
                        SGD.SET(DeviceLanguagesSgd, "zpl", connection);
                    }

                    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
                    ZebraPrinterLinkOs linkOsPrinter = ZebraPrinterFactory.CreateLinkOsPrinter(printer);

                    if (format.Source == FormatSource.Printer)
                    {
                        format.Content = Encoding.UTF8.GetString(printer.RetrieveFormatFromPrinter(format.PrinterPath));
                    }

                    FieldDescriptionData[] variableFields = printer.GetVariableFields(format.Content);
                    foreach (FieldDescriptionData variableField in variableFields)
                    {
                        viewModel.FormatVariableList.Add(new FormatVariable
                        {
                            Name = variableField.FieldName ?? $"Field {variableField.FieldNumber}",
                            Index = variableField.FieldNumber,
                        });
                    }
                });
            }
            catch (Exception e)
            {
                await DisplayError(e.Message);
            }
            finally
            {
                if (linePrintEnabled)
                {
                    await ResetPrinterLanguageToLinePrintAsync(connection);
                }

                await Task.Factory.StartNew(() => {
                    try
                    {
                        connection?.Close();
                    }
                    catch (ConnectionException) { }

                    viewModel.IsVariableFieldListRefreshing = false;
                });
            }
        }

        private string GetPrinterStatusErrorMessage(PrinterStatus printerStatus)
        {
            if (printerStatus.isReadyToPrint)
            {
                if (printerStatus.isHeadCold)
                {
                    return "Cabezal de impresora muy frio.";
                }
                else if (printerStatus.isPartialFormatInProgress)
                {
                    return "Formato parcial en progreso.";
                }
            }
            else
            {
                if (printerStatus.isHeadTooHot)
                {
                    return "Cabezal de impresora muy caliente";
                }
                else if (printerStatus.isHeadOpen)
                {
                    return "Cubierta abierta.";
                }
                else if (printerStatus.isPaperOut)
                {
                    return "Papel agotado.";
                }
                else if (printerStatus.isReceiveBufferFull)
                {
                    return "Búfer de recepción lleno.";
                }
                else if (printerStatus.isRibbonOut)
                {
                    return "Error de cinta";
                }
                else if (printerStatus.isPaused)
                {
                    return "Impresora en pausa";
                }
                else
                {
                    return "Error desconocido";
                }
            }
            return null;
        }

        private Dictionary<int, string> BuildFormatVariableDictionary()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (FormatVariable formatVariable in viewModel.FormatVariableList)
            {
                dictionary.Add(formatVariable.Index, formatVariable.Value);
            }
            return dictionary;
        }

        private Dictionary<int, string> BuildFormatVariableDictionary(SanctionModel sanction)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>
            {
                //TODO: Add variables to generate the QR code correctly
                { 1, sanction.SanctionReason.Description },
                { 2, sanction.VehicleType.Description },
                { 3, sanction.Plates },
                { 4, sanction.Brand.Description },
                { 5, sanction.SubBrand.Description },
                { 6, sanction.Model.ToString() },
                { 7, sanction.State.Name },
                { 8, sanction.Key },
                { 9, string.Format("{0:C2}", sanction.Amount) },
                { 10, sanction.Payment.PaymentReference },                
                // qr{ 11, string.Concat("MULTAS DE TRANSITO. NUMERO DE INFRACCION: ", sanction.Key, " MULTAS: ", sanction.Amount, " DESCUENTOS: 0.00 RECARGOS: 0.00 TOTAL: ",
                //                    sanction.Amount, " VIGENCIA: 31 DE DICIEMBRE DEL 2020 LINEA DE CAPTURA :", sanction.Payment.PaymentReference) },
            };
            return dictionary;
        }

        private async Task ResetPrinterLanguageToLinePrintAsync(Connection connection)
        {
            await Task.Factory.StartNew(() => {
                try
                {
                    connection?.Open();
                    SGD.SET(DeviceLanguagesSgd, "line_print", connection);
                }
                catch (ConnectionException) { }
            });
        }

        private async void PrintButton_Clicked(object sender, EventArgs eventArgs)
        {
            await Task.Factory.StartNew(() => {
                viewModel.IsSendingPrintJob = true;
            });

            Connection connection = null;
            bool linePrintEnabled = false;

            try
            {
                await Task.Factory.StartNew(() => {
                    connection = ConnectionCreator.Create(selectedPrinter);
                    connection.Open();

                    string originalPrinterLanguage = SGD.GET(DeviceLanguagesSgd, connection);
                    linePrintEnabled = "line_print".Equals(originalPrinterLanguage, StringComparison.OrdinalIgnoreCase);

                    if (linePrintEnabled)
                    {
                        SGD.SET(DeviceLanguagesSgd, "zpl", connection);
                    }

                    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
                    ZebraPrinterLinkOs linkOsPrinter = ZebraPrinterFactory.CreateLinkOsPrinter(printer);

                    string errorMessage = GetPrinterStatusErrorMessage(printer.GetCurrentStatus());
                    if (errorMessage != null)
                    {
                        throw new PrinterNotReadyException($"{errorMessage}. Please check your printer and try again.");
                    }
                    else
                    {
                        if (format.Source != FormatSource.Printer)
                        {
                            connection.Write(Encoding.UTF8.GetBytes(format.Content));
                        }

                        linkOsPrinter.PrintStoredFormatWithVarGraphics(format.PrinterPath, BuildFormatVariableDictionary(), "UTF-8");
                    }
                });

                if (linePrintEnabled)
                {
                    await ResetPrinterLanguageToLinePrintAsync(connection);
                }

                await Task.Factory.StartNew(() => {
                    viewModel.IsSendingPrintJob = false;
                });
            }
            catch (PrinterNotReadyException e)
            {
                if (linePrintEnabled)
                {
                    await ResetPrinterLanguageToLinePrintAsync(connection);
                }

                await Task.Factory.StartNew(() => {
                    viewModel.IsSendingPrintJob = false;
                });

                await DisplayError(e.Message);
            }
            catch (Exception e)
            {
                if (linePrintEnabled)
                {
                    await ResetPrinterLanguageToLinePrintAsync(connection);
                }

                await Task.Factory.StartNew(() => {
                    viewModel.IsSendingPrintJob = false;
                });

                await DisplayError(e.Message);
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

        private async Task PrintReceipt(SanctionModel sanction)
        {
            await Task.Factory.StartNew(() => {
                viewModel.IsSendingPrintJob = true;
            });

            Connection connection = null;
            bool linePrintEnabled = false;

            try
            {
                await Task.Factory.StartNew(() => {
                    connection = ConnectionCreator.Create(selectedPrinter);
                    connection.Open();

                    string originalPrinterLanguage = SGD.GET(DeviceLanguagesSgd, connection);
                    linePrintEnabled = "line_print".Equals(originalPrinterLanguage, StringComparison.OrdinalIgnoreCase);

                    if (linePrintEnabled)
                    {
                        SGD.SET(DeviceLanguagesSgd, "zpl", connection);
                    }

                    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
                    ZebraPrinterLinkOs linkOsPrinter = ZebraPrinterFactory.CreateLinkOsPrinter(printer);

                    string errorMessage = GetPrinterStatusErrorMessage(printer.GetCurrentStatus());
                    if (errorMessage != null)
                    {
                        throw new PrinterNotReadyException($"{errorMessage}. Por favor, revise la impresora y vuelva a intentarlo.");
                    }
                    else
                    {
                        if (format.Source != FormatSource.Printer)
                        {
                            connection.Write(Encoding.UTF8.GetBytes(format.Content));
                        }

                        linkOsPrinter.PrintStoredFormatWithVarGraphics(format.PrinterPath, BuildFormatVariableDictionary(sanction), "UTF-8");                       
                    }
                });

                if (linePrintEnabled)
                {
                    await ResetPrinterLanguageToLinePrintAsync(connection);
                }

                await Task.Factory.StartNew(() => {
                    viewModel.IsSendingPrintJob = false;
                });
            }
            catch (PrinterNotReadyException e)
            {
                if (linePrintEnabled)
                {
                    await ResetPrinterLanguageToLinePrintAsync(connection);
                }

                await Task.Factory.StartNew(() => {
                    viewModel.IsSendingPrintJob = false;
                });

                await DisplayError(e.Message);
            }
            catch (Exception e)
            {
                if (linePrintEnabled)
                {
                    await ResetPrinterLanguageToLinePrintAsync(connection);
                }

                await Task.Factory.StartNew(() => {
                    viewModel.IsSendingPrintJob = false;
                });

                await DisplayError(e.Message);
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

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }

        private void PanicButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}