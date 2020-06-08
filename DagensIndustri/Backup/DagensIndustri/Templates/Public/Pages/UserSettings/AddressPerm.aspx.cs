using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using System.Text;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions.CirixMappers;
using DagensIndustri.Templates.Public.Units.Placeable.AddressForm;
using DIClassLib.Membership;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class AddressPerm : DiTemplatePage, IAddress
    {
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

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!User.Identity.IsAuthenticated)
            {
                HandleNotLoggedIn();
                return;
            }

            if (!IsPostBack)
            {
                PopulateCurrentAddress();
                Address1.Date1Min = DateMinAddrChange;
                Address1.Date2Min = DateMinAddrChange;
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
                ShowMessage("Adressen har raderats.", false, false);
                HandleGuiVisibility(false, true, false);
            }
            else
            {
                ShowMessage(GetErrMess(), false, false);
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
            Address1.Visible = true;
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
            #endregion

            //Response.Write("cmd: " + ObjHolderForAddrFormClick.Command);
            if (dh.Date1 > DateMinAddrChange)
                dh.Date1 = GetClosestIssueDateByUsersRole(dh.Date1);


            if (ObjHolderForAddrFormClick.SqlCommand == "insert")
            {
                string permAdrRet = Subscriber.CreatePermanentAddress(dh.StreetName, dh.HouseNum, dh.Staircase, dh.CirixApartment, dh.CirixStreet2, dh.Zip, dh.Date1);

                if (permAdrRet == "OK")
                {
                    SendCustMail(dh);
                    new Logger(Settings.LogEvent_PermAddressChange, Subscriber.Cusno, true);
                    HandleGuiVisibility(true, false, false);
                    ShowMessage("Din adress har sparats.", false, false);
                }
                else
                {
                    //MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), "Spara permanent adressändring misslyckades", "Kundnummer: " + Subscriber.Cusno + "<br><br>Önskad adress<br>" + dh.ToString(), true);
                    new Logger("HandleAddressButtonClick() (permAddrChagne) failed for cusno: " + Subscriber.Cusno, dh.ToString());
                    ShowMessage(GetErrMess(), false, true);
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
                        SendCustMail(dh);
                        new Logger(Settings.LogEvent_PermAddressChange, Subscriber.Cusno, true);
                        HandleGuiVisibility(true, false, false);
                        ShowMessage("Din adress har uppdaterats.", false, false);
                    }
                }

                if (remove != "OK" || insert != "OK")
                {
                    HandleGuiVisibility(true, false, true);
                    ShowMessage(GetErrMess(), false, true);
                }
            }
        }


        
        private void HandleNotLoggedIn()
        {
            MySettingsMenu1.Visible = false;
            PlaceHolderCurrAddrAndForm.Visible = false;
            ShowMessage("/mysettings2/notloggedin", true, true);
        }

        public DateTime GetClosestIssueDateByUsersRole(DateTime dt)
        {
            if (Page.User.IsInRole(DiRoleHandler.RoleDiRegular))
                return CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DI, Settings.ProductNo_Regular);
            
            if (Page.User.IsInRole(DiRoleHandler.RoleDiWeekend))
                return CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DI, Settings.ProductNo_Weekend);

            if (Page.User.IsInRole(DiRoleHandler.RoleDiY))
            {
                DateTime tmp = CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DIY, Settings.ProductNo_Regular);
                if (tmp >= DateTime.Now.Date)
                    return tmp;
            }

            return CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DI, Settings.ProductNo_Regular);
        }

        private void SendCustMail(AddressDataHolder dh)
        {
            if (!MiscFunctions.IsValidEmail(Subscriber.Email))
                return;

            StringBuilder sb = new StringBuilder();
            sb.Append("Tack, vi har mottagit din permanenta adressändring från " + dh.Date1.ToShortDateString() + " till adress:<br><br>");
            sb.Append(dh.GetAddressAsHtml(Subscriber));
            sb.Append("<br><br><br>");
            sb.Append("Vid eventuella frågor kontakta vår kundtjänst på tel 08-573 651 00 eller <a href='mailto:pren@di.se'>pren@di.se</a>.<br><br>");
            sb.Append("Med vänliga hälsningar<br>");
            sb.Append("Dagens industri AB");

            MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), Subscriber.Email, "Bekräftelse permanent adressändring", sb.ToString(), true);
        }

        private void PopulateCurrentAddress()
        {
            LiteralName1Now.Text = Subscriber.RowText1;

            if (Subscriber.IsCompanyCust)
                LiteralName2Now.Text = ", " + Subscriber.RowText2;

            LiteralStreetAddrNow.Text = Subscriber.StreetName;
            LiteralStreetNumNow.Text = Subscriber.HouseNo;
            LiteralEntranceNow.Text = Subscriber.Staricase;
            LiteralStarisNow.Text = Subscriber.Apartment;
            
            if (!string.IsNullOrEmpty(Subscriber.Street2))
                Literal_Co_ApartmentNo_Now.Text = "Co/Lgh: " + Subscriber.Street2 + "<br>";

            LiteralZipNow.Text = Subscriber.Zip;
            LiteralCityNow.Text = Subscriber.PostName;
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
        
    }
}