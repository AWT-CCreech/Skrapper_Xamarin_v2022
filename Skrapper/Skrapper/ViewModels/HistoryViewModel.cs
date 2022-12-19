using MyWebService;
using Skrapper.Models;
using Skrapper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper
{
    public class HistoryViewModel : MainViewModel
    {
        public Command DeletePartScanCommand { private set; get; }
        public Command AddNoteCommand { get; }
        public Command DoRefreshHistoryCommand { get; }

        public HistoryViewModel()
        {
            Title = "HISTORY";

            AddNoteCommand = new Command(async () => await OnAddNoteClicked());
            DoRefreshHistoryCommand = new Command(DoRefreshHistory);
        }

        public void OnAppearing()
        {
            bool b = ScanHistory.Count <= 0;
            if (b)
            {
                DoRefreshHistoryCommand.Execute(true);
            }
            else
            {
                if(Globals.pSkidItem != ScanHistory[0].SkidNo)
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
                    ScanHistory.Clear();
                    OnPropertyChanged("ScanHistory");
                    OnPropertyChanged("ScanCountString");
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
                    ScanHistory = t.Result;
                    OnPropertyChanged("ScanHistory");
                    OnPropertyChanged("ScanCountString");
                }
                catch (Exception ex)
                {
                    IsRefreshing = false;
                    Console.WriteLine("[ERROR: HistoryViewModel.cs] (DoRefreshHistory) >> " + ex.ToString());
                    await _messageService.DisplayError("[ERROR: HistoryViewModel.cs]", ex.Message, "dismiss");
                }
                finally
                {
                    IsRefreshing = false;
                }
            }
        }


        public async Task OnAddNoteClicked()
        {
            if (!eHelpDeskContext.WebServiceConnected()) return;

            try
            {
                using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                {
                    IsBusy = true;
                    if (noteEntryText.Trim().Length > 0)
                    {
                        Console.WriteLine("[HistoryViewModel.cs] (OnAddNoteClicked) NoteEntryText >> " + noteEntryText);
                        //await SkidInfoDataSave(true);
                        locWS.Skp_SkidUpdateNote(selectedUser, "SKP_Skid", selectedSkidItem, noteEntryText);

                        SkidPrintLabel = "SKID UPDATED";
                        OnPropertyChanged("SkidPrintLabel");
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Console.WriteLine("[ERROR: HistoryViewModel.cs] (OnAddNoteClicked) ex >> " + ex.ToString());
                await _messageService.DisplayError("[ERROR: HistoryViewModel.cs]", "(OnAddNoteClicked)\r\n" + ex.Message, "dismiss");
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
