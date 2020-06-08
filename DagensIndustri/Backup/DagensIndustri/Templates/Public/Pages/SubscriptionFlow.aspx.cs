using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using DIClassLib.CardPayment.Nets;

using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Membership;
using DIClassLib.CardPayment;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class SubscriptionFlow : DiTemplatePage
    {
        #region Properties
        /// <summary>
        /// Type of subscription the user wants to subscribe to
        /// </summary>
        public SubscriptionType.TypeOfSubscription SubscriptionType 
        { 
            get
            {
                return (SubscriptionType.TypeOfSubscription)ViewState["SubscriptionType"];
            }
            set
            {
                ViewState["SubscriptionType"] = value;
            }
        }

        /// <summary>
        /// CampaignNo 1
        /// </summary>
        public long CampaignNo1
        {
            get
            {
                return Convert.ToInt64(ViewState["CampaignNo1"]);
            }
            set
            {
                ViewState["CampaignNo1"] = value;
            }
        }

        /// <summary>
        /// CampaignNo 2
        /// </summary>
        public long CampaignNo2
        {
            get
            {
                return Convert.ToInt64(ViewState["CampaignNo2"]);
            }
            set
            {
                ViewState["CampaignNo2"] = value;
            }
        }

        /// <summary>
        /// Defines whether user wants to become a Di Gold member or not
        /// </summary>
        public bool BecomeDiGoldMember
        {
            get
            {
                return ViewState["BecomeDiGoldMember"] != null && (bool)ViewState["BecomeDiGoldMember"];
            }
            set
            {
                ViewState["BecomeDiGoldMember"] = value;
            }
        }
       
        private Subscription Subscription1
        {
            get
            {
                return (Subscription)ViewState["Subscription1"];
            }
            set
            {
                ViewState["Subscription1"] = value;
            }
        }

        private Subscription Subscription2
        {
            get
            {
                return (Subscription)ViewState["Subscription2"];
            }
            set
            {
                ViewState["Subscription2"] = value;
            }
        }        
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            
            if (!IsPostBack)
            {
                //This happens only when the visitor have been to Nets, paymethod credit card
                if (Request.QueryString["responseCode"] != null)
                {
                    string successUrl = CurrentPage.LinkURL + "&sv=t";
                    NetsCardPayReturn ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                    PopulatePropsFromPersistedObj((SubsPersistWrapper)ret.NetsPreparePersisted.PersistedObj);

                    bool paySuccess = ret.HandleNetsReturn(successUrl, Subscription1.Subscriber.Email);
                    if (paySuccess)
                    {
                        SaveSubsToDbs(ret.NetsPreparePersisted.CustomerRefNo);
                    }
                }
                else
                {
                    if (Request.QueryString["sv"] == null)
                    {
                        SubscriptionMultiView.ActiveViewIndex = 0;
                        //Set selected subscription type to ordinary Di subs
                        SubscriptionType = DIClassLib.Subscriptions.SubscriptionType.TypeOfSubscription.Di;
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            BackLinkButton.Visible = SubscriptionMultiView.ActiveViewIndex > 0;
            if (SubscriptionMultiView.ActiveViewIndex == 0)
            {
                ContinueButton.Visible = false;
                AlterBodyCss(true);
                ((MasterPages.MasterPage)Page.Master).ShowSideBarBoxes(false);
            }
            else
            {
                ContinueButton.Visible = SubscriptionMultiView.ActiveViewIndex <= SubscriptionMultiView.Views.Count - 1;
                AlterBodyCss(false);
            }

            LoadTopImages();
            HeadingPlaceHolder.Visible = !string.IsNullOrEmpty(GetHeading());
        }

        public void ContinueButton_Click(object sender, EventArgs e)
        {
            UserMessageControl.ClearMessage();
            bool createSubs = false;
                        
            if (SubscriptionMultiView.GetActiveView().ID == SubscriptionMatrixView.ID)
            {
                //If user is leaving the matrix view, then we have to check what subscription he/she has choosen and 
                //whether he/she has decided to become a Di Gold member or not.
                HandleCommandArgument(sender);
                SubscriptionMultiView.SetActiveView(SubscriberDetailsView);                
            }            
            else if (SubscriptionMultiView.GetActiveView().ID == SubscriberDetailsView.ID)
            {
                if (SubscriberDetailsControl.IsValid())
                {
                    //If user is leaving the subscriber detailsview, check if he/she has chosen another invoice recipient. 
                    //If so, move to InvoiceRecipientView.
                    //Otherwise if subscriber has chosen DiGold and accepted the promotional offer, go to PromotionalOfferView.
                    //Otherwise, subscriber is ready to be created.
                    if (SubscriberDetailsControl.PaymentMethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer)
                    {
                        SubscriptionMultiView.SetActiveView(InvoiceRecipientView);
                    }
                    else
                    {
                        if (BecomeDiGoldMember && SubscriberDetailsControl.AcceptedPromotionalOffer)
                        {
                            SubscriptionMultiView.SetActiveView(PromotionalOfferView);
                        }
                        else
                        {
                            createSubs = true;
                        }
                    }
                }
            }
            else if (SubscriptionMultiView.GetActiveView().ID == InvoiceRecipientView.ID)
            {
                if (InvoiceRecipientControl.IsValid())
                {
                    //If user is leaving InvoiceRecipientView, check if he/she has chosen DiGold and 
                    //accepted the promotional offer, go to PromotionalOfferView.
                    //Otherwise, subscriber is ready to be created.
                    if (BecomeDiGoldMember && SubscriberDetailsControl.AcceptedPromotionalOffer)
                    {
                        SubscriptionMultiView.SetActiveView(PromotionalOfferView);
                    }
                    else
                    {
                        createSubs = true;
                    }
                }
            }
            else if (SubscriptionMultiView.GetActiveView().ID == PromotionalOfferView.ID)
            {
                if (PromotionalOfferControl.IsValid())
                {
                    createSubs = true;
                }
            }
            
            //It is time to create a subscription user and subscription in Cirix
            if (createSubs)
            {
                HandleSubscriptionFlow();
            }
        }

        protected void BackLinkButton_Click(object sender, EventArgs e)
        {
            UserMessageControl.ClearMessage();

            if (SubscriptionMultiView.GetActiveView().ID == PromotionalOfferView.ID)
            {
                if (SubscriberDetailsControl.PaymentMethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer)
                    SubscriptionMultiView.ActiveViewIndex -= 1;
                else
                    SubscriptionMultiView.ActiveViewIndex -= 2;
            }
            else
            {
                SubscriptionMultiView.ActiveViewIndex--;
            }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Load top images. If matris view, load all images, otherwise only the image for the selected subscription type
        /// </summary>
        private void LoadTopImages()
        {
            if (SubscriptionMultiView.GetActiveView().ID == SubscriptionMatrixView.ID)
            {
                TopImageControl.LoadAllImages();
            }
            else
            {
                TopImageControl.LoadSelectedSubscriptionTypeImage();
            }
        }

        /// <summary>
        /// Alter css of body
        /// </summary>
        /// <param name="addCss"></param>
        private void AlterBodyCss(bool addCss)
        {
            HtmlGenericControl body = Master.FindControl("Body") as HtmlGenericControl;
            if (body != null)
            {
                if (addCss)
                {
                    body.Attributes.Add("class", "matrix");
                }
                else
                {
                    body.Attributes.Remove("class");
                }
            }
        }

        /// <summary>
        /// Get heading of the page
        /// </summary>
        /// <returns></returns>
        protected string GetHeading()
        {
            string heading = string.Empty;
            if (SubscriptionMultiView.GetActiveView().ID == SubscriberDetailsView.ID)
            {
                heading = SubscriberDetailsControl.Heading;
            }
            else if (SubscriptionMultiView.GetActiveView().ID == InvoiceRecipientView.ID)
            {
                heading = InvoiceRecipientControl.Heading;
            }
            else if (SubscriptionMultiView.GetActiveView().ID == PromotionalOfferView.ID)
            {
                heading = PromotionalOfferControl.Heading;
            }
            return heading;
        }

        /// <summary>
        /// Handle command argument if it exists
        /// </summary>
        /// <param name="sender"></param>
        private void HandleCommandArgument(object sender)
        {
            string commandArgument = ((Button)sender).CommandArgument;
            if (!string.IsNullOrEmpty(commandArgument))
            {
                //If the click event occured in the first page, command argument is passed.
                //The command argument then consists of format "SubscriptionType|CampaignNo1|CampaignNo2|User wants to become Di Gold member" i.e. (2|1234|1201|true)
                //The CampaignNo 1 and 2 are only given when Di Premium, otherwise only CampaignNo1 is given.
                string[] args = commandArgument.Split('|');

                int subscriptionType;
                if (int.TryParse(args[0], out subscriptionType))
                {
                    SubscriptionType = (SubscriptionType.TypeOfSubscription)subscriptionType;

                    //If type of subscription is direct debit, then direct debit section has to be shown in payment method area.
                    bool showDirectDebit = SubscriptionType == DIClassLib.Subscriptions.SubscriptionType.TypeOfSubscription.DiDirectDebit;
                    SubscriberDetailsControl.ShowDirectDebit(showDirectDebit);
                    
                }

                long tempCampaignNo;
                if (long.TryParse(args[1], out tempCampaignNo))
                {
                    CampaignNo1 = tempCampaignNo;
                    SubscriberDetailsControl.SetFirstStartSubsDate(GetFirstSubsStartDate(CampaignNo1));
                }

                tempCampaignNo = -1;
                if (long.TryParse(args[2], out tempCampaignNo))
                {
                    CampaignNo2 = tempCampaignNo;
                }

                bool becomeDiGoldMember;
                bool.TryParse(args[3], out becomeDiGoldMember);
                BecomeDiGoldMember = becomeDiGoldMember;

               

            }
        }

        private DateTime GetFirstSubsStartDate(long campNo)
        {
            DateTime dt = DateTime.MinValue;
            DataSet ds = CirixDbHandler.GetCampaign(campNo);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                String paperCode = ds.Tables[0].Rows[0]["PAPERCODE"].ToString();
                String productNo = ds.Tables[0].Rows[0]["PRODUCTNO"].ToString();
                return CirixDbHandler.GetNextIssueDateIncDiRules(DateTime.Now, paperCode, productNo);                
            }

            return dt;
        }

        private void HandleSubscriptionFlow()
        {            
            bool success = CreateSubscriptions();

            if (success)
            {
                if (SubscriberDetailsControl.PaymentMethod == PaymentMethod.TypeOfPaymentMethod.CreditCard)
                {
                    HandleCreditCardPayment();
                    return;
                }
                else
                    SaveSubsToDbs(0);
            }
        }

        /// <summary>
        /// Create subscription for the user. If CampaignNo2 contains a value, then it is a combo subscription. 
        /// A new subscription for that campaign is created as well.
        /// </summary>     
        private bool CreateSubscriptions()
        {
            Subscription subscription1 = null;
            Subscription subscription2 = null;

            //DateTime orderDate = DateTime.Now;
            DateTime subsStartDate = SubscriberDetailsControl.GetSelectedSubsStartDate();
            Person subscriberPerson = SubscriberDetailsControl.GetPerson();
            Person subscriptionPayerPerson = (SubscriberDetailsControl.PaymentMethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer) ? 
                                                InvoiceRecipientControl.GetPerson() : 
                                                null;

            subscriberPerson.IsGoldMember = BecomeDiGoldMember;
            
            if(subscriptionPayerPerson == null)
                subscriberPerson.IsPersonalEx = true;

            //Create a subscription with subscriber and subscriptionPayer (if exists)
            bool succeeded = CreateSubscription(CampaignNo1, subscriberPerson, subscriptionPayerPerson,subsStartDate, out subscription1);

            //If CampaignNo2 contains a value, then it is a combo subscription. Then we'll have to create a new subscription for that campaign no
            if (succeeded && CampaignNo2 > 0)
                succeeded = CreateSubscription(CampaignNo2, subscriberPerson, subscriptionPayerPerson,subsStartDate, out subscription2);

            string tgFallback = "IDAGENSIN";
            if (string.IsNullOrEmpty(subscription1.TargetGroup))
                subscription1.TargetGroup = tgFallback;

            if (subscription2 != null && string.IsNullOrEmpty(subscription2.TargetGroup))
                subscription2.TargetGroup = tgFallback;

            Subscription1 = subscription1;
            Subscription2 = subscription2;

            return succeeded;
        }

        /// <summary>
        /// Create a subscription and subscriber for a certain campaign
        /// </summary>
        /// <param name="campaignNo"></param>
        /// <param name="subscriptionType"></param>
        /// <param name="subscriber"></param>
        /// <param name="subscriptionPayer"></param>
        /// <param name="orderDate"></param>
        /// <param name="createNewSubscribers"></param>
        /// <param name="subscription"></param>
        /// <returns></returns>
        private bool CreateSubscription(long campaignNo, Person subscriber, Person subscriptionPayer, DateTime subStartDate, out Subscription subscription)
        {
            subscription = null;
            try
            {
                subscription = new Subscription(string.Empty, campaignNo, SubscriberDetailsControl.PaymentMethod, subStartDate, false)
                            {
                                Subscriber = subscriber,
                                SubscriptionPayer = subscriptionPayer
                            };
            }
            catch (Exception ex)
            {
                new Logger("CreateSubscription() - failed", ex.ToString());
                ShowMessage("/common/errors/errorcustomerservice", true, true);
                return false;
            }

            return true;
        }
        /// <summary>
        /// Add new subscriptions in cirix, save customer data in database and login user if everything was successful. 
        /// If user wanted to become a Di Gold member, add Di Gold role to him/her and if he/she had chosen promotional offer
        /// save that data in database as well. When everything is OK, redirect user to a proper landing page.
        /// </summary>      
        private void SaveSubsToDbs(int cusRefNo)
        {
            if (!TryInsertSubsInCirix())
            {
                ShowMessage("/common/errors/error", true, true);
                return;
            }

            if (cusRefNo > 0 && Subscription1.Subscriber != null)
                MsSqlHandler.AppendToPayTransComment(cusRefNo, "cusno: " + Subscription1.Subscriber.Cusno);

            bool loggedIn = HttpContext.Current.User.Identity.IsAuthenticated;
            bool savedInMssql = SaveSubsInMssql();

            if (savedInMssql && SubscriptionType != DIClassLib.Subscriptions.SubscriptionType.TypeOfSubscription.DiWeekend)
            {
                //Login user if subscriptions type is not DiWeekend. 
                loggedIn = LoginUtil.ReLoginUserRefreshCookie(Subscription1.Subscriber.UserName, Subscription1.Subscriber.Password);
                
                if (BecomeDiGoldMember && SubscriberDetailsControl.AcceptedPromotionalOffer)
                    TrySavePromotionalOffer();
            }

            if (savedInMssql)
                RedirectToLandingPage(BecomeDiGoldMember, loggedIn);
            else
                ShowMessage("/common/errors/error", true, true);
        }


        private bool TryInsertSubsInCirix()
        {
            if (!string.IsNullOrEmpty(CirixDbHandler.TryInsertSubscription2(Subscription1, null)))
                return false;

            if (Subscription2 != null)
            {
                Subscription2.Subscriber = Subscription1.Subscriber;
                Subscription2.SubscriptionPayer = Subscription1.SubscriptionPayer;
                if (!string.IsNullOrEmpty(CirixDbHandler.TryInsertSubscription2(Subscription2, null)))
                    return false;
            }

            return true;
        }

        private bool SaveSubsInMssql()
        {
            try
            {
                if (Subscription1.SubsNo > 0)
                {
                    long sub1Cusno = Subscription1.Subscriber.Cusno;
                    
                    DataSet dsCustomer = MsSqlHandler.GetCustomer(Subscription1.Subscriber.UserName);
                    if (dsCustomer == null || dsCustomer.Tables.Count == 0 || dsCustomer.Tables[0].Rows[0]["cusno"] == DBNull.Value)
                        MsSqlHandler.InsertCustomer(sub1Cusno, Subscription1.Subscriber.UserName, Subscription1.Subscriber.Password, Subscription1.Subscriber.Email, Subscription1.Subscriber.SocialSecurityNo);
                    else
                        MsSqlHandler.UpdateCustomer(sub1Cusno, Subscription1.Subscriber.UserName, Subscription1.Subscriber.Password, Subscription1.Subscriber.Email, Subscription1.Subscriber.SocialSecurityNo);

                    MsSqlHandler.InsertSubscription(Subscription1.SubsNo, sub1Cusno, Subscription1.ProductNo, Subscription1.PaperCode, Subscription1.SubsEndDate, true);

                    //combo subs, add second subs
                    if (Subscription2 != null)
                        MsSqlHandler.InsertSubscription(Subscription2.SubsNo, Subscription2.Subscriber.Cusno, Subscription2.ProductNo, Subscription2.PaperCode, Subscription2.SubsEndDate, true);

                    if (BecomeDiGoldMember)
                        DiRoleHandler.AddUserToRoles(Subscription1.Subscriber.UserName, new string[] { DiRoleHandler.RoleDiGold });

                    return true;
                }
            }
            catch (Exception ex)
            {
                new Logger("SaveSubsToMssql() - failed", ex.ToString());
                ShowMessage("/common/errors/errorcustomerservice", true, true);
            }

            return false;
        }

        private void TrySavePromotionalOffer()
        {
            try
            {
                //If user accepted the promotaional offer, save data in database
                PromotionalOfferControl.CustomerNumber = Subscription1.Subscriber.Cusno;
                PromotionalOfferControl.SaveData();
            }
            catch (Exception ex)
            {
                new Logger("SavePromotionalOffer() - failed", ex.ToString());
                ShowMessage("/common/errors/errorcustomerservice", true, true);
            }
        }

        /// <summary>
        /// Handle payment with CreditCard
        /// </summary>
        private void HandleCreditCardPayment()
        {
            string url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string goodsDescription = SubscriptionType.ToString();
            string comment = Translate("/subscription/newsubscription");
            string consumerName = string.Format("Subscriber {0} {1}", Subscription1.Subscriber.FirstName, Subscription1.Subscriber.LastName).Trim();
           
            double priceIncVat = CalculatePriceInclVAT(Subscription1);
            if (Subscription2 != null)
                priceIncVat += CalculatePriceInclVAT(Subscription2);

            double priceExVat = CalculatePriceExclVAT(Subscription1, Subscription2);
            double vatAmount = Math.Round(priceIncVat - priceExVat, MidpointRounding.AwayFromZero);

            var prep = new NetsCardPayPrepare(priceIncVat, vatAmount, null, true, false, url, goodsDescription, comment, consumerName, Subscription1.Subscriber.Email, null, PaymentMethod.TypeOfPaymentMethod.CreditCard, GetObjToPersist());
        }

        /// <summary>
        /// Calculate price with vat
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        private double CalculatePriceInclVAT(Subscription subscription)
        {
            double price = 0.0;
            if (subscription != null)
            {
                int vatPercentage = (subscription.PaperCode == Settings.PaperCode_DI)
                                        ? Convert.ToInt32(MiscFunctions.GetAppsettingsValue("DI_VATPercentage"))
                                        : Convert.ToInt32(MiscFunctions.GetAppsettingsValue("IPAD_VATPercentage"));

                double VAT = ((vatPercentage + 100) / 100.0);

                if (subscription.TotalPriceExVat > 0)
                    price = Math.Round(subscription.TotalPriceExVat * VAT, MidpointRounding.AwayFromZero);
            }
            return price;
        }

        /// <summary>
        /// Calculate price with vat
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        private double CalculatePriceExclVAT(Subscription subscription1, Subscription subscription2)
        {
            double price = 0.0;
            if (subscription1 != null)
                price = subscription1.TotalPriceExVat;

            if (subscription2 != null)
                price += subscription2.TotalPriceExVat;
            
            return price;
        }

        /// <summary>
        /// Get landing page url
        /// </summary>
        private void RedirectToLandingPage(bool redictToDiGoldLandingPage, bool userWasLoggedIn)
        {
            //When everything was saved successfully, redirect user to a standard landing page (if Di Gold membership was not accepted)
            //or to Di Gold landing page (if Di Gold membership was accepted)
            PageReference landingPagePageLink = redictToDiGoldLandingPage
                                              ? EPiFunctions.SettingsPageSetting(CurrentPage, "SubscriptionDiGoldLandingPage") as PageReference
                                              : EPiFunctions.SettingsPageSetting(CurrentPage, "SubscriptionStandardLandingPage") as PageReference;

            if (landingPagePageLink == null)
                landingPagePageLink = EPiFunctions.StartPage().PageLink;

            string url = EPiFunctions.GetFriendlyAbsoluteUrl(landingPagePageLink);

            if (userWasLoggedIn)
            {
                //If user was logged in, he/she was logged out and the session was abandoned. Therefore everything we store in session now, 
                // will be lost when the page is changed.
                url = string.Format("{0}?std={1}", url, Subscription1.SubsStartDate.ToShortDateString().Replace("-", ""));
            }
            else
            {
                Session["NewSubscriptionStartDate"] = Subscription1.SubsStartDate.ToShortDateString();
                Session["NewSubscriptionType"] = SubscriptionType;
            }

            Response.Redirect(url);
        }

        /// <summary>
        /// Save values in session so that user can get back to the same state
        /// </summary>      
        private SubsPersistWrapper GetObjToPersist()
        {
            SubsPersistWrapper subsObj = new SubsPersistWrapper()
            {
                TypeOfPaymentMethod = SubscriberDetailsControl.PaymentMethod,
                TypeOfSubscription = SubscriptionType,
                CampNo1 = CampaignNo1,
                CampNo2 = CampaignNo2,
                DiGoldMember = BecomeDiGoldMember,
                ActiveIndex = SubscriptionMultiView.ActiveViewIndex,
                Subs1 = Subscription1,
                Subs2 = Subscription2,
                AcceptedPromotionalOffer = SubscriberDetailsControl.AcceptedPromotionalOffer,
                OfferReciever = PromotionalOfferControl.GetDetails()
            };

            return subsObj;
        }

        /// <summary>
        /// Get the subscriptionSession object from session or file 
        /// </summary>
        /// <param name="id">Id of the file</param>
        private void PopulatePropsFromPersistedObj(SubsPersistWrapper subPers)
        {
            if (subPers != null)
            {
                SubscriberDetailsControl.PaymentMethod = subPers.TypeOfPaymentMethod;
                SubscriptionType = subPers.TypeOfSubscription;
                CampaignNo1 = subPers.CampNo1;
                CampaignNo2 = subPers.CampNo2;
                BecomeDiGoldMember = subPers.DiGoldMember;
                SubscriptionMultiView.ActiveViewIndex = subPers.ActiveIndex;
                Subscription1 = subPers.Subs1;
                Subscription2 = subPers.Subs2;
                SubscriberDetailsControl.AcceptedPromotionalOffer = subPers.AcceptedPromotionalOffer;

                SubscriptionMatrixControl.SetDiGoldMember();
                SubscriberDetailsControl.FillControl(Subscription1.Subscriber);
                InvoiceRecipientControl.FillControl(Subscription1.SubscriptionPayer);
                PromotionalOfferControl.FillControl(subPers.OfferReciever);
            }            
        }

  
        #endregion
    }

    #region Classes
    [Serializable]
    public class SubsPersistWrapper
    {
        public PaymentMethod.TypeOfPaymentMethod TypeOfPaymentMethod { get; set; }
        public SubscriptionType.TypeOfSubscription TypeOfSubscription { get; set; }
        public long CampNo1 { get; set; }
        public long CampNo2 { get; set; }
        public bool DiGoldMember { get; set; }        
        public int ActiveIndex { get; set; }
        public Subscription Subs1 { get; set; }
        public Subscription Subs2 { get; set; }
        public bool AcceptedPromotionalOffer { get; set; }
        public OfferRecieverDetails OfferReciever { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>SubsPersistWrapper</b><br>");
            sb.Append("TypeOfPaymentMethod:" + TypeOfPaymentMethod.ToString() + "<br>");
            sb.Append("TypeOfSubscription:" + TypeOfSubscription.ToString() + "<br>");
            sb.Append("CampNo1:" + CampNo1.ToString() + "<br>");
            sb.Append("CampNo2:" + CampNo2.ToString() + "<br>");
            sb.Append("DiGoldMember:" + DiGoldMember.ToString() + "<br>");
            sb.Append("ActiveIndex:" + ActiveIndex.ToString() + "<br>");
            sb.Append("AcceptedPromotionalOffer:" + AcceptedPromotionalOffer.ToString() + "<hr>");
            
            if (OfferReciever != null)
                sb.Append(OfferReciever.ToString());
            
            sb.Append(Subs1.ToString());
            
            if(Subs2 != null)
                sb.Append(Subs2.ToString());

            return sb.ToString();
        }
    }

    [Serializable]
    public class OfferRecieverDetails
    {
        public string StreetAddress { get; set; }
        public string HouseNo { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string CareOf { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>OfferRecieverDetails</b><br>");
            sb.Append("StreetAddress:" + StreetAddress + "<br>");
            sb.Append("HouseNo:" + HouseNo + "<br>");
            sb.Append("ZipCode:" + ZipCode + "<br>");
            sb.Append("City:" + City + "<br>");
            sb.Append("CareOf:" + CareOf + "<hr>");
            return sb.ToString();
        }
    }
#endregion
}