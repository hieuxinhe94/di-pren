using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Templates.Public.Units.Placeable.AddressForm;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.SyncSubs;
using DIClassLib.Membership;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.CirixMappers;
using EPiServer;

namespace DagensIndustri.Templates.Public.Units.Placeable.UserSettings
{
    public partial class AddressTempUC : UserControlBase, IAddress
    {
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

        public SubscriptionCirixMap Subscription
        {
            get
            {
                if (ViewState["sub"] != null)
                    return (SubscriptionCirixMap)ViewState["sub"];

                if (Subscriber != null)
                {
                    foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
                    {
                        if (sub.Subsno == UrlSubsno && sub.SubsCusno == Subscriber.Cusno)
                        {
                            ViewState["sub"] = sub;
                            return (SubscriptionCirixMap)ViewState["sub"];
                        }
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
                {
                    if (Page.User.Identity.IsAuthenticated && MembershipFunctions.UserIsLoggedInWithProvider(MembershipSettings.DiMembershipProviderName))
                    {
                        ViewState["Subscriber"] = new SubscriptionUser2();
                    }
                }

                return (SubscriptionUser2)ViewState["Subscriber"];
            }
            set
            {
                ViewState["Subscriber"] = value;
            }
        }

        public DateTime DateMinAddrChange
        {
            get
            {
                if (ViewState["DateMinAddrChange"] != null)
                    return (DateTime)ViewState["DateMinAddrChange"];

                ViewState["DateMinAddrChange"] = SubscriptionController.GetNextIssueDateIncDiRules(DateTime.Now, Subscription.PaperCode, Subscription.ProductNo);

                return (DateTime)ViewState["DateMinAddrChange"];
            }
        }

        protected AddressMap SelectedAddress { get; set; }

        protected List<AddressMap> FutureTempAddresses
        {
            get
            {
                var ret = new List<AddressMap>();

                foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
                {
                    if (sub.Subsno == UrlSubsno)
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

        private void HandleVisibility(bool showAddressesAndForm)
        {
            PlaceHolderCurrAddrAndForm.Visible = showAddressesAndForm;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (UrlSubsno == 0)
            {
                this.Visible = false;
                return;
            }
            
            //base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!IsPostBack)
            {
                if (Subscriber == null || Subscription == null)
                    return;

                PopulateDdlAdresses();
                Address1.Date1Min = DateMinAddrChange;
                Address1.Date2Min = DateMinAddrChange;

                //possible to have multiple active subs
                //DateTime dt = Subscriber.SubsActive.SingleOrDefault(x => x.Subsno == Subsno).SubsEndDate;  
                DateTime dt = DateTime.MinValue;
                foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
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
        }

        public void gvAddresses_RowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
            if (Subscriber.DeleteTemporaryAddress(UrlSubsno, SelectedAddress.Addrno, SelectedAddress.StartDate) == "OK")
            {
                UserMessageControl.ShowMessage("Adressen har raderats.", false, false);
                Address1.Visible = false;
            }
            else
            {
                UserMessageControl.ShowMessage(GetErrMess(), false, false);
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
                Address1.SetFieldsFromAddressMap(SelectedAddress);
                Address1.Visible = true;
                ObjHolderForAddrFormClick = new ObjHolderForAddressFormClick("insert", SelectedAddress);
            }
        }

        public void HandleAddressButtonClick(AddressDataHolder dh)
        {
            if (!Page.IsValid)
            {
                UserMessageControl.ShowMessage("/mysettings2/errorpagenotvalid", true, true);
                return;
            }

            if (dh.Date1 == DateTime.MinValue)
            {
                UserMessageControl.ShowMessage("Ange fråndatum", false, true);
                return;
            }

            if (dh.Date1 < DateMinAddrChange)
            {
                UserMessageControl.ShowMessage("Tidigaste möjliga fråndatum är " + DateMinAddrChange.ToString("yyyy-MM-dd") + ".<br>Var god välj ett senare datum.", false, true);
                return;
            }

            if (dh.Date1 > DateMinAddrChange)
                dh.Date1 = SubscriptionController.GetNextIssueDateIncDiRules(dh.Date1, Subscription.PaperCode, Subscription.ProductNo);

            //dh.Date2 = Subscriber.GetClosestIssueDateByUsersRole(Page.User, dh.Date2).AddDays(-1);
            DateTime tmpD2 = SubscriptionController.GetNextIssueDateIncDiRules(dh.Date2, Subscription.PaperCode, Subscription.ProductNo);
            if (tmpD2.Date > dh.Date2.Date)
                dh.Date2 = tmpD2.AddDays(-1);

            if (dh.Date1 >= dh.Date2)
            {
                UserMessageControl.ShowMessage("Fråndatum måste vara ett tidigare datum än tilldatum.<br>Var god försök igen.", false, true);
                return;
            }

            if (!DateSpanOk(dh))
            {
                UserMessageControl.ShowMessage("Datumintervallet kolliderar med tidigare sparad adressändring.<br>Var god försök igen.", false, true);
                return;
            }


            if (ObjHolderForAddrFormClick.SqlCommand == "update")
            {
                //if (!ObjHolderForAddrFormClick.AddressOrg.IsInProgress)
                //{
                Subscriber.DeleteTemporaryAddress(UrlSubsno, ObjHolderForAddrFormClick.AddressOrg.Addrno, ObjHolderForAddrFormClick.AddressOrg.StartDate);
                System.Threading.Thread.Sleep(1500);
                //}
            }

            if (CreateTempAdr(dh))
            {
                new Logger(Settings.LogEvent_TempAddressChange, Subscriber.Cusno, true);
                Address1.Visible = false;
                PopulateDdlAdresses();
                gvAddresses.SelectedIndex = -1;
                SendCustMail(dh);
                UserMessageControl.ShowMessage("Adressen har sparats.", false, false);
            }
            else
            {
                //MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), "Spara permanent adressändring misslyckades", "Kundnummer: " + Subscriber.Cusno + "<br><br>Önskad adress<br>" + dh.ToString(), true);
                new Logger("HandleAddressButtonClick() (tempAddrChagne) failed for cusno: " + Subscriber.Cusno, dh.ToString());
                UserMessageControl.ShowMessage(GetErrMess(), false, true);
            }
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

        private bool CreateTempAdr(AddressDataHolder dh)
        {
	        var am = new AddressMap()
	        {
		        StreetName = dh.StreetName,
		        Houseno = dh.HouseNum,
		        Staircase = dh.Staircase,
		        Apartment = dh.CirixApartment,
		        Street2 = dh.CirixStreet2,
		        Street3 = "",
		        CountryCode = Settings.Country,
		        ZipCode = dh.Zip
	        };

			return (Subscriber.CreateTemporaryAddress(UrlSubsno, am, dh.Date1, dh.Date2) == "OK");


			#region old code
			//editing active address
			//if (ObjHolderForAddrFormClick.AddressOrg.IsInProgress)
			//return (Subscriber.TemporaryAddressChangePeriod(Subscriber.Cusno, Subsno, ObjHolderForAddrFormClick.AddressOrg.Addrno, ObjHolderForAddrFormClick.AddressOrg.StartDate, dh.Date1, dh.Date2) == "OK");


			//comment: 150417 - TemporaryAddressChangeNewAddress() should not be used any more
			//foreach (AddressMap am in Subscriber.SelectableTempAddresses)
			//{
			//	if (am.AreEqual(dh))
			//		return (Subscriber.CreateTemporaryAddress(UrlSubsno, am, dh.Date1, dh.Date2) == "OK");
			//}

			//return (Subscriber.CreateTemporaryNewAddress(UrlSubsno, dh.StreetName, dh.HouseNum, dh.Staircase, dh.CirixApartment, dh.CirixStreet2, dh.Zip, dh.Date1, dh.Date2) == "OK");

			//Ws.AddNewTemporaryAddress_CII_(sUserId, lCusno, lSubsno, iExtno, 
			//	addressMap.StreetName, addressMap.Houseno, addressMap.Staircase, addressMap.Apartment,
			//	addressMap.Street2, addressMap.Street3, addressMap.CountryCode, addressMap.ZipCode, 
			//	dAddrStartDate, dAddrEndDate, sInvToTempAddress, sNewName1, sNewName2,
			//	Settings.PaperCode_DI, Settings.sReceiveType, false);
			#endregion
		}



        private void SendCustMail(AddressDataHolder dh)
        {
            if (!MiscFunctions.IsValidEmail(Subscriber.Email))
                return;

            StringBuilder sb = new StringBuilder();
            sb.Append("Tack, vi har mottagit din tillfälliga adressändring för perioden " + dh.Date1.ToString("yyyy-MM-dd") + " - " + dh.Date2.ToString("yyyy-MM-dd") + " till adress:<br><br>");
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

    }
}