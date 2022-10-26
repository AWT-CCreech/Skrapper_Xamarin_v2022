using Skrapper.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Skrapper
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            Routing.RegisterRoute(nameof(SkidPage), typeof(SkidPage));
            Routing.RegisterRoute(nameof(ScanPage), typeof(ScanPage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }

    }
}
