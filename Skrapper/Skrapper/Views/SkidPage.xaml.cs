using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skrapper.Views
{
    public partial class SkidPage : ContentPage
    {
        SkidViewModel _skidViewModel;
        public SkidPage()
        {
            InitializeComponent();
            BindingContext = _skidViewModel = new SkidViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _skidViewModel.OnAppearing();
        }
    }
}