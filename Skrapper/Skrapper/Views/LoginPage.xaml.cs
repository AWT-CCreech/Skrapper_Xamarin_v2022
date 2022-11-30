using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skrapper.Views
{
    public partial class LoginPage : ContentPage
    {
        LoginViewModel _loginViewModel;
        public LoginPage()
        {
            InitializeComponent(); 
            BindingContext= _loginViewModel = new LoginViewModel();
        }
    }
}