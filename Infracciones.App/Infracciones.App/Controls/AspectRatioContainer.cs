using Infracciones.App.Services.DependencyServices;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Infracciones.App.Controls
{
    public class AspectRatioContainer : ContentView
    {
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, widthConstraint * this.AspectRatio));
        }

        public static BindableProperty AspectRatioProperty = BindableProperty.Create(nameof(AspectRatioContainer), typeof(double), typeof(AspectRatioContainer), (double)1);

        public double AspectRatio
        {
            get { return (double)this.GetValue(AspectRatioProperty); }
            set { this.SetValue(AspectRatioProperty, value); }
        }

        public static BindableProperty ImageToSizeProperty = BindableProperty.Create(nameof(ImageToSizeProperty), typeof(string), typeof(AspectRatioContainer), null, propertyChanged: OnImageToSyzeChanged);

        private static async void OnImageToSyzeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var size = await DependencyService.Get<IImageSizer>().GetSizeForImage((string)newValue);
            (bindable as AspectRatioContainer).AspectRatio = size.Height / size.Width;
        }

        public string ImageToSize
        {
            get { return (string)this.GetValue(ImageToSizeProperty); }
            set { this.SetValue(ImageToSizeProperty, value); }
        }
    }
}
