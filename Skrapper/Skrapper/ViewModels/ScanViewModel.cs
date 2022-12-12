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
    }
}
