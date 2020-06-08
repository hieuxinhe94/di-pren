using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.Misc;


namespace DIClassLib.EPiJobs.SyncSubs
{
    public class Subscription
    {
        public int Subsno;
        public string ProductNo;
        public string PaperCode;
        public string PackageId;
        public DateTime ExpireDate;
        public bool SubsActive;


        public bool ShouldBeSaved
        {
            get
            {
                if (!SubsActive || ExpireDate.Date < DateTime.Now.Date)
                    return false;

                if (PaperCode == Settings.PaperCode_DI || PaperCode == Settings.PaperCode_DISE || PaperCode == Settings.PaperCode_IPAD || PaperCode == Settings.PaperCode_DIY || PaperCode == Settings.PaperCode_AGENDA)
                    return true;

                return false;
            }
        }


        public Subscription(int subsno, string productNo, string paperCode, DateTime expireDate, bool subsActive)
        {
            Subsno = subsno;
            ProductNo = productNo;
            PaperCode = paperCode;
            ExpireDate = expireDate;
            SubsActive = subsActive;
        }

        public Subscription(int subsno, string packageId, string productNo, string paperCode, DateTime expireDate, bool subsActive)
        {
            Subsno = subsno;
            PackageId = packageId;
            ProductNo = productNo;
            PaperCode = paperCode;
            ExpireDate = expireDate;
            SubsActive = subsActive;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Subsno: " + Subsno + Environment.NewLine);
            sb.Append("PackageId: " + PackageId + Environment.NewLine);
            sb.Append("ProductNo: " + ProductNo + Environment.NewLine);
            sb.Append("PaperCode: " + PaperCode + Environment.NewLine);
            sb.Append("ExpireDate: " + ExpireDate + Environment.NewLine);
            sb.Append("SubsActive: " + SubsActive + Environment.NewLine);
            sb.Append("ShouldBeSaved: " + ShouldBeSaved + Environment.NewLine);

            return sb.ToString();
        }
    }
}
