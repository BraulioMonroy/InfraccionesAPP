using Xamarin.Forms;

namespace Infracciones.App.Controls
{
    public class CustomErrorEntry : Entry
    {
        public static readonly BindableProperty IsBorderErrorVisibleProperty =
            BindableProperty.Create(nameof(IsBorderErrorVisible), typeof(bool), typeof(CustomErrorEntry), false, BindingMode.TwoWay);

        public bool IsBorderErrorVisible
        {
            get => (bool)GetValue(IsBorderErrorVisibleProperty);
            set => SetValue(IsBorderErrorVisibleProperty, value);
        }

        public static readonly BindableProperty BorderErrorColorProperty =
            BindableProperty.Create(nameof(BorderErrorColor), typeof(Color), typeof(CustomErrorEntry), Color.Transparent, BindingMode.TwoWay);

        public static readonly BindableProperty BorderSuccessColorProperty =
            BindableProperty.Create(nameof(BorderSuccessColor), typeof(Color), typeof(CustomErrorEntry), Color.Transparent, BindingMode.TwoWay);

        public Color BorderErrorColor
        {
            get => (Color)GetValue(BorderErrorColorProperty);
            set => SetValue(BorderErrorColorProperty, value);
        }

        public Color BorderSuccessColor
        {
            get => (Color)GetValue(BorderSuccessColorProperty);
            set => SetValue(BorderSuccessColorProperty, value);
        }

        public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(CustomErrorEntry), string.Empty);

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }
    }
}
