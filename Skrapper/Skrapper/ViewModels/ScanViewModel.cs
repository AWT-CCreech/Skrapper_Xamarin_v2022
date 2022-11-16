using Skrapper.Services;
using System;
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
            EnterPartNumberCommand = new Command(
                execute: async () =>
                {
                    if (PartNumberChoices == null) return;
                    bool b = await _messageService.CustomInputDialog(PartNumberChoices.Result, "Part # Entry", "Please insert part number below...", "OK");
                    if (b)
                    {
                        SetProperty(ref selectedPartNumberIndex, 0);
                        OnPropertyChanged("SelectedPartNumberIndex");
                    }
                });
        }

        int selectedPartNumberIndex;
        public int SelectedPartNumberIndex
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (SelectedPartNumberIndex) >> " + value);
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
                Console.WriteLine("[ScanViewModel.cs] (SelectedPartNumber) >> " + value);
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
                Console.WriteLine("[ScanViewModel.cs] (AltPartNumber) >> " + value.Result);
                SetProperty(ref altPartNumber, value);
            }
            get { return altPartNumber; }
        }


        int partQuantity = Globals.ePartQuantity;
        public int PartQuantity
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (PartQuantity) >> " + value);
                if (value >= 1) 
                { 
                    SetProperty(ref partQuantity, value); 
                }
                else 
                { 
                    SetProperty(ref partQuantity, 1);
                }
            }
            get
            {
                if (partQuantity <= 1)
                    SetProperty(ref partQuantity, 1);
                return partQuantity;
            }
        }

        bool autoChecked = Globals.cbAuto;
        public bool AutoChecked
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (AutoChecked) >> " + value);
                SetProperty(ref autoChecked, value);
                Globals.cbAuto = autoChecked;

                if (autoChecked)
                    Globals.gLastScanData = string.Empty;
            }
            get { return autoChecked; }
        }

        string serialNumber = "";
        public string SerialNumber
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (SerialNumber) >> " + value);
                SetProperty(ref serialNumber, value.ToUpper());
                Globals.eSerialNoText = serialNumber.ToUpper();
            }
            get { return serialNumber.ToUpper(); }
        }

        string validationText = Globals.gValidStatus;
        public string ValidationText
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (ValidationText) >> " + value);
                SetProperty(ref validationText, value);
            }
            get { return validationText; }
        }

        Color validationTextColor = Globals.gValidStatusTextColor;
        public Color ValidationTextColor
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (ValidationTextColor) >> " + value);
                SetProperty(ref validationTextColor, value);
            }
            get { return validationTextColor; }
        }
    }
}
