using MyWebService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper.Services
{
    public class UtilityService : MainViewModel
    {
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
        /// Method for reading in string and converting string into dictionary using delimiter.
        /// </summary>
        /// <param name="ParamString"></param>
        /// <param name="Delimiter"></param>
        /// <returns>(Dictionary<string, string>) d</returns>
        public static Dictionary<string, string> CreateDictionaryFromString(string ParamString, string Delimiter)
        {
            Dictionary<string, string> d = new(5);
            char[] delims = Delimiter.ToCharArray();
            string[] strs = ParamString.Split(delims);

            // now break apart each Key, value pair
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
        /// Method for checking if selected part number has carrier code in last 3 characters
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns>(string) result</returns>
        public static async Task<string> PartNumberCarrierCode(string inValue)
        {
            // if the part number's last 3 digits matches a carrier code
            // return the code, else return ""  (blank)
            string result = "";
            string curPartNo = inValue.ToUpper();
            try
            {
                // see if the part numbers are consigment parts
                if (curPartNo.Length > 3)
                {
                    string last3Digits = curPartNo.Substring((curPartNo.Length - 3), 3);
                    if (Globals.pCarrierItem.Contains(last3Digits))
                    {
                        // we found a match, so return the last 3 digits of partnum (the Code)
                        result = last3Digits;
                    }
                }
            }
            catch(Exception ex)
            {
                await _messageService.DisplayError("[ERROR: UtilityService.cs]","(PartNumberCarrierCode)\r\n ex >> " + ex.Message, "dismiss");
            }

            return result;
        }
    }
}
