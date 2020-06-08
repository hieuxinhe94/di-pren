using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.DbHandlers;
using DagensIndustri.Templates.Public.Units.Placeable.AddressForm;
using DIClassLib.Subscriptions.CirixMappers;
using System.Text;
using DagensIndustri.Templates.Public.Pages;

namespace DagensIndustri.Templates.Public.Units.Placeable.MySettings2
{

    public partial class AddressChangePerm : UserControlBase, IAddress
    {

        public DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2 MySettingsPage
        {
            get
            {
                return new DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2();  //(DagensIndustri.Templates.Public.Pages.MySettings2)Page;
            }
        }

        public SubscriptionUser2 Subscriber
        {
            get
            {
                DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2 parentPage = (DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2)Page;
                return (SubscriptionUser2)parentPage.Subscriber;
            }
        }

        public AddressMap CurrentPermAddress
        {
            get
            {
                foreach (AddressMap adr in Subscriber.PermAddresses)
                {
                    if (adr.Addrno == 1 && adr.ChangeType == Settings.AdressChangeType_Current)
                        return adr;
                }

                return new AddressMap();
            }
        }

        public DateTime DateMinAddrChange 
        {
            get
            {
                if (ViewState["DateMinAddrChange"] != null)
                    return (DateTime)ViewState["DateMinAddrChange"];

                ViewState["DateMinAddrChange"] = GetClosestIssueDateByUsersRole(DateTime.Now);

                return (DateTime)ViewState["DateMinAddrChange"];
            }
        }

        protected AddressMap SelectedAddress { get; set; }

        protected List<AddressMap> FuturePermAddresses
        {
            get
            {
                List<AddressMap> ret = new List<AddressMap>();

                foreach (AddressMap a in Subscriber.PermAddresses)
                {
                    if (a.StartDate > DateTime.Now.Date)
                        ret.Add(a);
                }

                return ret;
            }
        }

        private ObjHolderForAddressFormClick ObjHolderForAddrFormClick
        {
            get
            {
                if (ViewState["objHolderForAddressFormClick_perm"] != null)
                    return (ObjHolderForAddressFormClick)ViewState["objHolderForAddressFormClick_perm"];

                return null;
            }
            set
            {
                ViewState["objHolderForAddressFormClick_perm"] = value;
            }
        }


        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvAddresses.RowCommand += new GridViewCommandEventHandler(gvAddresses_OnRowCommand);
            gvAddresses.RowDeleting += new GridViewDeleteEventHandler(gvAddresses_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MySettingsPage.UserMessageControl.ClearMessage();

            if (!IsPostBack)
            {
                PopulateCurrentAddress();
                Address1.Date1Min = DateMinAddrChange;
            }

            bool bo = (FuturePermAddresses.Count > 0);
            HandleGuiVisibility(bo, !bo, false);
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            gvAddresses.DataSource = FuturePermAddresses;
            gvAddresses.DataBind();
        }



        public void gvAddresses_OnRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            //Response.Write("e.CommandName: " + e.CommandName + "<br>");
            //Response.Write("e.CommandArgument: " + e.CommandArgument + "<br>");
        }

        protected void gvAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedId = gvAddresses.SelectedValue as String;
            SelectedAddress = FuturePermAddresses.SingleOrDefault(x => x.Id == selectedId);
            Address1.SetFieldsFromAddressMap(SelectedAddress);
            HandleGuiVisibility(true, false, true);
            ObjHolderForAddrFormClick = new ObjHolderForAddressFormClick("update", SelectedAddress);
        }

        public void gvAddresses_RowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
            //String selectedId = gvAddresses.SelectedValue as String;
            //AddressMap am = FuturePermAddresses.SingleOrDefault(x => x.Id == selectedId);
            AddressMap am = FuturePermAddresses[0];
            if (Subscriber.DefinitiveAddressChangeRemove(am.StartDate) == "OK")
            {
                MySettingsPage.ShowMessage("Adressen har raderats.", false, false);
                HandleGuiVisibility(false, true, false);
            }
            else
            {
                MySettingsPage.ShowMessage(GetErrMess(), false, false);
                HandleGuiVisibility(true, false, false);
            }
        }

        protected void LinkButtonNewPermAddress_Click(object sender, EventArgs e)
        {
            //gvAddresses.SelectedIndex = -1;
            Address1.ClearForms();
            ObjHolderForAddrFormClick = new ObjHolderForAddressFormClick("insert", null);
            HandleGuiVisibility(false, true, true);
        }

        public void HandleAddressButtonClick(AddressDataHolder dh)
        {
            #region check form
            if (!Page.IsValid)
            {
                MySettingsPage.ShowMessage("/mysettings2/errorpagenotvalid", true, true);
                return;
            }
            
            if (dh.Date1 == DateTime.MinValue)
            {
                MySettingsPage.ShowMessage("Ange fråndatum", false, true);
                return;
            }
            
            if (dh.Date1 < DateMinAddrChange)
            {
                MySettingsPage.ShowMessage("Tidigaste möjliga fråndatum är " + DateMinAddrChange.ToShortDateString() + ".<br>Var god välj ett senare datum.", false, true);
                return;
            }
            #endregion

            //Response.Write("cmd: " + ObjHolderForAddrFormClick.Command);
            if (dh.Date1 > DateMinAddrChange)
                dh.Date1 = GetClosestIssueDateByUsersRole(dh.Date1);


            if (ObjHolderForAddrFormClick.SqlCommand == "insert")
            {
                string permAdrRet = Subscriber.CreatePermanentAddress(dh.StreetName, dh.HouseNum, dh.Staircase, dh.CirixApartment, dh.CirixStreet2, dh.Zip, dh.Date1);

                if (permAdrRet == "OK")
                {
                    //SendCustMail(dh);
                    new Logger(Settings.LogEvent_PermAddressChange, Subscriber.Cusno, true);
                    HandleGuiVisibility(true, false, false);
                    MySettingsPage.ShowMessage("Din adress har sparats.", false, false);
                }
                else
                {
                    //MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), "Spara permanent adressändring misslyckades", "Kundnummer: " + Subscriber.Cusno + "<br><br>Önskad adress<br>" + dh.ToString(), true);
                    new Logger("HandleAddressButtonClick() (permAddrChagne) failed for cusno: " + Subscriber.Cusno, dh.ToString());
                    MySettingsPage.ShowMessage(GetErrMess(), false, true);
                }
            }


            if (ObjHolderForAddrFormClick.SqlCommand == "update")
            {
                string insert = "";
                string remove = Subscriber.DefinitiveAddressChangeRemove(ObjHolderForAddrFormClick.AddressOrg.StartDate);
                
                if (remove == "OK")
                {
                    insert = Subscriber.CreatePermanentAddress(dh.StreetName, dh.HouseNum, dh.Staircase, dh.CirixApartment, dh.CirixStreet2, dh.Zip, dh.Date1);
                    if (insert == "OK")
                    {
                        //SendCustMail(dh);
                        new Logger(Settings.LogEvent_PermAddressChange, Subscriber.Cusno, true);
                        HandleGuiVisibility(true, false, false);
                        MySettingsPage.ShowMessage("Din adress har uppdaterats.", false, false);
                    }
                }

                if (remove != "OK" || insert != "OK")
                { 
                    HandleGuiVisibility(true, false, true);
                    MySettingsPage.ShowMessage(GetErrMess(), false, true);
                }
            }
        }


        
        private DateTime GetClosestIssueDateByUsersRole(DateTime dt)
        {
            if (Page.User.IsInRole(DiRoleHandler.RoleDiY))
                return CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DIY, Settings.ProductNo_Regular);
            else if (Page.User.IsInRole(DiRoleHandler.RoleDiWeekend))
                return CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DI, Settings.ProductNo_Weekend);
            else
                return CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DI, Settings.ProductNo_Regular);
        }

        private void SendCustMail(AddressDataHolder dh)
        {
            if (!MiscFunctions.IsValidEmail(Subscriber.Email))
                return;

            //SendConfirmationMail((string)CurrentPage["MailFrom"], (string)CurrentPage["MailSubjectPA"], (string)CurrentPage["MailBodyPA"]);
            StringBuilder sb = new StringBuilder();
            sb.Append("Tack, vi har mottagit ditt meddelande om permanent adressändring från " + dh.Date1.ToShortDateString() + " till adress:<br><br>");
            sb.Append(GetNewAddressAsHtml(dh));
            sb.Append("<br><br>");
            sb.Append("Vi tar hand om ditt ärende snarast möjligt. Vid eventuella frågor kontakta vår kundtjänst på tel 08-573 651 00 eller <a href='mailto:pren@di.se'>pren@di.se</a>.<br><br>");
            sb.Append("Med vänliga hälsningar<br>");
            sb.Append("Dagens industri AB");

            MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), Subscriber.Email, "Bekräftelse permanent adressändring", sb.ToString(), true);
        }

        private void PopulateCurrentAddress()
        {
            LiteralName1Now.Text = Subscriber.RowText1;

            if (Subscriber.IsCompanyCust)
                LiteralName2Now.Text = ", " + Subscriber.RowText2;

            LiteralStarisNow.Text = Subscriber.Apartment;
            LiteralCityNow.Text = Subscriber.PostName;
            LiteralEntranceNow.Text = Subscriber.Staricase;

            if (!string.IsNullOrEmpty(Subscriber.Co_ApartmentNum))
                Literal_Co_ApartmentNo_Now.Text = "Co/Lgh: " + Subscriber.Co_ApartmentNum + "<br>";

            LiteralStreetAddrNow.Text = Subscriber.StreetName;
            LiteralStreetNumNow.Text = Subscriber.HouseNo;
            LiteralZipNow.Text = Subscriber.Zip;
        }

        // SSAB, Olle Jansson
        // Gatan 1 B 4TR LGH4001
        // C/O Anna Jönsson
        // 11245 Stockholm
        private string GetNewAddressAsHtml(AddressDataHolder dh)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Subscriber.RowText1);

            if (Subscriber.IsCompanyCust)
                sb.Append(", " + Subscriber.RowText2);

            sb.Append("<br>");
            sb.Append(dh.StreetName);

            if (!string.IsNullOrEmpty(dh.HouseNum))
                sb.Append(" " + dh.HouseNum);

            if (!string.IsNullOrEmpty(dh.Staircase))
                sb.Append(" " + dh.Staircase);

            if (!string.IsNullOrEmpty(dh.Stairs))
                sb.Append(" " + dh.Stairs + "TR");

            if (!string.IsNullOrEmpty(dh.ApartmentNo))
                sb.Append(" LGH" + dh.ApartmentNo);

            sb.Append("<br>");

            if (!string.IsNullOrEmpty(dh.CareOf))
                sb.Append("C/O " + dh.CareOf + "<br>");

            sb.Append(dh.Zip + " " + dh.City + "<br><br>");

            return sb.ToString();
        }

        private string GetErrMess()
        {
            StringBuilder err = new StringBuilder();
            err.Append("Ett tekniskt fel uppstod.<br>");
            err.Append("Ring gärna kundtjänst på 08-573 651 00 så hjälper vi till att lösa problemet.");
            return err.ToString();
        }

        private void HandleGuiVisibility(bool placeHolderFutureAddr, bool buttonNewAddr, bool addrForm)
        {
            PlaceHolderFutureAdresses.Visible = placeHolderFutureAddr;
            LinkButtonNewPermAddress.Visible = buttonNewAddr;
            Address1.Visible = addrForm;
        }


        #region old code
        //private DateTime DateMinAddrChange
        //{
        //    get
        //    {
        //        if (ViewState["DateMinAddrChange"] != null)
        //            return (DateTime)ViewState["DateMinAddrChange"];

        //        bool has6DaySub = false;
        //        bool hasWeekendSub = false;

        //        foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
        //        {
        //            if (sub.ProductNo == Settings.ProductNo_Regular)
        //                has6DaySub = true;

        //            if (sub.ProductNo == Settings.ProductNo_Weekend)
        //                hasWeekendSub = true;
        //        }

        //        string prodNo = (!has6DaySub && hasWeekendSub) ? Settings.ProductNo_Weekend : Settings.ProductNo_Regular;

        //        ViewState["DateMinAddrChange"] = CirixDbHandler.GetNextIssueDateIncDiRules(DateTime.Now.Date, Settings.PaperCode_DI, prodNo);

        //        return (DateTime)ViewState["DateMinAddrChange"];
        //    }
        //}

        //private DateTime DateMaxAddrChange
        //{
        //    get
        //    {
        //        if (ViewState["DateMaxAddrChange"] != null)
        //            return (DateTime)ViewState["DateMaxAddrChange"];

        //        DateTime dt = DateTime.MinValue.Date;
        //        foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
        //        {
        //            if(sub.SubsEndDate > dt)
        //                dt = sub.SubsEndDate.Date;
        //        }

        //        ViewState["DateMaxAddrChange"] = dt;

        //        return (DateTime)ViewState["DateMaxAddrChange"];
        //    }
        //}

        //private AddressDataHolder AdrDh 
        //{
        //    get 
        //    {
        //        if (ViewState["AdrDh"] != null)
        //            return (AddressDataHolder)ViewState["AdrDh"];

        //        return null;
        //    }
        //    set
        //    {
        //        ViewState["AdrDh"] = value;
        //    }
        //}

        //protected void LinkButtonEditCurrentPermAddress_Click(object sender, EventArgs e)
        //{
        //    gvAddresses.SelectedIndex = -1;
        //    Address1.SetFieldsFromAddressMap(CurrentPermAddress);
        //    //Address1.ClearForms();
        //    Address1.Visible = true;
        //    ObjHolderForAddrFormClick = new ObjHolderForAddressFormClick("update", CurrentPermAddress);
        //}

        //protected void SavePermanentAddress_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DateTime fromDate;
        //        string stairs = string.Empty;
        //        string coAndApartment = string.Empty;

        //        if (Page.IsValid && DateTime.TryParse(PermanentAddressFromInput.Text, out fromDate))
        //        {
        //            //Get earliest date to stop. Parse to DateTime, to remove HH:MM:SS
        //            DateTime earliestDateToStop = DateTime.Parse(EarliestDateAddress.ToString("yyyy-MM-dd"));

        //            //check input date, must be ahead of earliestDateToStop
        //            if (fromDate < earliestDateToStop)
        //            {
        //                MySettingsPage.ShowMessage(string.Format(Translate("/mysettings/errors/subscription/date"), earliestDateToStop.ToString("yyyy-MM-dd"), Subscriber.SubEnd.ToString("yyyy-MM-dd")), false, true);
        //                return;
        //            }


        //            //Number of stairs must always be followed by TR without any space
        //            stairs = PermanentAddressStairsInput.Text;
        //            if (!string.IsNullOrEmpty(stairs) && !stairs.ToUpper().EndsWith("TR"))
        //                stairs = stairs + "TR";

        //            coAndApartment = string.Format("{0} {1}", PermanentAddressCoInput.Text, PermanentAddressApartmentInput.Text).Trim();

        //            if (Subscriber.CreatePermanentAddress(PermanentAddressStreetInput.Text, PermanentAddressHouseNoInput.Text, PermanentAddressStairCaseInput.Text,
        //                                                    stairs, coAndApartment, PermanentAddressZipInput.Text, fromDate) == "OK")
        //            {
        //                //Send confirmation mail to user
        //                //SendConfirmationMail((string)CurrentPage["MailFrom"], (string)CurrentPage["MailSubjectPA"], (string)CurrentPage["MailBodyPA"]);

        //                MySettingsPage.ShowMessage("/mysettings/subscription/permanentaddress/registered", true, false);

        //                //SetDefaultValues();

        //                List<string> xtra = CirixDbHandler.GetCustomerXtraFields(Subscriber.Cusno);
        //                if (!string.IsNullOrEmpty(xtra[0] + xtra[1] + xtra[2]))
        //                    DIClassLib.OneByOne.Obo.OboUnsubscribe(Subscriber.Cusno, xtra[0], xtra[1], xtra[2]);
        //            }
        //            else
        //            {
        //                //Send mail to pren
        //                MailErrorToPren("WebService FAILED: permanent adressändring", "Kundnummer: " + Subscriber.Cusno + "<br />Start: " + fromDate);
        //                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
        //                new Logger("SavePermanentAddress_Click() - failed to create permanent address in Cirix",
        //                                CreatePermanentAddrSaveDetailsMessage(PermanentAddressStreetInput.Text, PermanentAddressHouseNoInput.Text, PermanentAddressStairCaseInput.Text,
        //                                                                        stairs, coAndApartment, PermanentAddressZipInput.Text, fromDate));
        //            }
        //        }
        //        else
        //        {
        //            MySettingsPage.ShowMessage("/mysettings/errors/subscription/dateformat", true, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("SavePermanentAddress_Click() - failed", "Cusno: " + Subscriber.Cusno + " " + ex.ToString());
        //        //Send mail to Pren
        //        MailErrorToPren("WebService ERROR: permanent adressändring", "Kundnummer: " + Subscriber.Cusno + "<br />Start: " + PermanentAddressFromInput.Text);
        //        MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
        //    }

        //    new Logger(Settings.LogEvent_PermAddressChange, Subscriber.Cusno, true);
        //}

        //private void DisplayPlaceHolder(bool dispCurrAddrAndForm, bool dispConf)
        //{
        //    PlaceHolderCurrAddrAndForm.Visible = dispCurrAddrAndForm;
        //    PlaceHolderConf.Visible = dispConf;
        //}

        //private DateTime EarliestDateAddress
        //{
        //    get
        //    {
        //        DateTime dt;
        //        switch (DateTime.Now.DayOfWeek)
        //        {
        //            case DayOfWeek.Monday:
        //            case DayOfWeek.Tuesday:
        //            case DayOfWeek.Wednesday:
        //                dt = DateTime.Now.Date.AddDays(3);
        //                break;
        //            default:
        //                dt = DateTime.Now.Date.AddDays(5);
        //                break;
        //        }

        //        MiscFunctions.GetSubsMinDate("");

        //        return dt;
        //    }
        //}
        #endregion

    }


}