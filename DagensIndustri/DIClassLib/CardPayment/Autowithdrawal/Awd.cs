using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.Subscriptions.DiPlus;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.Misc;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions;


namespace DIClassLib.CardPayment.Autowithdrawal
{

    [Serializable]
    public class Awd
    {
        public string AurigaSubsId { get; set; }
        public int Cusno { get; set; }
        public int Subsno { get; set; }
        public int Campno { get; set; }
        public Guid PageGuid { get; set; }
        public DateTime DateSubsEnd { get; set; }
        public double PriceExVat { get; set; }
        public double PriceIncVat { get; set; }
        public double VatAmount { get; set; }
        public RenewalSubscription RenewSub = null;
        public Subscription Sub = null;



        public Awd(string aurigaSubsId, int cusno, int subsno, int campno, Guid pageGuid, DateTime dateSubsEnd, List<Subscription> subs)
        {
            AurigaSubsId = aurigaSubsId;
            Cusno = cusno;
            Subsno = subsno;
            Campno = campno;
            PageGuid = pageGuid;
            DateSubsEnd = dateSubsEnd;

            RenewSub = GetRenewSub();

            Sub = TryGetSubFromSubs(subs);
            if (Sub == null)
            {
                Sub = new Subscription(string.Empty, Campno, PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal, DateTime.Now, false);
                subs.Add(Sub);
            }
            if (RenewSub != null)
            {
                Sub.SubsStartDate = SubscriptionController.GetNextIssuedate(Sub.PaperCode, Sub.ProductNo, RenewSub.EndDate.AddDays(1));
            }
            
            Sub.SetSubsDateMembers(true);
            SetPrices();
        }

        private Subscription TryGetSubFromSubs(List<Subscription> subs)
        {
            foreach (Subscription sub in subs)
            {
                if (sub.CampNo == Campno)
                    return sub;
            }

            return null;
        }

        private void SetPrices()
        {
            double vatPct = SubscriptionController.GetProductVat(Sub.PaperCode, Sub.ProductNo);
            PriceCalculator pc = new PriceCalculator(null, Sub.TotalPriceExVat, vatPct, 1);
            PriceExVat = (double)pc.PriceExVat;
            PriceIncVat = (double)pc.PriceIncVat;
            VatAmount = (double)pc.VatAmount;
        }

        private RenewalSubscription GetRenewSub()
        {
            RenewalSubscription reSub = null;

            var ds = SubscriptionController.GetSubscriptions(Cusno, true);
            if (!DbHelpMethods.DataSetHasRows(ds))
            {
                return null;
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var sn = int.Parse(dr["SUBSNO"].ToString());
                if (sn != Subsno)
                {
                    continue;
                }
                var extno = 0;
                int.TryParse(dr["EXTNO"].ToString(), out extno);

                if (reSub != null && reSub.Extno >= extno)
                {
                    continue;
                }
                var cancelReason = dr["CANCELREASON"].ToString().Trim();
                var subsState = dr["SUBSSTATE"].ToString();
                var startDate = DateTime.Parse(dr["SUBSSTARTDATE"].ToString());
                var endDate = DateTime.Parse(dr["SUBSENDDATE"].ToString());
                //var packageId = dr["PACKAGEID"].ToString();
                reSub = new RenewalSubscription(sn, extno, cancelReason, subsState, startDate, endDate);
            }
            return reSub;

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<hr><b>Awd</b><br>");
            sb.Append("AurigaSubsId: " + AurigaSubsId + "<br>");
            sb.Append("Cusno: " + Cusno + "<br>");
            sb.Append("Subsno: " + Subsno + "<br>");
            sb.Append("Campno: " + Campno + "<br>");
            sb.Append("DateSubsEnd: " + DateSubsEnd.ToString() + "<br>");
            sb.Append("PriceExVat: " + PriceExVat + "<br>");
            sb.Append("PriceIncVat: " + PriceIncVat + "<br>");
            sb.Append("VatAmount: " + VatAmount + "<br>");
            
            if (RenewSub == null)
                sb.Append("RenewSub: NULL<br>");
            else
                sb.Append(RenewSub.ToString());

            if (Sub == null)
                sb.Append("Sub: NULL<br>");
            else
                sb.Append(Sub.ToString());

            return sb.ToString();
        }

    }
}
