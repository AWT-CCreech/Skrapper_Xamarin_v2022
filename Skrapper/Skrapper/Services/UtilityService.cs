using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper.Services
{
    public class UtilityService : MainViewModel
    {
        /// <summary>
        /// Async method that reads in part number as a string (inPart) 
        /// and cleans trailing space/tab characters and data occuring after space/tab.
        /// (i.e. reads in part number like "NTGA98BA 07" and cleans to "NTGA98BA.")
        /// </summary>
        /// <param name="inPart"></param>
        /// <returns>(string) result</returns>
        public static async Task<string> CleanPartNumber(string inPart)
        {
            // this function converts part# like "NTGA98BA 07" to "NTGA98BA"
            // or a part# like "NTM12345...GJ1" to "NTM12345"
            string result = inPart;
            int x = inPart.IndexOf(' ', 3); // start 4 chars in to mark sure we have a near valid part# first

            try
            {
                if (x > 0)
                {
                    // ignore the space and everything after it
                    result = inPart[..x];
                }
                else
                {
                    x = inPart.IndexOf("...", 3);
                    if (x > 0)
                        result = inPart[..x];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR: UtilityService.cs]", "(CleanPartNumber)\r\n" + ex.Message, "dismiss");
            }
            Console.WriteLine("[UtilityService.cs] (CleanPartNumber) result >> " + result);
            return result;
        }

        /// <summary>
        /// Async method for determining whether or not 
        /// last 3 chars of selectedPartNumber matches
        /// a carrier code.
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns>(string) carrierCode</returns>
        public static async Task<string> PartNumberCarrierCode(string inValue)
        {
            // if the part number's last 3 digits matches a carrier code
            // return the code, else return ""  (blank)
            string result = string.Empty;
            string curPartNo = inValue.ToUpper();
            try
            {
                // see if the part numbers are consigment parts
                if (curPartNo.Length > 3)
                {
                    string last3Digits = curPartNo.Substring((curPartNo.Length - 3), 3);
                    if (Globals.CarrierCodes.Contains(last3Digits))
                    {
                        // we found a match, so return the last 3 digits of partnum (Carrier Code)
                        result = last3Digits;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR: UtilityService.cs]", "(PartNumberCarrierCode)\r\n" + ex.Message, "dismiss");
            }


            return result;
        }

        /// <summary>
        /// Async method that reads in part number and checks if part number has TMO carrier code.
        /// </summary>
        /// <param name="inPart"></param>
        /// <returns>(bool) carrCode == "TMO"</returns>
        public static async Task<bool> PartNumberIsTMO(string inPart)
        {
            Task<string> ts;
            string carrCode = string.Empty;
            try
            {
                string pn = inPart.ToUpper().TrimEnd().TrimStart();
                ts = Task.Run(() => CleanPartNumber(pn));
                string iPn = ts.Result;
                ts = Task.Run(() => PartNumberCarrierCode(iPn));
                carrCode = ts.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR: UtilityService.cs]", "(PartNumberIsTMO)\r\n" + ex.Message, "dismiss");
            }

            return (carrCode == "TMO");
        }

        /// <summary>
        /// Async method that reads in selectedPartNumber and string entered into serial number entry.
        /// Determines whether or not part number was scanned into serial number entry.
        /// </summary>
        /// <param name="PnString"></param>
        /// <param name="SnString"></param>
        /// <returns>(bool) result</returns>
        public static async Task<bool> CheckIfPartNumScannedInSnField(string PnString, string SnString)
        {
            Task<string> ts;
            bool result = false;
            try
            {
                /************************************************
				 * update 5/9/2012 (Ryan) - try to detect when
				 * a part number is accidentally scanned and
				 * reset S/N field for another Scan try
				 ************************************************/
                string sn = SnString.ToUpper();
                ts = Task.Run(() => CleanPartNumber(sn.TrimEnd().TrimStart()));
                Console.WriteLine("[UtilityService.cs] (CheckIfPartNumScannedInSnField) ts.Result >> " + ts.Result);
                string SSNfld = ts.Result;
                string curPartNo = PnString.ToUpper();
                if (SSNfld.CompareTo(curPartNo) == 0)
                {
                    // S/N matches part number, miss scan
                    result = true;
                }
                else
                {
                    // see if the part numbers are consigment parts
                    if (curPartNo.Length > 3)
                    {
                        string last3Digits = curPartNo.Substring((curPartNo.Length - 3), 3);
                        if (Globals.CarrierCodes.Contains(last3Digits))
                        {
                            // we found a match, so remove the last 3 digits for comparison
                            curPartNo = curPartNo[..^3];
                        }
                    }
                    if (SSNfld.CompareTo(curPartNo) == 0)
                        result = true;
                }

                if (result)
                {
                    AddToResults("PartNum Scanned in S/N field. Rescan");
                    //LabelService.UpdateValidationText("P/N Scanned - Rescan S/N", Color.IndianRed);
                    result = false;
                    //StartSound(gSndPartNumScanInSn);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR: UtilityService.cs]", "(CheckIfPartNumScannedInSnField)\r\n" + ex.Message, "dismiss");
            }

            return result;
        }

        #region --: LOGGING :--
        /// <summary>
        /// Reads in a string and concats to existing Globals.gLastScanResults string.
        /// </summary>
        /// <param name="data"></param>
        public static void AddToResults(string data)
        {
            int x = Globals.gLastScanResults.Length;
            if (x > 1024)
            {
                // clear some of the text to save on memory
                Globals.gLastScanResults = Globals.gLastScanResults[513..x];
            }
            Globals.gLastScanResults += data + "\r\n";
        }
        /// <summary>
		/// Reads in a string and concats to existing Globals.gLastScanResults string.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="args"></param>
		public static void AddToResults(string data, params object[] args)
        {
            Task.Run(() => AddToResults(string.Format(data, args)));
        }

        /// <summary>
		/// Clears Globals.gLastScanResults
		/// </summary>
		public static void ClearResults()
        {
            Globals.gLastScanResults = string.Empty;
        }
        #endregion

        #region --: UTILITY :--
        /// <summary>
        /// Async method for determining whether or not input string is an integer.
        /// </summary>
        /// <param name="inVal"></param>
        /// <returns>(bool) result</returns>
        public static async Task<bool> IsStringInteger(string inVal)
        {
            bool result;

            try
            {
                int x = int.Parse(inVal);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR: UtilityService.cs]", "()\r\n" + ex.Message, "dismiss");
            }

            return result;
        }

        /// <summary>
        /// Async method for converting string to integer.
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="defaultVal"></param>
        /// <returns>(int) result</returns>
        public static async Task<int> ConvertStringToInt(string inVal, int defaultVal)
        {
            int result;
            try
            {
                result = int.Parse(inVal);
            }
            catch (Exception ex)
            {
                result = defaultVal;
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR: UtilityService.cs]", "()\r\n" + ex.Message, "dismiss");
            }
            return result;
        }

        /// <summary>
        /// Async method for converting string to bool
        /// </summary>
        /// <param name="value"></param>
        /// <returns>(bool) result</returns>
        public static async Task<bool> ConvertStringToBool(string value)
        {
            bool result = false;
            try
            {
                switch (value.ToLower())
                {
                    case "1":
                        result = true;
                        break;
                    case "true":
                        result = true;
                        break;
                    case "yes":
                        result = true;
                        break;
                    case "on":
                        result = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR: UtilityService.cs]", "()\r\n" + ex.Message, "dismiss");
            }

            return result;
        }

        /// <summary>
        /// Async method for converting bool to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>(string) result</returns>
        public static async Task<string> ConvertBoolToString(bool value)
        {
            string result = string.Empty;
            try
            {
                if (value)
                    result = "True";
                else
                    result = "False";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR: UtilityService.cs]", "()\r\n" + ex.Message, "dismiss");
            }

            return result;
        }

        /// <summary>
        /// Async method for retrieving count of delimited string.
        /// </summary>
        /// <param name="inValStr"></param>
        /// <param name="inDelimiter"></param>
        /// <returns>(int) gdAry.Length</returns>
        public static async Task<int> GetDelimitedStrCount(string inValStr, string inDelimiter)
        {
            char[] delimiter;
            string[] gdAry = null;

            try
            {
                delimiter = inDelimiter.ToCharArray();
                gdAry = inValStr.Split(delimiter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _messageService.DisplayError("[ERROR: UtilityService.cs]", "()\r\n" + ex.Message, "dismiss");
            }
            return gdAry.Length;
        }

        /// <summary>
        /// Method for filling object with specific data.
        /// </summary>
        /// <param name="cObj"></param>
        /// <param name="dataToFill"></param>
        /// <param name="clearList"></param>
        public static void FillListControl(object cObj, string dataToFill, bool clearList)
        {
            //System.Windows.Forms.ComboBox c = (System.Windows.Forms.ComboBox)cObj;  // this works also: c.Items.Add(ss);
            char[] delims = new char[] { ',' };
            string[] sList = dataToFill.Split(delims);
            if (clearList) (cObj as Picker).Items.Clear();
            foreach (string ss in sList)
                (cObj as Picker).Items.Add(ss);
        }

        /// <summary>
        /// Reads in a delimitted string and converts to dictionary object.
        /// </summary>
        /// <param name="ParamString"></param>
        /// <param name="Delimiter"></param>
        /// <returns></returns>
        public static Dictionary<string, string> CreateDictionaryFromString(string ParamString, string Delimiter)
        {
            Dictionary<string, string> d = new(5);
            char[] delims = Delimiter.ToCharArray();
            string[] strs = ParamString.Split(delims);

            // now break apart each Key/value pair
            foreach (string s in strs)
            {
                int pos = s.IndexOf('=');
                if (pos >= 0)
                {
                    string K = s[..pos];
                    string V = s.Substring(pos + 1, s.Length - pos - 1);
                    d.Add(K, V);
                }
                else
                {
                    // no value found, so add as empty
                    d.Add(s, "");
                }
            }
            return d;
        }

        /// <summary>
        /// Reads in two string values and removes second string from end of first string.
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="NameOrCode"></param>
        /// <returns>(string) result</returns>
        public static string CleanConsigneeString(string inValue, string NameOrCode)
        {
            string result = inValue;
            int p = inValue.IndexOf(" - ");
            if (NameOrCode == Constants.NAME)
            {
                if (p > 2)
                {
                    result = inValue[(p + 3)..];
                }
                if (result.StartsWith("AirWay"))
                    result = "AirWay";
            }
            else
            {
                if (p > 2)
                {
                    result = inValue[..3];
                }
            }
            return result;
        }
        #endregion
    }
}
