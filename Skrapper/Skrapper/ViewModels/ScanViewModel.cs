using Skrapper.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Skrapper
{
    public class ScanViewModel : MainViewModel
    {
        public IMessageService _messageService = new MessageService();
        public ScanViewModel()
        {
            Title = "SCAN";
        }

        int selectedPartNumberIndex;
        public int SelectedPartNumberIndex
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (SelectedPartNumberIndex) >> " + value);
                SetProperty(ref selectedPartNumberIndex, value);
                Globals.pPartNumIdx = selectedPartNumberIndex;
            }
            get { return selectedPartNumberIndex; }
        }

        string selectedPartNumber;
        public string SelectedPartNumber
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (SelectedPartNumber) >> " + value);
                SetProperty(ref selectedPartNumber, value);
                Globals.pPartNumItem = selectedPartNumber;

                Task<string> ts;

                if (!string.IsNullOrEmpty(value))
                {
                    ts = Task.Run(async () => await LabelService.GetAltPartNumber(selectedPartNumber));
                    AltPartNumber = new NotifyTaskCompletion<string>(ts);
                }
                else
                {
                    ts = Task.Run(async () => await LabelService.DoStrReturn(""));
                    AltPartNumber = new NotifyTaskCompletion<string>(ts);
                }
            }
            get
            {
                selectedPartNumber ??= string.Empty;
                return selectedPartNumber;
            }
        }

        NotifyTaskCompletion<string> altPartNumber;
        public NotifyTaskCompletion<string> AltPartNumber
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (AltPartNumber) >> " + value.Result);
                SetProperty(ref altPartNumber, value);
            }
            get { return altPartNumber; }
        }

    }
}
