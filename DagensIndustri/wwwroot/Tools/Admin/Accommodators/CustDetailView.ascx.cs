using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Membership;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.CirixMappers;

namespace DagensIndustri.Tools.Admin.Accommodators
{
    public partial class CustDetailView : System.Web.UI.UserControl
    {
        public SubscriptionUser2 Subscriber
        {
            get
            {
                if (ViewState["Subscriber"] != null)
                    return (SubscriptionUser2)ViewState["Subscriber"];

                return null;
            }
            set
            {
                ViewState["Subscriber"] = value;
            }
        }

        private List<long> _subscribersSubsnos = null;
        public List<long> SubscribersSubsnos
        {
            get
            {
                if (_subscribersSubsnos == null)
                {
                    var cns = new HashSet<long>();
                    foreach (var sub in Subscriber.SubsActiveOrPassive)
                        cns.Add(sub.Subsno);

                    _subscribersSubsnos = cns.ToList();
                }

                return _subscribersSubsnos;
            }
        }

        public DateTime CirixMaxDate { get { return new DateTime(1800, 1, 1); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            ClearOverviewFields();

            if (Subscriber == null)
                this.Visible = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Subscriber != null)
            {
                PrintNameAndAddress();
                PrintTempAddress();
                PrintSubs();
                PrintInvoices();
            }
        }

        private void PrintNameAndAddress()
        {
            string r1 = Subscriber.RowText1;
            string r2 = Subscriber.RowText2;
            if (!string.IsNullOrEmpty(r2))
                r1 += "<br>" + r2;

            LabelCustName.Text = r1;


            var sb = new StringBuilder();
            sb.Append(Subscriber.StreetName);

            string hn = Subscriber.HouseNo;
            if (!string.IsNullOrEmpty(hn))
                sb.Append(" " + hn);

            string sc = Subscriber.Staricase;
            if (!string.IsNullOrEmpty(sc))
                sb.Append(" " + sc);

            string zip = Subscriber.Zip;
            if (!string.IsNullOrEmpty(zip))
                sb.Append(", " + zip);

            string pn = Subscriber.PostName;
            if (!string.IsNullOrEmpty(pn))
                sb.Append(" " + pn);

            string coAndApNo = Subscriber.Street2;
            if (!string.IsNullOrEmpty(coAndApNo))
            {
                sb.Append("<br>");
                if (!coAndApNo.StartsWith("LGH"))
                    sb.Append("C/O ");

                sb.Append(coAndApNo);
            }

            LabelAddress.Text = sb.ToString();
            LabelAddress.Visible = true;
        }

        private void PrintTempAddress()
        {
            var addresses = new List<AddressMap>();

            var subnos = new List<long>();
            foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
            {
                if (subnos.Contains(sub.Subsno))
                    continue;

                subnos.Add(sub.Subsno);
                addresses.AddRange(sub.TempAddresses);
            }

            if (addresses.Count > 1)
                addresses.Sort();

            StringBuilder sb = new StringBuilder();
            foreach (AddressMap am in addresses)
                sb.Append(am.StartDate.ToString("yyyy-MM-dd") + " - " + am.EndDate.ToString("yyyy-MM-dd") + " " + am.Street1 + " " + am.Street2 + "<br>");

            LabelTempAddress.Text = sb.ToString();
            LabelTempAddress.Visible = true;
        }

        private void PrintSubs()
        {
            if (Subscriber.SubsActiveOrPassive.Count > 0)
            {
                var sb = new StringBuilder();
                sb.Append("<table border=0 cellspacing=0 cellpaddning=0>");
                sb.Append("<tr>");
                sb.Append("<td class='small'>PRENNR</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>START</td>");
                sb.Append("<td></td>");
                sb.Append("<td class='small'>SLUT</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>PRODUKT</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>STATUS</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>UPPEHÅLL</td>");
                sb.Append("</tr>");

                foreach (SubscriptionCirixMap sub in Subscriber.SubsActiveOrPassive)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>" + sub.Subsno.ToString() + "</td>");
                    sb.Append("<td width=10></td>");
                    sb.Append("<td>" + sub.InvStartDate.ToString("yyyy-MM-dd") + "</td>");
                    sb.Append("<td> - </td>");
                    sb.Append("<td>" + sub.SubsEndDate.ToString("yyyy-MM-dd") + "</td>");
                    sb.Append("<td width=10></td>");
                    sb.Append("<td>" + Settings.GetName_Product(sub.PaperCode, sub.ProductNo) + "</td>");
                    sb.Append("<td width=10></td>");
                    sb.Append("<td>" + GetSusbsState(sub.SubsState) + "</td>");
                    sb.Append("<td width=10></td>");
                    sb.Append("<td>");

                    if (sub.SubsState == "02")
                        sb.Append(GetCurrentSubsSleepInterval(sub.SubsSleeps));

                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                
                LabelSubs.Text = sb.ToString();
                LabelSubs.Visible = true;
            }
        }

        private string GetCurrentSubsSleepInterval(List<SubsSleepsCirixMap> subsSleeps)
        {
            foreach (SubsSleepsCirixMap sleep in subsSleeps)
            {
                DateTime now = DateTime.Now.Date;

                if (now >= sleep.SleepStartDate && (now <= sleep.SleepEndDate || sleep.SleepEndDate == CirixMaxDate))
                {
                    string endDate = (sleep.SleepEndDate == CirixMaxDate) ? "Tillsvidare" : sleep.SleepEndDate.ToString("yyyy-MM-dd");
                    return sleep.SleepStartDate.ToString("yyyy-MM-dd") + " - " + endDate;
                }
            }

            return string.Empty;
        }

        private void PrintInvoices()
        {
            DataSet ds = Subscriber.GetOpenInvoices();

            if (DbHelpMethods.DataSetHasRows(ds))
            {
                var sb = new StringBuilder();
                sb.Append("<table border=0 cellspacing=0 cellpaddning=0>");
                sb.Append("<tr>");
                sb.Append("<td class='small'>PRENNR</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>FAKTNR</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>FAKTDATUM</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>FÖRFDATUM</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>EX.MOMS</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>MOMS</td>");
                sb.Append("<td width=10></td>");
                sb.Append("<td class='small'>INK.MOMS</td>");
                sb.Append("</tr>");
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        long subsno = -10;
                        long.TryParse(dr["SUBSNO"].ToString(), out subsno);
                        
                        if (SubscribersSubsnos.Contains(subsno))
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>" + dr["SUBSNO"].ToString() + "</td>");
                            sb.Append("<td width=10></td>");
                            sb.Append("<td>" + dr["INVNO"].ToString() + "</td>");
                            sb.Append("<td width=10></td>");
                            sb.Append("<td>" + Convert.ToDateTime(dr["INVDATE"]).ToString("yyyy-MM-dd") + "</td>");
                            sb.Append("<td width=10></td>");
                            sb.Append("<td>" + Convert.ToDateTime(dr["EXPDATE"]).ToString("yyyy-MM-dd") + "</td>");
                            sb.Append("<td width=10></td>");
                            sb.Append("<td>" + dr["INVAMOUNT"].ToString() + "kr</td>");
                            sb.Append("<td width=10></td>");
                            sb.Append("<td>" + dr["VATAMOUNT"].ToString() + "kr</td>");
                            sb.Append("<td width=10></td>");
                            sb.Append("<td>" + dr["OPENAMOUNT"].ToString() + "kr</td>");
                            sb.Append("</tr>");
                        }
                    }
                }
                sb.Append("</table>");
                
                LabelInvoices.Text = sb.ToString();
                LabelInvoices.Visible = true;
            }
        }

        private string GetSusbsState(string code)
        {
            switch (code)
            {
                case "00":
                case "30":
                    return "Giltig from";
                case "01":
                    return "Aktiv";
                case "02":
                    return "Uppehåll";
                default:
                    return "Avslutad";
                //break;
            }
        }

        private void ClearOverviewFields()
        {
            LabelAddress.Text = "";
            LabelCustName.Text = "";
            LabelInvoices.Text = "";
            LabelSubs.Text = "";
        }
    }
}