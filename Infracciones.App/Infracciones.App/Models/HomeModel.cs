using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace Infracciones.App.Models
{
    /// <summary>
    /// Model for Sections in Home
    /// </summary>
    [Preserve(AllMembers = true)]
    public class HomeModel : INotifyPropertyChanged
    {
        #region Fields

        private string sectionName;

        private string description;

        private string image;

        private string sectionKey;

        #endregion

        #region EventHandler

        /// <summary>
        /// EventHandler of property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the key of section
        /// </summary>
        /// <value>The name.</value>
        public string Sectionkey
        {
            get
            {
                return this.sectionKey;
            }

            set
            {
                this.sectionKey = value;
                this.OnPropertyChanged(nameof(sectionKey));
            }
        }

        /// <summary>
        /// Gets or sets the name of section
        /// </summary>
        /// <value>The name.</value>
        public string SectionName
        {
            get
            {
                return this.sectionName;
            }

            set
            {
                this.sectionName = value;
                this.OnPropertyChanged(nameof(SectionName));
            }
        }

        /// <summary>
        /// Gets or sets the description of a section.
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
        /// Gets or sets the image of a section.
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
