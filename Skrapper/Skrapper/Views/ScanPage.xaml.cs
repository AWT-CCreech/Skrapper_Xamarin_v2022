using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skrapper.Views
{
    public partial class ScanPage : ContentPage
    {
        ScanViewModel _scanViewModel;
        public ScanPage()
        {
            InitializeComponent();
            BindingContext = _scanViewModel = new ScanViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _scanViewModel.OnAppearing();
        }

        private void CbAuto_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Dispatcher.BeginInvokeOnMainThread(delegate
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsChecked == true)
                    EntSerialNo.Focus();
            });
        }

        private void EntSerialNo_Focused(object sender, FocusEventArgs e)
        {
            Dispatcher.BeginInvokeOnMainThread(delegate
            {
                var entry = (Entry)sender;
                EntSerialNo.CursorPosition = 0;
                EntSerialNo.SelectionLength = EntSerialNo.Text != null ? EntSerialNo.Text.Length : 0;
            });
        }
    }
}