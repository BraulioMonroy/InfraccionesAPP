using Infracciones.App.ViewModels;
using Infracciones.Infrastructure.ZebraPrintStation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Infracciones.Infrastructure.ZebraPrintStation.ViewModels
{
    public class FormatViewModel : BaseViewModel
    {
        private int databaseId;
        private string driveLetter;
        private string name;
        private string extension;
        private string content;
        private FormatSource source;
        private FormatType type;
        private bool isDeleting = false;
        private bool isSaving = false;

        public int DatabaseId
        {
            get => databaseId;
            set
            {
                databaseId = value;
                NotifyPropertyChanged();
            }
        }

        public string DriveLetter
        {
            get => driveLetter;
            set
            {
                driveLetter = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        public string Extension
        {
            get => extension;
            set
            {
                extension = value;
                NotifyPropertyChanged();
            }
        }

        public string Content
        {
            get => content;
            set
            {
                content = value;
                NotifyPropertyChanged();
            }
        }

        public FormatSource Source
        {
            get => source;
            set
            {
                source = value;
                NotifyPropertyChanged();
            }
        }

        public FormatType Type
        {
            get => type;
            set
            {
                type = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsDeleting
        {
            get => isDeleting;
            set
            {
                isDeleting = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsSaving
        {
            get => isSaving;
            set
            {
                isSaving = value;
                NotifyPropertyChanged();
            }
        }

        public Command OnDeleteButtonClicked { get; set; }

        public Command OnSaveButtonClicked { get; set; }

        public Command OnPrintButtonClicked { get; set; }

        public string PrinterPath
        {
            get => $"{DriveLetter}:{Name}.{Extension}";
        }

        public string StoredPath
        {
            get => $"{Name}.{Extension}";
        }
    }
}
