using MyWebService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Skrapper.Services
{
    public class ScanService
    {
        public static IMessageService _messageService = new MessageService();

        #region --: Duplicate SN Checker Function :--
        /// <summary>
        /// Async method that determines whether scanned serial number has previously been scanned for working order.
        /// </summary>
        /// <param name="SerNum"></param>
        /// <param name="OrderType"></param>
        /// <param name="OrderNum"></param>
        /// <returns>(string) dupeStatus</returns>
        public static async Task<string> CheckForDupeSerialNo(string SerNum, string OrderType, string OrderNum)
        {
            string dupeStatus = "NoSN";
            string SoNum = "", sNotes = "";

            Task<string> ts;
            Task<int> ti;

            if (OrderType.EndsWith("#"))
                OrderType = OrderType.Remove(OrderType.Length - 1, 1);

            if (string.IsNullOrEmpty(SerNum))
            {
                return dupeStatus;
            }
            else
            {
                try
                {
                    if (!eHelpDeskContext.WebServiceConnected()) return "Error";

                    string PnVerify = string.Empty;
                    using (ScannerWebServiceSoapClient SnCheck = eHelpDeskContext.GetWebServiceRef())
                    {
                        try
                        {
                            ts = Task.Run(() => SnCheck.VerifySerialNumber_V2(SerNum, OrderType, OrderNum));
                            PnVerify = ts.Result;
                            SnCheck.Close();
                            Console.WriteLine("[ScanService.cs] (CheckForDupeSerialNo) PnVerify >> " + PnVerify);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[ERROR: ScanService.cs] (CheckForDupeSerialNo) VerifySerialNumber_V2 >> " + ex.Message + "\r\n" + ex.ToString());
                        }
                    }
                    UtilityService.AddToResults("[ScanService.cs] (CheckForDupeSerialNo) PnVerify >> {0}", PnVerify);

                    if (PnVerify.ToLower() != "not found")
                    {
                        // return value format is: "OrderType:SO*OrderNum:57879*ID:101851*Notes:!HOT!"
                        ti = Task.Run(() => UtilityService.GetDelimitedStrCount(PnVerify, "*"));
                        int i = ti.Result;
                        if (i > 0)
                        {

                            char[] delims = new char[] { '*' };
                            string[] gdAry = PnVerify.Split(delims);
                            int aLen = gdAry.Length;
                            await Task.Run(() => UtilityService.AddToResults("\raLen={0}", aLen.ToString()));
                            if (aLen >= 4)  // this array should have 4 elements as the above example show
                            {
                                string OrdType = gdAry[0].Remove(0, 10);
                                SoNum = gdAry[1].Remove(0, 9);
                                string sId = gdAry[2].Remove(0, 3);
                                sNotes = gdAry[3].Remove(0, 6);
                                await Task.Run(() => UtilityService.AddToResults("Notes={0}", sNotes));
                            }

                            if (sNotes.IndexOf("HOT!") >= 0)
                                dupeStatus = "*HOT*";
                            else if (SoNum == OrderNum)
                                dupeStatus = "DUPE";
                        }
                    }
                    else
                        await Task.Run(() => UtilityService.AddToResults("no *\r"));

                    // if else - // Serial Number not found in DB, so no dupes found
                }
                catch (Exception ec)
                {
                    string s = "Exception in CheckForDupeSerialNo";
                    await Task.Run(() => UtilityService.AddToResults(s));
                    await Task.Run(() => UtilityService.AddToResults(ec.ToString()));
                    Console.WriteLine(ec.ToString());
                    await _messageService.DisplayError("Error", s + ec.ToString(), "dismiss");
                }
            }

            return dupeStatus;
        }
        #endregion

    }
}
