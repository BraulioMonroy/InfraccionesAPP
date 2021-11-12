using Infracciones.Models;
using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace Infracciones.App.Models
{
    /// <summary>
    /// Model for Type Sanctions
    /// </summary>
    [Preserve(AllMembers = true)]
    public class TypeSanctionModel : INotifyPropertyChanged
    {
        #region Fields

        private string sanctionName;

        private string description;

        private string image;

        private string disabledImage;

        private SanctionReason sanctionReason;

        private string sanctionnKey;

        #endregion

        #region EventHandler

        /// <summary>
        /// EventHandler of property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the key of sanction
        /// </summary>
        /// <value>The key of the sanction.</value>
        public string SanctionKey
        {
            get
            {
                return this.sanctionnKey;
            }

            set
            {
                this.sanctionnKey = value;
                this.OnPropertyChanged(nameof(sanctionnKey));
            }
        }

        /// <summary>
        /// Gets or sets the name of sanction
        /// </summary>
        /// <value>The name.</value>
        public string SanctionName
        {
            get
            {
                return this.sanctionName;
            }

            set
            {
                this.sanctionName = value;
                this.OnPropertyChanged(nameof(sanctionName));
            }
        }

        /// <summary>
        /// Gets or sets the description of a sanction.
        /// </summary>
        /// <value>The designation.</value>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
                this.OnPropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// Gets or sets the image of a sanction.
        /// </summary>
        /// <value>The image.</value>
        public string Image
        {
            get
            {
                return this.image;
            }

            set
            {
                this.image = value;
                this.OnPropertyChanged(nameof(Image));
            }
        }

        /// <summary>
        /// Gets or sets the image of a sanction.
        /// </summary>
        /// <value>The image.</value>
        public string DisabledImage
        {
            get
            {
                return this.disabledImage;
            }

            set
            {
                this.disabledImage = value;
                this.OnPropertyChanged(nameof(disabledImage));
            }
        }

        /// <summary>
        /// Gets or sets the image of a sanction.
        /// </summary>
        /// <value>The image.</value>
        public SanctionReason SanctionReason
        {
            get
            {
                return this.sanctionReason;
            }

            set
            {
                this.sanctionReason = value;
                this.OnPropertyChanged(nameof(sanctionReason));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The PropertyChanged event occurs when changing the value of property.
        /// </summary>
        /// <param name="propertyName">The PropertyName</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
