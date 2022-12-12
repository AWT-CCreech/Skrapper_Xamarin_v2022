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

        public static Color gAirWayLive = Color.FromHex("#e8e8e8");
        public static Color gAirWayTest = Color.FromHex("#99afc1");
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
        public static string SkidNumberList = null;
        public static string PartTypeList = null;
        public static string SkidTypeList = null;
        public static string CarrierList = null;
        public static string LocationList = null;

        public static int pSkidIdx = -1;
        public static string pSkidItem = "";

        public static int pActionIdx = 0;
        public static string pActionItem = "ADD";

        public static int pCarrierIdx = -1;
        public static string pCarrierItem = "";

        public static string lblSkidPrint = string.Empty;
        #endregion


        #region--:[SCAN TAB]:--
        public static bool showCompleteSkidWarningAnswer = false;

        public static string PartsList = string.Empty;

        public static int pPartNumIdx = -1; //pkrPartNum.SelectedIndex
        public static string pPartNumItem = "";

        public static int pPartTypeIdx = -1; //31
        public static string pPartTypeItem = ""; //"Telecom - General"

        public static int ePartQuantity = 1;

        public static bool cbAuto = true;
        public static string eSerialNoText = null;

        public static string cdi = null;
        public static string gStatus = ""; //lblStatus
        public static string gValidate = null; //lblValidate

        public static string gValidStatus = null; //labValidStatus
        public static Color gValidStatusTextBGColor;
        public static Color gValidStatusTextColor = Color.IndianRed;

        public static int gCntScanForOrderType = 0;
        public static int gCntScanForOrderNum = 0;
        public static int gRemaining = 0;

        public static int rowID = 0;
        public static int iAdjID = 0;
        #endregion


        #region--:[INFO/HISTORY TAB]:--
        public static string dgSelectedScanItem = string.Empty;
        public static string eNoteText = "";
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
