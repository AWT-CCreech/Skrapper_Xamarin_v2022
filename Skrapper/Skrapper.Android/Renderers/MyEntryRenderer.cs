using Android.Content;
using Android.Views.InputMethods;
using Skrapper;
using Skrapper.Droid;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(KeyboardlessEntry), typeof(KeyboardlessEntryRenderer))]
namespace Skrapper.Droid
{
    class KeyboardlessEntryRenderer : EntryRenderer
    {
        public KeyboardlessEntryRenderer(Context context) : base(context)
        {
        }
        protected override void OnFocusChangeRequested(object sender, VisualElement.FocusRequestArgs e)
        {
            e.Result = true;

            if (e.Focus)
                this.Control.RequestFocus();
            else
                this.Control.ClearFocus();

            // Disable the Keyboard on Focus
            //TODO: Enable
            //Control.ShowSoftInputOnFocus = Globals.bOverrideKeyboardlessEntry;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                ((KeyboardlessEntry)e.NewElement).PropertyChanging += OnPropertyChanging;
            }

            if (e.OldElement != null)
            {
                ((KeyboardlessEntry)e.OldElement).PropertyChanging -= OnPropertyChanging;
            }
            // Disable the Keyboard on Focus
            Control.ShowSoftInputOnFocus = Globals.bOverrideKeyboardlessEntry;
        }

        private void OnPropertyChanging(object sender, PropertyChangingEventArgs propertyChangingEventArgs)
        {
            // Check if the view is about to get Focus
            if (propertyChangingEventArgs.PropertyName == VisualElement.IsFocusedProperty.PropertyName)
            {
                try
                {
                    // Disable the Keyboard on Focus
                    Control.ShowSoftInputOnFocus = Globals.bOverrideKeyboardlessEntry;

                    // incase if the focus was moved from another Entry
                    // Forcefully dismiss the Keyboard 
                    InputMethodManager imm = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(Control.WindowToken, 0);
                }
                catch (Exception ex)
                {
                    _ = ex;
                }
            }
        }
    }
}