using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skrapper.Views
{
    public partial class HistoryPage : ContentPage
    {
        HistoryViewModel _historyViewModel;
        public HistoryPage()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();
            BindingContext = _historyViewModel = new HistoryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _historyViewModel.OnAppearing();
        }
    }
}