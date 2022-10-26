using Skrapper.Models;
using Skrapper.Services;
using Skrapper.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Skrapper
{
    public class MainViewModel : ViewModelBase
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        public MainViewModel()
        {
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set 
            {
                Console.WriteLine("[MainViewModel.cs] (IsBusy) >> " + value);
                SetProperty(ref isBusy, value); 
            }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

    }
}
