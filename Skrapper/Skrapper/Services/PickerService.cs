using Android.App;
using Android.Hardware.Biometrics;
using MyWebService;
using Skrapper.Models;
using Skrapper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Skrapper
{
    /// <summary>
    /// Main class for retrieving all ObservableCollection data for pickers. 
    /// </summary>
    public class PickerService : ViewModelBase
    {

        public static async Task<ObservableCollection<History>> LoadGridFromSkidNum(string skidNo, bool loadGrid, bool loadPartList)
        {
            ObservableCollection<History> result = new();
            if (loadGrid && skidNo.Length == 9)
            {
                try
                {
                    using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                    {
                        DataSet dataSet = new("SkidInfo");
                        //int count = 0;
                        dataSet = locWS.Skp_SkidGetInfo(skidNo);
                        var history = dataSet.Tables[0].AsEnumerable()
                            .Select(dataRow => new History
                            {
                                SkidNo = dataRow.Field<string>("SkidNo"),
                                PartNo = dataRow.Field<string>("PartNo"),
                                Qty = dataRow.Field<int>("Qty"),
                                SerialNo = dataRow.Field<string>("SerialNo"),
                                SerialNoB = dataRow.Field<string>("SerialNoB"),
                                HeciCode = dataRow.Field<string>("HeciCode"),
                                ParentID = dataRow.Field<int>("ParentID")
                            }).ToList();
                        result = new ObservableCollection<History>(history);

                        /*
                        foreach (DataTable dt in dataSet.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                Console.WriteLine("--------------------------count:" + count++);
                                foreach (var i in dr.ItemArray)
                                {
                                    Console.WriteLine(i);
                                }
                            }
                        }
                        */
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[ERROR: PickerService.cs] (LoadGridFromSkidNum) ex >> " + ex.ToString());
                    await _messageService.DisplayError("[ERROR: PickerService.cs]", "(LoadGridFromSkidNum)\r\n" + ex.Message, "dismiss");
                }
            }
            return result;
        }

        #region *** USER *** 
        /// <summary>
        /// Get all active usernames.
        /// </summary>
        /// <returns>(ObservableCollection<string>) usernames</returns>
        public static async Task<ObservableCollection<string>> GetUserPickerList()
        {

            List<string> users;
            Task<string> ul;
            char[] delims = new char[] { ',' };
            string[] sList;
            ObservableCollection<string> result = new();


            try
            {
                using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                {
                    ul = Task.Run(() => locWS.UsersGetList());
                    Globals.UsersList = ul.Result;
                    locWS.Close();
                }
            }
            catch (Exception ex)
            {
                await _messageService.DisplayError("[ERROR: PickerService.cs]", "(LoadUserPickList)\r\n" + ex.Message, "dismiss");
                Console.WriteLine("[ERROR: PickerService.cs] (LoadUserPickList) >> " + ex.ToString());
            }


            string s = Globals.UsersList;
            Console.WriteLine("[PickerService.cs] (LoadUserPickList) s >> " + s);

            sList = s.Split(delims);
            users = sList.ToList();

            result = new ObservableCollection<string>(users);

            return result;
        }
        #endregion

        #region *** SKID ***
        /// <summary>
        /// Get all active skrapper skid numbers for past two months.
        /// </summary>
        /// <returns>(ObservableCollection<string>) skids</returns>
        public static async Task<ObservableCollection<string>> GetSkidNumbers()
        {
            List<string> skids;
            Task<string> skn;
            char[] delims = new char[] { ',' };
            string[] sList;
            ObservableCollection<string> result = new();

            if (eHelpDeskContext.WebServiceConnected())
            {
                try
                {
                    using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                    {
                        //string sList = ws.Skp_SkidListGetAll();	// returns a comma delimited list of all skid number
                        // update Nov 7, 2012 - just get last 2 months of skids
                        DateTime dt = DateTime.Today.AddDays(-60);

                        skn = Task.Run(() => locWS.Skp_SkidListGetAfterDate(dt));
                        Globals.SkidNumberList = skn.Result;
                        locWS.Close();
                    }
                }
                catch (Exception ex)
                {
                    await _messageService.DisplayError("[ERROR: PickerService.cs]", "(GetSkidNumbers)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: PickerService.cs] (GetSkidNumbers) >> " + ex.ToString());
                }
            }
            string s = Globals.SkidNumberList;
            Console.WriteLine(Globals.SkidNumberList);
            sList = s.Split(delims);
            skids = sList.ToList();

            result = new ObservableCollection<string>(skids);

            return result;
        }

        /// <summary>
        /// Get all part types.
        /// </summary>
        /// <returns>(ObservableCollection<string>) partTypes</returns>
        public static async Task<ObservableCollection<string>> GetPartTypes()
        {
            List<string> types;
            Task<string> ts;
            char[] delims = new char[] { ',' };
            string[] sList;
            ObservableCollection<string> result = new();
            if (eHelpDeskContext.WebServiceConnected())
            {
                try
                {
                    using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                    {
                        ts = Task.Run(() => locWS.Skp_SkidGetMetalPartTypes());
                        Globals.PartTypeList = ts.Result;
                        locWS.Close();
                    }
                }
                catch (Exception ex)
                {
                    await _messageService.DisplayError("[ERROR: PickerService.cs]", "(GetPartTypes)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: PickerService.cs] (GetPartTypes) >> " + ex.ToString());
                }
            }
            string s = Globals.PartTypeList;
            sList = s.Split(delims);
            types = sList.ToList();

            result = new ObservableCollection<string>(types);

            return result;
        }

        /// <summary>
        /// Get all skid types.
        /// </summary>
        /// <returns>(ObservableCollection<string>) skidTypes</returns>
        public static async Task<ObservableCollection<string>> GetSkidTypes()
        {
            List<string> types;
            Task<string> ts;
            char[] delims = new char[] { ',' };
            string[] sList;
            ObservableCollection<string> result = new();
            if (eHelpDeskContext.WebServiceConnected())
            {
                try
                {
                    using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                    {
                        ts = Task.Run(() => locWS.Skp_SkidGetSkidTypes());
                        Globals.SkidTypeList = ts.Result;
                        locWS.Close();
                    }
                }
                catch (Exception ex)
                {
                    await _messageService.DisplayError("[ERROR: PickerService.cs]", "(GetSkidTypes)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: PickerService.cs] (GetSkidTypes) >> " + ex.ToString());
                }
            }
            string s = Globals.SkidTypeList;
            sList = s.Split(delims);
            types = sList.ToList();

            result = new ObservableCollection<string>(types);

            return result;
        }

        /// <summary>
        /// Get all active carriers.
        /// </summary>
        /// <returns>(ObservableCollection<string>) carriers</returns>
        public static async Task<ObservableCollection<string>> GetCarriers()
        {
            List<string> carriers;
            Task<string> cns;
            char[] delims = new char[] { ',' };
            string[] sList;
            ObservableCollection<string> result = new();
            if (eHelpDeskContext.WebServiceConnected())
            {
                try
                {
                    using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                    {
                        cns = Task.Run(() => locWS.Skp_SkidGetCarrierList());
                        Globals.CarrierList = cns.Result;
                        locWS.Close();
                    }
                }
                catch (Exception ex)
                {
                    await _messageService.DisplayError("[ERROR: PickerService.cs]", "(GetCarriers)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: PickerService.cs] (GetCarriers) >> " + ex.ToString());
                }
            }
            string s = Globals.CarrierList;
            sList = s.Split(delims);
            carriers = sList.ToList();

            result = new ObservableCollection<string>(carriers);

            return result;
        }

        /// <summary>
        /// Get all locations.
        /// </summary>
        /// <returns>(ObservableCollection<string>) locations</returns>
        public static async Task<ObservableCollection<string>> GetLocations()
        {
            List<string> locations;
            Task<string> locs;
            char[] delims = new char[] { ',' };
            string[] sList;
            ObservableCollection<string> result = new();
            if (eHelpDeskContext.WebServiceConnected())
            {
                try
                {
                    using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                    {
                        locs = Task.Run(() => locWS.Skp_SkidGetLocationList());
                        Globals.LocationList = locs.Result;
                        locWS.Close();
                    }
                }
                catch (Exception ex)
                {
                    await _messageService.DisplayError("[ERROR: PickerService.cs]", "(GetLocations)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: PickerService.cs] (GetLocations) >> " + ex.ToString());
                }
            }
            string s = Globals.LocationList;
            sList = s.Split(delims);
            locations = sList.ToList();

            result = new ObservableCollection<string>(locations);

            return result;
        }
        #endregion
    }
}
