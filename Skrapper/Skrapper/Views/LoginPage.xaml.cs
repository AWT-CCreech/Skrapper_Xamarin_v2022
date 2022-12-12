using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skrapper.Views
{
    public partial class LoginPage : ContentPage
    {
        MainViewModel _mainViewModel;
        public LoginPage()
        {
            InitializeComponent(); 
            BindingContext= _mainViewModel = new MainViewModel();
        }
    }
}