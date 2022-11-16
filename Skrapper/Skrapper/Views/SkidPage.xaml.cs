using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skrapper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkidPage : ContentPage
    {
        SkidViewModel _viewModel;
        public SkidPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SkidViewModel();
        }
    }
}