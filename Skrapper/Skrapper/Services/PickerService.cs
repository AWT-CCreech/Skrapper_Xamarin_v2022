using MyWebService;
using Skrapper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Skrapper
{
    public class PickerService : ViewModelBase
    {
        public static IMessageService _messageService = new MessageService();
        #region *** USER *** 
        /// <summary>
        /// Async method for loading all usernames for login page and settings tab.
        /// </summary>
        /// <returns>(ObservableCollection<string>) usernames</returns>
        public static async Task<ObservableCollection<string>> LoadUserPickerList()
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
            catch (Exception e)
            {
                await _messageService.DisplayError("[ERROR: PickerService.cs]", "(LoadUserPickList)\r\n" + e.Message + "\r\n" + e.ToString(), "dismiss");
                Console.WriteLine("[ERROR: PickerService.cs] (LoadUserPickList) e.ToString() >> " + e.ToString());
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
                        Globals.SkidsList = skn.Result;
                        locWS.Close();
                    }
                }
                catch (Exception ex)
                {
                    await _messageService.DisplayError("[ERROR: PickerService.cs]", "(GetSkidNumbers)\r\n..." + ex.Message, "dismiss");
                }
            }
            string s = Globals.SkidsList;
            sList = s.Split(delims);
            skids = sList.ToList();

            result = new ObservableCollection<string>(skids);

            return result;
        }
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
                    Console.WriteLine("[ERROR: PickerService.cs] (GetCarriers) >> " + ex.ToString());
                }
            }
            string s = Globals.CarrierList;
            sList = s.Split(delims);
            carriers = sList.ToList();

            result = new ObservableCollection<string>(carriers);

            return result;
        }
        #endregion
    }
}
