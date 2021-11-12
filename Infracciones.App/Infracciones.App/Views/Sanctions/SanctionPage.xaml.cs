using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.Services.DTO;
using Infracciones.App.Services.Media;
using Infracciones.App.Views.Home;
using Infracciones.App.Views.Payment;
using Infracciones.Models;
using Infracciones.Services.Sqlite;
using Infracciones.Services.Sqlite.MediaService;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Sanctions.Media;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Sanctions
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SanctionPage : ContentPage
    {
        private static readonly SqlMediaService _sqlMediaService = new SqlMediaService();
        private static readonly MediaFileService _mediaFileService = new MediaFileService();
        private List<SanctionReasonModel> sanctionReasonList;

        public SanctionPage()
        {
            InitializeComponent();

            Task.Run(async () => await ClearData());
            ClearMedia();

            sanctionReasonList = new List<SanctionReasonModel>();
            GetSanctionReasonCollection();
            DisabledAllAddButtons();
        }

        private async Task ClearData()
        {
            // Remove previous media from database
            await _sqlMediaService.RemoveAll();
        }

        private void ClearMedia()
        {
            // Remove previous media from app folder
            var appFolder = DependencyService.Get<IPathService>().GetPrivateExternalFolder();
            var photosFolder = Path.Combine(appFolder, "Pictures");
            var videosFolder = Path.Combine(appFolder, "Movies");
            if (Directory.Exists(photosFolder))
            {
                Directory.Delete(photosFolder, true);
            }
            if (Directory.Exists(videosFolder))
            {
                Directory.Delete(videosFolder, true);
            }
        }

        private async void GetSanctionReasonCollection()
        {
            sanctionReasonList = await SanctionReasonService.GetAll();
        }

        private void SwitchGases_Toggled(object sender, ToggledEventArgs e)
        {
            ButtonAddEmisiones.IsEnabled = SwitchGases.IsToggled;
            ButtonEditEmisiones.IsEnabled = SwitchGases.IsToggled;

            if (SwitchGases.IsToggled)
            {
                ImageEmisiones.Source = "icnEmisionDeGasesGreen.png";
                LabelEmisiones.Style = (Style)Application.Current.Resources["EnabledLabelStyle"];
                ButtonAddEmisiones.Source = "icnAgregarFotoGreen.png";
                ButtonEditEmisiones.Source = "icnLapizGreen.png";
            }
            else
            {
                ImageEmisiones.Source = "icnEmisionDeGasesGrey.png";
                LabelEmisiones.Style = (Style)Application.Current.Resources["DisabledLabelStyle"];
                ButtonAddEmisiones.Source = "icnAgregarFotoGrey.png";
                ButtonEditEmisiones.Source = "icnLapizGrey.png";
            }
        }

        private void SwitchDocuments_Toggled(object sender, ToggledEventArgs e)
        {
            ButtonAddDocuments.IsEnabled = SwitchDocuments.IsToggled;
            ButtonEditDocumentos.IsEnabled = SwitchDocuments.IsToggled;

            if (SwitchDocuments.IsToggled)
            {
                ImageDocuments.Source = "icnFaltaDeDocumentosGreen.png";
                LabelDocuments.Style = (Style)Application.Current.Resources["EnabledLabelStyle"];
                ButtonAddDocuments.Source = "icnAgregarFotoGreen.png";
                ButtonEditDocumentos.Source = "icnLapizGreen.png";
            }
            else
            {
                ImageDocuments.Source = "icnFaltaDeDocumentosGrey.png";
                LabelDocuments.Style = (Style)Application.Current.Resources["DisabledLabelStyle"];
                ButtonAddDocuments.Source = "icnAgregarFotoGrey.png";
                ButtonEditDocumentos.Source = "icnLapizGrey.png";
            }
        }

        private void SwitchNoCircula_Toggled(object sender, ToggledEventArgs e)
        {
            ButtonAddNoCircula.IsEnabled = SwitchNoCircula.IsToggled;
            ButtonEditNoCircula.IsEnabled = SwitchNoCircula.IsToggled;

            if (SwitchNoCircula.IsToggled)
            {
                ImageNoCircula.Source = "icnCircularEnHorarioGreen.png";
                LabelNoCircula.Style = (Style)Application.Current.Resources["EnabledLabelStyle"];
                ButtonAddNoCircula.Source = "icnAgregarFotoGreen.png";
                ButtonEditNoCircula.Source = "icnLapizGreen.png";
            }
            else
            {
                ImageNoCircula.Source = "icnCircularenHorarioGrey.png";
                LabelNoCircula.Style = (Style)Application.Current.Resources["DisabledLabelStyle"];
                ButtonAddNoCircula.Source = "icnAgregarFotoGrey.png";
                ButtonEditNoCircula.Source = "icnLapizGrey.png";
            }
        }

        private async void ButtonAddEmisiones_Clicked(object sender, EventArgs e)
        {
            try
            {
                var popupView = new SelectMediaTypePopupView();
                popupView.BindingContext = GetTypeSanctionSelected(sender);
                await Navigation.PushPopupAsync(popupView, true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", string.Concat("No es posible agregar elementos multimedia. ", ex.Message), "Continuar");
            }
        }

        private async void ButtonEditEmisiones_Clicked(object sender, EventArgs e)
        {
            var typeSanction = GetTypeSanctionSelected(sender);
            var page = new CarouselGalleryPage(typeSanction);
            await Navigation.PushAsync(page, true);
        }

        private async void ButtonAddDocuments_Clicked(object sender, EventArgs e)
        {
            try
            {
                var popupView = new SelectMediaTypePopupView();
                popupView.BindingContext = GetTypeSanctionSelected(sender);
                await Navigation.PushPopupAsync(popupView, true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", string.Concat("No es posible agregar elementos multimedia. ", ex.Message), "Continuar");
            }
        }

        private async void ButtonEditDocumentos_Clicked(object sender, EventArgs e)
        {
            var typeSanction = GetTypeSanctionSelected(sender);
            var page = new CarouselGalleryPage(typeSanction);
            await Navigation.PushAsync(page, true);
        }

        private async void ButtonAddNoCircula_Clicked(object sender, EventArgs e)
        {
            try
            {
                var popupView = new SelectMediaTypePopupView();
                popupView.BindingContext = GetTypeSanctionSelected(sender);
                await Navigation.PushPopupAsync(popupView, true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", string.Concat("No es posible agregar elementos multimedia. ", ex.Message), "Continuar");
            }
        }

        private async void ButtonEditNoCircula_Clicked(object sender, EventArgs e)
        {
            var typeSanction = GetTypeSanctionSelected(sender);
            var page = new CarouselGalleryPage(typeSanction);
            await Navigation.PushAsync(page, true);
        }

        private async void BtnPay_Clicked(object sender, EventArgs e)
        {
            try
            {
                var loading = new LoadingPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = "Generando sanciones...",
                        TextStyle = "PopupContentLabelStyle"
                    }
                };

                await Navigation.PushPopupAsync(loading);

                if (string.IsNullOrEmpty(EntResponsible.Text))
                    throw new Exception("Debe de especificar al Responsable del vehículo.");

                var sanctions = new SanctionDTO
                {
                    Vehicle = (VehicleModel)this.BindingContext,
                    SanctionReasons = GetSanctions()
                };

                var result = await SanctionService.MakeSanctions(sanctions);

                if (result.Count == 0)
                    throw new Exception("No fue posible generar la(s) sanción(es). Por favor, vuelta a intentarlo. Si el problema persiste consulte al Administrador.");

                //Upload files
                foreach (var sanction in result)
                {
                    SanctionReason sanctionReason = SanctionReason.EmisionDeGases;
                    if (sanction.SanctionReasonId == (int)SanctionReason.EmisionDeGases)
                        sanctionReason = SanctionReason.EmisionDeGases;
                    else if (sanction.SanctionReasonId == (int)SanctionReason.FaltaDeDocumentos)
                        sanctionReason = SanctionReason.FaltaDeDocumentos;
                    else
                        sanctionReason = SanctionReason.HorarioNoPermitido;

                    var media = await _sqlMediaService.GetAllBySantionReason(sanctionReason);
                    if (media.Count > 0)
                    {
                        var imagePaths = media.Select(x => x.ImagePath).ToList();
                        await _mediaFileService.AddMedia(sanction.Id, imagePaths);
                    }
                }

                var paymentReference = new PaymentReferenceFoundPage(result);

                await Navigation.PopPopupAsync(true);
                await Navigation.PushAsync(paymentReference, true);
            }
            catch (Exception ex)
            {
                CloseAllPopup();

                var errorPopupView = new ErrorPopupView()
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = ex.Message,
                        Image = "Error.png"
                    }
                };

                await Navigation.PushPopupAsync(errorPopupView);
            }
        }

        private List<SanctionReasonModel> GetSanctions()
        {
            try
            {
                var list = new List<SanctionReasonModel>();
                var sanctionReason = new SanctionReasonModel();

                if (SwitchGases.IsToggled)
                {
                    sanctionReason = sanctionReasonList.Where(x => x.Id == (int)SanctionReason.EmisionDeGases).FirstOrDefault();

                    if (sanctionReason == null)
                        throw new Exception("No fue posible agregar la sanción para Emisión de Gases, por favor vuelva a intentarlo. Si el problema persiste comuniquese con el Administrador.");

                    list.Add(sanctionReason);
                }

                if (SwitchDocuments.IsToggled)
                {
                    sanctionReason = sanctionReasonList.Where(x => x.Id == (int)SanctionReason.FaltaDeDocumentos).FirstOrDefault();

                    if (sanctionReason == null)
                        throw new Exception("No fue posible agregar la sanción para Falta de documentos, por favor vuelva a intentarlo. Si el problema persiste comuniquese con el Administrador.");

                    list.Add(sanctionReason);
                }

                if (SwitchNoCircula.IsToggled)
                {
                    sanctionReason = sanctionReasonList.Where(x => x.Id == (int)SanctionReason.HorarioNoPermitido).FirstOrDefault();

                    if (sanctionReason == null)
                        throw new Exception("No fue posible agregar la sanción para Circular en horario no correspondiente, por favor vuelva a intentarlo. Si el problema persiste comuniquese con el Administrador.");

                    list.Add(sanctionReason);
                }

                if (list.Count == 0)
                    throw new Exception("Debe de seleccionar al menos un motivo de sanción.");

                if (!CheckMediaFiles())
                    throw new Exception("Debe de agregar evidencia multimedia para cada sanción.");

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool CheckMediaFiles()
        {
            //TODO: Check there are minium 1 photo and 1 video  per Sanction
            return true;
        }

        private async void ButtonCanel_Clicked(object sender, EventArgs e)
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
            await Navigation.PopAsync(true);
            await Navigation.RemovePopupPageAsync(cancelPopupView);
        }

        private void DisabledAllAddButtons()
        {
            ButtonAddEmisiones.IsEnabled = false;
            ButtonAddDocuments.IsEnabled = false;
            ButtonAddDocuments.IsEnabled = false;
            ButtonEditDocumentos.IsEnabled = false;
            ButtonEditEmisiones.IsEnabled = false;
            ButtonEditNoCircula.IsEnabled = false;
        }

        private TypeSanctionModel GetTypeSanctionSelected(object sender)
        {
            var typeSanction = new TypeSanctionModel();

            try
            {
                var buttonClicked = (sender as ImageButton);
                switch (buttonClicked.ClassId)
                {
                    case "Emisiones":
                        typeSanction.SanctionName = "Emisión de gases";
                        typeSanction.SanctionReason = SanctionReason.EmisionDeGases;
                        typeSanction.Image = "icnEmisionDeGasesGreen.png";
                        typeSanction.Description = "Emisión de humo negro, azul o vehículo ostensiblemente contaminante.";
                        typeSanction.SanctionKey = "Emisiones";
                        break;
                    case "Documentos":
                        typeSanction.SanctionName = "Falta de documentos";
                        typeSanction.SanctionReason = SanctionReason.FaltaDeDocumentos;
                        typeSanction.Image = "icnFaltaDeDocumentosGreen.png";
                        typeSanction.Description = "Holograma o Certificado de Verificación Vehicular Vigente, entre otros.";
                        typeSanction.SanctionKey = "Documentos";
                        break;
                    case "NoCircula":
                        typeSanction.SanctionName = "Circular en horario no correspondiente";
                        typeSanction.SanctionReason = SanctionReason.HorarioNoPermitido;
                        typeSanction.Image = "icnCircularEnHorarioGreen.png";
                        typeSanction.Description = "Circular en horario o día restringido";
                        typeSanction.SanctionKey = "NoCircula";
                        break;
                }

                return typeSanction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }
    }
}