using Xamarin.Forms;

namespace Skrapper.Services
{
    public class UtilityService : ViewModelBase
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
    }
}
