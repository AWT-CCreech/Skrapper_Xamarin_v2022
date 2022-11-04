using MyWebService;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper.Services
{
    /// <summary>
    /// LABEL SERVICE - methods for manipulation of Label Objects.
    /// </summary>
    public class LabelService
    {
        public static IMessageService _messageService = new MessageService();

        /// <summary>
        /// Asynchronous operation for returning specified string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>(string) res: specified string</returns>
        public static async Task<string> DoStrReturn(string s)
        {
            string res = string.Empty;
            try
            {
                res = s;
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR]", "Error while clearing string.", "dismiss");
            }
            return res;
        }

        /// <summary>
        /// Asynchronous operation for returning specified integer.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>(int) res: spcified integer</returns>
        public static async Task<int> DoIntReturn(int i)
        {
            int res = 0;
            try
            {
                res = i;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR]", "Error while returning integer.", "dismiss");
            }
            return res;
        }

        /// <summary>
        /// Reads in selectedPartNumber (AWT Part Number) and returns Alt Part Number (MFG Part Number).
        /// </summary>
        /// <param name="pn">awt part number</param>
        /// <returns string="result">mfg part number</returns>
        public static async Task<string> GetAltPartNumber(string pn)
        {
            string result = null;
            Task<string> ts;
            try
            {
                using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                {
                    ts = Task.Run(() => locWS.GetAltPartNumber(pn));
                    result = ts.Result;
                    locWS.Close();
                }
            }
            catch(Exception ex)
            {
                await _messageService.DisplayError("[ERROR: LabelService.cs]", "(GetAltPatrNumber)\r\n" + ex.Message, "dismiss");
                Console.WriteLine("[ERROR: LabelService.cs] (GetAltPartNumber) >> " + ex.ToString());
            }
            
            if (string.IsNullOrEmpty(result.ToString()))
                result = string.Empty;
            return result;
        }
        
        /// <summary>
        /// Sets globals params for Validation Text and Validation Text Color.
        /// </summary>
        /// <param name="newStatus">String: To be displayed</param>
        /// <param name="txtClr">Color: Color.IndianRed deems the scan as bad, and will not update Scan Status area.</param>
        public static void UpdateValidationText(string newStatus, Color txtClr)
        {
            Console.WriteLine("[LabelService.cs] (SetValidationText)\r\nnewStatus >> " + newStatus + "\r\ntxtClr >> " + txtClr.ToString());

            if (Globals.bTestMode)
            {
                Globals.gValidStatusTextBGColor = Globals.gAirWayTest;
                Globals.gValidStatusTextColor = txtClr;
                Globals.gValidStatus = newStatus;
            }
            else
            {
                Globals.gValidStatusTextBGColor = Globals.gAirWayLive;
                Globals.gValidStatusTextColor = txtClr;
                Globals.gValidStatus = newStatus;
            }

        }
    }
}
