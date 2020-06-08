using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DIClassLib.CardPayment.Nets;

using EPiServer.Core;
using DagensIndustri.Tools.Classes.WebControls;
using EPiServer.Web.WebControls;
using DIClassLib.DbHandlers;
using DIClassLib.Subscriptions;
using DIClassLib.CardPayment;
using DagensIndustri.Tools.Classes;
using DIClassLib.Misc;
using DIClassLib.DbHelpers;
using DIClassLib.StudentVerification;
using System.Data;
using DagensIndustri.Tools.Classes.Campaign;
using System.Data.SqlClient;
using DIClassLib.Membership;
using System.Collections.Specialized;
using DIClassLib.BonnierDigital;
using System.Text;

namespace DagensIndustri.Templates.Public.Pages.bscampaign
{
    public partial class Campaign : EPiServer.TemplatePage
    {
        private const string QSTTARGETGROUP = "tg";
        private const string QSTPAYSUCCESS = "pay";
        private bool IsFromMatrix { get { return (Request.QueryString["pm"] != null); } }

        public string PrimaryCampaign { get { return RbSecondaryCampaignStep1.Checked ? "Campaign2" : "Campaign1"; } }
        public string SecondaryCampaign { get { return RbSecondaryCampaignStep1.Checked ? "Campaign1" : "Campaign2"; } }
        public string SelectedCampaign { get; set; }
        
        /// <summary>
        /// Campaign2 will only be shown if one of the campid is set
        /// </summary>
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

        public Subscription Sub
        {
            get { return (Subscription)ViewState["subscription"]; }
            set { ViewState["subscription"] = value; }
        }

        public Person GetPerson(CampaignForm formControl, bool otherPayer)
        {
            return new Person(!otherPayer,
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
                              "", //formControl.City,
                              formControl.PhoneMobile,
                              otherPayer ? string.Empty : formControl.Email,
                              formControl.BirthNo,
                              formControl.OrganisationNumber,
                              otherPayer ? formControl.Attention : string.Empty,
                              string.Empty);
        }


        private bool IsTrialPeriod
        {
            get
            {
                if (CurrentPage[SelectedCampaign + "IsTrial"] == null)
                    return false;

                return true;
            }
        }

        #region Cirix

        /// <summary>
        /// Prevent activation of campaign offer within 6 months of last one (if this is a campaign)
        /// </summary>
        private bool DenyShortSub()
        {
            int subLenDays = (Sub.SubsLenMons * 30) + Sub.SubsLenDays;

            if (subLenDays <= 90)
            {
                // the new subscription is 3 months or less
                AddSubsToCustHelper subsHelper = new AddSubsToCustHelper(Sub);
                if (subsHelper.CustHadTrialPeriodLast6Months())
                {
                    String err = DIClassLib.Subscriptions.AddCustAndSub.Message.ErrMess_DenyShortSub;
                    MiscFunctions.SendStaffMailFailedSaveSubs(err, Sub, null);
                    UcCampaignForm.DisplayError(err);
                    return true;
                }
            }

            return false;
        }

        private String SaveSubscription()
        {
            if (UcCampaignForm.IsDigitalCampaign && Sub != null && Sub.Subscriber != null && string.IsNullOrEmpty(Sub.Subscriber.ZipCode))
                Sub.Subscriber.ZipCode = "10000";

            string err = CirixDbHandler.TryInsertSubscription2(Sub, null);
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
                string selCampId = "";
                string campId = "";
                string paperCode = "";
                string productNo = "";

                try
                {
                    selCampId = (RbSecondaryCampaignStep2.Checked ? SecondaryCampaign : PrimaryCampaign) + "Id";
                    campId = CurrentPage[selCampId] as string;
                    DataSet dsCamp = CirixDbHandler.GetCampaign(campId);
                    paperCode = dsCamp.Tables[0].Rows[0]["PAPERCODE"].ToString();
                    productNo = dsCamp.Tables[0].Rows[0]["PRODUCTNO"].ToString();
                    UcCampaignForm.WantedStartDate = CirixDbHandler.GetNextIssueDateIncDiRules(DateTime.Now, paperCode, productNo).ToString("yyyy-MM-dd");
                }
                catch(Exception ex)
                {
                    //new Logger("Campaign.SetSubStartDate() failed on page with id: '" + CurrentPage.PageLink.ID + "' failed", ex.ToString());
                    new Logger(
                        "Campaign.SetSubStartDate() failed for " +
                        "EPiPageId: " + CurrentPage.PageLink.ID +
                        ", selCampId: " + selCampId +
                        ", campId: " + campId +
                        ", paperCode: " + paperCode +
                        ", productNo: " + productNo,
                        ex.ToString());
                }
            }
        }

        #endregion

        #region Nets

        private void HandleCreditCardPayment()
        {
            var isAutoWithdrawal = UcCampaignForm.SelectedPayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal;
            //string url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) + "?md=campaign=" + SelectedCampaign; //Query md contains metadata
            string url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) + "?campaign=" + SelectedCampaign;

            //Collect all GA query strings and add to metadata parameter 
            //Also check for tref query, that is used in tip-a-friend link
            //We must put all stuff in one query parameter, otherwise Auriga will go nuts
            //var queries = Request.QueryString;
            //foreach (var query in queries.AllKeys)
            //{
            //    if (query.ToLower().StartsWith("utm_") || query.ToLower().Equals("tref"))                
            //        url += "|" + query + "=" + Request.QueryString[query];
            //}

            string goodsDescr = Settings.GetName_Product(Sub.PaperCode, Sub.ProductNo);
            string comment = Translate("/subscription/newsubscription");
            string consumerName = string.Format("Subscriber {0} {1}", Sub.Subscriber.FirstName, Sub.Subscriber.LastName).Trim();
            double vatPct = (Sub.PaperCode == Settings.PaperCode_DI) ? Settings.VatPaper : Settings.VatIpad;
            PaymentMethod.TypeOfPaymentMethod payMet = isAutoWithdrawal ? PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal : PaymentMethod.TypeOfPaymentMethod.CreditCard;

            var prep = new NetsCardPayPrepare(Sub.TotalPriceExVat, null, vatPct, false, false, url, goodsDescr, comment, consumerName, Sub.Subscriber.Email, null, payMet, Sub);
        }

        private bool HandleReturnFromNets()
        {
            if (Request.QueryString["responseCode"] != null)
            {
                var transId = MiscFunctions.REC(Request.QueryString["transactionId"]);
                string paySuccessUrl = CurrentPage.LinkURL + "&" + QSTPAYSUCCESS + "=1&crn=" + transId;

                var metaData = Request.QueryString["md"] ?? string.Empty;
                foreach (var dataDataItem in metaData.Split('|'))
                    paySuccessUrl += "&" + dataDataItem;

                var ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                var prep = ret.NetsPreparePersisted;
                Sub = (Subscription)prep.PersistedObj;

                bool payOk = ret.HandleNetsReturn(paySuccessUrl, Sub.Subscriber.Email);
                if (payOk)
                {
                    Sub.CreditCardPaymentOk = true;
                    SaveSubscription();

                    if (Sub.SubsNo > 0)
                    {
                        MsSqlHandler.AppendToPayTransComment(prep.CustomerRefNo, "cusno: " + Sub.Subscriber.Cusno);
                        if (ret.NetsPreparePersisted.PayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)
                        {
                            CirixDbHandler.CreateNewInvoice(Sub.SubsNo, Sub.PaperCode);
                            MsSqlHandler.InsertAwd2(ret, Sub.Subscriber.Cusno, Sub.SubsNo, Sub.CampNo, Sub.SubsEndDate);
                        }
                    }
                }

                return true;
            }

            return false;
        }

        private bool HandlePaySuccess()
        {
            if (Request.QueryString[QSTPAYSUCCESS] != null)
            {
                SetUpStep3();
                return true;
            }

            return false;
        }

        #endregion

        #region Verifiers

        private bool VerifyCampaigns()
        {
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
                campNo = CirixDbHandler.GetCampno(CurrentPage[campaign + "Id"] as string);
                if (campNo < 1)
                    return "Felaktigt kampid på " + campaign;
            }

            if (verifyCampIdAutopay)
            {
                if (!IsValue(campaign + "IdAutopay"))
                    return "Du måste ange kampid för autogiro för " + campaign;

                campNoAutopay = CirixDbHandler.GetCampno(CurrentPage[campaign + "IdAutopay"] as string);
                if (campNoAutopay < 1)
                    return "Felaktigt kampid för autogiro på " + campaign;
                if (campNoAutopay == campNo)
                    return "Felaktigt kampid för autogiro på " + campaign + ", får ej vara samma kampid som för kort/faktura-betalning";            
            }

            if (verifyCampIdAutowithdrawal)
            {
                if (!IsValue(campaign + "IdAutowithdrawal"))
                    return "Du måste ange kampid för autodragning för " + campaign;

                campNoAutowithdrawal = CirixDbHandler.GetCampno(CurrentPage[campaign + "IdAutowithdrawal"] as string);
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
            var allTargGrs = CirixDbHandler.GetTargetGroups(Settings.PaperCode_DI);
            allTargGrs.UnionWith(CirixDbHandler.GetTargetGroups(Settings.PaperCode_DISE));
            allTargGrs.UnionWith(CirixDbHandler.GetTargetGroups(Settings.PaperCode_IPAD));

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

        /// <summary>
        /// This class will determine if another list of papers shall be loaded on click on checkbox.
        /// Only enable jquery load if secondary campaign has another collection than primary campaign
        /// </summary>
        public string JqueryLoadClass
        {
            get
            {
                return CurrentPage["Campaign2IncludedProducts"] != null ? "enable" : "disable";
            }
        }

        /// <summary>
        /// Used to keep track on active step in progress bar
        /// </summary>
        protected string GetProgressClass(int index)
        {
            return MvSteps.ActiveViewIndex == index ? "active" : "todo";
        }

        protected string GetTrackScript(bool primary)
        {
            return "Track('ChooseOffer','" + (primary ? PrimaryCampaign : SecondaryCampaign) + "','Step2');ResetStartDate();";
        }

        #endregion

        #region Event handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LitPageTitle.Text = (IsValue("CampaignTitle") ? CurrentPage["CampaignTitle"] : CurrentPage.PageName) + " - Dagens industri";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LiteralDigErr.Visible = false;

            if (!IsPostBack)
            {
                if (!VerifyCampaigns())
                    return;

                if (Request.QueryString["responseCode"] == null && Request.QueryString[QSTPAYSUCCESS] == null)
                    LiteralAdWordsScriptOnLoad.Text = CurrentPage["AdWordsScriptOnLoad"] as string ?? string.Empty;

                if (HandleReturnFromNets())
                    return;
                if (HandlePaySuccess())
                    return;
            }

            View activeView = MvSteps.GetActiveView();

            if (activeView == Step1)
                SetUpStep1();

            if (activeView != Step3 && (activeView == Step2 || IsFromMatrix))          
                SetUpStep2();

            if (activeView == Step3)
                SetUpStep3();

            //Make sure you bind the form to the selected campaign
            SelectedCampaign = RbSecondaryCampaignStep2.Checked ? SecondaryCampaign : PrimaryCampaign;
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

            //If user clicked on back-button in form
            if (UcCampaignForm.GoBack)
            {
                //If user is from prenmatrix, redirect back to pren page
                if (IsFromMatrix)
                    EPiFunctions.RedirectToPage(Page, EPiFunctions.SettingsPageSetting(CurrentPage, "PrenPage") as PageReference, string.Empty);

                SetUpStep1();
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

        protected void BtnSubmitStep1OnClick(object sender, EventArgs e)
        {            
            SetUpStep2();
        }

        #endregion

        #region Action

        protected void SetUpStep1() 
        {
            MvSteps.SetActiveView(Step1);
            //Hide checkbox areas if only campaign1 is set
            PhSecondaryArea1.Visible = ShowCampaign2;
        }

        protected void SetUpStep2()
        {
            MvSteps.SetActiveView(Step2);
            //If guid in Url the form will be populated with data from MD
            if(UcCampaignForm.FormIsEmpty)
                TryPopulateForm(); 
            //Bind radiobutton selectors
            RbPrimaryCampaignStep2.DataBind();
            RbSecondaryCampaignStep2.DataBind();
            
            //Hide checkbox areas if only one campaign set
            //PhSecondaryArea2.Visible = false; //ShowCampaign2;         //130821 - ylva: hide camp2 on step 2
            PhSecondaryArea2.Visible = (ShowCampaign2 && IsFromMatrix);  //130828 - ylva: show camp2 if user from matrix page

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
                    SetUpStep3();
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
                    SetUpStep3();
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
                if (DenyShortSub())
                    return;

                HandleCreditCardPayment(); //redirect to Nets
            }
        }

        protected void SetUpStep3()
        {
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
            var crn = Request.QueryString["crn"];
            if(!string.IsNullOrEmpty(crn))
            {
                NetsCardPayPrepare prep = NetsCardPayReturn.ReadPersisted(crn);
                if(prep != null)
                    Sub = (Subscription)prep.PersistedObj;
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
                
                //Sync to login-table
                DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncCustToMssqlLoginTables((int)Sub.Subscriber.Cusno);
                //Google analytics tracking
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "GATrack", "Track('Form', '" + Sub.PayMethod + "', 'Step2');", true);
            }
            //Get correct thankstext depending on payment method
            PropThankYouText.PropertyName = GetThankYouTextPropertyName();
            //DiGold handling
            if (IsValue(SelectedCampaign + "IsDiGold"))
                RedirectToLandingPage();                    //todo: maby not redirect to gold before user has created an Di-account

            MvSteps.SetActiveView(Step3);
            SideBarStep3.BindPapers(SelectedCampaign, SelectedCampaign.Equals(PrimaryCampaign) ? SecondaryCampaign : PrimaryCampaign);
            LiteralAdWordsScriptOnThankYou.Text = CurrentPage[SelectedCampaign + "AdWordsScriptOnThankYou"] as string ?? string.Empty;

            TextBoxDigUser.Text = Sub.Subscriber.Email;
        }

        private void ShowErrorView(string errorMsg) 
        {
            LblError.Text = errorMsg;
            MvSteps.SetActiveView(ErrorView);
        }

        /// <summary>
        /// Used by DiGoldCampaigns
        /// </summary>
        private void RedirectToLandingPage()
        {
            if (Sub != null)
            {
                //Add user to role DiGold
                DiRoleHandler.AddUserToRoles(Sub.Subscriber.UserName, new string[] { DiRoleHandler.RoleDiGold });                
                //Login in the user
                LoginUtil.ReLoginUserRefreshCookie(Sub.Subscriber.UserName, Sub.Subscriber.Password);
            }

            var landingPage = CurrentPage[SelectedCampaign + "DiGoldLandingPage"] as PageReference;
            var url = EPiFunctions.GetFriendlyAbsoluteUrl(landingPage);
            Response.Redirect(url);            
        }

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
                        //UcCampaignForm.City = subscriber.PostName;
                    }
                }
                catch (Exception ex)
                {
                    new Logger("CampaignForm.TryPopulateForm() for url code '" + code + "' failed", ex.ToString());
                }
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

        #region Add user to S+

        protected void ButtonDigAcc_Click(object sender, EventArgs e)
        {
            string divStart = "<div style='border:1px solid #ffffff; padding:8px;'>";
            string divEnd = "</div><br>";

            string err = TryGetDigFormErr();
            if (err.Length > 0)
            {
                LiteralDigErr.Text = divStart + err + divEnd;
                LiteralDigErr.Visible = true;
                return;
            }

            //PlaceHolderDigPasswds visible     ==> create new S+ user
            //PlaceHolderDigPasswds NOT visible ==> add sub to S+ user
            err = TryAddUserToBonDig(!PlaceHolderDigPasswds.Visible);
            if (err.Length > 0)
            {
                LiteralDigErr.Text = divStart + err + divEnd;
                LiteralDigErr.Visible = true;
                return;
            }

            PlaceHolderDigForm.Visible = false;
            PlaceHolderDigLinks.Visible = true;
        }

        private string TryGetDigFormErr()
        {
            StringBuilder sb = new StringBuilder();
            string user = MiscFunctions.REC(TextBoxDigUser.Text);

            if (string.IsNullOrEmpty(user))
                sb.Append("Ange ett användarnamn. ");

            if (!MiscFunctions.IsValidEmail(user))
                sb.Append("Användarnamnet måste vara en giltig e-postadress. ");

            if (PlaceHolderDigPasswds.Visible)
            {
                string pass1 = MiscFunctions.REC(TextBoxDigPass1.Text);
                string pass2 = MiscFunctions.REC(TextBoxDigPass2.Text);

                if (pass1.Length < 6 || pass2.Length < 6)
                    sb.Append("Lösenordet måste bestå av minst 6 tecken. ");

                if (pass1 != pass2)
                    sb.Append("Lösenorden måste vara identiska. ");
            }

            return sb.ToString();
        }

        private string TryAddUserToBonDig(bool addSubToExistingDiAccount)
        {
            string contact = " Var god kontakta kundtjäst.";

            // Return: 
            // 1=new passwd saved to S+, 
            // 2=existing passwd not changed in S+, 
            // -1=S+ productId not found, 
            // -2=S+ username does not exist, 
            // -3=S+ username not available, 
            // -4=save user to S+ failed, 
            // -5=S+ userId missing, 
            // -6=failed to create S+ import
            int ret = BonDigHandler.TryAddCustAndSubToBonDig(Sub.PaperCode, Sub.ProductNo, Sub.Subscriber.Cusno, Sub.SubsNo, TextBoxDigUser.Text.Trim(), Sub.Subscriber.FirstName,
                                                             Sub.Subscriber.LastName, Sub.Subscriber.MobilePhone, TextBoxDigPass1.Text.Trim(), addSubToExistingDiAccount);

            if (ret == -1)
                return "Tekniskt fel: efterfrågad produkt hittades inte." + contact;

            if (ret == -2)
                return "Angivet användarnamn hittades tyvärr inte. Var god kontrollera stavningen och försök igen ELLER klicka på länken " +
                       "'Jag har inget Di-konto' för att skapa ett nytt konto.";

            if (ret == -3)
                return "Angivet användarnamn är tyvärr upptaget. Var god försök med ett annat användarnamn ELLER koppla prenumerationen till angivet " +
                       "användarnamn genom att klicka på länken 'Jag har redan ett Di-konto'.";

            if (ret == -4)
                return "Tekniskt fel: kunduppgifterna kunde inte sparas." + contact;

            if (ret == -5)
                return "Tekniskt fel: giltigt användar-ID saknas." + contact;

            if (ret == -6)
                return "Tekniskt fel: Prenumerationen kunde inte sparas på kundbilden." + contact;

            return string.Empty;
        }

        protected void LinkButtonDigHaveAcc_Click(object sender, EventArgs e)
        {
            PlaceHolderDigPasswds.Visible = false;
            LinkButtonDigHaveAcc.Visible = false;
            LinkButtonDigNoAcc.Visible = true;
            ButtonDigAcc.Text = "Koppla Di-konto";
        }

        protected void LinkButtonDigNoAcc_Click(object sender, EventArgs e)
        {
            PlaceHolderDigPasswds.Visible = true;
            LinkButtonDigHaveAcc.Visible = true;
            LinkButtonDigNoAcc.Visible = false;
            ButtonDigAcc.Text = "Skapa Di-konto";
        }

        #endregion
    }
}