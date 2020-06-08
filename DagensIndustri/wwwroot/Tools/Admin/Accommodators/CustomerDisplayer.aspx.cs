using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
        public string UrlShow
        {
            get
            {
                if (Request.QueryString["show"] != null)
                    return Request.QueryString["show"].ToString();

                return string.Empty;
            }
        }

        public long UrlCusno
        {
            get
            {
                if (Request.QueryString["cusno"] != null)
                {
                    long cn = 0;
                    long.TryParse(Request.QueryString["cusno"].ToString(), out cn);
                    return cn;
                }

                return 0;
            }
        }

        public long UrlSubsno
        {
            get
            {
                if (Request.QueryString["sid"] != null)
                {
                    long sid = 0;
                    long.TryParse(Request.QueryString["sid"].ToString(), out sid);
                    return sid;
                }

                return 0;
            }
        }

        private SubscriptionUser2 _subscriberEdited = null;
        public SubscriptionUser2 SubscriberEdited
        {
            get
            {
                if (_subscriberEdited != null)
                    return _subscriberEdited;

                if (UrlCusno > 0)
                {
                    _subscriberEdited = new SubscriptionUser2(UrlCusno);
                    return _subscriberEdited;
                }

                return null;
            }
        }


        #region Accommodator related

        public bool AdminAccommodatorRestricted
        {
            get { return (AdminInAccommodatorRole && AdminAccommodatorCusno > 0 && !AdminAccommodatorFullAccess); }
        }

        private List<SubscriptionCirixMap> AdminAccommodatorEditableSubs
        {
            get
            {
                var subs = new List<SubscriptionCirixMap>();

                if (AdminAccommodatorRestricted)
                {
                    foreach (var sub in SubscriberEdited.SubsActiveOrPassive)
                    {
                        if (AdminAccommodatorCusno == sub.PayCusno)
                        {
                            if (!subs.Contains(sub))
                            subs.Add(sub);
                        }
                    }
                }

                return subs;
            }
        }

        /// <summary>
        /// if logged in username starts with "for-adm-" user can view any customer in accomodation GUI.
        /// </summary>
        private bool AdminAccommodatorFullAccess
        {
            get 
            {
                return User.Identity.Name.ToLower().StartsWith("for-adm-");
            }
        }

        private long AdminAccommodatorCusno 
        {
            get
            {
                if (AdminInAccommodatorRole)
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
                        case "for-Vannerus":    //Vannerus & CO AB.
                            return 4013109;
                        default:
                            return -1;
                    }
                }

                return -1;
            }
        }

        private bool AdminInAccommodatorRole
        {
            get { return User.IsInRole("Formedlare"); }
        }

        public bool AdminAllowedToViewCust
        {
            get
            {
                //? allow accommodators to edit themselves ?
                //if (AdminAccommodatorCusno == SubscriberEdited.Cusno)
                //    return true;
                
                if (AdminAccommodatorRestricted && AdminAccommodatorEditableSubs.Count == 0)
                    return false;

                return true;
            }
        }

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (UrlCusno > 0)
            {
                TextBoxCusno.Text = UrlCusno.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UrlCusno > 0 && SubscriberEdited != null && SubscriberEdited.Cusno > 0)
            {
                CustDetailView1.Subscriber = SubscriberEdited;
                AddrPerm1.Subscriber = SubscriberEdited;
                AddrTemp1.Subscriber = SubscriberEdited;
                SubsSleep1.Subscriber = SubscriberEdited;
                Complaint1.Subscriber = SubscriberEdited;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            HandleCtrlVisibility(false, false, false, false, false);
            
            if (string.IsNullOrEmpty(UrlShow) || UrlCusno == 0)
            {
                ShowMess("Ange ett kundnummer och klicka på önskad funktion.");
                return;
            }

            if (SubscriberEdited == null || SubscriberEdited.Cusno == 0 || !AdminAllowedToViewCust)
            {
                ShowMess("Behörighet att se kund saknas.<br>(Förmedlare kan endast se sina egna kunder).");
                return;
            }

            if (!string.IsNullOrEmpty(UrlShow) && UrlCusno > 0)
            {
                if (UrlShow == "overview")
                    HandleCtrlVisibility(true, false, false, false, false);

                if (UrlShow == "addrPerm")
                    HandleCtrlVisibility(false, true, false, false, false);

                if (UrlShow == "addrTemp")
                    HandleCtrlVisibility(false, false, true, false, false);

                if (UrlShow == "subsSleep")
                    HandleCtrlVisibility(false, false, false, true, false);

                if (UrlShow == "complaint")
                    HandleCtrlVisibility(false, false, false, false, true);
            }
        }

        private void HandleCtrlVisibility(bool showOverview, bool showAddrPerm, bool showAddrTemp, bool showSubsSleeps, bool showComplaint)
        {
            if (showOverview) LiteralHeadline.Text = "Översikt";
            if (showAddrPerm) LiteralHeadline.Text = "Permanent adress";
            if (showAddrTemp) LiteralHeadline.Text = "Tillfällig adress";
            if (showSubsSleeps) LiteralHeadline.Text = "Uppehåll";
            if (showComplaint) LiteralHeadline.Text = "Reklamation";

            CustDetailView1.Visible = showOverview;
            AddrPerm1.Visible = showAddrPerm;
            AddrTemp1.Visible = showAddrTemp;
            SubsSleep1.Visible = showSubsSleeps;
            Complaint1.Visible = showComplaint;

            if (UrlSubsno == 0)
            {
                AddrTemp1.Visible = false;
                SubsSleep1.Visible = false;
            }

            if (showAddrTemp || showSubsSleeps)
                PopulateDDLSubs();
            else
                DDLSubs.Visible = false;
        }

        private void PopulateDDLSubs()
        {
            DDLSubs.Items.Clear();

            var editableSubs = (AdminAccommodatorRestricted) ? AdminAccommodatorEditableSubs : SubscriberEdited.SubsActive;

            var subnos = new List<long>();
            foreach (SubscriptionCirixMap sub in editableSubs)
            {
                var sn = sub.Subsno;
                if (subnos.Contains(sn))
                    continue;

                subnos.Add(sn);
                DDLSubs.Items.Add(new ListItem(Settings.GetName_Product(sub.PaperCode, sub.ProductNo) + " " + sn, sn.ToString()));
            }

            DDLSubs.Items.Insert(0, new ListItem("Välj prenumeration", "0"));

            if (UrlSubsno > 0)
                DDLSubs.SelectedValue = UrlSubsno.ToString();

            DDLSubs.Visible = true;
        }

        private void ShowMess(string mess)
        {
            if (string.IsNullOrEmpty(mess))
            {
                LabelMess.Text = string.Empty;
                LabelMess.Visible = false;
            }
            else
            {
                LabelMess.Text = mess;
                LabelMess.Visible = true;
            }
        }

       
        protected void ButtonLogout_Click(object sender, EventArgs e)
        {
            LoginUtil.LogoutUser();
            Response.Redirect(Page.AppRelativeVirtualPath);
        }

        protected void DDLSubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath + "?show=" + UrlShow + "&cusno=" + UrlCusno + "&sid=" + DDLSubs.SelectedValue);
        }

        protected void LinkButtonOverview_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath + "?show=overview&cusno=" + TextBoxCusno.Text);
        }

        protected void LinkButtonPermAddr_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath + "?show=addrPerm&cusno=" + TextBoxCusno.Text);
        }

        protected void LinkButtonTempAddr_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath + "?show=addrTemp&cusno=" + TextBoxCusno.Text);
        }

        protected void LinkButtonSubsSleep_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath + "?show=subsSleep&cusno=" + TextBoxCusno.Text);
        }

        protected void LinkButtonComplaint_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath + "?show=complaint&cusno=" + TextBoxCusno.Text);
        }

    }
}