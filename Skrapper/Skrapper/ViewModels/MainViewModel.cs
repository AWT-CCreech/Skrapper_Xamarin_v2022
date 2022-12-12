using Android.Hardware;
using MyWebService;
using Skrapper.Models;
using Skrapper.Services;
using Skrapper.Themes;
using Skrapper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace Skrapper
{
    public class MainViewModel : ViewModelBase
    {
        public string VersionLabel { get; } = Constants.VERSION;
        public string CopyrightLabel { get; } = Constants.COPYRIGHT;

        public MainViewModel()
        {
            GetTheme();

            LoginCommand = new Command(OnLoginClicked);
            QuitCommand = new Command(OnQuitClicked);
            DisplayScanCommand = new Command(OnDisplayScanClicked);

            Users = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetUserPickerList());
            Skids = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetSkidNumbers());
            Carriers = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetCarriers());
            PartTypes = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetPartTypes());
        }

        #region [USER]
        public NotifyTaskCompletion<ObservableCollection<string>> Users { get; private set; }

        public int selectedUserIndex = Globals.pUserIdx;
        public int SelectedUserIndex
        {
            set
            {

                Console.WriteLine("[MainViewModel.cs] (SelectedUserIndex) >> " + value);
                SetProperty(ref selectedUserIndex, value);
                Globals.pUserIdx = selectedUserIndex;
                if (messageLabel.Length > 0)
                {
                    SetProperty(ref messageLabel, string.Empty);
                    SetProperty(ref messageLabelColor, Color.Black);
                    OnPropertyChanged("MessageLabel");
                    OnPropertyChanged("MessageLabelColor");
                }
            }
            get { return selectedUserIndex; }

        }

        public string selectedUser = Globals.UserName;
        public string SelectedUser
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (SelectedUser) >> " + value);

                SetProperty(ref selectedUser, value);
                Globals.UserName = selectedUser;
            }
            get { return selectedUser; }
        }
        #endregion

        #region --: Login Page :--
        public Command LoginCommand { get; }
        public Command QuitCommand { get; }
        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;

            Task<bool> validated = Task.Run(() => IsValidUser());
            if (validated.Result)
            {
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                App.IsUserLoggedIn = true;
                await Xamarin.Essentials.SecureStorage.SetAsync("isUserLogged", "1");
                Application.Current.MainPage = new MainTabbedPage();
            }
            else
            {
                IsBusy = false;
                return;
            }
        }

        private void OnQuitClicked(object obj)
        {
            Globals.gWS = null;
            App.IsUserLoggedIn = false;
            Environment.Exit(0);
        }

        private async Task<bool> IsValidUser()
        {
            bool res = false;
            try
            {
                res = selectedUserIndex > -1;
                if (res)
                    SetProperty(ref messageLabel, string.Empty);
                else
                    SetProperty(ref messageLabel, "[ERROR: Select Username]");
                OnPropertyChanged("MessageLabel");
            }
            catch (Exception ex)
            {
                await _messageService.DisplayError("[ERROR: LoginViewModel.cs]", ex.Message, "dismiss");
            }

            IsBusy = false;
            return res;
        }

        Color messageLabelColor = Color.IndianRed;
        public Color MessageLabelColor
        {
            set
            {
                if (value != null)
                {
                    if (value != messageLabelColor)
                        messageLabelColor = value;
                }
                else
                {
                    messageLabelColor = Color.IndianRed;
                }

            }
            get { return messageLabelColor; }
        }

        public string messageLabel = string.Empty;
        public string MessageLabel
        {
            set
            {
                Console.WriteLine("[LoginViewModel.cs] (MessageLabel) >> " + value);

                SetProperty(ref messageLabel, value);
            }
            get { return messageLabel; }
        }
        #endregion

        #region --: Skid Page :-- 
        public NotifyTaskCompletion<ObservableCollection<string>> Skids { get; set; }
        public NotifyTaskCompletion<ObservableCollection<string>> Carriers { get; set; }

        public NotifyTaskCompletion<ObservableCollection<XElement>> SkidGrid { get; set; }


        public int selectedSkidIndex = Globals.pSkidIdx;
        public int SelectedSkidIndex
        {
            set
            {
                SetProperty(ref selectedSkidIndex, value);
                Globals.pSkidIdx = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedSkidIndex) >> " + value);

            }
            get { return selectedSkidIndex; }
        }

        public string selectedSkidItem = Globals.pSkidItem;
        public string SelectedSkidItem
        {
            set
            {
                SetProperty(ref selectedSkidItem, value);
                Globals.pSkidItem = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedSkidItem) >> " + value);
                if (selectedSkidIndex > -1)
                    Task.Run(()=>SkidInfoDataLoad(Globals.pSkidItem));

                SelectedPartNumberIndex = -1;
                OnPropertyChanged("SelectedPartNumberIndex");
            }
            get { return selectedSkidItem; }
        }

        public int selectedActionIndex = Globals.pActionIdx;
        public int SelectedActionIndex
        {
            get { return selectedActionIndex; }
            set
            {
                SetProperty(ref selectedActionIndex, value);
                Globals.pActionIdx = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedActionIndex) >> " + value);
            }
        }

        public string selectedActionItem = Globals.pActionItem;
        public string SelectedActionItem
        {
            get { return selectedActionItem; }
            set
            {
                SetProperty(ref selectedActionItem, value);
                Globals.pActionItem = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedActionItem) >> " + value);
            }
        }

        public int selectedCarrierIndex = Globals.pCarrierIdx;
        public int SelectedCarrierIndex
        {
            get { return selectedCarrierIndex; }
            set
            {
                SetProperty(ref selectedCarrierIndex, value);
                Globals.pCarrierIdx = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedCarrierIndex) >> " + value);
            }
        }

        public string selectedCarrierItem = Globals.pCarrierItem;
        public string SelectedCarrierItem
        {
            get { return selectedCarrierItem; }
            set
            {
                SetProperty(ref selectedCarrierItem, value);
                Globals.pCarrierItem = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedCarrierItem) >> " + value);
            }
        }

        public string skidPrintLabel = Globals.lblSkidPrint;
        public string SkidPrintLabel
        {
            set { SetProperty(ref skidPrintLabel, value); }
            get
            {
                skidPrintLabel ??= string.Empty;
                return skidPrintLabel;
            }
        }
        #endregion

        #region --: Scan Page :--
        public NotifyTaskCompletion<ObservableCollection<string>> PartNumberChoices { get; set; }
        public NotifyTaskCompletion<ObservableCollection<string>> PartTypes { get; set; }

        public int selectedPartNumberIndex = Globals.pPartNumIdx;
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

        public string selectedPartNumber = Globals.pPartNumItem;
        public string SelectedPartNumber
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (SelectedPartNumber) >> " + value);
                SetProperty(ref selectedPartNumber, value);
                Globals.pPartNumItem = selectedPartNumber;

                SelectedPartType = Globals.pPartTypeItem;

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

        public NotifyTaskCompletion<string> altPartNumber;
        public NotifyTaskCompletion<string> AltPartNumber
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (AltPartNumber) >> " + value.Result);
                SetProperty(ref altPartNumber, value);
            }
            get { return altPartNumber; }
        }
        public int selectedPartTypeIndex = Globals.pPartTypeIdx;
        public int SelectedPartTypeIndex
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (SelectedPartTypeIndex) value >> " + value);
                SetProperty(ref selectedPartTypeIndex, value);
                Globals.pPartTypeIdx = value;
            }
            get { return selectedPartTypeIndex; }
        }

        public string selectedPartType = Globals.pPartTypeItem;
        public string SelectedPartType
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (SelectedPartType) value >> " + value);
                SetProperty(ref selectedPartType, value);
                Globals.pPartTypeItem = value;
            }
            get
            {
                selectedPartType ??= string.Empty;
                return selectedPartType;
            }
        }


        public int partQuantity = Globals.ePartQuantity;
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

        public bool autoChecked = Globals.cbAuto;
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

        public string serialNumber = "";
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

        public string validationText = Globals.gValidStatus;
        public string ValidationText
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (ValidationText) >> " + value);
                SetProperty(ref validationText, value);
            }
            get { return validationText; }
        }

        public Color validationTextColor = Globals.gValidStatusTextColor;
        public Color ValidationTextColor
        {
            set
            {
                Console.WriteLine("[ScanViewModel.cs] (ValidationTextColor) >> " + value);
                SetProperty(ref validationTextColor, value);
            }
            get { return validationTextColor; }
        }

        public void SetOrderInProcess(bool inProcess)
        {
            SetProperty(ref orderInProcess, inProcess);
            OnPropertyChanged("OrderInProcess");
        }
        #endregion

        #region --: History Page :--
        public Command DisplayScanCommand { get; }

        public ObservableCollection<History> skidHistory = null;
        public ObservableCollection<History> SkidHistory
        {
            set { SetProperty(ref skidHistory, value); }
            get
            {
                skidHistory ??= new ObservableCollection<History>();
                return skidHistory;
            }
        }

        public History selectedScan = null;
        public History SelectedScan
        {
            set
            {
                if (value != null)
                {
                    SetProperty(ref selectedScan, value);
                    Console.WriteLine("[MainViewModel.cs] (SelectedScan) selectedScan.rowID           >> " + selectedScan.SkidNo);
                    Console.WriteLine("[MainViewModel.cs] (SelectedScan) selectedScan.iAdjID          >> " + selectedScan.PartNo);
                    Console.WriteLine("[MainViewModel.cs] (SelectedScan) selectedScan.PartNum         >> " + selectedScan.SerialNo);
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
                selectedScan ??= new History();
                return selectedScan;
            }
        }

        public string noteEntryText = Globals.eNoteText;
        public string NoteEntryText
        {
            set 
            { 
                SetProperty(ref noteEntryText, value); 
                Globals.eNoteText= value;
                Console.WriteLine("[MainViewModel.cs] (NoteEntryText) noteEntryText >> "+$"{noteEntryText}");
            }
            get 
            {
                noteEntryText ??= string.Empty;
                return noteEntryText; 
            }
        }

        public string skidCountString = "Scan Count: 0";
        public string SkidCountString
        {
            set { SetProperty(ref skidCountString, value); }
            get
            {
                string s = "Scan Count: " + $"{SkidHistory.Count}";
                return s;
            }
        }

        public async void OnDisplayScanClicked(object obj)
        {
            await _messageService.DisplayError("Scan Details", selectedScan.PartNo + "\r\n", "dismiss");
        }
        #endregion

        #region --: Settings Page : --

        #endregion

        #region --: Helper Functions :--

        public async Task SkidInfoDataLoad(string SkidNo)
        {
            //DoClearSkidInfo();
            if (SkidNo.Length > 0 && eHelpDeskContext.WebServiceConnected())
            {
                try
                {
                    IsBusy = true;
                    using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                    {
                        string fieldData = locWS.Skp_SkidGetFieldData(SkidNo, "");
                        Console.WriteLine("[MainViewModel.cs] (SkidInfoDataLoad) fieldData >> " + fieldData);

                        // leave whatField blank to return all field data in a parameterized list
                        Dictionary<string, string> d = UtilityService.CreateDictionaryFromString(fieldData, ";");

                        // fields for SKP_Skid
                        // SkidID, SkidNo, DateCode, SkidSeq, CreatedOn, CreatedBy, CarrierCode, 
                        //		LocationID, LocationStr, Weight, Wrapped, SkidFull, SkidType
                        string fldName = "";
                        string fldValue = "";

                        //fldName = "LocationStr";
                        //if (d.TryGetValue(fldName, out fldValue))
                        //AddLocationToCombo(fldValue, true);

                        //fldName = "Weight";
                        //if (d.TryGetValue(fldName, out fldValue))
                        //WeightTextbox.Text = fldValue;

                        //fldName = "SkidType";
                        //if (d.TryGetValue(fldName, out fldValue))
                        //cmbSkidTypes.Text = fldValue;

                        fldName = "CarrierCode";
                        if (d.TryGetValue(fldName, out fldValue))
                        {
                            SetProperty(ref selectedCarrierItem, fldValue);
                            OnPropertyChanged("SelectedCarrierItem");
                        }

                        fldName = "PartType";
                        if (d.TryGetValue(fldName, out fldValue))
                        {
                            //cmbPrimaryPartType.Text = fldValue;
                            Globals.pPartTypeItem = fldValue;
                            //SetProperty(ref selectedPartType, fldValue);
                            SelectedPartType = fldValue;
                            OnPropertyChanged("SelectedPartType");
                        }

                        //fldName = "SkidFull";       // A.K.A.  SkidComplete/Closed
                        //if (d.TryGetValue(fldName, out fldValue))
                        //{
                        //SkidComplete = ConvertStringToBool(fldValue);
                        //btnCompleteOrder.Enabled = !SkidComplete;
                        //cbSkidFull.Checked = SkidComplete;
                        //}

                        //ADDED BY BEN - NOV 2016 TO ALLOW USERS TO MARK A SKID AS LOADED (ONTO A TRUCK)
                        //fldName = "SkidLoaded";
                        //if (d.TryGetValue(fldName, out fldValue))
                        //{
                        //SkidIsLoaded = ConvertStringToBool(fldValue);
                        //chkLoaded.Checked = SkidIsLoaded;

                        //}

                        //fldName = "Deleted";
                        //if (d.TryGetValue(fldName, out fldValue))
                        //{
                        //SkidDeleted = ConvertStringToBool(fldValue);
                        //if (SkidDeleted)
                        //{
                        //btnCompleteOrder.Enabled = false;
                        //bSubmit.Enabled = false;
                        //string sDl = "Skid DELETED!";
                        //ChangeAppTitleBar(sDl);
                        //UpdateValidationText(sDl, Color.Red, Color.Yellow);
                        //SkidPrintLabel.Text = sDl;
                        //}
                        //}

                        //CommentsTextbox.Text = "";  // we now always add to notes, not overwrite the notes field

                    }
                }
                catch(Exception ex)
                {
                    IsBusy = false;
                    await _messageService.DisplayError("[ERROR: MainViewModel.cs]", "(SkidInfoDataLoad)\r\n"+ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: MainViewModel.cs] (SkidInfoDataLoad) ex >>" + ex.Message);
                }
                finally
                {
                    IsBusy= false;
                }
            }
        }

        public async Task SkidInfoDataSave(bool DisplaySaveResults)
        {
            // save data changes made tot he Skid Info tab
            string s = "UPDATE SKP_Skid SET LocationStr='{0}', Weight={1}, SkidType='{2}',"
                    + " CarrierCode='{3}', PartType='{4}' WHERE SkidNo = '{5}'";

            //if (WeightTextbox.Text.Length == 0)
                //WeightTextbox.Text = "0";
            //else
            //{
                // is weight a number?
                //if (!IsStringInteger(WeightTextbox.Text))
                    //WeightTextbox.Text = "0";
            //}

            string SKIDtype = "Pallet";
            string sql = string.Format(s, "", "0", SKIDtype, selectedCarrierItem, "Telecom - General", selectedSkidItem);

            if (!eHelpDeskContext.WebServiceConnected()) return;

            try
            {
                using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                {
                    string result = locWS.ExecProcedure(sql, "OK");
                    string SkidID = locWS.Skp_SkidGetFieldData(selectedSkidItem, "SkidID");

                    //locWS.Skp_SkidUpdateField(selectedSkidItem, "SkidFull", cbSkidFull.Checked ? "1" : "0");
                    locWS.Skp_SkidUpdateField(selectedSkidItem, "SkidFull", "0");

                    //ADDED BY BEN NOV 2016 - LOADED CHECKBOX TO SHOW THE SKID HAS BEEN LOADED
                    //locWS.Skp_SkidUpdateField(selectedSkidItem, "SkidLoaded", (chkLoaded.Checked ? "1" : "0"));
                    locWS.Skp_SkidUpdateField(selectedSkidItem, "SkidLoaded", "0");

                    // update skid notes - only adds to notes/comments
                    // MOVED TO HISTORY VIEW MODEL
                    //if (noteEntryText.Trim().Length > 0)
                    //{
                    //    locWS.Skp_SkidUpdateNote(Globals.UserName, "SKP_Skid", SkidID, noteEntryText);
                    //}

                    if (DisplaySaveResults)
                        await _messageService.DisplayError("SKID UPDATED", result, "dismiss");
                    else
                    {
                        SkidPrintLabel = "SKID UPDATED";
                        OnPropertyChanged("SkidPrintLabel");
                    }
                }
                
            }
            catch
            {
                await _messageService.DisplayError("[ERROR: MainViewModel.cs] (SkidInfoDataSave)", "Check Web Service", "dismiss");
            }
        }

        public async void GetTheme()
        {
            try
            {
                Theme theme;
                if (!Globals.bTestMode)
                    theme = Theme.Live;
                else
                    theme = Theme.Test;

                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    mergedDictionaries.Clear();

                    switch (theme)
                    {
                        case Theme.Test:
                            mergedDictionaries.Add(new TestTheme());
                            break;
                        case Theme.Live:
                            mergedDictionaries.Add(new LiveTheme());
                            break;
                        default:
                            mergedDictionaries.Add(new LiveTheme());
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                await _messageService.DisplayError("[ERROR: MainViewModel.cs]", "(GetTheme)\r\n" + ex.Message, "dismiss");
            }
        }
        #endregion
    }
}
