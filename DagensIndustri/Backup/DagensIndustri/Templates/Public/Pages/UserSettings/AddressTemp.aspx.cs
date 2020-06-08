using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using System.Text;
using DagensIndustri.Templates.Public.Units.Placeable.AddressForm;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions.CirixMappers;
using DIClassLib.EPiJobs.SyncSubs;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class AddressTemp : DiTemplatePage, IAddress
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

        private long Subsno
        {
            get
            {
                if (Request.QueryString["sid"] != null && MiscFunctions.IsNumeric(Request.QueryString["sid"].ToString()))
                    return long.Parse(Request.QueryString["sid"].ToString());

                return 0;
            }
        }

        public SubscriptionCirixMap Subscription
        { 
            get
            {
                if (ViewState["sub"] != null)
                    return (SubscriptionCirixMap)ViewState["sub"];

                foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
                {
                    if (sub.Subsno == Subsno && sub.SubsCusno == Subscriber.Cusno)
                    {
                        ViewState["sub"] = sub;
                        return (SubscriptionCirixMap)ViewState["sub"];
                    }
                }

                return null;
            }
        }

        public SubscriptionUser2 Subscriber
        {
            get
            {
                if (ViewState["Subscriber"] == null)
                    ViewState["Subscriber"] = new SubscriptionUser2();

                return (SubscriptionUser2)ViewState["Subscriber"];
            }
            set
            {
                ViewState["Subscriber"] = value;
            }
        }

        private bool SubsnoBelongsToSubscriber
        {
            get { return (Subscription != null); }
        }

        public DateTime DateMinAddrChange
        {
            get
            {
                if (ViewState["DateMinAddrChange"] != null)
                    return (DateTime)ViewState["DateMinAddrChange"];

                ViewState["DateMinAddrChange"] = CirixDbHandler.GetNextIssueDateIncDiRules(DateTime.Now, Subscription.PaperCode, Subscription.ProductNo);

                return (DateTime)ViewState["DateMinAddrChange"];
            }
        }

        protected AddressMap SelectedAddress { get; set; }

        protected List<AddressMap> FutureTempAddresses
        {
            get
            {
                List<AddressMap> ret = new List<AddressMap>();

                foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
                {
                    if (sub.Subsno == Subsno)
                    {
                        ret = sub.TempAddresses;
                        ret.Sort();
                        return ret;
                    }
                }

                return ret;
            }
        }

        private ObjHolderForAddressFormClick ObjHolderForAddrFormClick
        {
            get
            {
                if (ViewState["objHolderForAddressFormClick_temp"] != null)
                    return (ObjHolderForAddressFormClick)ViewState["objHolderForAddressFormClick_temp"];

                return null;
            }
            set
            {
                ViewState["objHolderForAddressFormClick_temp"] = value;
            }
        }

        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvAddresses.RowCommand += new GridViewCommandEventHandler(gvAddresses_OnRowCommand);
            gvAddresses.RowDeleting += new GridViewDeleteEventHandler(gvAddresses_RowDeleting);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!IsPostBack)
            {
                #region try to log in user by code in url
                if (!string.IsNullOrEmpty(UrlCode) && MiscFunctions.IsGuid(UrlCode))
                {
                    HandleVisibility(false, false);

                    //get cusno from code
                    long cusno = MsSqlHandler.MdbGetCusnoByCode(new Guid(UrlCode));
                    if (cusno < 1)
                    {
                        new Logger("GetCusnoByCode() failed for code:" + UrlCode, "not an exception");
                        UserMessageControl.ShowMessage("Kunduppgifter hittades inte för kod: " + UrlCode, false, true);
                        return;
                    }

                    int ret = SyncSubsHandler.SyncCustToMssqlLoginTables((int)cusno);
                    if (ret != 1)
                    {
                        new Logger("SyncCustToMssqlLoginTables() failed for cusno:" + cusno + ", ret:" + ret, "not an exception: -1 does not have active subs, -2 could not find customer facts in cirix");
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Ett tekniskt problem uppstod när dina uppgifter skulle hämtas från prenumerationssystemet.<br>");
                        sb.Append("Var god kontakta kundtjänst.<br>");
                        sb.Append("Ditt kundnummer är: " + cusno.ToString());
                        UserMessageControl.ShowMessage(sb.ToString(), false, true);
                        return;
                    }

                    if (!LoginUtil.TryLoginUserToDagensIndstri(cusno))
                    {
                        new Logger("TryLoginUserToDagensIndstri() failed for cusno:" + cusno, "not an exception");
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Den automatiska inloggningen misslyckades.<br>");
                        sb.Append("Var god kontakta kundtjänst om problemet kvarstår.<br>");
                        sb.Append("Ditt kundnummer är: " + cusno.ToString());
                        UserMessageControl.ShowMessage(sb.ToString(), false, true);
                        return;
                    }

                    //redir on success
                    Response.Redirect(EPiFunctions.GetFriendlyUrl(CurrentPage) + "?sid=" + Subsno);
                    return;
                }
                #endregion


                if (!User.Identity.IsAuthenticated)
                {
                    HandleVisibility(false, false);
                    ShowMessage("/mysettings2/notloggedin", true, true);
                    return;
                }

                if (Subsno <= 0)
                {
                    HandleVisibility(true, false);
                    ShowMessage("Ogiltigt prenumerationsnummer.", false, true);
                    return;
                }

                if (!SubsnoBelongsToSubscriber)
                {
                    HandleVisibility(true, false);
                    ShowMessage("Prenumerationsnummer: " + Subsno.ToString() + " tillhör inte inloggad person.", false, true);
                    return;
                }

                
                PopulateDdlAdresses();
                Address1.Date1Min = DateMinAddrChange;
                Address1.Date2Min = DateMinAddrChange;

                //possible to have multiple active subs
                //DateTime dt = Subscriber.SubsActive.SingleOrDefault(x => x.Subsno == Subsno).SubsEndDate;  
                DateTime dt = DateTime.MinValue;
                foreach(SubscriptionCirixMap sub in Subscriber.SubsActive)
                {
                    if (dt < sub.SubsEndDate)
                        dt = sub.SubsEndDate;
                }

                if (dt > DateTime.Now.Date)
                {
                    Address1.Date1Max = dt;
                    Address1.Date2Max = dt;
                }

                Address1.Visible = false;
            }
        }

        private void PopulateDdlAdresses()
        {
            DropDownListAddresses.Items.Clear();
            DropDownListAddresses.Items.Add(new ListItem() { Value = "", Text = "Använd tidigare sparad adress" });

            foreach (AddressMap am in Subscriber.SelectableTempAddresses)
            {
                ListItem li = new ListItem();
                li.Value = am.Id;
                li.Text = am.StreetName + " " + am.Houseno + am.Staircase;
                DropDownListAddresses.Items.Add(li);
            }
            
            DropDownListAddresses.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            gvAddresses.DataSource = FutureTempAddresses;
            gvAddresses.DataBind();
            PlaceHolderFutureAdresses.Visible = (FutureTempAddresses.Count > 0);
        }



        public void gvAddresses_OnRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            SelectedAddress = FutureTempAddresses.SingleOrDefault(x => x.Id == e.CommandArgument.ToString());
        }

        protected void gvAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListAddresses.SelectedIndex = -1;
            Address1.SetFieldsFromAddressMap(SelectedAddress);
            Address1.Visible = true;
            ObjHolderForAddrFormClick = new ObjHolderForAddressFormClick("update", SelectedAddress);

            //Response.Write("Subscriber.Cusno: " + Subscriber.Cusno.ToString() + "<br>");
            //Response.Write("Subsno: " + Subsno.ToString() + "<br>");
            //Response.Write("ObjHolderForAddrFormClick.AddressOrg.Addrno: " + ObjHolderForAddrFormClick.AddressOrg.Addrno.ToString() + "<br>");
            //Response.Write("ObjHolderForAddrFormClick.AddressOrg.StartDate: " + ObjHolderForAddrFormClick.AddressOrg.StartDate.ToShortDateString() + "<br>");

        }

        public void gvAddresses_RowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
            if (Subscriber.DeleteTemporaryAddress(Subsno, SelectedAddress.Addrno, SelectedAddress.StartDate) == "OK")
            {
                ShowMessage("Adressen har raderats.", false, false);
                Address1.Visible = false;
            }
            else
            {
                ShowMessage(GetErrMess(), false, false);
                Address1.Visible = false;
            }
        }

        protected void LinkButtonNewAddress_Click(object sender, EventArgs e)
        {
            gvAddresses.SelectedIndex = -1;
            Address1.ClearForms();
            DropDownListAddresses.SelectedIndex = -1;
            ObjHolderForAddrFormClick = new ObjHolderForAddressFormClick("insert", null);
            Address1.Visible = true;
        }

        protected void DropDownListAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvAddresses.SelectedIndex = -1;
            
            DropDownList ddl = (DropDownList)sender;
            if (ddl.SelectedIndex == 0)
            {
                Address1.Visible = false;
            }
            else
            {
                SelectedAddress = Subscriber.SelectableTempAddresses.SingleOrDefault(x => x.Id == ddl.SelectedValue.ToString());
                //SelectedAddress.StartDate = DateTime.MinValue;
                //SelectedAddress.EndDate = DateTime.MinValue;

                Address1.SetFieldsFromAddressMap(SelectedAddress);
                Address1.Visible = true;
                ObjHolderForAddrFormClick = new ObjHolderForAddressFormClick("insert", SelectedAddress);
            }
        }

        public void HandleAddressButtonClick(AddressDataHolder dh)
        {
            if (!Page.IsValid)
            {
                ShowMessage("/mysettings2/errorpagenotvalid", true, true);
                return;
            }

            if (dh.Date1 == DateTime.MinValue)
            {
                ShowMessage("Ange fråndatum", false, true);
                return;
            }

            if (dh.Date1 < DateMinAddrChange)
            {
                ShowMessage("Tidigaste möjliga fråndatum är " + DateMinAddrChange.ToShortDateString() + ".<br>Var god välj ett senare datum.", false, true);
                return;
            }
            
            if (dh.Date1 > DateMinAddrChange)
                dh.Date1 = CirixDbHandler.GetNextIssueDateIncDiRules(dh.Date1, Subscription.PaperCode, Subscription.ProductNo);

            //dh.Date2 = Subscriber.GetClosestIssueDateByUsersRole(Page.User, dh.Date2).AddDays(-1);
            DateTime tmpD2 = CirixDbHandler.GetNextIssueDateIncDiRules(dh.Date2, Subscription.PaperCode, Subscription.ProductNo);
            if (tmpD2.Date > dh.Date2.Date)
                dh.Date2 = tmpD2.AddDays(-1);

            if (dh.Date1 >= dh.Date2)
            {
                ShowMessage("Fråndatum måste vara ett tidigare datum än tilldatum.<br>Var god försök igen.", false, true);
                return;
            }

            if (!DateSpanOk(dh))
            {
                ShowMessage("Datumintervallet kolliderar med tidigare sparad adressändring.<br>Var god försök igen.", false, true);
                return;
            }


            if (ObjHolderForAddrFormClick.SqlCommand == "update")
            {
                //if (!ObjHolderForAddrFormClick.AddressOrg.IsInProgress)
                //{
                    Subscriber.DeleteTemporaryAddress(Subsno, ObjHolderForAddrFormClick.AddressOrg.Addrno, ObjHolderForAddrFormClick.AddressOrg.StartDate);
                    System.Threading.Thread.Sleep(1500);
                //}
            }

            if (InsertNewOrExistingTempAdr(dh))
            {
                new Logger(Settings.LogEvent_TempAddressChange, Subscriber.Cusno, true);
                Address1.Visible = false;
                PopulateDdlAdresses();
                gvAddresses.SelectedIndex = -1;
                SendCustMail(dh);
                ShowMessage("Adressen har sparats.", false, false);
            }
            else
            {
                //MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), "Spara permanent adressändring misslyckades", "Kundnummer: " + Subscriber.Cusno + "<br><br>Önskad adress<br>" + dh.ToString(), true);
                new Logger("HandleAddressButtonClick() (tempAddrChagne) failed for cusno: " + Subscriber.Cusno, dh.ToString());
                ShowMessage(GetErrMess(), false, true);
            }

            #region old code
            //if (ObjHolderForAddrFormClick.SqlCommand == "insert")
            //{
            //    string ret = "";
            //    AddressMap amIdentical = TryGetIdenticalExistingTempAddr(dh);
            //    if (amIdentical != null)
            //        ret = Subscriber.CreateTemporaryAddress(Subsno, amIdentical.Addrno, dh.Date1, dh.Date2);
            //    else
            //        ret = Subscriber.CreateTemporaryNewAddress(Subsno, dh.StreetName, dh.HouseNum, dh.Staircase, dh.CirixApartment, dh.CirixStreet2, dh.Zip, dh.Date1, dh.Date2);

            //    if (ret == "OK")
            //        HandleSuccess();
            //    else
            //    {
            //        //MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), "Spara permanent adressändring misslyckades", "Kundnummer: " + Subscriber.Cusno + "<br><br>Önskad adress<br>" + dh.ToString(), true);
            //        new Logger("HandleAddressButtonClick() (permAddrChagne) failed for cusno: " + Subscriber.Cusno, dh.ToString());
            //        ShowMessage(GetErrMess(), false, true);
            //    }
            //}


            //if (ObjHolderForAddrFormClick.SqlCommand == "update")
            //{
            //    string retDelete = Subscriber.DeleteTemporaryAddress(Subsno, ObjHolderForAddrFormClick.AddressOrg.Addrno, ObjHolderForAddrFormClick.AddressOrg.StartDate);
            //    string ret = "";

            //    if (retDelete == "OK")
            //    {
            //        AddressMap amIdentical = TryGetIdenticalExistingTempAddr(dh);
            //        if (amIdentical != null)
            //            ret = Subscriber.CreateTemporaryAddress(Subsno, amIdentical.Addrno, dh.Date1, dh.Date2);
            //        else
            //            ret = Subscriber.CreateTemporaryNewAddress(Subsno, dh.StreetName, dh.HouseNum, dh.Staircase, dh.CirixApartment, dh.CirixStreet2, dh.Zip, dh.Date1, dh.Date2);

            //        if (ret == "OK")
            //            HandleSuccess();
            //    }

            //    if (retDelete != "OK" || ret != "OK")
            //    {
            //        Address1.Visible = true;
            //        ShowMessage(GetErrMess(), false, true);
            //    }
            //}

            //private void HandleSuccess()
            //{
            //    //SendCustMail(dh);
            //    new Logger(Settings.LogEvent_TempAddressChange, Subscriber.Cusno, true);
            //    Address1.Visible = false;
            //    PopulateDdlAdresses();
            //    ShowMessage("Adressen har sparats.", false, false);
            //}

            //private AddressMap TryGetIdenticalExistingTempAddr(AddressDataHolder dh)
            //{
            //    foreach (AddressMap am in Subscriber.SelectableTempAddresses)
            //    {
            //        if (am.AreEqual(dh))
            //            return am;
            //    }

            //    return null;
            //}
            #endregion
        }

        private bool DateSpanOk(AddressDataHolder dh)
        {
            AddressMap org = ObjHolderForAddrFormClick.AddressOrg;

            foreach (AddressMap old in FutureTempAddresses)
            {
                if (org != null && old.AreEqual(org) && old.StartDate == org.StartDate && old.EndDate == org.EndDate)
                    continue;
                
                //StartDate in old interval
                if (dh.Date1 >= old.StartDate && dh.Date1 <= old.EndDate)
                    return false;

                //EndDate in old interval
                if (dh.Date2 >= old.StartDate && dh.Date2 <= old.EndDate)
                    return false;

                //overlapping entire old interval
                if (dh.Date1 < old.StartDate && dh.Date2 > old.EndDate)
                    return false;
            }

            return true;
        }

        private bool InsertNewOrExistingTempAdr(AddressDataHolder dh)
        {
            //editing active address
            //if (ObjHolderForAddrFormClick.AddressOrg.IsInProgress)
            //return (Subscriber.TemporaryAddressChangePeriod(Subscriber.Cusno, Subsno, ObjHolderForAddrFormClick.AddressOrg.Addrno, ObjHolderForAddrFormClick.AddressOrg.StartDate, dh.Date1, dh.Date2) == "OK");

            foreach (AddressMap am in Subscriber.SelectableTempAddresses)
            {
                if (am.AreEqual(dh))
                    return (Subscriber.CreateTemporaryAddress(Subsno, am.Addrno, dh.Date1, dh.Date2) == "OK");
            }

            return (Subscriber.CreateTemporaryNewAddress(Subsno, dh.StreetName, dh.HouseNum, dh.Staircase, dh.CirixApartment, dh.CirixStreet2, dh.Zip, dh.Date1, dh.Date2) == "OK");
        }


        private void SendCustMail(AddressDataHolder dh)
        {
            if (!MiscFunctions.IsValidEmail(Subscriber.Email))
                return;

            StringBuilder sb = new StringBuilder();
            sb.Append("Tack, vi har mottagit din tillfälliga adressändring för perioden " + dh.Date1.ToShortDateString() + " - " + dh.Date2.ToShortDateString() + " till adress:<br><br>");
            sb.Append(dh.GetAddressAsHtml(Subscriber));
            sb.Append("<br><br><br>");
            sb.Append("Vid eventuella frågor kontakta vår kundtjänst på tel 08-573 651 00 eller <a href='mailto:pren@di.se'>pren@di.se</a>.<br><br>");
            sb.Append("Med vänliga hälsningar<br>");
            sb.Append("Dagens industri");

            MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), Subscriber.Email, "Bekräftelse tillfällig adressändring", sb.ToString(), true);
        }

        private string GetErrMess()
        {
            StringBuilder err = new StringBuilder();
            err.Append("Ett tekniskt fel uppstod.<br>");
            err.Append("Ring gärna kundtjänst på 08-573 651 00 så hjälper vi till att lösa problemet.");
            return err.ToString();
        }

        private void HandleVisibility(bool showMenu, bool showAddressesAndForm)
        {
            MySettingsMenu1.Visible = showMenu;
            PlaceHolderCurrAddrAndForm.Visible = showAddressesAndForm;
        }
    }
}