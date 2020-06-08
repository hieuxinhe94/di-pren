using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.CardPayment.Autowithdrawal
{
    /// <summary>
    /// Get subs for given subsno, then get sub with top Extno.
    /// </summary>
    [Serializable]
    public class RenewalSubscription
    {
        public long Subsno { get; set; }
        public int Extno { get; set; }
        public string CancelReason { get; set; }
        public string SubsState { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public RenewalSubscription(long subsno, int extno, string cancelReason, string subsState, DateTime startDate, DateTime endDate)
        {
            Subsno = subsno;
            Extno = extno;
            CancelReason = cancelReason;
            SubsState = subsState;
            StartDate = startDate;
            EndDate = endDate;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<hr><b>RenewalSubscription</b><br>");
            sb.Append("Subsno: " + Subsno + "<br>");
            sb.Append("Extno: " + Extno + "<br>");
            sb.Append("CancelReason: " + CancelReason + "<br>");
            sb.Append("SubsState: " + SubsState + "<br>");
            sb.Append("StartDate: " + StartDate.ToString() + "<br>");
            sb.Append("EndDate: " + EndDate.ToString() + "<hr>");
            return sb.ToString();
        }
    }
}
