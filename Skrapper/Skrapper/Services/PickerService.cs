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
    }
}
