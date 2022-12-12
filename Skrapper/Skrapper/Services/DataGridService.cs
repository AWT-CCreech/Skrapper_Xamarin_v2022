using Java.Sql;
using MyWebService;
using Skrapper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Skrapper.Services
{
    public class DataGridService : ViewModelBase
    {
        /// <summary>
        /// Async method for retrieveing all skid data for skid number.
        /// </summary>
        /// <param name="skidNo"></param>
        /// <param name="loadGrid"></param>
        /// <param name="loadPartList"></param>
        /// <returns>(ObservableCollection<Hitory>) result</returns>
        public static async Task<ObservableCollection<History>> LoadGridFromSkidNum(string skidNo)
        {
            ObservableCollection<History> result = new();
            if (skidNo.Length == 9)
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
                                //rowID = dataRow.Field<int>("rowID"),
                                SkidNo = dataRow.Field<string>("SkidNo"),
                                PartNo = dataRow.Field<string>("PartNo"),
                                Qty = dataRow.Field<int>("Qty"),
                                SerialNo = dataRow.Field<string>("SerialNo"),
                                SerialNoB = dataRow.Field<string>("SerialNoB"),
                                HeciCode = dataRow.Field<string>("HeciCode"),
                                ParentID = dataRow.Field<int>("ParentID")
                            }).ToList();
                        result = new ObservableCollection<History>(history);
                        Console.WriteLine("[PickerService.cs] (LoadGridFromSkidNum) result.Count >> " + result.Count.ToString());
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

        public static async Task<ObservableCollection<string>> LoadPartListFromSkidNum(string skidNo)
        {
            List<string> parts;
            Task<string> pl;
            char[] delims = new char[] { ',' };
            string[] pList;
            ObservableCollection<string> result = new();
            if (skidNo.Length == 9)
            {
                try
                {
                    using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                    {
                        pl = Task.Run(() => locWS.Skp_SkidGetPartNumbers(skidNo));
                        Globals.PartsList = pl.Result;
                        locWS.Close();
                    }
                }
                catch (Exception ex)
                {
                    await _messageService.DisplayError("[ERROR: DataGridService.cs]", "(LoadPartListFromSkidNum)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: DataGridService.cs] (LoadPartListFromSkidNum) ex >> " + ex.ToString());
                }
                string s = Globals.PartsList;
                Console.WriteLine("[DataGridService.cs] (LoadPartListFromSkidNum) s >> " + s);

                pList = s.Split(delims);
                parts = pList.ToList();

                result = new ObservableCollection<string>(parts);

            }
            return result;
        }
    }
}
