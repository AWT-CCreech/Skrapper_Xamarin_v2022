using System;
using System.Collections;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Provider.Settings;

namespace Skrapper
{
    public class Globals
    {
        #region ********** Global Variables **********
        #region--:[DEVICE]:--
        public static Android.Content.Context _context = Android.App.Application.Context;
        public static string _sn = Secure.GetString(_context.ContentResolver, Secure.AndroidId);
        public static string DeviceSN = _sn.ToUpper();
        public static string gDeviceModel = DeviceInfo.Model.ToString();
        public static string gDeviceManufacturer = DeviceInfo.Manufacturer.ToString();
        public static string gDeviceName = DeviceInfo.Name.ToString();
        public static string gDeviceVersion = DeviceInfo.VersionString;
        public static string gDevicePlatform = DeviceInfo.Platform.ToString();
        public static string gDeviceType = DeviceInfo.DeviceType.ToString();
        #endregion


        #region--:[WEBSERVICE]:--
        //Main webservice declaration
        //ALL relevant get/set functions located in eHelpDeskContext.cs
        public static MyWebService.ScannerWebServiceSoapClient gWS = null;
        #endregion

        //[USER]
        public static string UsersList = null;
        public static string UserName = null;
        public static int unameIdx = -1;

        #region--:[SKID TAB]:--
        public static string SkidsList = null;
        public static string CarrierList = null;
        public static int pSkidIdx = -1;
        public static string pSkidItem = string.Empty;
        public static int pActionIdx = -1;
        public static string pActionItem = string.Empty;
        public static int pCarrierIdx = -1;
        public static string pCarrierItem = string.Empty;
        #endregion


        #region--:[SCAN TAB]:--
        public static int pPartNumIdx = -1; //pkrPartNum.SelectedIndex
        public static string pPartNumItem = null;

        public static string eAltPartNum = null;

        public static string eSerialNoText = null;
        public static string eHeciText = "";
        public static int rowID = 0;
        public static int iAdjID = 0;
        #endregion


        #region--:[INFO/HISTORY TAB]:--
        #endregion


        #region--:[SETTINGS TAB]:--
        //username
        public static int pUserIdx = -1; //pkrUser.SelectedIndex

        //checkboxes
        public static bool bAutoFocusSn = false;
        public static bool bOverrideKeyboardlessEntry = false;
        public static bool bTestMode = false;

        //debug
        public static string gLastScanResults = "";   // stores the last few scan results for debug purposes;
        public static string gLastScanData = "";      // ditto; accessed thru options/Debug screen
        #endregion
        #endregion
    }
}
