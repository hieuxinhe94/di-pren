using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using DIClassLib.BonnierDigital;
using DIClassLib.CardPayment.Nets;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.StudentVerification;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.AddCustAndSub;
using EPiServer.UI;
using PrenDiSe.Templates.Public.Units;
using PrenDiSe.Templates.Public.Units.CampaignTemplates;
using PrenDiSe.Tools.Classes;

namespace PrenDiSe.Templates.Public.Pages.CampaignTemplates.CampaignDigital
{
    public partial class Campaign : EPiServer.TemplatePage
    {
        private const string GeneralErrorMessage = "Ett tekniskt fel uppstod tyvärr då beställningen skulle göras.<br />Vänligen kontakta vår kundtjänst på tel <a href=\"tel:0857365100\">08-573 651 00</a>";
        private const string ServicePlusAppIdTemplate = "appId={0}";
        private const string QSTTARGETGROUP = "tg";
        private const string QSTPAYSUCCESS = "pay";
        private const string TokenKey = "token";
        private const string ReturnUrlKey = "returnurl";
        private const string SessionUserOriginUrlKey = "UserOriginUrl";

        private ServicePlusUserOutputWrapper SessionSavedUserData
        {
            get
            {
                return (ServicePlusUserOutputWrapper)Session["SessionSavedUserData"];
            }
            set
            {
                Session["SessionSavedUserData"] = value;
            }
        }

        private Uri ReturnUri { get; set; }
        
        private string UserOriginUrl
        {
            get { return Session[SessionUserOriginUrlKey] != null ? Session[SessionUserOriginUrlKey].ToString() : CurrentPage["DefaultReturnUrl"] != null ? CurrentPage["DefaultReturnUrl"] as string : "http://di.se"; }
        }

        private bool SaveAfterCardPayError{ get { return Request.QueryString["sacpf"] != null; } }

        /// <summary>
        /// retured from nets
        /// </summary>
        public string UrlTransactionId
        {
            get
            {
                var transId = Request.QueryString["transactionId"];

                if (transId != null)
                    return transId;

                return string.Empty;
            }
        }

        private bool ShowCampaign2 { get { return IsValue("Campaign2Id") || IsValue("Campaign2IdAutopay") || IsValue("Campaign2IdAutowithdrawal"); } }

        private string TargetGroup
        {
            get 
            {
                if (!string.IsNullOrEmpty(Request.QueryString[QSTTARGETGROUP]))
                    return Request.QueryString[QSTTARGETGROUP];

                var usrAgt = Request.UserAgent;
                var isMobile = !string.IsNullOrEmpty(usrAgt) && usrAgt.ToLower().Contains("mobile");

                return isMobile ? CurrentPage["TargetGroupMobile"] as string : CurrentPage["TargetGroup"] as string;            
            }
        }

        public string PrimaryCampaign { get { return "Campaign1"; } }
        public string SecondaryCampaign { get { return "Campaign2"; } }
        public string SelectedCampaign { get; set; }
        public Subscription Sub
        {
            get { return (Subscription)ViewState["subscription"]; }
            set { ViewState["subscription"] = value; }
        }

        protected Uri PageFriendlyUri { get; set; }
        public enum AuthenticatedStrings
        {
            LoggedIn,
            NotLoggedIn
        }
        protected AuthenticatedStrings GaAuthenticatedString { get; set; }

        private bool IsTrialPeriod
        {
            get
            {
                if (CurrentPage[SelectedCampaign + "IsTrial"] == null)
                    return false;

                return true;
            }
        }


        #region Event handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            plhMaintenanceScript.DataBind();            

            GaAuthenticatedString = SessionSavedUserData != null && !string.IsNullOrEmpty(SessionSavedUserData.Token) ? AuthenticatedStrings.LoggedIn : AuthenticatedStrings.NotLoggedIn;
            PageFriendlyUri = new Uri(EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage));
            UcCampaignForm.GaTrackingFirstPart = PageFriendlyUri.LocalPath; //For event tracking in the form

            SetReturnUrls();
            if (SessionSavedUserData == null)
            {
                SessionSavedUserData = new ServicePlusUserOutputWrapper();
            }

            if (CurrentPage["CampaignTheme"] as string == "CampThemeAgenda")
            {
                phAgendaCss.Visible = true;
            }
            if (CurrentPage["CampaignTheme"] as string == "CampThemeTPN")
            {
                phTpnCss.Visible = true;
            }
            UcCampaignForm.FirstNameReadOnly = true;
            UcCampaignForm.LastNameReadOnly = true;
            UcCampaignForm.EmailReadOnly = true; //Should not be possible to change email

            if (IsValue("ProductInfoImage"))
            {
                imgProductInfoImage.ImageUrl = CurrentPage["ProductInfoImage"] as string;
                imgProductInfoImageEnd.ImageUrl = imgProductInfoImage.ImageUrl;
                imgProductInfoImage.Visible = true;
                imgProductInfoImageEnd.Visible = true;
            }
            if (IsValue("ProductInfoImageMobile"))
            {
                imgProductInfoImageMobile.ImageUrl = CurrentPage["ProductInfoImageMobile"] as string;
                imgProductInfoImageMobileEnd.ImageUrl = imgProductInfoImageMobile.ImageUrl;
                imgProductInfoImageMobile.Visible = true;
                imgProductInfoImageMobileEnd.Visible = true;
            }


            //Make sure you bind the form to the selected campaign
            LitPageTitle.Text = (IsValue("CampaignTitle") ? CurrentPage["CampaignTitle"] : CurrentPage.PageName) + " - Dagens industri";
            LitPageSubTitle.Text = IsValue("CampaignSubTitle") ? CurrentPage["CampaignSubTitle"].ToString() : string.Empty;

            mainBody.Attributes.Add("data-pagetypename", CurrentPage.PageTypeName); 

            plhMaintenanceScript.DataBind();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(SessionSavedUserData.SelectedCampaign))
                {
                    if (SessionSavedUserData.SelectedCampaign == SecondaryCampaign)
                        RbSecondaryCampaignStep2.Checked = true;
                    else
                        RbPrimaryCampaignStep2.Checked = true;
                }
                else
                    RbPrimaryCampaignStep2.Checked = true;
            }
            //Make sure you bind the form to the selected campaign, and remember it in a Session if redirected to Service Plus for login
            SelectedCampaign = RbSecondaryCampaignStep2.Checked ? SecondaryCampaign : PrimaryCampaign;
            SessionSavedUserData.SelectedCampaign = SelectedCampaign;
            
            CheckUserLogin();

            imgLogotype.ImageUrl = CurrentPage["CampaignLogo"] as string;
            imgLogotypeEnd.ImageUrl = CurrentPage["CampaignLogo"] as string;
            lnkLogotype.NavigateUrl = UserOriginUrl;
            lnkLogotypeEnd.NavigateUrl = UserOriginUrl;
            litAdditionalScript.Text = string.Format("<script type=\"text/javascript\">{0}</script>", CurrentPage["AdditionalOnLoadScript"] as string);

            if (!IsPostBack)
            {
                if (SaveAfterCardPayError)
                    UcCampaignForm.DisplayError(GeneralErrorMessage);
                else
                {
                    if (!VerifyCampaigns())
                        return;

                    if (string.IsNullOrEmpty(UrlTransactionId) && Request.QueryString[QSTPAYSUCCESS] == null)
                        LiteralAdWordsScriptOnLoad.Text = CurrentPage["AdWordsScriptOnLoad"] as string ?? string.Empty;

                    if (HandleReturnFromNets())
                        return;

                    if (HandlePaySuccess())
                        return;
                }
            }

            var activeView = MvSteps.GetActiveView();
            if (activeView == Step2)
                SetUpStep2();
            else if (activeView == Step3)
                SetUpStep3(UcCampaignForm.SelectedPayMethod);

            UcCampaignForm.PropertyPrefix = SelectedCampaign;
            UcCampaignFormOtherPayer.PropertyPrefix = SelectedCampaign;
            //Set form text editor
            var isCamp2 = SelectedCampaign.Equals("Campaign2") && IsValue("Campaign2FormEditor");
            PropFormEditor.PropertyName = isCamp2 ? "Campaign2FormEditor" : "Campaign1FormEditor";
        }

        /// <summary>
        /// On PreRender we take care of changed status in form control
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            BtnBack.Text = IsValue("BackButtonText") ? CurrentPage["BackButtonText"] as string : "TILLBAKA";
            btnBackStep3.Text = IsValue("FinalButtonText") ? CurrentPage["FinalButtonText"] as string : "TILLBAKA";
            UcCampaignForm.BackButtonText = IsValue("BackButtonText") ? CurrentPage["BackButtonText"] as string : "TILLBAKA";

            //If user clicked on back-button in form
            if (UcCampaignForm.GoBack)
            {
                //Redirect back to where user came from
                //EPiFunctions.RedirectToPage(Page, EPiFunctions.SettingsPageSetting(CurrentPage, "PrenPage") as PageReference, string.Empty);
                Response.Redirect(UserOriginUrl);
                return;
            }

            if (UcCampaignFormOtherPayer.GoBack)
            {
                //Reset visible form shown in step 2
                UcCampaignForm.Visible = true;
                UcCampaignFormOtherPayer.Visible = false;
            }


            //If form control is done, the user clicked the submit button
            if (UcCampaignForm.FormDone)
                FormSubmit(false);

            //Otherpayer form is done. 
            if (UcCampaignFormOtherPayer.FormDone)
                FormSubmit(true);
        }

        #endregion

        #region Cirix

        private String SaveSubscription(int? customerRefNo = null, PaymentMethod.TypeOfPaymentMethod payMethod = PaymentMethod.TypeOfPaymentMethod.Invoice, NetsCardPayReturn ret=null)
        {
            string err = string.Empty;
            if (!UcCampaignForm.ShowPaymentFields || (Sub != null && (Sub.PayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCard || Sub.PayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)))
                Sub.Subscriber.ZipCode = "10000";

            try
            {
                //err = CirixDbHandler.TryInsertSubscription2(Sub, null);
                if (Sub == null || string.IsNullOrEmpty(Sub.CampId))
                {
                    new Logger("Pren.di.se error CampaignDigital: sub or campid null");
                    return GeneralErrorMessage + "<!-- Error: sub null -->";
                }
                
                var addCustAndSubHandler = new AddCustAndSubHandler();
                AddCustAndSubReturnObject addCustSubRet = addCustAndSubHandler.TryAddCustAndSub(Sub, customerRefNo, false);
                if (addCustSubRet.CirixSubsno > 0 && customerRefNo != null)
                {
                    MsSqlHandler.AppendToPayTransComment((int)customerRefNo, "cusno: " + addCustSubRet.CirixSubscriberCusno);
                    if (payMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)
                    {
                        SubscriptionController.CreateNewInvoice(Sub);
                        MsSqlHandler.InsertAwd2(ret, addCustSubRet.CirixSubscriberCusno, addCustSubRet.CirixSubsno, Sub.CampNo, Sub.SubsEndDate);
                    }
                }
                // Something went wrong!
                if (addCustSubRet.HasMessages)
                {
                    var returnMessages = new StringBuilder();
                    foreach (var msg in addCustSubRet.Messages)
                    {
                        returnMessages.Append(msg.MessageCustomer);
                        if (Sub.Subscriber != null)
                            new Logger(string.Format("{0} {1} ({2}): {3}", Sub.Subscriber.FirstName, Sub.Subscriber.LastName, Sub.Subscriber.Email, msg.MessageSweStaff));
                        else
                            new Logger(msg.MessageSweStaff);
                    }
                    
                    return returnMessages.ToString();
                }
            }
            catch (Exception ex)
            {
                err = GeneralErrorMessage + "<!-- Error: " + ex.Message + "-->";
                new Logger("Pren.di.se error CampaignDigital(2): ", ex.Message);
            }
            

            if (!string.IsNullOrEmpty(err))
                return err;

            if (Sub.Subscriber == null)
                new Logger("SaveSubscription() failed - subscriber=null");

            return string.Empty;
        }

        private bool TryPopulateSub()
        {
            var paymethod = UcCampaignForm.SelectedPayMethod;
            var campId = string.Empty;

            //Get campId depending on paymethod
            if (paymethod == PaymentMethod.TypeOfPaymentMethod.CreditCard ||
                paymethod == PaymentMethod.TypeOfPaymentMethod.Invoice ||
                paymethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer)
            {
                campId = CurrentPage[SelectedCampaign + "Id"] as string;
            }
            else if (paymethod == PaymentMethod.TypeOfPaymentMethod.DirectDebit)
            {
                campId = CurrentPage[SelectedCampaign + "IdAutopay"] as string;
            }
            else
            {
                campId = CurrentPage[SelectedCampaign + "IdAutowithdrawal"] as string;
            } 

            //Set startdate on sub
            DateTime dateParser;
            var startDate = DateTime.TryParse(UcCampaignForm.WantedStartDate, out dateParser) ? dateParser : DateTime.Now;
            //Create subscription
            Sub = new Subscription(campId, 0, paymethod, startDate, IsTrialPeriod);
            if (Sub.CampNo <= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Sets earliest startdate in form according to business rules
        /// </summary>
        private void SetSubStartDate()
        {
            //TODO: finns det inget "snyggare" sätt? /TKM
            //WantedStartDate is reset by js when active campaign changes
            //TODO: Move to CirixDbHandler? /TKM
            if (string.IsNullOrEmpty(UcCampaignForm.WantedStartDate))
            {
                try
                {
                    DataSet dsCamp = SubscriptionController.GetCampaign(CurrentPage[(RbSecondaryCampaignStep2.Checked ? SecondaryCampaign : PrimaryCampaign) + "Id"] as string);
                    var paperCode = dsCamp.Tables[0].Rows[0]["PAPERCODE"].ToString();
                    var productNo = dsCamp.Tables[0].Rows[0]["PRODUCTNO"].ToString();
                    UcCampaignForm.WantedStartDate = SubscriptionController.GetNextIssueDateIncDiRules(DateTime.Now, paperCode, productNo).ToString("yyyy-MM-dd");
                }
                catch(Exception ex)
                {
                    new Logger("Campaign.SetSubStartDate() failed on page with id: '" + CurrentPage.PageLink.ID + "' failed", ex.ToString());
                }
            }
        }

        #endregion

        #region Nets

        private void HandleCreditCardPayment()
        {
            //Collect all GA query strings
            //Also check for tref query, that is used in tip-a-friend link
            var orgUrl = PageFriendlyUri.AbsoluteUri + "?campaign=" + SelectedCampaign;
            orgUrl += GetSelectedQueryStringParams(true);

            string goodsDescr = Settings.GetName_Product(Sub.PaperCode, Sub.ProductNo);
            string comment = Translate("/subscription/newsubscription");
            string consumerName = string.Format("Subscriber {0} {1}", Sub.Subscriber.FirstName, Sub.Subscriber.LastName).Trim();
            var merchIdProp = EPiFunctions.SettingsPageSetting(CurrentPage, "MerchIDPren");
            string merchId = merchIdProp != null ? merchIdProp.ToString() : string.Empty;
            double vatPct = SubscriptionController.GetProductVat(Sub.PaperCode, Sub.ProductNo);
            var payMet = UcCampaignForm.SelectedPayMethod;
            
            // Note to self: Last parameter object in this call will be serialized
            var prep = new NetsCardPayPrepare(Sub.TotalPriceExVat, null, vatPct, false, false, orgUrl, goodsDescr, comment, consumerName, Sub.Subscriber.Email, null, payMet, Sub);
        }

        private string GetSelectedQueryStringParams(bool forceAppendTo)
        {
            var sb = new StringBuilder();
            var qsGlue = forceAppendTo ? "&" : "?";
            //Collect all GA query strings
            //Also check for tref query, that is used in tip-a-friend link
            var queries = Request.QueryString;
            foreach (var query in queries.AllKeys)
            {
                if (query.ToLower().StartsWith("utm_") || query.ToLower().Equals("tref"))
                {
                    sb.Append(qsGlue + query + "=" + Request.QueryString[query]);
                    qsGlue = "&";
                }
            }
            return sb.ToString();
        }

        //Return true if to stop executing further code in OnLoad, after call to this method
        private bool HandleReturnFromNets()
        {
            if (!string.IsNullOrEmpty(UrlTransactionId))
            {
                var ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                var prep = ret.NetsPreparePersisted;
                Sub = (Subscription)prep.PersistedObj;
                
                // Will do redirects and return a bool?!?!
                //string.Empty as parameter for successUrl tell method that I do not want it to redirect on success, I handle it further down AFTER subcription save is also successful
                var payOk = ret.HandleNetsReturn(string.Empty, Sub.Subscriber.Email);
                if (!payOk)
                    return false;
                
                Sub.CreditCardPaymentOk = true;
                var err = SaveSubscription(prep.CustomerRefNo, prep.PayMethod, ret);
               
                //Parameter sacpf = SaveAfterCardPayFail
                string redirectAfterCardPayUrl = (string.IsNullOrEmpty(err)) ? string.Format("{0}&{1}=1&crn={2}{3}", CurrentPage.LinkURL, QSTPAYSUCCESS, UrlTransactionId, GetSelectedQueryStringParams(true)) : string.Format("{0}&{1}=1&sacpf=1&crn={2}{3}", CurrentPage.LinkURL, QSTPAYSUCCESS, UrlTransactionId, GetSelectedQueryStringParams(true));
                Response.Redirect(redirectAfterCardPayUrl, true);
                return true;
            }
            return false;
        }


        private bool HandlePaySuccess()
        {
            if (Request.QueryString[QSTPAYSUCCESS] != null)
            {
                SetUpStep3(PaymentMethod.TypeOfPaymentMethod.CreditCard);
                return true;
            }

            return false;
        }

        #endregion

        #region Verifiers

        private void CheckUserLogin()
        {
            var servicePlusToken = Request.QueryString[TokenKey];
            // If providing different token as querystring parameter than already exist in Session, then redirect user to do a proper url without token in url
            if (!string.IsNullOrEmpty(SessionSavedUserData.Token) && !string.IsNullOrEmpty(servicePlusToken))
            {
                if (SessionSavedUserData.Token != servicePlusToken)
                {
                    SessionSavedUserData = null;
                    Response.Redirect(PageFriendlyUri.AbsoluteUri + GetSelectedQueryStringParams(false), true); // To redirect to a clean Url without "token"
                }
            }

            // If user have no token set in Session
            if (!IsPostBack && string.IsNullOrEmpty(SessionSavedUserData.Token) && Request.QueryString[QSTPAYSUCCESS] == null && string.IsNullOrEmpty(UrlTransactionId))
            {
                var servicePlusCheckLoginUrl = BonDigMisc.GetCheckLoggedInUrl(GetExternalUrl(), string.Empty);
                if (Sub != null)
                {
                    return;
                }

                // No token was read from querystring above, so redirect to login page
                if (servicePlusToken == null)
                {
                    Response.Redirect(servicePlusCheckLoginUrl, true);
                    return;
                }

                // Token exist and is not null from querystring above
                if (servicePlusToken.Length > 0)
                {
                    var servicePlusUserData = RequestHandler.GetUserByToken(servicePlusToken);
                    if (servicePlusUserData.user == null)
                    {
                        Response.Redirect(PageFriendlyUri.AbsoluteUri + GetSelectedQueryStringParams(false), true); // No user at S+ on that token so redirect to a clean Url without failed "token" in url
                        return;
                    }
                    SessionSavedUserData.SetUserOutputData(servicePlusUserData);
                    SessionSavedUserData.Token = servicePlusToken;
                    Response.Redirect(PageFriendlyUri.AbsoluteUri + GetSelectedQueryStringParams(false), true); // To redirect to a clean Url without "token"
                }
                else
                {
                    SessionSavedUserData = null;
                    phForms.Visible = false;
                    phNotLoggedInMessage.Visible = true;
                    phNotLoggedInMessage.Visible = true;
                }
            }
        }
        
        private bool VerifyCampaigns()
        {
            //return true;

            if ((IsValue("Campaign1IsStudent") && !IsValue("Campaign1HideInvoiceOther")) || (IsValue("Campaign2IsStudent") && !IsValue("Campaign2HideInvoiceOther")))
            {
                ShowErrorView("Du kan inte ha betalalternativ 'Faktura annan betalare' på en studentkampanj");
                return false;            
            }

            var error = CampIdVerified();
            if (!string.IsNullOrEmpty(error))
            {
                ShowErrorView(error);
                return false;
            }

            if (!TargetGroupsVerified())
            {
                ShowErrorView("Felaktig målgrupp");
                return false;
            }

            if (!GoldVerified())
            {
                ShowErrorView("Du måste ange Di guld landningssida om du markerat att det är en Guldkampanj");
                return false;
            }

            return true;
        }

        private string CampIdVerified()
        {
            var error = CampIdVerified("Campaign1");

            //Only verify campaign2 if at least one campId is set
            if (ShowCampaign2)
                error += CampIdVerified("Campaign2");

            return error;
        }

        private string CampIdVerified(string campaign)
        {            
            //If card or invoice payment options is visible, verify campId
            var verifyCampId = !IsValue(campaign + "HideCard") || !IsValue(campaign + "HideInvoice") || !IsValue(campaign + "HideInvoiceOther");
            //If autogiro is visible, confirm that a correct campId is set
            var verifyCampIdAutopay = !IsValue(campaign + "HideAutopay");
            //If autowithdrawal is visible, confirm that a correct campId is set
            var verifyCampIdAutowithdrawal = !IsValue(campaign + "HideAutoWithdrawal");

            long campNo = -1, campNoAutopay = -1, campNoAutowithdrawal = -1;

            if (verifyCampId)
            {
                if (!IsValue(campaign + "Id"))
                    return "Du måste ange ett kampid för " + campaign;
                
                //Verify that correct campId is used
                campNo = SubscriptionController.GetCampno(CurrentPage[campaign + "Id"] as string);
                if (campNo < 1)
                    return "Felaktigt kampid på " + campaign;
            }

            if (verifyCampIdAutopay)
            {
                if (!IsValue(campaign + "IdAutopay"))
                    return "Du måste ange kampid för autogiro för " + campaign;

                campNoAutopay = SubscriptionController.GetCampno(CurrentPage[campaign + "IdAutopay"] as string);
                if (campNoAutopay < 1)
                    return "Felaktigt kampid för autogiro på " + campaign;
                if (campNoAutopay == campNo)
                    return "Felaktigt kampid för autogiro på " + campaign + ", får ej vara samma kampid som för kort/faktura-betalning";            
            }

            if (verifyCampIdAutowithdrawal)
            {
                if (!IsValue(campaign + "IdAutowithdrawal"))
                    return "Du måste ange kampid för autodragning för " + campaign;

                campNoAutowithdrawal = SubscriptionController.GetCampno(CurrentPage[campaign + "IdAutowithdrawal"] as string);
                if (campNoAutowithdrawal < 1)
                    return "Felaktigt kampid för autodragning på " + campaign;
                if (campNoAutowithdrawal == campNo)
                    return "Felaktigt kampid för autodragning på " + campaign + ", får ej vara samma kampid som för kort/faktura-betalning";              
            }

            return string.Empty;
        }

        private bool GoldVerified() 
        {
            bool goldVerified1 = true;
            bool goldVerified2 = true;

            if(IsValue("Campaign1IsDiGold"))
                goldVerified1 = IsValue("Campaign1DiGoldLandingPage");

            if (IsValue("Campaign2IsDiGold"))
                goldVerified2 = IsValue("Campaign2DiGoldLandingPage");

            return goldVerified1 && goldVerified2;
        }

        private bool TargetGroupsVerified()
        {
            var targetGroup = CurrentPage["TargetGroup"] as string;
            var targetGroupMobile = CurrentPage["TargetGroupMobile"] as string;
            var targetGroupQuery = Request.QueryString[QSTTARGETGROUP]; //Get target group from querystring

            //Collect all target groups
            var allTargGrs = SubscriptionController.GetTargetGroups(Settings.PaperCode_DI);
            allTargGrs.UnionWith(SubscriptionController.GetTargetGroups(Settings.PaperCode_DISE));
            allTargGrs.UnionWith(SubscriptionController.GetTargetGroups(Settings.PaperCode_IPAD));
            allTargGrs.UnionWith(SubscriptionController.GetTargetGroups(Settings.PaperCode_AGENDA));

            //Verify that targetgroup from querystring is an existing target group
            var queryTgIsVerified = string.IsNullOrEmpty(targetGroupQuery) ? true : allTargGrs.Contains(targetGroupQuery);

            return allTargGrs.Contains(targetGroup) && allTargGrs.Contains(targetGroupMobile) && queryTgIsVerified;
        }

        private bool VerifyFullTimeStudent(string birthNo)
        {
            birthNo = birthNo.Substring(2);

            StudentVerifier ver = new StudentVerifier();
            if (ver.VerifyByBirthNum(birthNo) == "1")
                return true;

            return false;
        }

        #endregion

        #region Code front helpers
        
        protected string SetGobackAndGetTrackScript(bool primary)
        {
            UcCampaignFormOtherPayer.GoBack = true;
            return "Track('"+ GetGaTrackingString() + "','ChooseOffer','" + (primary ? PrimaryCampaign : SecondaryCampaign) + "');ResetStartDate();";
        }

        #endregion

        #region Code behind helpers

        private string ReplaceAppId(string url)
        {
            if (IsValue("AppIdOverride"))
            {
                var appIdString = string.Format(ServicePlusAppIdTemplate, CurrentPage["AppIdOverride"]);
                var appIdsetting = ConfigurationManager.AppSettings["BonDigAppIdDagInd"] ?? "dagensindustri.se";
                return url.Replace("appId=" + appIdsetting, appIdString);
            }
            return url;
        }

        public Person GetPerson(CampaignForm formControl, bool otherPayer)
        {
            var personObj = new Person(!otherPayer,
                false,
                formControl.FirstName,
                formControl.LastName,
                formControl.Co,
                formControl.Company,
                formControl.StreetAddress,
                formControl.HouseNo,
                otherPayer ? string.Empty : formControl.StairCase,
                otherPayer ? string.Empty : formControl.Stairs,
                otherPayer ? string.Empty : formControl.AppNo,
                formControl.PostalCode,
                formControl.City,
                otherPayer ? string.Empty : formControl.PhoneMobile,
                otherPayer ? string.Empty : formControl.Email,
                formControl.BirthNo,
                formControl.OrganisationNumber,
                otherPayer ? formControl.Attention : string.Empty,
                otherPayer ? formControl.PhoneMobile : string.Empty, 
                SessionSavedUserData.Token, 
                SessionSavedUserData.user.id);
            return personObj;
        }

        private string GetExternalUrl()
        {
            var currentPageUri = new Uri(Request.Url.AbsoluteUri);
            var selectedQs = GetSelectedQueryStringParams(true);
            var currentPageExternalUrlBuilder = new UriBuilder(currentPageUri.Host)
            {
                Path = CurrentPage.ExternalLinkUrl(),
                Query = (ReturnUri != null) ? string.Format("returnurl={0}{1}", ReturnUri, selectedQs) : selectedQs.TrimStart('&')
            };
            return currentPageExternalUrlBuilder.ToString();
        }

        private void SetReturnUrls()
        {
            Uri urlReferrer = null;
            var returnUrl = Request.QueryString[ReturnUrlKey];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ReturnUri = new Uri(returnUrl);
                Session[SessionUserOriginUrlKey] = ReturnUri.ToString();
            }
            if (Session[SessionUserOriginUrlKey] == null)
            {
                urlReferrer = Request.UrlReferrer; // Only care of the referrer if Session have not been set, otherwise this can be a return from ServicePlus and we do not want that refferer
                Session[SessionUserOriginUrlKey] = (ReturnUri != null) ? ReturnUri.ToString() : (urlReferrer != null && !urlReferrer.ToString().Contains(Request.Url.Host)) ? urlReferrer.ToString() : CurrentPage["DefaultReturnUrl"] as string;
            }
        }

        #endregion

        #region Action
        private void SetUpStep2()
        {
            MvSteps.SetActiveView(Step2);

            //If guid in Url the form will be populated with data from MD
            if(UcCampaignForm.FormIsEmpty)
                TryPopulateForm();

            // If token is fetched, user is logged in, so try populate form with user info
            if (UcCampaignForm.FormIsEmpty)
                TryPopulateFormFromServicePlusToken();

            //Bind radiobutton selectors
            RbPrimaryCampaignStep2.DataBind();
            RbSecondaryCampaignStep2.DataBind();
            
            //Hide checkbox areas if only one campaign set
            //PhSecondaryArea2.Visible = false; //ShowCampaign2;         //130821 - ylva: hide camp2 on step 2
            PhSecondaryArea2.Visible = ShowCampaign2;  //130828 - ylva: show camp2 if user from matrix page

            //Set startdate
            SetSubStartDate();

            //Bind papers associated with campaigns
            SideBarPrimary.BindPapers(PrimaryCampaign, SecondaryCampaign);
            SideBarSecondary.BindPapers(SecondaryCampaign, PrimaryCampaign);
        }

        protected void FormSubmit(bool otherPayer)
        {
            if (otherPayer)
            {
                Sub.SubscriptionPayer = GetPerson(UcCampaignFormOtherPayer, true);
                var err = SaveSubscription();

                if (String.IsNullOrEmpty(err))
                    SetUpStep3(PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer);
                else
                    UcCampaignFormOtherPayer.DisplayError(err);

                return;
            }

            if (!TryPopulateSub())
            {
                UcCampaignForm.DisplayError(Translate("/common/errors/errorcamp"));
                return;
            }

            //8902154957
            if (UcCampaignForm.IsStudentCampaign && !VerifyFullTimeStudent(UcCampaignForm.BirthNo.Trim()))
            {
                UcCampaignForm.DisplayError(Translate("/dicampaign/forms/studentverification"));
                return;
            }

            var selectedPayMethod = UcCampaignForm.SelectedPayMethod;
            if (!UcCampaignForm.ShowPaymentFields)//!UcCampaignForm.ShowAdressFields
            {
                selectedPayMethod = PaymentMethod.TypeOfPaymentMethod.Invoice; // To prevent redirecting to Nets if switching to a free offer, and credit card is chosen in other offer (even though it is hidden, value is set!)
            }

            Sub.SetMembersByPayMethod(selectedPayMethod);
            Sub.TargetGroup = TargetGroup; 
            Sub.Subscriber = GetPerson(UcCampaignForm, false);
            Sub.SubscriptionPayer = null;

            //Flag as Gold member
            if (IsValue(SelectedCampaign + "IsDiGold")) 
                Sub.Subscriber.IsGoldMember = true;

            if (selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.Invoice || selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.DirectDebit )
            {
                var err = SaveSubscription();
                if (String.IsNullOrEmpty(err))
                    SetUpStep3(selectedPayMethod);
                else
                    UcCampaignForm.DisplayError(err);
            }
            else if (selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer)
            {
                UcCampaignForm.Visible = false;
                UcCampaignFormOtherPayer.Visible = true;                
            }
            else if (selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCard || selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)
            {
                var err = string.Empty;
                if (SubscriptionController.DenyShortSub(Sub, out err))
                {
                    UcCampaignForm.DisplayError(err);
                    return;
                }

                HandleCreditCardPayment(); //redirect to Nets
            }
        }
        
        protected void SetUpStep3(PaymentMethod.TypeOfPaymentMethod paymethod)
        {
            AddThisFieldset2.Headline = IsValue("AddThisFieldsetHeadline") ? CurrentPage["AddThisFieldsetHeadline"].ToString() : "Tipsa om erbjudandet";
            //To be backward compatible with all already created Agenda pages, default is "Agenda"
            AddThisFieldset2.EmailTemplateName = IsValue("AddThisFieldsetEmailTemplate") ? CurrentPage["AddThisFieldsetEmailTemplate"].ToString() : "Agenda";
            //Set selected campaign, used on code-front to show correct image and text
            var campQuery = Request.QueryString["campaign"];
            if (!string.IsNullOrEmpty(campQuery)) //From Nets
                SelectedCampaign = campQuery;
            else
                SelectedCampaign = RbSecondaryCampaignStep2.Checked ? SecondaryCampaign : PrimaryCampaign; //Postback

            //Tref is a query from tip-a-friend page, flag user as pren
            var tref = Request.QueryString["tref"]; 
            if (!string.IsNullOrEmpty(tref) && MiscFunctions.IsGuid(tref)) 
                MsSqlHandler.UpdateInviteFriend(new Guid(tref), true);  

            //From Nets, set Subscription object
            var transId = Request.QueryString["crn"];
            if (!string.IsNullOrEmpty(transId))
            {
                var auPrep = NetsCardPayReturn.ReadPersisted(transId);
                if (auPrep != null)
                    Sub = (Subscription)auPrep.PersistedObj;
            }

            //Sync subscriber to mssql logn table, and add property for "accepterar prenvillkor"
            if (Sub != null && Sub.Subscriber.Cusno > 0)
            {
                //Add samtycke the finnish way (for pren only, nothing dirty)
                var propertyHandler = new CustomerPropertyHandler(Sub.Subscriber.Cusno);
                if (propertyHandler.AllCustomerProperties != null) {
                    List<CustomerProp> newProps = new List<CustomerProp>() { new CustomerProp("94", "01") }; //Samtycke, 01 = ja, 02 = nej                    
                    propertyHandler.InsertCusProps(propertyHandler.AllCustomerProperties, newProps);
                }
                paymethod = Sub.PayMethod;
            }

            //Google analytics tracking
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "GATrackpageView", "TrackPageview('" + GetGaTrackingString(paymethod + "/done") + "');", true);
            
            //Get correct thankstext depending on payment method
            PropThankYouText.PropertyName = GetThankYouTextPropertyName();
            MvSteps.SetActiveView(Step3);
            LiteralAdWordsScriptOnThankYou.Text = CurrentPage[SelectedCampaign + "AdWordsScriptOnThankYou"] as string ?? string.Empty;
        }

        private void ShowErrorView(string errorMsg) 
        {
            LblError.Text = errorMsg;
            MvSteps.SetActiveView(ErrorView);
        }

        /// <summary>
        /// Used by DiGoldCampaigns
        /// </summary>
        
        /// <summary>
        /// If code is in MDB, populate the form
        /// </summary>
        private void TryPopulateForm()
        {
            //94FF8B04-63AC-4421-9980-CA8A88957495
            //3513209

            var code = Request.QueryString["code"];

            if (string.IsNullOrEmpty(code) && !MiscFunctions.IsGuid(code))
                return;

            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    UcCampaignForm.HideParArea();

                    long cusNo = MsSqlHandler.MdbGetCusnoByCode(new Guid(code));
                    
                    if (cusNo > 0)
                    {
                        var subscriber = new SubscriptionUser2(cusNo);

                        UcCampaignForm.Company = subscriber.IsCompanyCust ? subscriber.RowText1 : string.Empty;

                        //TODO: IsCompanyCust verkar inte riktigt stämma? /TKM
                        //Tips från coachen (Janne): reverseline innehåller 0,1 eller 2. Om den har något annat värde än 0 så talar den om vilken av rowtext1 eller rowtext2 som innehåller personnamn.
                        //Den är dock inte heltäckande, är värdet 0 så betyder det att det inte är satt. Vilket inte är samma sak som att ingen rad innehåller person.
                        var name = subscriber.IsCompanyCust ? subscriber.RowText2 : subscriber.RowText1;

                        if (name != null)
                        {
                            var nameArray = name.Split(' ');

                            UcCampaignForm.LastName = nameArray.Length > 0 ? nameArray[0] : string.Empty;
                            UcCampaignForm.FirstName = nameArray.Length > 1 ? nameArray[1] : string.Empty;
                        }

                        UcCampaignForm.PhoneMobile = subscriber.OPhone;
                        UcCampaignForm.Email = subscriber.Email;
                        UcCampaignForm.StreetAddress = subscriber.StreetName;
                        UcCampaignForm.HouseNo = subscriber.HouseNo;
                        UcCampaignForm.StairCase = subscriber.Staricase;
                        UcCampaignForm.Stairs = subscriber.Apartment;

                        if (subscriber.Street2 != null)
                        {
                            var lghflag = false;
                            foreach (var item in subscriber.Street2.Split(' '))
                            {
                                if (item.ToLower().StartsWith("lgh"))
                                    lghflag = true;

                                if (lghflag)
                                    UcCampaignForm.AppNo += item;
                                else
                                    UcCampaignForm.Co += item + " ";
                            }
                        }

                        //Remove prefix lgh
                        UcCampaignForm.AppNo = UcCampaignForm.AppNo.ToLower().Replace("lgh", string.Empty);
                        UcCampaignForm.PostalCode = subscriber.Zip;
                        UcCampaignForm.City = subscriber.PostName;
                    }
                }
                catch (Exception ex)
                {
                    new Logger("CampaignForm.TryPopulateForm() for url code '" + code + "' failed", ex.ToString());
                }
            }
        }

        private void TryPopulateFormFromServicePlusToken()
        {
            if (SessionSavedUserData != null && !string.IsNullOrEmpty(SessionSavedUserData.Token))
            {
                UcCampaignForm.FirstName = SessionSavedUserData.user.firstName ?? string.Empty;
                UcCampaignForm.LastName = SessionSavedUserData.user.lastName ?? string.Empty;
                UcCampaignForm.Email = SessionSavedUserData.user.email ?? string.Empty;
                UcCampaignForm.PhoneMobile = SessionSavedUserData.user.phoneNumber ?? string.Empty;
            }
        }

        private string GetThankYouTextPropertyName() 
        {
            var propertyName = "ThankYouText";

            if (Sub != null)
            {
                switch (Sub.PayMethod) { 
                    case PaymentMethod.TypeOfPaymentMethod.CreditCard:
                        propertyName = "ThankYouTextCard";
                        break;
                    case PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal:
                        propertyName = "ThankYouTextCard";
                        break;
                    case PaymentMethod.TypeOfPaymentMethod.DirectDebit:
                        propertyName = "ThankYouTextAutopay";
                        break;
                }
            }


            return propertyName;
        }

        #endregion

        protected string GetGaTrackingString(string addOn = "")
        {
            var formInfo = (UcCampaignFormOtherPayer.Visible && !UcCampaignForm.Visible) ? "/FormInvoiceOther" : string.Empty;
            formInfo = (!UcCampaignFormOtherPayer.Visible && UcCampaignForm.Visible) ? "/step1" : formInfo;
            return string.Format("{0}{1}/{2}{3}/{4}", PageFriendlyUri.LocalPath, GaAuthenticatedString, SelectedCampaign, formInfo, addOn);
        }

        protected void BtnBack_OnClick(object sender, EventArgs e)
        {
            var tmpUrl = UserOriginUrl;
            Session[SessionUserOriginUrlKey] = null; // If leaving page then clear this Session
            Response.Redirect(tmpUrl);
        }

        protected void btnLogin_OnClick(object sender, EventArgs e)
        {
            var loginUrl = ReplaceAppId(BonDigMisc.GetLoginUrl(GetExternalUrl(), string.Empty));

            if (!string.IsNullOrEmpty(loginUrl))
            {
                Response.Redirect(loginUrl);
            }
        }
    }
}