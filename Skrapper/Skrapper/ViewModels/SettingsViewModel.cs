using MyWebService;
using Skrapper.Services;
using Skrapper.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Android.Webkit.ConsoleMessage;

namespace Skrapper
{
    public class SettingsViewModel : MainViewModel
    {
        public IMessageService _messageService = new MessageService();

        #region *** SETTINGS TAB *****************************************************************************
        public Command LogoutCommand { private set; get; }
        public Command CheckServerCommand { private set; get; }
        public Command DebugResultsCommand { private set; get; }
        public Command ShowDeviceInfoCommand { private set; get; }

        public SettingsViewModel()
        {
            Title = "SETTINGS";


            DebugResultsCommand = new Command(
                execute: async () =>
                {
                    if (Globals.gLastScanData.Length > 0 || Globals.gLastScanResults.Length > 0)
                        await _messageService.DisplayError("Debug", "gLastScanResults...\r\n" + Globals.gLastScanResults + "\r\n\r\ngLastScanData...\r\n" + Globals.gLastScanData, "dismiss");
                    else
                        return;
                });

            CheckServerCommand = new Command(
                execute: async () =>
                {
                    Task<string> ts;
                    string s, t;
                    try
                    {
                        using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                        {
                            if (!eHelpDeskContext.WebServiceConnected()) return;

                            ts = Task.Run(() => locWS.Endpoint.Address.ToString());
                            s = ts.Result;
                            ts = Task.Run(() => locWS.GetServerURL());
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
                });

            ShowDeviceInfoCommand = new Command(
                execute: async () =>
                {
                    string _info = "Model: " + Globals.gDeviceModel
                            + "\r\nName: " + Globals.gDeviceName
                            + "\r\nPlatform: " + Globals.gDevicePlatform
                            + "\r\nType: " + Globals.gDeviceType
                            + "\r\nVersion: " + Globals.gDeviceVersion
                            + "\r\nManufacturer: " + Globals.gDeviceManufacturer
                            + "\r\n\r\n S/N: " + Globals.DeviceSN;

                    await _messageService.DisplayError("Device Info", _info, "dismiss");
                });
        }

        int selectedUserIndex = Globals.pUserIdx;
        public int SelectedUserIndex
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (SelectedUserIndex) >> " + value);
                SetProperty(ref selectedUserIndex, value);
                Globals.pUserIdx = selectedUserIndex;
            }
            get { return selectedUserIndex; }

        }

        string selectedUser = Globals.UserName;
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

        bool testModeIsChecked;
        public bool TestModeIsChecked
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (TestModeIsChecked) >> " + value);

                SetProperty(ref testModeIsChecked, value);
                Globals.bTestMode = testModeIsChecked;
                Globals.gWS = eHelpDeskContext.GetWebServiceRef();

                Theme theme;
                if (!testModeIsChecked)
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
                        default:
                            mergedDictionaries.Add(new LiveTheme());
                            break;
                    }
                }

                //SelectedOrderTypeIndex = -1;
            }
            get { return testModeIsChecked; }
        }
        #endregion

    }
}
