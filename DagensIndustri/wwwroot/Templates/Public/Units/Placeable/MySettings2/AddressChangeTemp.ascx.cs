using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Templates.Public.Units.Placeable.AddressForm;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.CirixMappers;
using DIClassLib.DbHandlers;
using DIClassLib.Membership;
using DIClassLib.Misc;
using DIClassLib.DbHelpers;
using System.Text;


namespace DagensIndustri.Templates.Public.Units.Placeable.MySettings2
{
    public partial class AddressChangeTemp : UserControlBase, IAddress
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

        public long Subsno 
        { 
            get 
            {
                if (ViewState["subsnoTmpAddrChange"] != null)
                    return (long)ViewState["subsnoTmpAddrChange"];

                return 0;
            }
            set
            {
                ViewState["subsnoTmpAddrChange"] = value;
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

                return new List<AddressMap>{ new AddressMap(){ StartDate=DateTime.Now.Date, StreetName="Skitgatan" } };
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

        protected void Page_Load(object sender, EventArgs e)
        {
            MySettingsPage.UserMessageControl.ClearMessage();

            if (!IsPostBack)
            {
                Address1.Date1Min = DateMinAddrChange;
            }

            bool bo = (FutureTempAddresses.Count > 0);
            HandleGuiVisibility(bo, false);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            gvAddresses.DataSource = FutureTempAddresses;
            gvAddresses.DataBind();

            Response.Write("<br>Subsno: " + Subsno.ToString());
        }



        public void gvAddresses_OnRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            //Response.Write("e.CommandName: " + e.CommandName + "<br>");
            //Response.Write("e.CommandArgument: " + e.CommandArgument + "<br>");

            //String selectedId = gvAddresses.SelectedValue as String;
            SelectedAddress = FutureTempAddresses.SingleOrDefault(x => x.Id == e.CommandArgument.ToString());
        }

        protected void gvAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            //String selectedId = gvAddresses.SelectedValue as String;
            //SelectedAddress = FutureTempAddresses.SingleOrDefault(x => x.Id == selectedId);
            Address1.SetFieldsFromAddressMap(SelectedAddress);
            HandleGuiVisibility(true, true);
            ObjHolderForAddrFormClick = new ObjHolderForAddressFormClick("update", SelectedAddress);
        }

        public void gvAddresses_RowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
            //String selectedId = gvAddresses.SelectedValue as String;
            //AddressMap am = FutureTempAddresses.SingleOrDefault(x => x.Id == selectedId);
            
            if (Subscriber.DeleteTemporaryAddress(Subsno, SelectedAddress.Addrno, SelectedAddress.StartDate) == "OK")
            {
                MySettingsPage.ShowMessage("Adressen har raderats.", false, false);
                HandleGuiVisibility(false, false);
            }
            else
            {
                MySettingsPage.ShowMessage(GetErrMess(), false, false);
                HandleGuiVisibility(true, false);
            }
        }

        protected void LinkButtonNewAddress_Click(object sender, EventArgs e)
        {
            //gvAddresses.SelectedIndex = -1;
            Address1.ClearForms();
            ObjHolderForAddrFormClick = new ObjHolderForAddressFormClick("insert", null);
            HandleGuiVisibility(false, true);
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
                string tmpAdrRet = Subscriber.CreateTemporaryNewAddress(Subsno, dh.StreetName, dh.HouseNum, dh.Staircase, dh.CirixApartment, dh.CirixStreet2, dh.Zip, dh.Date1, dh.Date2);

                if (tmpAdrRet == "OK")
                {
                    //SendCustMail(dh);
                    new Logger(Settings.LogEvent_TempAddressChange, Subscriber.Cusno, true);
                    HandleGuiVisibility(true, false);
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
                string ret = Subscriber.CreateTemporaryAddress(Subsno, ObjHolderForAddrFormClick.AddressOrg.Addrno, dh.Date1, dh.Date2);

                if (ret == "OK")
                {
                    //SendCustMail(dh);
                    new Logger(Settings.LogEvent_PermAddressChange, Subscriber.Cusno, true);
                    HandleGuiVisibility(true, false);
                    MySettingsPage.ShowMessage("Din adress har uppdaterats.", false, false);
                }
                else
                {
                    HandleGuiVisibility(true, true);
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

        private void HandleGuiVisibility(bool placeHolderFutureAddr, bool addrForm)
        {
            PlaceHolderFutureAdresses.Visible = placeHolderFutureAddr;
            Address1.Visible = addrForm;
        }

    }
}