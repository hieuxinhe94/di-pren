using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.DbHandlers;
using System.Data;
using System.Text;
using EPiServer.PlugIn;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions.CirixMappers;


namespace DagensIndustri.Tools.Admin.Accommodators
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Förmedlarsida", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Förmedlarsida", UrlFromUi = "/Tools/Admin/Accommodators/CustomerDisplayer.aspx", SortIndex = 3000)]
    public partial class CustomerDisplayer : System.Web.UI.Page
    {
        private long _cusno = -1;

        public bool UserInAccommodatorRole 
        {
            get { return User.IsInRole("Formedlare"); }
        }

        /// <summary>
        /// if logged in username starts with "for-adm-" user can view any customer in accomodation GUI.
        /// </summary>
        public bool IsAccommodatorAdmin
        {
            get 
            {
                return User.Identity.Name.ToLower().StartsWith("for-adm-");
            }
        }

        public long AccommodatorCusno 
        {
            get
            {
                if (UserInAccommodatorRole)
                {
                    switch (User.Identity.Name)
                    {
                        case "for-LID":         //LM INFORMATION DELIVERY
                            return 100;
                        case "for-LCS":         //LEHTIMARKET CONSUMER SERVI
                            return 110;
                        case "for-BTJ":         //BTJ NORDIC AB
                            return 130;
                        case "for-swets":       //SWETS
                            return 150;
                        case "for-stockmann":   //STOCKMANN
                            return 160;
                        case "for-prenax":      //PRENAX AB
                            return 170;
                        case "for-prenaxnor":   //PRENAX NORGE
                            return 175;
                        case "for-EIS":         //EBSCO INFORMATION SERVICE
                            return 3054538;
                        case "for-adminone":    //ADMIN ONE AB
                            return 3226519;
                        default:
                            return -1;
                    }
                }

                return -1;
            }
        }

        public DateTime CirixMaxDate { get { return new DateTime(1800, 1, 1); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            long.TryParse(TextBoxCusno.Text, out _cusno);
            ClearFields();
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            DataSet dsCust = null;
            
            if(_cusno > 0)
                dsCust = CirixDbHandler.Ws.GetCustomer_(_cusno);

            if (!DbHelpMethods.DataSetHasRows(dsCust))
            {
                DisplayCustomer(false, "Ingen kund hittades för angivet kundnummer.");
                return;
            }

            DataSet dsSubs = CirixDbHandler.Ws.GetSubscriptions_(_cusno, "TRUE", null);
            List<long> subNosToShow = GetSubNosToShow(dsSubs);

            if (subNosToShow.Count == 0)
            {
                DisplayCustomer(false, "Behörighet att se kund saknas.<br>(Förmedlare kan endast se sina egna kunder).");
                return;
            }

            SubscriptionUser2 subUser = new SubscriptionUser2(_cusno);

            PrintNameAndAddress(subUser);
            PrintTempAddress(subUser);
            PrintSubs(subUser, subNosToShow);
            PrintInvoices(subNosToShow);

            DisplayCustomer(true, "");
        }


        private void PrintNameAndAddress(SubscriptionUser2 subUser)
        {
            string r1 = subUser.RowText1;
            string r2 = subUser.RowText2;
            if (!string.IsNullOrEmpty(r2))
                r1 += "<br>" + r2;

            LabelCustName.Text = r1;


            StringBuilder sb = new StringBuilder();
            sb.Append(subUser.StreetName);

            string hn = subUser.HouseNo;
            if (!string.IsNullOrEmpty(hn))
                sb.Append(" " + hn);

            string sc = subUser.Staricase;
            if (!string.IsNullOrEmpty(sc))
                sb.Append(" " + sc);

            string zip = subUser.Zip;
            if (!string.IsNullOrEmpty(zip))
                sb.Append(", " + zip);

            string pn = subUser.PostName;
            if (!string.IsNullOrEmpty(pn))
                sb.Append(" " + pn);

            string coAndApNo = subUser.Street2;
            if (!string.IsNullOrEmpty(coAndApNo))
            {
                sb.Append("<br>");
                if(!coAndApNo.StartsWith("LGH"))
                    sb.Append("C/O ");

                sb.Append(coAndApNo);
            }
                
            LabelAddress.Text = sb.ToString();
        }
 
        //private void PrintNameAndAddress(DataSet dsCust)
        //{
        //    if (dsCust.Tables[0].Rows.Count > 0)
        //    {
        //        DataRow drCust = dsCust.Tables[0].Rows[0];
        //        //LabelCustName.Text = drCust["ROWTEXT1"] as string;

        //        string r1 = drCust["ROWTEXT1"] as string;
        //        string r2 = drCust["ROWTEXT2"] as string;
        //        if (!string.IsNullOrEmpty(r2))
        //            r1 += "<br>" + r2;

        //        LabelCustName.Text = r1;


        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(drCust["STREETNAME"] as string);

        //        string hn = drCust["HOUSENO"] as string;
        //        if(!string.IsNullOrEmpty(hn))
        //            sb.Append(" " + hn);

        //        string sc = drCust["STAIRCASE"] as string;
        //        if (!string.IsNullOrEmpty(sc))
        //            sb.Append(" " + sc);

        //        string zip = drCust["ZIPCODE"] as string;
        //        if (!string.IsNullOrEmpty(zip))
        //            sb.Append(", " + zip);

        //        string pn = drCust["POSTNAME"] as string;
        //        if (!string.IsNullOrEmpty(pn))
        //            sb.Append(" " + pn);

        //        string co = drCust["STREET2"] as string;
        //        if (!string.IsNullOrEmpty(co))
        //            sb.Append("<br>c/o " + co);

        //        LabelAddress.Text = sb.ToString();

        //        #region fields
        //        //Attention = drCust["ROWTEXT2"] as string;
        //        //Apartment = drCust["APARTMENT"] as string;
        //        //string co = drCust["STREET2"] as string;
        //        //CountryCode = drCust["COUNTRYCODE"] as string;
        //        //Zip = drCust["ZIPCODE"] as string;
        //        //PostName = drCust["POSTNAME"] as string;
        //        //Email = drCust["EMAILADDRESS"] as string;
        //        //HPhone = drCust["H_PHONE"] as string;
        //        //WPhone = drCust["W_PHONE"] as string;
        //        //OPhone = drCust["O_PHONE"] as string;
        //        //SalesDen = drCust["SALESDEN"] as string;
        //        //OfferdenDir = drCust["OFFERDEN_DIR"] as string;
        //        //OfferdenSal = drCust["OFFERDEN_SAL"] as string;
        //        //OfferdenEmail = drCust["OFFERDEN_EMAIL"] as string;
        //        //DenySmsMark = drCust["DENYSMSMARK"] as string;
        //        //AccnoBank = drCust["ACCNO_BANK"] as string;
        //        //AccnoAcc = drCust["ACCNO_ACC"] as string;
        //        //Notes = drCust["NOTES"] as string;

        //        //long tmpEcusno;
        //        //if (long.TryParse(drCust["ECUSNO"] as string, out tmpEcusno))
        //        //    Ecusno = tmpEcusno;

        //        //OtherCusno = drCust["OTHER_CUSNO"] as string;
        //        ////WwwUserId = customerDr["WWWUSERID"] as string;
        //        //Expday = drCust["EXPDAY"] as string;

        //        //double tmpDiscPercent;
        //        //if (double.TryParse(drCust["DISCPERCENT"] as string, out tmpDiscPercent))
        //        //    DiscPercent = tmpDiscPercent;

        //        //Terms = drCust["TERMS"] as string;
        //        //SocialSecNo = drCust["SOCIALSECNO"] as string;
        //        //Category = drCust["CATEGORY"] as string;

        //        //long tmpMasterCusno;
        //        //if (long.TryParse(drCust["MASTERCUSNO"].ToString(), out tmpMasterCusno))
        //        //    MasterCusno = tmpMasterCusno;
        //        #endregion
        //    }
        //}

        private void PrintTempAddress(SubscriptionUser2 subUser)
        { 
            List<AddressMap> addresses = new List<AddressMap>();
            
            List<long> subnos = new List<long>();
            foreach (SubscriptionCirixMap sub in subUser.SubsActive)
            {
                if (subnos.Contains(sub.Subsno))
                    continue;

                subnos.Add(sub.Subsno);
                addresses.AddRange(sub.TempAddresses);
            }

            if(addresses.Count > 1)
                addresses.Sort();

            StringBuilder sb = new StringBuilder();
            foreach (AddressMap am in addresses)
                sb.Append(am.StartDate.ToString("yyyy-MM-dd") + " - " + am.EndDate.ToString("yyyy-MM-dd") + " " + am.Street1 + " " + am.Street2 + "<br>");

            LabelTempAddress.Text = sb.ToString();
        }

        private void PrintSubs(SubscriptionUser2 subUser, List<long> subNosToShow)
        {
            if (subNosToShow.Count == 0)
                return;

            if (subUser.SubsActiveOrPassive.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
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
                
                foreach (SubscriptionCirixMap sub in subUser.SubsActiveOrPassive)
                {
                    if (subNosToShow.Contains(sub.Subsno))
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
                }
                sb.Append("</table>");
                LabelSubs.Text = sb.ToString();
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

        //private void PrintSubs_OLD(DataSet dsSubs, List<long> subNosToShow)
        //{
        //    if (subNosToShow.Count == 0)
        //        return;

        //    if (DbHelpMethods.DataSetHasRows(dsSubs))
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<table border=0 cellspacing=0 cellpaddning=0>");
        //        sb.Append("<tr>");
        //        sb.Append("<td class='small'>PRENNR</td>");
        //        sb.Append("<td width=10></td>");
        //        sb.Append("<td class='small'>START</td>");
        //        sb.Append("<td></td>");
        //        sb.Append("<td class='small'>SLUT</td>");
        //        sb.Append("<td width=10></td>");
        //        sb.Append("<td class='small'>PRODUKT</td>");
        //        sb.Append("<td width=10></td>");
        //        sb.Append("<td class='small'>STATUS</td>");
        //        sb.Append("</tr>");
        //        foreach (DataTable dt in dsSubs.Tables)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                long subsno = -10;
        //                long.TryParse(dr["SUBSNO"].ToString(), out subsno);
        //                if (subNosToShow.Contains(subsno))
        //                {
        //                    sb.Append("<tr>");
        //                    sb.Append("<td>" + dr["SUBSNO"].ToString() + "</td>");
        //                    sb.Append("<td width=10></td>");
        //                    sb.Append("<td>" + Convert.ToDateTime(dr["INVSTARTDATE"]).ToString("yyyy-MM-dd") + "</td>");
        //                    sb.Append("<td> - </td>");
        //                    sb.Append("<td>" + Convert.ToDateTime(dr["SUBSENDDATE"]).ToString("yyyy-MM-dd") + "</td>");
        //                    sb.Append("<td width=10></td>");
        //                    sb.Append("<td>" + dr["PRODUCTNAME"].ToString() + "</td>");
        //                    sb.Append("<td width=10></td>");
        //                    sb.Append("<td>" + GetSusbsState(dr["SUBSSTATE"].ToString()) + "</td>");
        //                    sb.Append("</tr>");
        //                }
        //            }
        //        }
        //        sb.Append("</table>");
        //        LabelSubs.Text = sb.ToString();
        //    }
        //}


        private void PrintInvoices(List<long> subNosToShow)
        {
            if (subNosToShow.Count == 0)
                return;

            DataSet ds = CirixDbHandler.Ws.GetOpenInvoices_(_cusno, null);

            if (ds != null && ds.Tables != null)
            {
                StringBuilder sb = new StringBuilder();
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
                        if (subNosToShow.Contains(subsno))
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
            }
        }



        /// <summary>
        /// payer-cusno must be same as accommodator-cusno - else subs info should not be displayed for accommodator
        /// </summary>
        private List<long> GetSubNosToShow(DataSet dsSubs)
        {
            List<long> ret = new List<long>();

            if (DbHelpMethods.DataSetHasRows(dsSubs))
            {
                foreach (DataTable dt in dsSubs.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        long payCusno = -10;
                        long.TryParse(dr["PAYCUSNO"].ToString(), out payCusno);
                        
                        if (!UserInAccommodatorRole || AccommodatorCusno == payCusno || IsAccommodatorAdmin)
                            ret.Add(long.Parse(dr["SUBSNO"].ToString()));
                    }
                }
            }

            return ret;
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

        //private bool DataExists(DataSet ds)
        //{
        //    if (ds == null || ds.Tables == null || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0)
        //        return false;

        //    return true;
        //}

        private void DisplayCustomer(bool bo, string errMess)
        {
            LabelMess.Text = errMess;
            LabelMess.Visible = !bo;
            PlaceHolderCust.Visible = bo;
        }

        private void ClearFields()
        {
            LabelAddress.Text = "";
            LabelCustName.Text = "";
            LabelInvoices.Text = "";
            LabelSubs.Text = "";
        }

        protected void ButtonLogout_Click(object sender, EventArgs e)
        {
            LoginUtil.LogoutUser();
            Response.Redirect(Page.AppRelativeVirtualPath);
        }

    }
}