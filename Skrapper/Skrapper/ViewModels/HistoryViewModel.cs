using Skrapper.Models;
using Skrapper.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Skrapper
{
    public class HistoryViewModel : MainViewModel
    {
        public IMessageService _messageService = new MessageService();

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

        private async void DoRefreshHistory(object obj)
        {
            IsBusy = true;
            try
            {

            }
            catch(Exception ex)
            {
                await _messageService.DisplayError("[ERROR: HistoryViewModel.cs]", ex.Message, "dismiss");
                Console.WriteLine("[ERROR: HistoryViewModel.cs] (DoRefreshHistory) >> " + ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        Scan selectedScan = null;
        public Scan SelectedScan
        {
            set
            {
                if (value != null)
                {
                    SetProperty(ref selectedScan, value);
                    Console.WriteLine("[MainViewModel.cs] (SelectedScan) selectedScan.UserName        >> " + selectedScan.UserName);
                    Console.WriteLine("[MainViewModel.cs] (SelectedScan) selectedScan.rowID           >> " + selectedScan.rowID);
                    Console.WriteLine("[MainViewModel.cs] (SelectedScan) selectedScan.iAdjID          >> " + selectedScan.iAdjID);
                    Console.WriteLine("[MainViewModel.cs] (SelectedScan) selectedScan.PartNum         >> " + selectedScan.PartNum);
                    Console.WriteLine("[MainViewModel.cs] (SelectedScan) selectedScan.SerialNumber    >> " + selectedScan.SerialNumber);
                    Console.WriteLine("[MainViewModel.cs] (SelectedScan) selectedScan.ScanDate        >> " + selectedScan.ScanDate);
                    DisplayScanCommand.Execute(true);
                    //_messageService.DisplayError("Detail", string.Format("UserName: {0}\r\nrowID: {1}\r\niAdjID: {2}\r\nPartNum: {3}\r\nSerialNum: {4}\r\nScanDate: {5}", selectedScan.UserName, selectedScan.rowID, selectedScan.iAdjID, selectedScan.PartNum, selectedScan.SerialNumber, selectedScan.ScanDate), "dismiss");
                }
                else
                {
                    SetProperty(ref selectedScan, null);
                }
            }
            get
            {
                if (selectedScan != null)
                    return selectedScan;
                else
                    return null;
            }
        }

        string scanCountString;
        public string ScanCountString
        {
            set { SetProperty(ref scanCountString, value); }
            get
            {
                string s = "Scan Count: " + scanHistory.Count.ToString();
                return s;
            }
        }

        ObservableCollection<Scan> scanHistory = null;
        public ObservableCollection<Scan> ScanHistory
        {
            set { SetProperty(ref scanHistory, value); }
            get
            {
                scanHistory ??= new ObservableCollection<Scan>();
                return scanHistory;
            }
        }

        public void AddScan()
        {
            ScanHistory.Add(new Scan { rowID = Globals.rowID, iAdjID = Globals.iAdjID, UserName = Globals.UserName, PartNum = Globals.pPartNumItem, SerialNumber = Globals.eSerialNoText, ScanDate = DateTime.Now });
            OnPropertyChanged("ScanCountString");
        }
        public void ClearScanHistory()
        {
            ScanHistory.Clear();
            OnPropertyChanged("ScaHistory");
            OnPropertyChanged("ScanCountString");
            //TODO
            //SetProperty(ref selectedScan, null);
            //OnPropertyChanged("SelectedScan");
        }

        private async void OnAddNoteClicked(object obj)
        {
            IsBusy = true;
            try
            {
                Console.WriteLine("TEST");
            }
            catch(Exception ex)
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
