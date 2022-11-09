using Android.Content.Res;
using Skrapper_Xamarin_v2022.Droid;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("PlainEntryGroup")]
[assembly: ExportEffect(typeof(PlainEntryEffect), "PlainEntryEffect")]
namespace Skrapper_Xamarin_v2022.Droid
{
    public class PlainEntryEffect : PlatformEffect
    {
        public PlainEntryEffect()
        {
        }
        protected override void OnAttached()
        {
            try
            {
                if (Control != null)
                {
                    Android.Graphics.Color entryLineColor = Android.Graphics.Color.Transparent;
                    Control.BackgroundTintList = ColorStateList.ValueOf(entryLineColor);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error... Unable to set property on attached control", ex.Message);
            }
        }
        protected override void OnDetached()
        {
        }
        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
        }
    }
}