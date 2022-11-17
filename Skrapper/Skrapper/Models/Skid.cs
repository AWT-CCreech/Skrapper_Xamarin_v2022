using System;
using System.Collections.ObjectModel;

namespace Skrapper.Models
{
    public class Skid
    {
        #region Singleton Pattern
        private Skid() 
        {
        }

        public static Skid Instance { get; } = new Skid();
        #endregion

        private ObservableCollection<string> _skids;
        public ObservableCollection<string> Skids
        {
            get { return _skids; }
            set { _skids = value; }
        }
    }
}
