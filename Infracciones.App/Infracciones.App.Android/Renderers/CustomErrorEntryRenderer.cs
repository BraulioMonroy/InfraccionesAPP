using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Infracciones.App.Controls;
using Infracciones.App.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomErrorEntry), typeof(CustomErrorEntryRenderer))]
namespace Infracciones.App.Droid.Renderers
{
    public class CustomErrorEntryRenderer : EntryRenderer
    {
        public CustomErrorEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;

            UpdateBorders();
            Control.SetPadding(Control.PaddingLeft, 0, Control.PaddingRight, 0);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null) return;

            if (e.PropertyName == CustomErrorEntry.IsBorderErrorVisibleProperty.PropertyName)
                UpdateBorders();
        }

        void UpdateBorders()
        {
            //GradientDrawable shape = new GradientDrawable();
            //shape.SetShape(ShapeType.Rectangle);
            //shape.SetCornerRadius(0);

            //if (((CustomErrorEntry)this.Element).IsBorderErrorVisible)
            //{
            //    shape.SetStroke(2, ((CustomErrorEntry)this.Element).BorderErrorColor.ToAndroid());
            //}
            //else
            //{
            //    shape.SetStroke(3, ((CustomErrorEntry)this.Element).BorderSuccessColor.ToAndroid());

            //    //shape.SetStroke(3, Android.Graphics.Color.LightGray);
            //    //this.Control.SetBackground(shape);
            //}

            //this.Control.SetBackground(shape);
            
            if (((CustomErrorEntry)this.Element).IsBorderErrorVisible)
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#CB2833"));
            }
            else
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#96DEB0"));
            }
        }
    }
}