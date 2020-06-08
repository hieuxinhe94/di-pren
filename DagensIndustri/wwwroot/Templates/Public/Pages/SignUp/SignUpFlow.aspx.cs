using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DIClassLib.CardPayment.Nets;
using DIClassLib.Subscriptions;

using EPiServer;
using EPiServer.Core;
using DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow;
using DagensIndustri.Tools.Classes.SignUp;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes;
using DIClassLib.CardPayment;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Templates.Public.Pages.SignUp
{
    public partial class SignUpFlow : DiTemplatePage
    {
        #region Properties

        public PageData PdSignUpPage = null;
        public int EventId = 0;
        public int NumParticipants { get; set; }
        public int PayMethod { get; set; }
        public bool AurigaStatus { get; set; }
        public string Step { get; set; }

        public MultiView MyFlowMultiView
        {
            get { return FlowMultiView; }
        }
        public List<SignUpUser> ListUsers
        {
            get 
            { 
                //return (List<SignUpUser>)ViewState["SignUpUserList"]; 
                return (List<SignUpUser>)Session["SignUpUserList"]; 
            }
            set 
            { 
                //ViewState["SignUpUserList"] = value; 
                Session["SignUpUserList"] = value; 
            }
        }
        public RegisterParticipants Participants;
        public PayWithCreditCard PayWithCreditCard;
        public Receipt Receipt;
        
        public SignUpOrder SignUpOrder
        {
            get 
            {
                if (Session["SignUpOrderObject"] != null)
                    return Session["SignUpOrderObject"] as SignUpOrder;

                    return null;
            }
            set
            {
                Session["SignUpOrderObject"] = value;
            }
        }

        public int ActiveViewIndex
        {
            get 
            {
                if (Session["SignUpView"] != null)
                    return int.Parse(Session["SignUpView"].ToString());

                    return 0;
            }
            set
            {
                Session["SignUpView"] = value;
            }
        }

        #endregion

        
        #region Events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl;

            bool cardPayOk = false;
            
            EventId = SetIntFromUrl("eventId");
            NumParticipants = SetIntFromUrl("nop");
            PayMethod = SetIntFromUrl("pm");

            if (Request.QueryString["responseCode"] != null)
            {
                NetsCardPayReturn ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                SignUpOrder = (SignUpOrder)ret.NetsPreparePersisted.PersistedObj;
                NumParticipants = SignUpOrder.ListUsers.Count;
                EventId = SignUpOrder.EpiPageId;
                PayMethod = SignUpOrder.PayMethod;

                cardPayOk = ret.HandleNetsReturn("", SignUpOrder.ListUsers[0].Email);
                if (cardPayOk)
                {
                    foreach (int orderId in SignUpOrder.ListOrderIds)
                        MsSqlHandler.UpdateSignUpPaymentStatus(orderId, "Credit Card - OK");

                    Response.Redirect(CurrentPage.LinkURL + "&eventId=" + EventId + "&pm=" + PayMethod + "&nop=" + NumParticipants + "&gv=t&pay=1", true);
                    //Response.Redirect(EPiFunctions.GetFriendlyUrl(CurrentPage) + "?eventId=" + EventId + "&pm=" + PayMethod + "&nop=" + NumParticipants + "&gv=t&pay=1", true);
                }
            }


            if (EventId > 0)
                PdSignUpPage = EPiServer.DataFactory.Instance.GetPage(new PageReference(EventId));


            #region Create the views for the multiview control
            
            int i;

            for (i = 0; i < Convert.ToInt32(NumParticipants); i++)
            {
                Participants = (RegisterParticipants)Page.LoadControl("/Templates/Public/Units/Placeable/SignUpFlow/RegisterParticipants.ascx");
                Participants.ID = "RegisterParticipants" + i;
                View view = new View();
                view.ID = "View" + i;
                view.Controls.Add(Participants);
                FlowMultiView.Views.AddAt(i, view);
            }

            PlaceOrder placeOrder = (PlaceOrder)Page.LoadControl("/Templates/Public/Units/Placeable/SignUpFlow/PlaceOrder.ascx");
            placeOrder.ID = "PlaceOrder";
            View placeOrderView = new View();
            placeOrderView.ID = "placeOrderView";
            placeOrderView.Controls.Add(placeOrder);
            FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, placeOrderView);

            if (PayMethod == 1)
            {
                PayWithCreditCard = (PayWithCreditCard)Page.LoadControl("/Templates/Public/Units/Placeable/SignUpFlow/PayWithCreditCard.ascx");
                PayWithCreditCard.ID = "PaywithCreditCard";
                View payWithCreditCardView = new View();
                payWithCreditCardView.ID = "payWithCreditCardView";
                payWithCreditCardView.Controls.Add(PayWithCreditCard);
                FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, payWithCreditCardView);
            }

            Receipt = (Receipt)Page.LoadControl("/Templates/Public/Units/Placeable/SignUpFlow/Receipt.ascx");
            Receipt.ID = "Receipt";
            View receiptView = new View();
            receiptView.ID = "receiptView";
            receiptView.Controls.Add(Receipt);
            FlowMultiView.Views.AddAt(FlowMultiView.Views.Count, receiptView);

            FlowMultiView.DataBind();
            

            #endregion

            
            if (Request.QueryString["gv"] == null)
                FlowMultiView.ActiveViewIndex = 0;


            if (Request.QueryString["pay"] == "1")
            {
                SendConfMail();
                Receipt.PopulateParticipantsRepeater(SignUpOrder.ListUsers);
                FlowMultiView.ActiveViewIndex = (FlowMultiView.Views.Count - 1);  //receipt is last view
            }

            
            #region old set email info

            //if (_eventId > 0 && _pdSignUpPage["SendConfMail"] != null)
            //{
            //string mailPaymentMethod = string.Empty;
            //int numOfParticipants = Convert.ToInt32(Request.QueryString["nop"]);
            //string price = _pdSignUpPage["Price"].ToString();

            //if (_payMethod == "1")
            //    mailPaymentMethod = "Kort";
            //else if (_payMethod == "2")
            //    mailPaymentMethod = "Faktura";

            //_confMailFrom = _pdSignUpPage["ConfMailSenderAddress"].ToString() ?? "no-reply@di.se";  //EPiFunctions.SettingsPageSetting(CurrentPage, "SignUpConfirmationMailFrom").ToString() ?? "no-reply@di.se";
            //_confMailSubject = _pdSignUpPage["ConfMailHeader"].ToString() ?? "";                    //string.Format(Translate("/signup/mail/confirmation/subject"), pd["EventName"]);
            //_confMailBody = _pdSignUpPage["ConfMailText"].ToString() ?? "";                         //string.Format(Translate("/signup/mail/confirmation/body"), pd["EventName"], EPiFunctions.GetDate(pd, "Date"), (Convert.ToDateTime(pd["Date"])).ToString("HH:mm"), price, mailPaymentMethod);
            //}

            #endregion Mail information
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            SideBarStepBox sidebar = (SideBarStepBox)Page.LoadControl("/Templates/Public/Units/Placeable/SignUpFlow/SideBarStepBox.ascx");
            StepBoxPlaceHolder.Controls.Add(sidebar);

            NextButton.Visible = FlowMultiView.ActiveViewIndex != FlowMultiView.Views.Count - 1;
            BackButtonLinkButton.Visible = FlowMultiView.ActiveViewIndex != FlowMultiView.Views.Count - 1;

            if (FlowMultiView.ActiveViewIndex == 0)
                ListUsers = new List<SignUpUser>();
        }

        #endregion


        #region Methods

        protected void NextButton_Click(object sender, EventArgs e)
        {
            int actIndex = FlowMultiView.ActiveViewIndex;

            if (actIndex <= NumParticipants - 1)
            {
                View activeView = FlowMultiView.GetActiveView();
                RegisterParticipants registerParticipant = activeView.FindControl("RegisterParticipants" + FlowMultiView.ActiveViewIndex) as RegisterParticipants;
                
                if (registerParticipant != null)
                {
                    SignUpUser user = registerParticipant.GetUser();
                    if (user != null)
                        ListUsers.Add(user);
                }
            }
            else if (actIndex == NumParticipants + 1)
            {
                if (PayMethod == 1)
                {
                    try
                    {
                        Receipt.PopulateParticipantsRepeater(ListUsers);
                    }
                    catch (Exception ex)
                    {
                        new Logger("SignUpFlow 10 - failed", ex.ToString());
                        ShowMessage("/conference/errors/error", true, true);
                    }
                }
            }
            else if (actIndex == NumParticipants)
            {
                if (PayMethod == 1)
                {
                    try
                    {
                        SignUpOrder = new SignUpOrder(ListUsers, EventId, PayMethod);
                        ActiveViewIndex = FlowMultiView.ActiveViewIndex;
                    }
                    catch (Exception ex)
                    {
                        new Logger("SignUpFlow 20 - failed", ex.ToString());
                        ShowMessage("/conference/errors/error", true, true);
                    }

                    RedirectToNets();
                }

                if (PayMethod == 2)
                {
                    try
                    {
                        SignUpOrder = new SignUpOrder(ListUsers, EventId, PayMethod);
                        Receipt.PopulateParticipantsRepeater(ListUsers);
                        SendConfMail();
                    }
                    catch (Exception ex)
                    {
                        new Logger("SignUpFlow 30 - failed", ex.ToString());
                        ShowMessage("/conference/errors/error", true, true);
                    }
                }

            }

            FlowMultiView.ActiveViewIndex++;
        }

        protected void BackButtonLinkButton_Click(object sender, EventArgs e)
        {
            if (FlowMultiView.ActiveViewIndex == 0)
            {
                PageData pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(Convert.ToInt32(EventId)));
                Response.Redirect(pd.LinkURL);
            }
            else
            {
                if (FlowMultiView.ActiveViewIndex - 1 <= Convert.ToInt32(NumParticipants) - 1)
                    ListUsers.RemoveAt(ListUsers.Count - 1);
                
                FlowMultiView.ActiveViewIndex--;
            }
        }

        public string GetHeaderForStep()
        {
            string header = String.Empty;

            if (FlowMultiView.ActiveViewIndex <= Convert.ToInt32(NumParticipants) - 1)
            {
                header = "<h1>Registrera deltagare " + (FlowMultiView.ActiveViewIndex + 1) + " av " + NumParticipants + "</h1>";

                if (NumParticipants > 1 && PayMethod == 2)
                    header += "<p style='margin-left:20px; margin-top:-15px;'>Faktura skickas till deltagare 1</p>";

            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumParticipants))
            {
                header = "<h1>Bekräfta beställning</h1>";
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumParticipants) + 1)
            {
                if (PayMethod == 1)
                {
                    header = "<h1>Betala med kort</h1>";
                }

                if (PayMethod == 2)
                {
                    header = "<h1>Betala med faktura</h1>";
                }
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(NumParticipants) + 2)
            {
                header = "<h1>Bekräftelse</h1>";
            }
            else
            {
                header = "";
            }

            return header;
        }

        public void RedirectToNets()
        {
            string returnURL = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string goodsDescription = PdSignUpPage.PageName;

            double amount = Convert.ToDouble(PdSignUpPage["Price"].ToString()) * NumParticipants;
            double vatPct = Convert.ToDouble(PdSignUpPage["VatPercent"].ToString());
            //double vatAmount = ((vat / 100.0) * amount);
            string consumerName = string.Format("{0} {1}", SignUpOrder.ListUsers[0].FirstName, SignUpOrder.ListUsers[0].LastName);
            string consumerEmail = SignUpOrder.ListUsers[0].Email;
            string comment = PdSignUpPage.PageName;

            var prep = new NetsCardPayPrepare(amount, null, vatPct, true, false, returnURL, goodsDescription, comment, consumerName, consumerEmail, null, PaymentMethod.TypeOfPaymentMethod.CreditCard, SignUpOrder);
        }

        private void SendConfMail()
        {
            if (PdSignUpPage["SendConfMail"] != null)
                DIClassLib.Misc.MiscFunctions.SendMail(PdSignUpPage["ConfMailSenderAddress"].ToString(), SignUpOrder.ListUsers[0].Email, PdSignUpPage["ConfMailHeader"].ToString(), PdSignUpPage["ConfMailText"].ToString(), true);
        }

        private int SetIntFromUrl(string key)
        {
            int ret = 0;

            if (Request.QueryString[key] == null)
                return ret;

            int.TryParse(Request.QueryString[key].ToString(), out ret);

            return ret;
        }

        #endregion
    }
}