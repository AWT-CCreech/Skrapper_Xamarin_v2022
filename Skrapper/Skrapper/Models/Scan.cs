using System;

namespace Skrapper.Models
{
    public class Scan
    {
        public int rowID { get; set; }
        public int iAdjID { get; set; }
        //public string DeviceID { get; set; }
        //public string OrderType { get; set; }
        //public string OrderNum { get; set; }
        //public string MNSCompany { get; set; }
        public string PartNum { get; set; }
        public string SerialNumber { get; set; }
        //public string Heci { get; set; }
        public string UserName { get; set; }
        public DateTime ScanDate { get; set; }
    }
}
