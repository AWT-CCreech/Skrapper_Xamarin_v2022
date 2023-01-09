using Android.OS;
using Skrapper.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper
{
    public class ScanViewModel : MainViewModel
    {
        public Command EnterPartNumberCommand { private set; get; }
        public Command SerialNumberReturnCommand { private set; get; }
        public Command SubmitScanCommand { private set; get; }
        public Command CompleteSkidCommand { private set; get; }
        public Command DeleteLastScanCommand { private set; get; }

        public ScanViewModel()
        {
            Title = "SCAN";
            EnterPartNumberCommand = new Command(OnEnterPartNumberClicked);
            SubmitScanCommand = new Command(OnSubmitScanClicked);

            DeleteLastScanCommand = new Command(OnDeleteLastScanClicked);
        }

        public void OnAppearing()
        {
            Task<ObservableCollection<string>> parts = Task.Run(async () => await DataGridService.LoadPartListFromSkidNum(Globals.pSkidItem));
            if (string.IsNullOrEmpty(Globals.PartsList))
            {
                PartNumberChoices = new NotifyTaskCompletion<ObservableCollection<string>>(parts);
                OnPropertyChanged("PartNumberChoices");
            }
            if (Globals.pSkidIdx > -1 && Globals.pPartNumIdx < 0 && (parts.Result != PartNumberChoices.Result))
            {
                PartNumberChoices = new NotifyTaskCompletion<ObservableCollection<string>>(parts);
                OnPropertyChanged("PartNumberChoices");
            }
        }

        private async void OnEnterPartNumberClicked(object obj)
        {
            if (PartNumberChoices == null) return;
            bool b = await _messageService.CustomInputDialog(PartNumberChoices.Result, "Part # Entry", "Please insert part number below...", "OK", selectedPartNumber.ToString());
            if (b)
            {
                SetProperty(ref selectedPartNumberIndex, 0);
                OnPropertyChanged("SelectedPartNumberIndex");
            }
        }

        private async void OnSubmitScanClicked(object obj)
        {
            if (eHelpDeskContext.WebServiceConnected())
            {
                IsBusy = true;
                try
                {
                    if(orderInProcess == false)
                    {
                        SetOrderInProcess(true);
                    }

                    if(partQuantity > 1)
                    {
                        bool confirmSubmit = await _messageService.DisplayCustomPrompt("Confirmation Needed...", "Are you sure you would like to submit " + partQuantity + " of " + selectedPartNumber + "?", "yes", "no");
                    }    
                }
                catch(Exception ex)
                {
                    IsBusy = false;
                    await _messageService.DisplayError("[ScanViewModel.cs]", "(OnSubmitScanClicked)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ScanViewModel.cs] (OnSubmitScanClicked) ex >> " + ex.ToString());
                }
                finally
                {
                    IsBusy = false;
                }
            }
            
        }

        private async void OnDeleteLastScanClicked(object obj)
        {
            bool b = await _messageService.DisplayCustomPrompt("Delete Last Scan?", "Are you sure you would like to delete the following scan:\r\nP/N: " + Globals.dgSelectedScan.PartNo + "\r\nS/N: " + Globals.dgSelectedScan.SerialNo, "yes", "no");
            Console.WriteLine("[ScanViewModel.cs] (OnDeleteLastScanClicked) b >> " + b);
        }
    }
}
