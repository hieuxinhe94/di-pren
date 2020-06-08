using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.EPiJobs.SyncSubs;
using EPiServer;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.CirixMappers;
using DIClassLib.Misc;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using System.Text;
using System.Data;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;

using Microsoft.SqlServer.Server;

namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class MySettingsMenu : UserControlBase
    {   
        //personal code (from MBA db) enables auto login for user
        public string UrlCode
        {
            get
            {
                if (Request.QueryString["code"] != null)
                    return Request.QueryString["code"].ToString();

                return string.Empty;
            }
        }

        private long UrlSubsno
        {
            get
            {
                if (Request.QueryString["sid"] != null && MiscFunctions.IsNumeric(Request.QueryString["sid"].ToString()))
                    return long.Parse(Request.QueryString["sid"].ToString());

                return 0;
            }
        }

        private IUserSettingsPage ParentPage;

        SubscriptionUser2 _subscriber = null;
        public SubscriptionUser2 Subscriber
        {
            get 
            {
                //return new SubscriptionUser2(); 
                
                if (_subscriber == null)
                    _subscriber = new SubscriptionUser2();

                return _subscriber;
            }
        }

        List<SubscriptionCirixMap> _activeSubs = null;
        public List<SubscriptionCirixMap> ActiveSubs
        {
            get
            {
                if (_activeSubs != null)
                    return _activeSubs;

                _activeSubs = GetActiveSubs();

                return _activeSubs;
            }
        }

        //public List<SubscriptionCirixMap> ActiveSubsInSession 
        //{
        //    get
        //    {
        //        if (Session["actSubs"] != null)
        //            return (List<SubscriptionCirixMap>)Session["actSubs"];

        //        Session["actSubs"] = GetActiveSubs();

        //        return (List<SubscriptionCirixMap>)Session["actSubs"];
        //    }
        //}


        #region url:s
        public string UrlStart
        {
            get
            {
                if (ViewState["PageMySetStart"] != null)
                    return ViewState["PageMySetStart"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetStart") as PageReference);
                ViewState["PageMySetStart"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetStart"].ToString();
            }
        }

        public string UrlPersonInfo 
        {
            get
            {
                if (ViewState["PageMySetPersonInfo"] != null)
                    return ViewState["PageMySetPersonInfo"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetPersonInfo") as PageReference);
                ViewState["PageMySetPersonInfo"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetPersonInfo"].ToString();
            }
        }

        public string UrlLoginInfo
        {
            get
            {
                if (ViewState["PageMySetLoginInfo"] != null)
                    return ViewState["PageMySetLoginInfo"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetLoginInfo") as PageReference);
                ViewState["PageMySetLoginInfo"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetLoginInfo"].ToString();
            }
        }

        public string UrlAddressPerm
        {
            get
            {
                if (ViewState["PageMySetAddressPerm"] != null)
                    return ViewState["PageMySetAddressPerm"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetAddressPerm") as PageReference);
                ViewState["PageMySetAddressPerm"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetAddressPerm"].ToString();
            }
        }

        public string UrlGoldMembership
        {
            get
            {
                if (ViewState["PageMySetGoldMembership"] != null)
                    return ViewState["PageMySetGoldMembership"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetGoldMembership") as PageReference);
                ViewState["PageMySetGoldMembership"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetGoldMembership"].ToString();
            }
        }

        public string UrlAddressTemp
        {
            get
            {
                if (ViewState["PageMySetAddressTemp"] != null)
                    return ViewState["PageMySetAddressTemp"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetAddressTemp") as PageReference);
                ViewState["PageMySetAddressTemp"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetAddressTemp"].ToString();
            }
        }

        public string UrlSubsSleeps
        {
            get
            {
                if (ViewState["PageMySetSubsSleeps"] != null)
                    return ViewState["PageMySetSubsSleeps"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetSubsSleeps") as PageReference);
                ViewState["PageMySetSubsSleeps"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetSubsSleeps"].ToString();
            }
        }

        public string UrlInvoice
        {
            get
            {
                if (ViewState["PageMySetInvoice"] != null)
                    return ViewState["PageMySetInvoice"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetInvoice") as PageReference);
                ViewState["PageMySetInvoice"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetInvoice"].ToString();
            }
        }

        public string UrlCancelSubs
        {
            get
            {
                if (ViewState["PageMySetCancelSubs"] != null)
                    return ViewState["PageMySetCancelSubs"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetCancelSubs") as PageReference);
                ViewState["PageMySetCancelSubs"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetCancelSubs"].ToString();
            }
        }

        public string UrlComplaint
        {
            get
            {
                if (ViewState["PageMySetComplaint"] != null)
                    return ViewState["PageMySetComplaint"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetComplaint") as PageReference);
                ViewState["PageMySetComplaint"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetComplaint"].ToString();
            }
        }

        public string UrlAutowithdrawal
        {
            get
            {
                if (ViewState["PageMySetAutowithdrawal"] != null)
                    return ViewState["PageMySetAutowithdrawal"].ToString();

                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "PageMySetAutowithdrawal") as PageReference);
                ViewState["PageMySetAutowithdrawal"] = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

                return ViewState["PageMySetAutowithdrawal"].ToString();
            }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            #region try to log in user by code in url

            ParentPage = (IUserSettingsPage)Page;

            if (!Page.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(UrlCode) && MiscFunctions.IsGuid(UrlCode))
            {
                //get cusno from code
                long cusno = MsSqlHandler.MdbGetCusnoByCode(new Guid(UrlCode));
                if (cusno < 1)
                {
                    new Logger("GetCusnoByCode() failed for code:" + UrlCode, "not an exception");
                    ParentPage.ShowMessageFromChildCtrl("Kunduppgifter hittades inte för kod: " + UrlCode, false, true);
                    return;
                }

                int ret = SyncSubsHandler.SyncCustToMssqlLoginTables((int)cusno);
                if (ret != 1)
                {
                    new Logger("SyncCustToMssqlLoginTables() failed for cusno:" + cusno + ", ret:" + ret, "not an exception: -1 does not have active subs, -2 could not find customer facts in cirix");
                    var sb = new StringBuilder();
                    sb.Append("Ett tekniskt problem uppstod när dina uppgifter skulle hämtas från prenumerationssystemet.<br>");
                    sb.Append("Var god kontakta kundtjänst.<br>");
                    sb.Append("Ditt kundnummer är: " + cusno.ToString());
                    ParentPage.ShowMessageFromChildCtrl(sb.ToString(), false, true);
                    return;
                }

                if (!LoginUtil.TryLoginUserToDagensIndstri(cusno))
                {
                    new Logger("TryLoginUserToDagensIndstri() failed for cusno:" + cusno, "not an exception");
                    var sb = new StringBuilder();
                    sb.Append("Den automatiska inloggningen misslyckades.<br>");
                    sb.Append("Var god kontakta kundtjänst om problemet kvarstår.<br>");
                    sb.Append("Ditt kundnummer är: " + cusno.ToString());
                    ParentPage.ShowMessageFromChildCtrl(sb.ToString(), false, true);
                    return;
                }

                //redir on success
                var url = new StringBuilder();
                url.Append(EPiFunctions.GetFriendlyUrl(CurrentPage));
                if (UrlSubsno > 0)
                    url.Append("?sid=" + UrlSubsno);

                Response.Redirect(url.ToString(), true);
                return;
            }
            #endregion
            

            //dyn added controls - add on all page loads
            if (Page.User.Identity.IsAuthenticated)
                PlaceHolderSubsLinks.Controls.Add(GetPlaceHolderAllSubsLinks());

            if (!IsPostBack)
            {
                if (Page.User.Identity.IsAuthenticated)
                    PopulateCustomerFactsBox();
                
                HyperLinkStart.NavigateUrl = UrlStart;
                HyperLinkPerson.NavigateUrl = UrlPersonInfo;
                HyperLinkLogin.NavigateUrl = UrlLoginInfo;
                HyperLinkPermAddress.NavigateUrl = UrlAddressPerm;
                HyperLinkInvoice.NavigateUrl = UrlInvoice;
                HyperLinkGold.NavigateUrl = UrlGoldMembership;
                HyperLinkCancelSubs.NavigateUrl = UrlCancelSubs;
                HyperLinkComplaint.NavigateUrl = UrlComplaint;

                HideLinksForNonAddressCust();
            }
        }

        private void HideLinksForNonAddressCust()
        {
            bool hasAddressSub = false;

            foreach (var sub in ActiveSubs)
            {
                if (sub.PaperCode == Settings.PaperCode_DI || sub.PaperCode == Settings.PaperCode_DIY)
                {
                    hasAddressSub = true;
                    break;
                }
            }

            if (!hasAddressSub)
            {
                HyperLinkPermAddress.Visible = false;
                LitPermAdrBr.Visible = false;
                HyperLinkComplaint.Visible = false;
                LitReklBr.Visible = false;
            }
        }

        private Control GetPlaceHolderAllSubsLinks()
        {
            PlaceHolder ph = new PlaceHolder();
            ph.ID = "PlaceHolderLinks";

            foreach (SubscriptionCirixMap sub in ActiveSubs)
            {
                if (sub.PaperCode == Settings.PaperCode_DI)
                    ph.Controls.AddAt(0, GetSubLinks(sub));
                else
                    ph.Controls.Add(GetSubLinks(sub));
            }

            return ph;
        }

        private void PopulateCustomerFactsBox()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>Kunduppgifter</b>");
            sb.Append("<br><br>");
            sb.Append("Kundnummer: " + Subscriber.Cusno.ToString());
            sb.Append("<br><br>");

            sb.Append(Subscriber.RowText1);

            if (Subscriber.IsCompanyCust)
                sb.Append(", " + Subscriber.RowText2);

            sb.Append("<br>");

            sb.Append(Subscriber.StreetName);

            if (!string.IsNullOrEmpty(Subscriber.HouseNo))
                sb.Append(" " + Subscriber.HouseNo);

            if (!string.IsNullOrEmpty(Subscriber.Staricase))
                sb.Append(Subscriber.Staricase);

            if (!string.IsNullOrEmpty(Subscriber.Apartment))
                sb.Append(" " + Subscriber.Apartment);

            sb.Append("<br>");

            if (!string.IsNullOrEmpty(Subscriber.Street2))
                sb.Append("Co/Lgh: " + Subscriber.Street2 + "<br>");

            if (Subscriber.Zip != "10000")
                sb.Append(Subscriber.Zip + " " + Subscriber.PostName);
            
            sb.Append("<br>");

            foreach (SubscriptionCirixMap sub in ActiveSubs)
            {
                sb.Append("<br>" + Settings.GetName_Product(sub.PaperCode, sub.ProductNo) + "<br>");
                sb.Append("Startdatum: " + sub.SubsStartDate.ToShortDateString() + "<br>");
                string s = (sub.SubsEndDate >= new DateTime(2078, 12, 31)) ? "Tillsvidare" : sub.SubsEndDate.ToShortDateString();
                sb.Append("Slutdatum: " + s + "<br>");
            }

            LiteralCustomerFacts.Text = sb.ToString();
        }

        private Control GetSubLinks(SubscriptionCirixMap map)
        {
            string sn = map.Subsno.ToString();
            string pc = map.PaperCode;
            string sid = "?sid=" + sn;
            PlaceHolder ph = new PlaceHolder();

            var isActiveAwdSubs = MsSqlHandler.IsActiveAwdSubs(map.Subsno);
            if (isActiveAwdSubs || !IsDigitalSub(pc))
            {
                Literal headline = new Literal();
                headline.Text = "<b>" + Settings.GetName_Product(pc, map.ProductNo) + "</b><br />";
                ph.Controls.Add(headline);
            }

            //no temp address changes for digital subs
            if (!IsDigitalSub(pc))
            {
                ph.Controls.Add(GetLinkButton("Tillfällig adressändring", "tempAddress_" + sn, UrlAddressTemp + sid));
                ph.Controls.Add(new Literal() {Text = "<br>"});

                if (map.Autogiro != "Y" || !Settings.HideSubsSleepsForAutogiroCust)
                {
                    ph.Controls.Add(GetLinkButton("Uppehåll", "holidayStop_" + sn, UrlSubsSleeps + sid));
                    ph.Controls.Add(new Literal() { Text = "<br>" });
                }
            }

            if (isActiveAwdSubs)
            {
                ph.Controls.Add(GetLinkButton("Autodragning på kort", "autowithdrawal_" + sn, UrlAutowithdrawal + sid));
                ph.Controls.Add(new Literal() { Text = "<br>" });
            }

            //ph.Controls.Add(GetLinkButton("Avsluta prenumeration", "cancelSub_" + sn, UrlCancelSubs + sid));
            //ph.Controls.Add(new Literal() { Text = "<br>" });

            return ph;
        }

        private bool IsDigitalSub(string paperCode)
        {
            return (paperCode == Settings.PaperCode_DISE || paperCode == Settings.PaperCode_IPAD || paperCode == Settings.PaperCode_AGENDA);
        }

        private Control GetLinkButton(string text, string id, string address)
        {
            var lit = new Literal();
            lit.ID = id;
            lit.Text = "<a href='" + address + "' class='menuItem'>" + text + "</a>";
            return lit;
        }

        //private Control GetLinkButton(string text, string id, string address)
        //{
        //    LinkButton lb = new LinkButton();
        //    lb.ID = id;
        //    lb.Text = text;
        //    lb.PostBackUrl = address;
        //    lb.CssClass = "menuItem";
        //    //lb.CausesValidation = true;
        //    //lb.CommandArgument = id;
        //    //lb.Click += new EventHandler(LinkButtonMenu_Click);
        //    return lb;
        //}


        //todo: make sure that sub with top extno is in list
        private List<SubscriptionCirixMap> GetActiveSubs()
        {
            List<SubscriptionCirixMap> ret = new List<SubscriptionCirixMap>();

            List<long> subnos = new List<long>();
            foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
            {
                if (subnos.Contains(sub.Subsno))
                    continue;

                subnos.Add(sub.Subsno);
                ret.Add(sub);
            }

            return ret;
        }

    }
}