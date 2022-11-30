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
    }
}