using Skrapper.Models;
using Skrapper.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper
{
    public class HistoryViewModel : MainViewModel
    {
        public Command DeletePartScanCommand { private set; get; }
        public Command DisplayScanCommand { get; }
        public Command AddNoteCommand { get; }
        public Command DoRefreshHistoryCommand { get; }

        public HistoryViewModel()
        {
            Title = "HISTORY";

            AddNoteCommand = new Command(OnAddNoteClicked);
            DoRefreshHistoryCommand = new Command(DoRefreshHistory);
        }

        public void OnAppearing()
        {
            if(Globals.pSkidIdx >= 0)
            {
                DoRefreshHistoryCommand.Execute(true);
            }
        }

        private async void DoRefreshHistory(object obj)
        {
            IsRefreshing = true;
            try
            {
                Task<ObservableCollection<History>> t = Task.Run(async () => await PickerService.LoadGridFromSkidNum(Globals.pSkidItem, true, true));
                SkidHistory = t.Result;
                OnPropertyChanged("SkidHistory");
            }
            catch(Exception ex)
            {
                IsRefreshing= false;
                await _messageService.DisplayError("[ERROR: HistoryViewModel.cs]", ex.Message, "dismiss");
                Console.WriteLine("[ERROR: HistoryViewModel.cs] (DoRefreshHistory) >> " + ex.ToString());
            }
            finally
            {
                IsRefreshing = false;
            }
        }


        private async void OnAddNoteClicked(object obj)
        {
            IsBusy = true;
            try
            {
                Console.WriteLine("TEST");
            }
            catch (Exception ex)
            {
                await _messageService.DisplayError("TEST", ex.Message, "dismiss");
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
