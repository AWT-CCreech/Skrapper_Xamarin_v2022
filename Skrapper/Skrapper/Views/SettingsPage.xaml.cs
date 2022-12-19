using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skrapper.Views
{
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel _settingsViewModel;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = _settingsViewModel = new SettingsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _settingsViewModel.OnAppearing();
        }

        private void SwitchCell_Tapped(object sender, System.EventArgs e)
        {
            _settingsViewModel.CheckServerCommand.Execute(true);
        }
    }
}