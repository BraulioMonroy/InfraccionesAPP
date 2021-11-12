using Infracciones.App.Controls;
using Xamarin.Forms;

namespace Infracciones.Behaviors
{
    public class EmptyCustomErrorEntryBehavior : Behavior<CustomErrorEntry>
    {
        CustomErrorEntry control;
        string _placeHolder;
        Color _placeHolderColor;

        protected override void OnAttachedTo(CustomErrorEntry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            bindable.PropertyChanged += OnPropertyChanged;
            control = bindable;
            _placeHolder = bindable.Placeholder;
            _placeHolderColor = bindable.PlaceholderColor;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                ((CustomErrorEntry)sender).IsBorderErrorVisible = false;
            }
        }

        protected override void OnDetachingFrom(CustomErrorEntry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            bindable.PropertyChanged -= OnPropertyChanged;
        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CustomErrorEntry.IsBorderErrorVisibleProperty.PropertyName && control != null)
            {
                if (control.IsBorderErrorVisible)
                {
                    control.Placeholder = control.ErrorText;
                    control.PlaceholderColor = control.BorderErrorColor;
                    control.Text = string.Empty;
                }

                else
                {
                    control.Placeholder = _placeHolder;
                    control.PlaceholderColor = _placeHolderColor;
                }
            }
        }
    }
}
