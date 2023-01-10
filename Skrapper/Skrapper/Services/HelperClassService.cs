using Java.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Skrapper
{
    public class Beeps : ArrayList { }

    /// <summary>
    /// Store scan beep frequency and time.
    /// </summary>
    public class Beep 
    {
        private int mTime, mFreq;

        public Beep(int t, int f)
        {
            mTime = t;
            mFreq = f;
        }

        public Beep(string t, string f)
        {
            mTime = Convert.ToInt32(t);
            mFreq = Convert.ToInt32(f);
        }

        public int Time
        {
            get { return mTime; }
            set { mTime = value; }
        }

        public int Freq
        {
            get { return mFreq; }
            set { mFreq = value; }
        }

        public override string ToString()
        {
            return Convert.ToString(mTime) + ", " + Convert.ToString(mFreq);
        }
    }

    /// <summary>
    /// Store scan data being submitted to ScannerWebService.
    /// Allow for recall of IDs from SQL table, which allows
    /// for quicker delete of previous scan.
    /// </summary>
    public class SubmitHistory
    {
        private int mScanHistID, mTrkAdjID;
        private string mOrderType;
        private string mOrderNum;
        private string mScanUser;
        private string mPartNum;
        private string mSerNum;
        private string mStatDesc;
        private string msTime;

        public SubmitHistory()
        {
            msTime = DateTime.Now.TimeOfDay.ToString();
            mScanHistID = 0;
            mTrkAdjID = 0;
            mOrderType = "";
            mOrderNum = "";
            mScanUser = "";
            mPartNum = "";
            mSerNum = "";
            mStatDesc = "";
        }

        public SubmitHistory(string sStatusDesc, int ScanHistID, int TrkAdjId)
        {
            msTime = DateTime.Now.TimeOfDay.ToString();
            mStatDesc = sStatusDesc;
            mScanHistID = ScanHistID;
            mTrkAdjID = TrkAdjId;
            mOrderType = "";
            mOrderNum = "";
            mScanUser = "";
            mPartNum = "";
            mSerNum = "";
        }

        public int ScanHistoryID
        {
            get { return mScanHistID; }
            set { mScanHistID = value; }
        }

        public int TrkAdjustmentID
        {
            get { return mTrkAdjID; }
            set { mTrkAdjID = value; }
        }

        public string OrderType
        {
            get { return mOrderType; }
            set { mOrderType = value; }
        }

        public string OrderNum
        {
            get { return mOrderNum; }
            set { mOrderNum = value; }
        }

        public string ScanUser
        {
            get { return mScanUser; }
            set { mScanUser = value; }
        }

        public string PartNum
        {
            get { return mPartNum; }
            set { mPartNum = value; }
        }

        public string SerialNum
        {
            get { return mSerNum; }
            set { mSerNum = value; }
        }

        public string Description
        {
            get { return mStatDesc; }
            set { mStatDesc = value; }
        }

        public string ShowAllInfo()
        {
            string s = "";
            //if (msTime.Length > 5) s = msTime.Substring(0, 5);
            if (msTime.Length > 5) s = msTime[..5];

            return (mOrderType + mOrderNum
                + "\nPart# " + mPartNum
                + "\nS/N: " + mSerNum
                + "\nUser: " + mScanUser
                + "\nHistID: " + mScanHistID.ToString()
                + "\nAdjID: " + mTrkAdjID.ToString()
                + "\ntime: " + s
            );
        }

        public override string ToString()
        {
            string s = "";
            if (mScanUser.Length > 2)
                s = mScanUser[..2];
            s += "[";
            if (msTime.Length > 5)
                s += msTime[..5];
            s += "]" + mSerNum + " " + mStatDesc;
            return s;
        }
    }
}
