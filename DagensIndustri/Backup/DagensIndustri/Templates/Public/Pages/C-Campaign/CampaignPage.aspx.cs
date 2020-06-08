using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using DIClassLib.CardPayment.Nets;

using EPiServer.Core;
using DIClassLib.Subscriptions;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes;
using DIClassLib.CardPayment;
using System.Data;
using DIClassLib.BonnierDigital;
using System.Text;
using System.Text.RegularExpressions;
using DIClassLib.StudentVerification;
using DagensIndustri.Tools.Classes.WebControls;

namespace DagensIndustri.Templates.Public.Pages.C_Campaign
{
    public partial class CampaignPage : EPiServer.TemplatePage
    {
        #region Fields and constants

        private const string QSTTARGETGROUP = "tg";
        private const string QSTPAYSUCCESS = "pay";
        private const string QSTTIPAFRIEND = "tref";
        private const string QSTNETS = "crn";

        #endregion

        #region Properties

        private string ThankYouTextPropertyName
        {
            get
            {
                var propertyName = "ThankYouText";

                if (Sub != null)
                {
                    switch (Sub.PayMethod)
                    {
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
        }

        public bool IsPageInEditMode { get { PageReference pageVersionReference = PageReference.Parse(Request.QueryString["id"]); if (pageVersionReference.WorkID > 0) return true; return false; } } 

        private string SelectedCampaign 
        {
            get { return RbPrimaryCampaign.Checked ? "Campaign1" : "Campaign2"; } 
        }

        private Subscription Sub
        {
            get { return (Subscription)ViewState["subscription"]; }
            set { ViewState["subscription"] = value; }
        }

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

        private bool ShowCampaign2 { get { return IsValue("Campaign2Id") || IsValue("Campaign2IdAutopay") || IsValue("Campaign2IdAutowithdrawal"); } }

        private bool IsTrialPeriod
        {
            get
            {
                if (CurrentPage[SelectedCampaign + "IsTrial"] == null)
                    return false;

                return true;
            }
        }

        #endregion

        #region Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                TryPopulateForm(); 

                //if (IsPageInEditMode && !VerifyCampaigns())
                if (!VerifyCampaigns())
                    return;

                if (Request.QueryString["responseCode"] == null && Request.QueryString[QSTPAYSUCCESS] == null)
                    LiteralAdWordsScriptOnLoad.Text = CurrentPage["AdWordsScriptOnLoad"] as string ?? string.Empty;

                if (HandleReturnFromNets())
                    return;
                if (HandlePaySuccess())
                    return;

                // Sets earliest startdate in datepicker according to business rules
                TxtStartDate.Text = CampaignHelper.GetSubStartDate(CurrentPage.PageLink.ID, CurrentPage[SelectedCampaign + "Id"] as string);
            }

            BindProducts();
            Affix.DataBind();
        }

        protected void BtnSubmitInvoiceClick(object sender, EventArgs e)
        {
            if (CurrentPage[SelectedCampaign + "HideInvoice"] == null || CurrentPage[SelectedCampaign + "FreeCamp"] != null)
            {
                CreatePren(PaymentMethod.TypeOfPaymentMethod.Invoice);
            }
        }

        protected void BtnSubmitCardClick(object sender, EventArgs e)
        {
            if (CurrentPage[SelectedCampaign + "HideCard"] == null)
            {
                CreatePren(PaymentMethod.TypeOfPaymentMethod.CreditCard);
            }
        }

        protected void BtnSubmitInvoiceOtherPayerClick(object sender, EventArgs e)
        {
            if (CurrentPage[SelectedCampaign + "HideInvoiceOther"] == null)
            {
                CreatePren(PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer);
            }
        }

        protected void BtnSubmitAutopayClick(object sender, EventArgs e)
        {
            if (CurrentPage[SelectedCampaign + "HideAutoPay"] == null)
            {
                CreatePren(PaymentMethod.TypeOfPaymentMethod.DirectDebit);
            }
        }

        protected void BtnSubmitAutoWithdrawalClick(object sender, EventArgs e)
        {
            if (CurrentPage[SelectedCampaign + "HideAutoWithdrawal"] == null)
            {
                CreatePren(PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal);
            }
        }
       
        #endregion

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
                    String err = DIClassLib.Subscriptions.AddCustAndSub.Message.ErrMess_DenyShortSub; //Settings.ErrMess_DenyShortSub;
                    MiscFunctions.SendStaffMailFailedSaveSubs(err, Sub, null);
                    ShowError(err);
                    return true;
                }
            }

            return false;
        }

        private bool TryPopulateSub(PaymentMethod.TypeOfPaymentMethod paymethod)
        {
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
            var startDate = DateTime.TryParse(TxtStartDate.Text, out dateParser) ? dateParser : DateTime.Now;

            //Create subscription
            Sub = new Subscription(campId, 0, paymethod, startDate, IsTrialPeriod);

            if (Sub.CampNo <= 0)
                return false;

            return true;
        }

        private String SaveSubscription()
        {
            if (IsValue(SelectedCampaign + "IsDigital") && Sub != null && Sub.Subscriber != null && string.IsNullOrEmpty(Sub.Subscriber.ZipCode))
                Sub.Subscriber.ZipCode = "10000";

            string err = CirixDbHandler.TryInsertSubscription2(Sub, null);
            if (!string.IsNullOrEmpty(err))
                return err;

            if (Sub.Subscriber == null)
                new Logger("SaveSubscription() failed - subscriber=null");

            //TODO: Under testning /TKM
            //MiscFunctions.SendMail("localhost@di.se", "thorbjorn.karlstrom@di.se", "C! save subscription", Sub.ToString(), true);

            return string.Empty;
        }

        #endregion

        #region Nets

        private void HandleCreditCardPayment(PaymentMethod.TypeOfPaymentMethod payMethod)
        {
            string url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) + "?campaign=" + SelectedCampaign;                  

            //Add query strings to Nets request
            var queries = Request.QueryString;
            foreach (var query in queries.AllKeys)
            {
                if (query.ToLower().Equals("campaign") || query.ToLower().StartsWith("utm_") || query.ToLower().Equals("tref"))
                    url += "&" + query + "=" + Request.QueryString[query];
            }

            //Also add extra info field (from form)
            if (IsValue("ExtraInfoHeading") && !string.IsNullOrEmpty(TxtExtraInfo.Text))
            {
                url += "&extrainfomdb=" + TxtExtraInfo.Text;
            }

            string goodsDescr = Settings.GetName_Product(Sub.PaperCode, Sub.ProductNo);
            string comment = Translate("/subscription/newsubscription");
            string consumerName = string.Format("Subscriber {0} {1}", Sub.Subscriber.FirstName, Sub.Subscriber.LastName).Trim();
            double vatPct = (Sub.PaperCode == Settings.PaperCode_DI) ? Settings.VatPaper : Settings.VatIpad;

            var prep = new NetsCardPayPrepare(Sub.TotalPriceExVat, null, vatPct, false, false, url, goodsDescr, comment, consumerName, Sub.Subscriber.Email, null, payMethod, Sub);
        }

        private bool HandleReturnFromNets()
        {
            if (Request.QueryString["responseCode"] != null)
            {
                var transId = MiscFunctions.REC(Request.QueryString["transactionId"]);
                string paySuccessUrl = CurrentPage.LinkURL + "&" + QSTPAYSUCCESS + "=1&crn=" + transId;

                var ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                var prep = ret.NetsPreparePersisted;

                // Get redirecturl from serialized object. This because Nets only returns the first parameter from querystring, the rest is gone.
                // Reported to Nets 2014-04-28 / TKM
                var queryStrings = HttpUtility.ParseQueryString(new Uri(prep.RedirectUrl).Query);
                foreach (var query in queryStrings.AllKeys)
                {
                    if (query.ToLower().Equals("campaign") || query.ToLower().StartsWith("utm_") || query.ToLower().Equals("tref") || query.ToLower().Equals("extrainfomdb"))
                        paySuccessUrl += "&" + query + "=" + queryStrings[query];
                }

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
                SetUpConfirmArea();
                return true;
            }

            return false;
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

            MvConfirmView.SetActiveView(ViewConfirmStep2);
            //PlaceHolderDigForm.Visible = false;
            //PlaceHolderDigLinks.Visible = true;
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

        #region Verifiers

        private bool VerifyCampaigns()
        {
            if ((IsValue("Campaign1IsStudent") && !IsValue("Campaign1HideInvoiceOther")) || (IsValue("Campaign2IsStudent") && !IsValue("Campaign2HideInvoiceOther")))
            {
                ShowError("Du kan inte ha betalalternativ 'Faktura annan betalare' på en studentkampanj");
                return false;
            }

            var error = CampIdVerified();
            if (!string.IsNullOrEmpty(error))
            {
                ShowError(error);
                return false;
            }

            if (!TargetGroupsVerified())
            {
                ShowError("Felaktig målgrupp");
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
            if(string.IsNullOrEmpty(birthNo))
                return false;

            birthNo = birthNo.Substring(2);

            StudentVerifier ver = new StudentVerifier();
            if (ver.VerifyByBirthNum(birthNo) == "1")
                return true;

            return false;
        }

        #endregion

        private void BindProducts()
        {   
            var propName = "CampaignIncludedProducts";

            //Info: Currently no support for different products on Campaign1 and Campaign2
            if (IsValue(propName))
            {
                var productPages = CurrentPage[propName].ToString().Split(',');
                var campaignIncludedList = new DiLinkCollection(EPiFunctions.SettingsPage(CurrentPage), "CampaignProductList");
                var pages = campaignIncludedList.SelectedPages().Where(page => Array.IndexOf(productPages, page.PageLink.ID.ToString()) > -1).Select(t => t).ToList();

                PlProducts.DataSource = pages;
                PlProducts.DataBind();
            }
        }

        private Person GetPerson(bool otherPayer)
        {
            var ssn = TxtSsn.Text;
            //If manual form, handle as private person
            var isPrivate = string.IsNullOrEmpty(ssn) ? true : new Regex(@"(^[\d]{12}$)").IsMatch(ssn);  //TODO: check if regexp should be changed /TKM

            return new Person(!otherPayer,
                              false,
                              otherPayer ? string.Empty : TxtFirstName.Text,
                              otherPayer ? string.Empty : TxtLastName.Text,
                              otherPayer ? string.Empty : TxtAddressCo.Text,
                              otherPayer ? TxtOpCompany.Text : TxtCompany.Text,
                              otherPayer ? TxtOpStreetAddress.Text : TxtStreetAddress.Text,
                              otherPayer ? TxtOpHouseNo.Text : TxtHouseNo.Text,
                              otherPayer ? string.Empty : TxtStairCase.Text,
                              otherPayer ? string.Empty : TxtStairs.Text,
                              otherPayer ? string.Empty : TxtAppartmentNo.Text,
                              otherPayer ? TxtOpZip.Text : TxtZipCode.Text,
                              "", //formControl.City,
                              otherPayer ? TxtOpPhone.Text : TxtPhone.Text,
                              otherPayer ? string.Empty : TxtEmail.Text,
                              otherPayer ? string.Empty : ssn, //pno
                              otherPayer ? ssn : string.Empty, //orgno 
                              otherPayer ? TxtOpAttention.Text: string.Empty, //otherPayer ? formControl.Attention : string.Empty, 
                              string.Empty);
        }

        private void CreatePren(PaymentMethod.TypeOfPaymentMethod selectedPayMethod) 
        {
            if (!Page.IsValid)
                return;

            if (!TryPopulateSub(selectedPayMethod))
            {
                ShowError(Translate("/common/errors/errorcamp"));
                return;
            }

            if (IsValue(SelectedCampaign + "IsStudent") && !VerifyFullTimeStudent(TxtStudSsn.Text)) {
                ShowError(Translate("/dicampaign/forms/studentverification"));
                return;
            }

            Sub.SetMembersByPayMethod(selectedPayMethod);
            Sub.TargetGroup = TargetGroup;
            Sub.Subscriber = GetPerson(false);
            Sub.SubscriptionPayer = selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer ? GetPerson(true) : null;

            if (selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.Invoice || selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer || selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.DirectDebit)
            {
                var err = SaveSubscription();
                if (String.IsNullOrEmpty(err))
                {
                    SetUpConfirmArea();
                }
                else
                {
                    ShowError(err);
                }
                
            }
            else if (selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCard || selectedPayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)
            {
                if (DenyShortSub())
                    return;

                HandleCreditCardPayment(selectedPayMethod); //redirect to Nets
            }        
        }

        private void SetUpConfirmArea()
        {
            Affix.Visible = false;
            Benefits.Visible = false;
            PhSubHeader.Visible = false;
            Footer.Visible = false;
            ThankYouHeading.Visible = true;
         
            MvHeroView.SetActiveView(ViewConfirm);

            //Tref is a query from tip-a-friend page, flag user as pren
            var tref = Request.QueryString[QSTTIPAFRIEND];
            if (!string.IsNullOrEmpty(tref) && MiscFunctions.IsGuid(tref))
                MsSqlHandler.UpdateInviteFriend(new Guid(tref), true);

            //From Nets, set Subscription object
            var crn = Request.QueryString[QSTNETS];
            if (!string.IsNullOrEmpty(crn))
            {
                NetsCardPayPrepare prep = NetsCardPayReturn.ReadPersisted(crn);
                if (prep != null)
                    Sub = (Subscription)prep.PersistedObj;
            }

            //Sync subscriber to mssql logn table
            if (Sub != null && Sub.Subscriber.Cusno > 0)
            {
                DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncCustToMssqlLoginTables((int)Sub.Subscriber.Cusno);

                //From querystring if card payment
                var extraInfo = !string.IsNullOrEmpty(TxtExtraInfo.Text) ? TxtExtraInfo.Text : Request.QueryString["extrainfomdb"];

                //Save extra info to MDB
                if (IsValue("ExtraInfoHeading") && !string.IsNullOrEmpty(extraInfo))
                {
                    MsSqlHandler.MdbInsertExtraInfo(CurrentPage.PageLink.ID, Sub.Subscriber.Cusno, CurrentPage["ExtraInfoHeading"].ToString(), extraInfo);
                }
            }

            //Get correct thankstext depending on payment method
            var msg = CurrentPage[ThankYouTextPropertyName] as string;
            LblConfirmText.Text = string.Format(msg, Sub.SubsStartDate.ToString("dddd dd MMMM")); 

            LiteralAdWordsScriptOnThankYou.Text = CurrentPage[SelectedCampaign + "AdWordsScriptOnThankYou"] as string ?? string.Empty;
            TextBoxDigUser.Text = Sub.Subscriber.Email;
        }

        private void ShowError(string error) 
        {
            LblError.Text = error;

            //If in editmode, make the message very clear for editors
            var script = IsPageInEditMode ? 
                "<script type='text/javascript'>$('#modalError').modal({backdrop: 'static',show:true});$('#modalError .alert').css('height', '500');$('#modalError .modal-header, #modalError .modal-footer').css('display', 'none');$('#modalError').css('display', 'block');</script>" :
                "<script type='text/javascript'>$('#modalError').modal('show');</script>";

            LiteralScript.Text = script;
        }


        /// <summary>
        /// If code (GUID) is in MDB, populate the form
        /// </summary>
        private void TryPopulateForm()
        {
            var code = Request.QueryString["code"];

            if (string.IsNullOrEmpty(code) && !MiscFunctions.IsGuid(code))
                return;

            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    long cusNo = MsSqlHandler.MdbGetCusnoByCode(new Guid(code));

                    if (cusNo > 0)
                    {
                        var subscriber = new SubscriptionUser2(cusNo);

                        TxtCompany.Text = subscriber.IsCompanyCust ? subscriber.RowText1 : string.Empty;

                        //TODO: IsCompanyCust verkar inte riktigt stämma? /TKM
                        //Tips från coachen (Janne): reverseline innehåller 0,1 eller 2. Om den har något annat värde än 0 så talar den om vilken av rowtext1 eller rowtext2 som innehåller personnamn.
                        //Den är dock inte heltäckande, är värdet 0 så betyder det att det inte är satt. Vilket inte är samma sak som att ingen rad innehåller person.
                        var name = subscriber.IsCompanyCust ? subscriber.RowText2 : subscriber.RowText1;

                        if (name != null)
                        {
                            var nameArray = name.Split(' ');

                            TxtLastName.Text = nameArray.Length > 0 ? nameArray[0] : string.Empty;
                            TxtFirstName.Text = nameArray.Length > 1 ? nameArray[1] : string.Empty;
                        }

                        TxtPhone.Text = subscriber.OPhone;
                        TxtEmail.Text = subscriber.Email;
                        TxtStreetAddress.Text = subscriber.StreetName;
                        TxtHouseNo.Text = subscriber.HouseNo;
                        TxtStairCase.Text = subscriber.Staricase;
                        TxtStairs.Text = subscriber.Apartment;
                        
                        if (subscriber.Street2 != null)
                        {
                            var lghflag = false;
                            foreach (var item in subscriber.Street2.Split(' '))
                            {
                                if (item.ToLower().StartsWith("lgh"))
                                    lghflag = true;

                                if (lghflag)
                                    TxtAppartmentNo.Text += item;
                                else
                                    TxtAddressCo.Text += item + " ";
                            }
                        }

                        //Remove prefix lgh
                        TxtAppartmentNo.Text = TxtAppartmentNo.Text.ToLower().Replace("lgh", string.Empty);
                        TxtZipCode.Text = subscriber.Zip;
                    }
                }
                catch (Exception ex)
                {
                    new Logger("Campaign.TryPopulateForm() for url code '" + code + "' failed", ex.ToString());
                }
            }
        }
    }
}
