using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow;
using DIClassLib.CardPayment.Nets;
using DIClassLib.Subscriptions;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Templates.Public.Units.Placeable.GasellFlow;
using DagensIndustri.Tools.Classes.Gasell;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using PlaceOrder = DagensIndustri.Templates.Public.Units.Placeable.GasellFlow.PlaceOrder;
using Receipt = DagensIndustri.Templates.Public.Units.Placeable.GasellFlow.Receipt;
using RegisterParticipants = DagensIndustri.Templates.Public.Units.Placeable.GasellFlow.RegisterParticipants;
using SideBarStepBox = DagensIndustri.Templates.Public.Units.Placeable.GasellFlow.SideBarStepBox;

namespace DagensIndustri.Templates.Public.Pages.DiGasell
{
    public partial class GasellFlow : DiTemplatePage
    {
        #region Properties

        public string NumberOfParticipants { get; set; }
        public int GasellId { get; set; }
        private string GasellPageFriendlyUrl { get; set; }
        private string GoogleTrackDatePart { get; set; }
        protected string GoogleVirtualUrl { get; set; }
        public string PayMethod { get; set; }
        public bool AurigaStatus { get; set; }
        public string Step { get; set; }

        public string ConfirmationMailFrom { get; set; }
        public string ConfirmationMailSubject { get; set; }
        //public string ConfirmationMailBody { get; set; }

        public MultiView MyFlowMultiView  
        { 
            get
            {
                return FlowMultiView;
            }
        }

        public List<GasellUser> GasellUsers
        {
            get { return (List<GasellUser>)ViewState["GasellUserList"]; }
            set { ViewState["GasellUserList"] = value; }
        }

        public RegisterParticipants Participants;
        public Units.Placeable.GasellFlow.PayWithCreditCard PayWithCreditCard;
        public PayWithInvoice PayWithInvoice;
        public PayWithDiscountCode PayWithDiscountCode;
        public Receipt Receipt;
        
        public GasellOrder GasellOrder 
        {
            get 
            {
                if (Session["GasellOrderObject"] != null)
                    return Session["GasellOrderObject"] as GasellOrder;

                return null;
            }
            set
            {
                Session["GasellOrderObject"] = value;
            }
        }

        public int GasellView
        {
            get
            {
                if (Session["GasellView"] != null)
                    return int.Parse(Session["GasellView"].ToString());

                return 0;
            }
            set
            {
                Session["GasellView"] = value;
            }
        }

        public string MailCity { get; set; }
        public string MailDate { get; set; }
        public string MailTime { get; set; }
        public string MailPrice { get; set; }
        public string MailPayMethod { get; set; }

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl;

            NumberOfParticipants = Request.QueryString["nop"];
            
            int gasId = 0;
            int.TryParse(Request.QueryString["gasellid"], out gasId);
            if (gasId > 0)
                GasellId = gasId;
            //EpiPagelId = Request.QueryString["gasellid"];
            
            PayMethod = Request.QueryString["pm"];

            
            #region This happens only when the visitor have been to Nets, paymethod credit card

            if (Request.QueryString["responseCode"] != null)
            {
                NetsCardPayReturn ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                GasellOrder = (GasellOrder)ret.NetsPreparePersisted.PersistedObj;
                
                NumberOfParticipants = GasellOrder.GasellUsersList.Count.ToString();
                GasellId = GasellOrder.GasellId;
                PayMethod = GasellOrder.PaymentMethod;
                
                string successUrl = CurrentPage.LinkURL + "&gasellid=" + GasellId + "&pm=" + PayMethod + "&nop=" + NumberOfParticipants + "&gv=t";
                ret.HandleNetsReturn(successUrl, GasellOrder.GasellUsersList[0].Email);  //GasellUsers[0].Email;
            }

            #endregion

            #region Mail information
            
            if (GasellId > 0)
            {
                PageData pd = DataFactory.Instance.GetPage(new PageReference(GasellId));
                GasellPageFriendlyUrl = EPiFunctions.GetFriendlyUrl(pd);
                string mailPaymentMethod = string.Empty;
                int numOfParticipants = Convert.ToInt32(Request.QueryString["nop"]);
                int numOfPartToPay = numOfParticipants - (numOfParticipants / 4);
                string price = (Convert.ToInt32(pd["Price"].ToString()) * numOfPartToPay).ToString();

                if (PayMethod == "1")
                    mailPaymentMethod = "Kort";
                else if (PayMethod == "2")
                    mailPaymentMethod = "Faktura";
                else if (PayMethod == "3")
                {
                    price = "0";
                    mailPaymentMethod = "Rabattkod";
                }

                ConfirmationMailFrom = EPiFunctions.SettingsPageSetting(CurrentPage, "GasellConfirmationMailFrom").ToString() ?? "gasell@di.se";
                ConfirmationMailSubject = string.Format(Translate("/gasell/mail/confirmation/subject"), pd["GasellCity"]);

                //ConfirmationMailBody = string.Format(Translate("/gasell/mail/confirmation/body"), pd["GasellCity"], EPiFunctions.GetDate(pd, "Date"), (Convert.ToDateTime(pd["Date"])).ToString("HH:mm"), price, mailPaymentMethod);
                MailCity = pd["GasellCity"].ToString();
                MailDate = EPiFunctions.GetDate(pd, "Date");
                MailTime = (Convert.ToDateTime(pd["Date"])).ToString("HH:mm");
                MailPrice = price;
                MailPayMethod = mailPaymentMethod;

                //TODO: Do this from DiTemplate-page so it can be used on any page?
                //Set Google Analytics tracking date part of url
                GoogleTrackDatePart = (Convert.ToDateTime(pd["Date"])).ToString("yyyy/MM/dd");
            }

            #endregion Mail information

            #region Create the views for the multiview control
            //try
            //{
                int i;

                for (i = 0; i < Convert.ToInt32(NumberOfParticipants); i++)
                {
                    Participants = (RegisterParticipants)Page.LoadControl("/Templates/Public/Units/Placeable/GasellFlow/RegisterParticipants.ascx");
                    Participants.ID = "RegisterParticipants" + i;
                    View view = new View();
                    view.ID = "View" + i;
                    view.Controls.Add(Participants);
                    FlowMultiView.Views.AddAt(i, view);
                }

                PlaceOrder placeOrder = (PlaceOrder)Page.LoadControl("/Templates/Public/Units/Placeable/GasellFlow/PlaceOrder.ascx");
                placeOrder.ID = "PlaceOrder";
                View placeOrderView = new View();
                placeOrderView.ID = "placeOrderView";
                placeOrderView.Controls.Add(placeOrder);
                FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, placeOrderView);

                if (PayMethod == "1")
                {
                    //placeOrderView.ID = "placeOrderView";
                    //placeOrderView.Controls.Add(placeOrder);
                    //FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, placeOrderView);

                    PayWithCreditCard = (Units.Placeable.GasellFlow.PayWithCreditCard)Page.LoadControl("/Templates/Public/Units/Placeable/GasellFlow/PayWithCreditCard.ascx");
                    PayWithCreditCard.ID = "PaywithCreditCard";
                    View payWithCreditCardView = new View();
                    payWithCreditCardView.ID = "payWithCreditCardView"; 
                    payWithCreditCardView.Controls.Add(PayWithCreditCard);
                    FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, payWithCreditCardView);
                }

                if (PayMethod == "2")
                {
                    //placeOrderView.ID = "placeOrderView";
                    //placeOrderView.Controls.Add(placeOrder);
                    //FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, placeOrderView);

                    PayWithInvoice = (PayWithInvoice)Page.LoadControl("/Templates/Public/Units/Placeable/GasellFlow/PayWithInvoice.ascx");
                    PayWithInvoice.ID = "PayWithInvoice";
                    View payWithInvoiceView = new View();
                    payWithInvoiceView.ID = "payWithInvoiceView";
                    payWithInvoiceView.Controls.Add(PayWithInvoice);
                    FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, payWithInvoiceView);
                }

                if (PayMethod == "3")
                {

                    //placeOrderView.ID = "placeOrderView";
                    //placeOrderView.Controls.Add(placeOrder);
                    //FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, placeOrderView);

                    PayWithDiscountCode = (PayWithDiscountCode)Page.LoadControl("/Templates/Public/Units/Placeable/GasellFlow/PayWithDiscountCode.ascx");
                    PayWithDiscountCode.ID = "PayWithDiscountCode";
                    View payWithDiscountCodeView = new View();
                    payWithDiscountCodeView.ID = "payWithDiscountCodeView";
                    payWithDiscountCodeView.Controls.Add(PayWithDiscountCode);
                    FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, payWithDiscountCodeView);
                }

                Receipt = (Receipt)Page.LoadControl("/Templates/Public/Units/Placeable/GasellFlow/Receipt.ascx");
                Receipt.ID = "Receipt";
                View receiptView = new View();
                receiptView.ID = "receiptView";
                receiptView.Controls.Add(Receipt);
                FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, receiptView);

                FlowMultiView.DataBind();
            //}
            //catch (Exception ex)
            //{
            //    new Logger("GasellMultiview - failed", ex.ToString());
            //    ShowMessage("/conference/errors/error", true, true);
            //}

            #endregion

            //DataBind();

            #region This happens when everything went well with Nets credit card payment

            if (Request.QueryString["gv"] != null && (Request.QueryString["gv"] == "t"))
            {
                FlowMultiView.ActiveViewIndex = GasellView + 2;

                //Set status of card payment to ok for all gasellusers in this order
                foreach (int gasellOrderID in GasellOrder.OrderIDList)
                {
                    MsSqlHandler.UpdateGasellUserPayInfo(gasellOrderID, "Credit Card - OK");
                }

                //MiscFunctions.SendMail(ConfirmationMailFrom, GasellOrder.GasellUsersList[0].Email, ConfirmationMailSubject, ConfirmationMailBody, true);
                SendMailToAllParticipants(GasellOrder.GasellUsersList);

                Receipt.PopulateParticipantsRepeater(GasellOrder.GasellUsersList);
            }
            else
            {
                FlowMultiView.ActiveViewIndex = 0;
            }

            #endregion
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            SideBarStepBox sidebar = (SideBarStepBox)Page.LoadControl("/Templates/Public/Units/Placeable/GasellFlow/SideBarStepBox.ascx");
            //sidebar.ID = "SideBarStepBox";
            //StepBoxPlaceHolder.EnableViewState = false;
            //sidebar.EnableViewState = false;
            StepBoxPlaceHolder.Controls.Add(sidebar);

            NextButton.Visible = FlowMultiView.ActiveViewIndex != FlowMultiView.Views.Count - 1;
            BackButtonLinkButton.Visible = FlowMultiView.ActiveViewIndex != FlowMultiView.Views.Count - 1;

            if (FlowMultiView.ActiveViewIndex == 0)
            {
                GasellUsers = new List<GasellUser>();
            }

            //Set Google Analytics virtual page tracking url:
            // If user have a cookie set from a GoogleAnalyticsRedirect pagetype page, we want to use utm_source and utm_medium as well
            var parts = new List<string>();
            var googleCookieValues = GoogleAnalyticsHelper.ReadCookie(this.Context, GoogleAnalyticsHelper.CookieName);
            parts.AddRange(from googleCookieKeyValue in googleCookieValues
                where
                    string.Compare(googleCookieKeyValue.Key, GoogleAnalyticsHelper.GoogleTrackParams.utm_source.ToString(), true, CultureInfo.InvariantCulture) == 0
                    ||
                    string.Compare(googleCookieKeyValue.Key, GoogleAnalyticsHelper.GoogleTrackParams.utm_medium.ToString(), true, CultureInfo.InvariantCulture) == 0
                select googleCookieKeyValue.Value
                );
            parts.Add(GasellPageFriendlyUrl);
            parts.Add(GoogleTrackDatePart);

            switch (PayMethod)
            {
                case "1":
                    parts.Add("kort");
                    break;
                case "2":
                    parts.Add("faktura");
                    break;
                case "3":
                    parts.Add("rabattkod");
                    break;
            }
            var googleTrackStepPart = string.Empty;
            var includeParticipants = false;
            if (FlowMultiView.ActiveViewIndex <= Convert.ToInt32(NumberOfParticipants) - 1)
            {
                googleTrackStepPart = "steg1";
                includeParticipants = true;
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumberOfParticipants))
            {
                googleTrackStepPart = "steg2";
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumberOfParticipants) + 1)
            {
                googleTrackStepPart = "steg3";
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumberOfParticipants) + 2)
            {
                googleTrackStepPart = "steg4";
            }
            parts.Add(googleTrackStepPart);
            if (includeParticipants)
            {
                parts.Add(string.Format("/deltagare{0}", GasellUsers.Count + 1));
            }
            GoogleVirtualUrl = GoogleAnalyticsHelper.BuildVirtualPageString(parts);
        }

        #endregion

        #region Methods

        protected void NextButton_Click(object sender, EventArgs e)
        {
            if (FlowMultiView.ActiveViewIndex <= Convert.ToInt32(NumberOfParticipants) - 1)
            { 
                View activeView = FlowMultiView.GetActiveView();
                RegisterParticipants registerParticipant = activeView.FindControl("RegisterParticipants" + FlowMultiView.ActiveViewIndex) as RegisterParticipants;
                if (registerParticipant != null)
                {
                    GasellUser gasellUser = registerParticipant.GetUser();

                    if (gasellUser != null)
                    {
                        GasellUsers.Add(gasellUser);
                    }
                }
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumberOfParticipants) + 1)
            {
                if (PayMethod == "1")
                {
                    try
                    {
                        //gasellOrder = new GasellOrder(GasellUsers, null, null, gasellID, payMethod);
                        Receipt.PopulateParticipantsRepeater(GasellUsers);

                    }
                    catch (System.Exception ex)
                    {
                        //TODO: Change it?
                        new Logger("GasellPaymentMethodCreditCard() - failed", ex.ToString());
                        ShowMessage("/conference/errors/error", true, true);
                    }
                }

                if (PayMethod == "2")
                {
                    try
                    {
                        GasellCompany company = PayWithInvoice.GetCompany();
                        GasellOrder = new GasellOrder(GasellUsers, company, null, GasellId , PayMethod);
                        //DIClassLib.Misc.MiscFunctions.SendMail(ConfirmationMailFrom, GasellUsers[0].Email, ConfirmationMailSubject, ConfirmationMailBody, true);
                        SendMailToAllParticipants(GasellUsers);
                        Receipt.PopulateParticipantsRepeater(GasellUsers);
                    }
                    catch (System.Exception ex)
                    {
                        //TODO: Change it?
                        new Logger("GasellPaymentMethodInvoice() - failed", ex.ToString());
                        ShowMessage("/conference/errors/error", true, true);
                    }
                }

                if (PayMethod == "3")
                {
                    try
                    {
                        GasellDiscountCode discountCode = PayWithDiscountCode.GetDiscountCode();
                        string[] discountCodes = EPiFunctions.SettingsPageSetting(CurrentPage, "DiscountCodes").ToString().Split(';');
                        bool isValidCode = false;
                        foreach (string code in discountCodes)
                        {
                            if (code.ToLower() == discountCode.DiscountCode.ToLower())
                            {
                                GasellOrder = new GasellOrder(GasellUsers, null, discountCode.DiscountCode, GasellId, PayMethod);
                                //DIClassLib.Misc.MiscFunctions.SendMail(ConfirmationMailFrom, GasellUsers[0].Email, ConfirmationMailSubject, ConfirmationMailBody, true);
                                SendMailToAllParticipants(GasellUsers);
                                Receipt.PopulateParticipantsRepeater(GasellUsers);
                                isValidCode = true;
                                break;
                            }
                        }

                        if (!isValidCode)
                            return;
                    }
                    catch (System.Exception ex)
                    {
                        //TODO: Change it?
                        new Logger("GasellPaymentMethodDiscountCode() - failed", ex.ToString());
                        ShowMessage("/conference/errors/error", true, true);
                    }
                }
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumberOfParticipants))
            {
                if (PayMethod == "1")
                {
                    try
                    {
                        GasellOrder = new GasellOrder(GasellUsers, null, null, GasellId, PayMethod);
                        GasellView = FlowMultiView.ActiveViewIndex;
                    }
                    catch (Exception ex)
                    {
                        new Logger("GasellFlow() failed before Nets redir", ex.ToString());
                    }
              
                    RedirectToNets();
                }
            }
            
            FlowMultiView.ActiveViewIndex++;
        }

        protected void BackButtonLinkButton_Click(object sender, EventArgs e)
        {
            if (FlowMultiView.ActiveViewIndex == 0)
            {
                PageData pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(Convert.ToInt32(GasellId)));

                Response.Redirect(pd.LinkURL);
            }
            else
            {
                if (FlowMultiView.ActiveViewIndex-1 <= Convert.ToInt32(NumberOfParticipants) - 1)
                {
                    GasellUsers.RemoveAt(GasellUsers.Count-1);
                }
                FlowMultiView.ActiveViewIndex--;
            }
        }

        public string GetHeaderForStep()
        {
            string header = String.Empty;

            if(FlowMultiView.ActiveViewIndex <= Convert.ToInt32(NumberOfParticipants) - 1)
            {
                header = "Registrera deltagare " + (FlowMultiView.ActiveViewIndex + 1) + " av " + NumberOfParticipants;
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumberOfParticipants))
            {
                header = "Bekräfta beställning";
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumberOfParticipants) + 1)
            {
                if (PayMethod == "1")
                {
                    header = "Betala med kort";
                }

                if (PayMethod == "2")
                {
                    header = "Betala med faktura";
                }

                if (PayMethod == "3")
                {
                    header = "Betala med rabattkod";
                }
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumberOfParticipants) + 2)
            {
                header = "Bekräftelse";
            }
            else
            {
                header = "";
            }

            return header;
        }

        public void RedirectToNets()
        {
            string gasellFlowPageURL = "";

            PageData GasellFlowPage = new PageData();

            if (EPiFunctions.SettingsPageSetting(CurrentPage, "GasellFlowPage") != null)
            {                
                GasellFlowPage = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(CurrentPage, "GasellFlowPage") as PageReference);
                gasellFlowPageURL = GasellFlowPage.LinkURL;
            }

            PageData pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(GasellId));

            string returnURL = string.Empty; 
            string goodsDescription = string.Empty;
            
            returnURL = EPiFunctions.GetFriendlyAbsoluteUrl(GasellFlowPage);
            goodsDescription = pd.PageName;

            int numOfParticipants = Convert.ToInt32(Request.QueryString["nop"]);
            int numOfPartToPay = numOfParticipants - (numOfParticipants / 4);

            double amount = Convert.ToDouble(pd["Price"].ToString()) * Convert.ToDouble(numOfPartToPay);
            double vatAmount = ((25 / 100.0) * amount);
            string consumerName = string.Format("{0} {1}", GasellOrder.GasellUsersList[0].FirstName, GasellOrder.GasellUsersList[0].LastName);
            string consumerEmail = GasellOrder.GasellUsersList[0].Email;
            string comment = Translate("/gasell/gasell");

            var prep = new NetsCardPayPrepare(amount, vatAmount, Settings.VatGasellFlow, false, false, returnURL, goodsDescription, comment, consumerName, consumerEmail, null, PaymentMethod.TypeOfPaymentMethod.CreditCard, GasellOrder);
        }

        private void SendMailToAllParticipants(List<GasellUser> users)
        {
            string mailBody = string.Format(Translate("/gasell/mail/confirmation/body"), MailCity, MailDate, MailTime, MailPrice, MailPayMethod, GetBrSeparatedUserList(users));

            foreach (var usr in users)
            {
                var mail = usr.Email;
                if (MiscFunctions.IsValidEmail(mail))
                    MiscFunctions.SendMail(ConfirmationMailFrom, mail, ConfirmationMailSubject, mailBody, true);
            }
        }

        private object GetBrSeparatedUserList(List<GasellUser> users)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var usr in users)
                sb.Append(usr.FirstName + " " + usr.LastName + "<br>");

            return sb.ToString();
        }

        #endregion
    }
}
