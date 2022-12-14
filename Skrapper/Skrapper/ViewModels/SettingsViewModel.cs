using MyWebService;
using Skrapper.Services;
using Skrapper.Themes;
using Skrapper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper
{
    public class SettingsViewModel : MainViewModel
    {
        #region *** SETTINGS TAB *****************************************************************************
        public Command LogoutCommand { private set; get; }
        public Command CheckServerCommand { private set; get; }
        public Command DebugResultsCommand { private set; get; }
        public Command ShowDeviceInfoCommand { private set; get; }

        public SettingsViewModel()
        {
            Title = "SETTINGS";

            LogoutCommand = new Command(OnLogoutClicked);
            DebugResultsCommand = new Command(OnDebugClicked);
            CheckServerCommand = new Command(OnCheckServerClicked);
            ShowDeviceInfoCommand = new Command(OnShowDeviceInfoClicked);
        }

        public void OnAppearing()
        {
            //RefreshCommand.Execute(true);
        }

        private async void OnDebugClicked()
        {
            if (Globals.gLastScanData.Length > 0 || Globals.gLastScanResults.Length > 0)
                await _messageService.DisplayError("Debug", "gLastScanResults...\r\n" + Globals.gLastScanResults + "\r\n\r\ngLastScanData...\r\n" + Globals.gLastScanData, "dismiss");
            else
                return;
        }

        private async void OnCheckServerClicked()
        {
            Task<string> ts;
            string s, t;
            try
            {
                using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                {
                    if (!eHelpDeskContext.WebServiceConnected()) return;

                    ts = Task.Run(locWS.Endpoint.Address.ToString);
                    s = ts.Result;
                    ts = Task.Run(locWS.GetServerURL);
                    t = ts.Result;
                    locWS.Close();
                }
                s = string.Format("cbTestMode = {0}\n\nWebSvr\n{1}\n\nServer\n{2}", testModeIsChecked, s, t);
                await _messageService.DisplayError("Server Mode", s, "dismiss");
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
                await _messageService.DisplayError("[ERROR: MainViewModel.cs]", "(CheckServerCommand)\r\n" + err.Message, "dismiss");
            }
        }

        private async void OnShowDeviceInfoClicked()
        {
            string _info = "Device Model:\t\t" + Globals.gDeviceModel
                            + "\r\nDevice Name:\t\t\t" + Globals.gDeviceName
                            + "\r\nDevice Platform:\t" + Globals.gDevicePlatform
                            + "\r\nDevice Type:\t\t\t" + Globals.gDeviceType
                            + "\r\nAndroid Version:\t" + Globals.gDeviceVersion
                            + "\r\nManufacturer:\t" + Globals.gDeviceManufacturer
                            + "\r\n\r\nS/N:\t" + Globals.DeviceSN;

            await _messageService.DisplayError("Device Info", _info, "dismiss");
        }

        private async  void OnLogoutClicked()
        {
            SelectedUserIndex = -1;
            App.IsUserLoggedIn = false;
            await Xamarin.Essentials.SecureStorage.SetAsync("isUserLogged", "0");
            Application.Current.MainPage = new LoginPage();
        }

        ObservableCollection<string> heciCodes = null;
        public ObservableCollection<string> HeciCodes
        {
            set { heciCodes = value; }
            get
            {
                heciCodes ??= new ObservableCollection<string>();
                return heciCodes;
            }
        }

        bool autoFocusSn = Globals.bAutoFocusSn;
        public bool AutoFocusSn
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (AutoFocusSn) >> " + value);
                SetProperty(ref autoFocusSn, value);
                Globals.bAutoFocusSn = autoFocusSn;
            }
            get { return autoFocusSn; }
        }

        bool overrideKeyboardlessEntry = Globals.bOverrideKeyboardlessEntry;
        public bool OverrideKeyboardlessEntry
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (OverrideKeyboardlessEntry) >> " + value);
                SetProperty(ref overrideKeyboardlessEntry, value);
                Globals.bOverrideKeyboardlessEntry = overrideKeyboardlessEntry;
            }
            get { return overrideKeyboardlessEntry; }
        }

        public bool testModeIsChecked;
        public bool TestModeIsChecked
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (TestModeIsChecked) >> " + value);

                SetProperty(ref testModeIsChecked, value);
                Globals.bTestMode = testModeIsChecked;
                Globals.gWS = eHelpDeskContext.GetWebServiceRef();

                GetTheme();
            }
            get { return testModeIsChecked; }
        }
        #endregion

    }
}
