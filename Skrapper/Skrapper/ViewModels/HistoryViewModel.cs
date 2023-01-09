using MyWebService;
using Skrapper.Models;
using Skrapper.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper
{
    public class HistoryViewModel : MainViewModel
    {
        public Command DeleteScanCommand { private set; get; }
        public Command AddNoteCommand { get; }
        public Command DisplayScanCommand { get; }
        public Command DoRefreshHistoryCommand { get; }

        public HistoryViewModel()
        {
            Title = "HISTORY";

            DeleteScanCommand = new Command(async () => await OnDeleteScanClicked(selectedScan.SkidNo, selectedScan.PartNo)); ;
            AddNoteCommand = new Command(async () => await OnAddNoteClicked());
            DisplayScanCommand = new Command(async () => await OnDisplayScanClicked());
            DoRefreshHistoryCommand = new Command(DoRefreshHistory);
        }

        public void OnAppearing()
        {
            SelectedScan = null;
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

        private async Task OnDeleteScanClicked(string skidNo, string partNo)
        {
            IsBusy = true;
            try
            {
                bool b = await _messageService.DisplayCustomPrompt("Delete Scan?", "Are you sure you want to delete the following scan?\r\n\tP/N: " + selectedScan.PartNo + "\r\n\tS/N: " + selectedScan.SerialNo, "yes", "no");
                Console.WriteLine("[HistoryViewModel.cs] (OnDeleteScanClicked) b >> " + b);
            }
            catch(Exception ex)
            {
                IsBusy = false;
                Console.WriteLine("[ERROR: HistoryViewModel.cs] (OnDeleteScanClicked) ex >> " + ex.ToString());
                await _messageService.DisplayError("[ERROR: HistoryViewModel.cs]", "(OnDeleteScanClicked) ex >> " + ex.Message, "dismiss");
            }
            finally
            {
                SelectedScan = null;
                IsBusy = false;
            }
        }

        private async Task OnAddNoteClicked()
        {
            if (!eHelpDeskContext.WebServiceConnected()) return;

            IsBusy = true;
            try
            {
                using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                {
                    if (noteEntryText.Trim().Length > 0)
                    {
                        Console.WriteLine("[HistoryViewModel.cs] (OnAddNoteClicked) selectedUser >> " + selectedUser);
                        Console.WriteLine("[HistoryViewModel.cs] (OnAddNoteClicked) NoteEntryText >> " + noteEntryText);
                        Console.WriteLine("[HistoryViewModel.cs] (OnAddNoteClicked) selectedSkidItem >> " + Globals.pSkidItem);
                        //await SkidInfoDataSave(true);
                        Task<string> ts = Task.Run(() => locWS.Skp_SkidGetFieldData(Globals.pSkidItem, "SkidID"));
                        string skidID = ts.Result;
                        locWS.Skp_SkidUpdateNote(selectedUser, "SKP_Skid", skidID, noteEntryText);
                        await _messageService.DisplayError("NOTE ADDED", "You have successfully submitted a note for SKID#"+Globals.pSkidItem, "dismiss");
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
                SetProperty(ref skidPrintLabel, "NOTE SUBMITTED");
                OnPropertyChanged("SkidPrintLabel");
                NoteEntryText = string.Empty;
                OnPropertyChanged("NoteEntryText");
                IsBusy = false;
            }
        }

        private async Task OnDisplayScanClicked()
        {
            bool b = await _messageService.DisplayCustomPrompt("SKID#"+selectedScan.SkidNo+" \r\n(P/N: " + selectedScan.PartNo + ")", "S/N:\t" + selectedScan.SerialNo + "\r\nHeci: " + selectedScan.HeciCode, "delete", "dismiss");
            
            if (b)
                await OnDeleteScanClicked(selectedScan.SkidNo, selectedScan.PartNo);
            else
            {
                SelectedScan = null;
                return;
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




        public ObservableCollection<History> scanHistory = new();
        public ObservableCollection<History> ScanHistory
        {
            set { SetProperty(ref scanHistory, value); }
            get
            {
                scanHistory ??= new ObservableCollection<History>();
                return scanHistory;
            }
        }

        public History selectedScan = null;
        public History SelectedScan
        {
            set
            {
                if (value != null)
                {
                    Globals.dgSelectedScan = value;
                    SetProperty(ref selectedScan, value);
                    DisplayScanCommand.Execute(true);
                    //_messageService.DisplayError("Detail", string.Format("UserName: {0}\r\nrowID: {1}\r\niAdjID: {2}\r\nPartNum: {3}\r\nSerialNum: {4}\r\nScanDate: {5}", dgSelectedScan.UserName, dgSelectedScan.rowID, dgSelectedScan.iAdjID, dgSelectedScan.PartNum, dgSelectedScan.SerialNumber, dgSelectedScan.ScanDate), "dismiss");
                }
                else
                {
                    SetProperty(ref selectedScan, null);
                }
            }
            get
            {
                return selectedScan;
            }
        }

        public string noteEntryText = Globals.eNoteText;
        public string NoteEntryText
        {
            set
            {
                SetProperty(ref noteEntryText, value);
                Globals.eNoteText = value;
                Console.WriteLine("[MainViewModel.cs] (NoteEntryText) noteEntryText >> " + $"{noteEntryText}");
            }
            get
            {
                noteEntryText ??= string.Empty;
                return noteEntryText;
            }
        }

        public string scanCountString = "Scan Count: 0";
        public string ScanCountString
        {
            set { SetProperty(ref scanCountString, value); }
            get
            {
                string s = "Scan Count: " + $"{ScanHistory.Count}";
                return s;
            }
        }
    }
}
