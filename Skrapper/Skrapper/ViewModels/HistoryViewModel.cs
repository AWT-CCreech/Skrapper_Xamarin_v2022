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
            /*
            if(Globals.pSkidIdx >= 0)
            */
            bool b = SkidHistory.Count <= 0;
            if (b)
            {
                DoRefreshHistoryCommand.Execute(true);
            }
            else
            {
                if(Globals.pSkidItem != SkidHistory[0].SkidNo)
                {
                    DoRefreshHistoryCommand.Execute(true);
                }
            }
        }

        private async void DoRefreshHistory(object obj)
        {
            if(Globals.pSkidIdx < 0)
            {
                IsRefreshing = true;
                try
                {
                    SkidHistory.Clear();
                    OnPropertyChanged("SkidHistory");
                    OnPropertyChanged("SkidCountString");
                }
                catch(Exception ex)
                {
                    IsRefreshing = false;
                    await _messageService.DisplayError("[ERROR: HistoryViewModel.cs]", ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: HistoryViewModel.cs] (DoRefreshHistory) >> " + ex.ToString());
                }
                finally
                {
                    IsRefreshing= false;
                }
            }
            else
            {
                IsRefreshing = true;
                try
                {
                    Task<ObservableCollection<History>> t = Task.Run(async () => await DataGridService.LoadGridFromSkidNum(Globals.pSkidItem));
                    SkidHistory = t.Result;
                    OnPropertyChanged("SkidHistory");
                    OnPropertyChanged("SkidCountString");
                }
                catch (Exception ex)
                {
                    IsRefreshing = false;
                    await _messageService.DisplayError("[ERROR: HistoryViewModel.cs]", ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: HistoryViewModel.cs] (DoRefreshHistory) >> " + ex.ToString());
                }
                finally
                {
                    IsRefreshing = false;
                }
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
