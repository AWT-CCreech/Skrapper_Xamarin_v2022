using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skrapper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    [DesignTimeVisible(false)]
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }
    }
}