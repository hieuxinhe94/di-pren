using System;
using System.Collections.Generic;
using System.Linq;
using DIClassLib.CardPayment.Nets;
using DIClassLib.Subscriptions.AddCustAndSub;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using System.Data;
using System.Text;
using DagensIndustri.Tools.Classes;
using DIClassLib.StudentVerification;
using DIClassLib.Membership;
using DagensIndustri.Tools.Classes.Campaign;

namespace DagensIndustri.Templates.Public.Pages.DiCampaign
{
    public partial class Campaign : DiTemplatePage
    {
        private const string GeneralErrorMessage = "Ett tekniskt fel uppstod tyvärr då beställningen skulle göras.<br />Vänligen kontakta vår kundtjänst på tel <a href=\"tel:0857365100\">08-573 651 00</a>";
        private const string QSTEMAIL       = "excus";      //querystring variables
        private const string QSTPOSTAL      = "adr";
        private const string QSTTARGETGROUP = "tg";
        private const string QSTPAYSUCCESS  = "pay";
        private const string CAMPID         = "campId";


        public bool IsAutogiro
        {
            get { return EPiFunctions.HasValue(CurrentPage, "CampaignIsAutoGiro"); }
        }

        public bool IsAutowithdrawal
        {
            get { return EPiFunctions.HasValue(CurrentPage, "CampaignIsAutowithdrawal"); }
        }

        public PaymentMethod.TypeOfPaymentMethod DefaultPayMethod
        {
            get
            {
                if (IsAutogiro)
                    return PaymentMethod.TypeOfPaymentMethod.DirectDebit;

                if (IsAutowithdrawal)
                    return PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal;

                return PaymentMethod.TypeOfPaymentMethod.Invoice;
            }
        }

        //public bool IsStudentCampaign
        //{
        //    get { return EPiFunctions.HasValue(CurrentPage, "IsStudentCampaign"); }
        //}

        public bool IsDigitalCampaign
        {
            get { return EPiFunctions.HasValue(CurrentPage, "IsDigitalCampaign"); }
        }

        public bool HasDiGuldLandingPage
        {
            get { return EPiFunctions.HasValue(CurrentPage, "LandingPage"); }
        }


        public string AdWordsScriptOnLoad 
        {
            get 
            {
                if (EPiFunctions.HasValue(CurrentPage, "AdWordsScriptOnLoad"))
                    return CurrentPage["AdWordsScriptOnLoad"].ToString();

                return string.Empty;
            }
        }

        public string AdWordsScriptOnThankYou
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "AdWordsScriptOnThankYou"))
                    return CurrentPage["AdWordsScriptOnThankYou"].ToString();

                return string.Empty;
            }
        }


        public bool IsUrlCodeEmail
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString[QSTEMAIL]))
                    return true;

                return false;
            }
        }

        public bool IsUrlCodePostal
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString[QSTPOSTAL]))
                    return true;

                return false;
            }
        }

        public string UrlCode 
        {
            get
            {
                if(IsUrlCodeEmail || IsUrlCodePostal)
                    return IsUrlCodeEmail ? Request.QueryString[QSTEMAIL] : Request.QueryString[QSTPOSTAL];

                return string.Empty;
            }
        }

        public string UrlTargetGroup
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString[QSTTARGETGROUP]))
                    return Request.QueryString[QSTTARGETGROUP];

                return string.Empty;
            }
        }

        public Subscription Sub
        {
            get { return (Subscription)ViewState[CAMPID]; }
            set { ViewState[CAMPID] = value; }
        }

        public Subscription ThankYouSub
        {
            get { return (Subscription)ViewState["ThankYouSub"]; }
            set { ViewState["ThankYouSub"] = value; }
        }

        //private Subscription _subCardPayFailed = null;
        //public Subscription SessionSubCardPayFailed 
        //{
        //    get
        //    {
        //        if (_subCardPayFailed != null)
        //            return _subCardPayFailed;

        //        if (Session["SubCardPayFailed"] != null)
        //            return (Subscription)Session["SubCardPayFailed"];

        //        return null;
        //    }
        //    set
        //    {
        //        _subCardPayFailed = value;
        //        Session["SubCardPayFailed"] = value;
        //    }
        //}



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            int pageId = CurrentPage.PageLink.ID;
            if (Settings.CampEpiPageIdsRedirToMobPage.Contains(pageId))
            {
                string usrAgt = Request.UserAgent;
                if (!string.IsNullOrEmpty(usrAgt) && usrAgt.ToLower().Contains("mobile"))
                    Response.Redirect("/misc/mobilkampanj?pageid=" + pageId, true);
            }

            #region OLD redir with js
            //if (Settings.CampEpiPageIdsRedirToMobPage.Contains(CurrentPage.PageLink.ID))
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine("<script type='text/javascript'>");
            //    sb.AppendLine("$(document).ready(function () {");
            //    sb.AppendLine("if (navigator.userAgent.match(/mobile/i)) {");
            //    sb.AppendLine("window.location = '/misc/mobilkampanj?pageid=" + CurrentPage.PageLink.ID + "';");
            //    sb.AppendLine("}");
            //    sb.AppendLine("});");
            //    sb.AppendLine("</script>");
            //    Page.ClientScript.RegisterClientScriptBlock(GetType(), "jsRedirToMobCampPage", sb.ToString());
            //}
            #endregion
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //base.UserMessageControl = UserMessageControl;
            //UserMessageControl.ClearMessage();

            LiteralAdWordsScriptOnLoad.Visible = false;
            LiteralAdWordsScriptOnThankYou.Visible = false;

  
            if (!IsPostBack)
            {
                DisplayPlaceHolders(false, false);

                //first time page is loaded
                if (Request.QueryString["responseCode"] == null && Request.QueryString[QSTPAYSUCCESS] == null)
                    PrintAdWordsScriptOnLoad();

                if (HandleReturnFromNets())
                    return;

                if (HandlePaySuccess())         //pay=1 in url
                    return;

                if (IsAutogiro && IsAutowithdrawal)
                {
                    ShowMessage("Kampanjer kan inte ha betalningssätten 'Autogiro' OCH 'Autodragning' samtidigt - var god välj ett av dem.", true, true);
                    return;
                }
                
                if (!IsValue("CampId") || !TryPopulateSub())
                {
                    ShowMessage("/common/errors/errorcamp", true, true);
                    return;
                }

                TryPopulateForm();
                DisplayPlaceHolders(true, false);
                TrySetVisitDate();
            }
        }

        private void PrintAdWordsScriptOnLoad()
        {
            LiteralAdWordsScriptOnLoad.Text = AdWordsScriptOnLoad;
            LiteralAdWordsScriptOnLoad.Visible = true;
        }

        private bool HandleReturnFromNets()
        {
            if (Request.QueryString["responseCode"] != null)
            {
                //SessionSubCardPayFailed = null;
                
                var transId = MiscFunctions.REC(Request.QueryString["transactionId"]);
                string paySuccessUrl = CurrentPage.LinkURL + "&" + QSTPAYSUCCESS + "=1&crn=" + transId;

                var ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                NetsCardPayPrepare prep = ret.NetsPreparePersisted;
                Sub = (Subscription)prep.PersistedObj;

                bool payOk = ret.HandleNetsReturn(paySuccessUrl, Sub.Subscriber.Email);
                if (payOk)
                {
                    Sub.CreditCardPaymentOk = true;
                    SaveSubscription(prep.CustomerRefNo, prep.PayMethod, ret);

                    if (Sub.SubsNo > 0)
                    {
                        MsSqlHandler.AppendToPayTransComment(prep.CustomerRefNo, "cusno: " + Sub.Subscriber.Cusno);

                        //if (DefaultPayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)
                        //{
                        //    SubscriptionController.CreateNewInvoice(Sub);
                        //    MsSqlHandler.InsertAwd2(ret, Sub.Subscriber.Cusno, Sub.SubsNo, Sub.CampNo, Sub.SubsEndDate);
                        //}
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
                // läs in subscription
                string transactionId = Request.QueryString["crn"];
                NetsCardPayPrepare prep = NetsCardPayReturn.ReadPersisted(transactionId);
                Sub = (Subscription)prep.PersistedObj;
                ShowThankYou();
                return true;
            }

            return false;
        }


        public void ShowThankYou()
        {
            if (!TryHandleGoldCamp())
            {
                PrintLiteralAdWordsOnThankYou();
                DisplayPlaceHolders(false, false);
                ThankYouPlaceholder.Visible = true;
            }
        }

        /// <summary>
        /// add user to gold role, login user, redirect page
        /// </summary>
        private bool TryHandleGoldCamp()
        {
            if (CurrentPage["CampaignIsDiGold"] != null)
            {
                if (Sub.Subscriber.Cusno > 0 && DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncCustToMssqlLoginTables((int)Sub.Subscriber.Cusno) == 1)
                {
                    DiRoleHandler.AddUserToRoles(Sub.Subscriber.UserName, new string[] { DiRoleHandler.RoleDiGold });
                    if (LoginUtil.ReLoginUserRefreshCookie(Sub.Subscriber.UserName, Sub.Subscriber.Password))
                    {
                        Response.Redirect(GetGoldLandingPageUrl());
                        return true;
                    }
                }
            }

            return false;
        }

        private string GetGoldLandingPageUrl()
        {
            PageReference pr = CurrentPage.PageLink;

            if (CurrentPage["LandingPage"] != null)
                pr = (PageReference)CurrentPage["LandingPage"];

            return EPiFunctions.GetFriendlyAbsoluteUrl(pr);
        }


        private void PrintLiteralAdWordsOnThankYou()
        {
            LiteralAdWordsScriptOnThankYou.Text = AdWordsScriptOnThankYou;
            LiteralAdWordsScriptOnThankYou.Visible = true;
        }
       

        private bool TryPopulateSub()
        {
            Sub = new Subscription(CurrentPage["CampId"].ToString(), 0, DefaultPayMethod, DateTime.Now, false);
            
            if (Sub.CampNo <= 0)
                return false;

            return true;
        }

        
        public bool VerifyFullTimeStudent(string birthNo)
        {
            StudentVerifier ver = new StudentVerifier();
            if (ver.VerifyByBirthNum(birthNo) == "1")
                return true;

            return false;
        }

        // Validate for DI Guld landing page (birthdate) yyyymmdd
        public bool ValidateSocialSecurityNumber(string ssn)
        {
            if (!String.IsNullOrEmpty(ssn) && ssn.Length == 8)
            {
                long l = -1;
                if (Int64.TryParse(ssn,out l))
                {
                    return true;
                }
            }
                 
            return false;
        }

        public String SaveSubscription(int? customerRefNo = null, PaymentMethod.TypeOfPaymentMethod payMethod = PaymentMethod.TypeOfPaymentMethod.Invoice, NetsCardPayReturn ret = null)
        {
            /*if (IsDigitalCampaign && Sub != null && Sub.Subscriber != null && string.IsNullOrEmpty(Sub.Subscriber.ZipCode))
                Sub.Subscriber.ZipCode = "10000";

            string err = SubscriptionController.TryInsertSubscription2(Sub);
            if (!string.IsNullOrEmpty(err))
                return err;

            if (Sub.Subscriber != null)
                TrySetBoughtDate(Sub.Subscriber.Cusno);
            else
                new Logger("SaveSubscription() failed - subscriber=null");

            return "";
             * */
            string err = string.Empty;
            if (IsDigitalCampaign && Sub != null && Sub.Subscriber != null && string.IsNullOrEmpty(Sub.Subscriber.ZipCode))
                Sub.Subscriber.ZipCode = "10000";
            try
            {
                if (Sub == null || string.IsNullOrEmpty(Sub.CampId))
                {
                    new Logger("Pren error CampaignPaper: sub or campid null");
                    return GeneralErrorMessage + "<!-- Error: sub null -->";
                }

                var addCustAndSubHandler = new AddCustAndSubHandler();
                var addCustSubRet = addCustAndSubHandler.TryAddCustAndSub(Sub, customerRefNo, !IsDigitalCampaign);
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
                new Logger("Pren error Campaign.aspx template: ", ex.Message);
            }

            if (!string.IsNullOrEmpty(err))
                return err;

            if (Sub.Subscriber == null)
                new Logger("SaveSubscription() failed - subscriber=null");

            return string.Empty;
        }




        /// <summary>
        /// overriding DiTemplatePage/ShowMessage(), since it does not work when using MP/wideContTemplate
        /// </summary>
        private new void ShowMessage(string translateKey, bool isKey, bool isErrorMessage)
        {
            string message = isKey ? Translate(translateKey) : translateKey;
            LabelErr.ForeColor = isErrorMessage ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            LabelErr.Text = message;
            LabelErr.Visible = !string.IsNullOrEmpty(message);
        }
        
        //private void SaveSubscriptionDetails()
        //{
        //    try
        //    {
        //        if (Sub.SubsNo > 0)
        //        {
        //            long cusno = Sub.Subscriber.Cusno;

        //            //if customer does not exists in db: insert new, else update
        //            DataSet dsCustomer = MsSqlHandler.GetCustomer(Sub.Subscriber.UserName);
        //            if (dsCustomer == null || dsCustomer.Tables.Count == 0 || dsCustomer.Tables[0].Rows[0]["cusno"] == DBNull.Value)
        //                MsSqlHandler.InsertCustomer(cusno, Sub.Subscriber.UserName, Sub.Subscriber.Password, Sub.Subscriber.Email, Sub.Subscriber.SocialSecurityNo);
        //            else
        //                MsSqlHandler.UpdateCustomer(cusno, Sub.Subscriber.UserName, Sub.Subscriber.Password, Sub.Subscriber.Email, Sub.Subscriber.SocialSecurityNo);

        //            //Add subscription details
        //            MsSqlHandler.InsertSubscription(Sub.SubsNo, cusno, Sub.ProductNo, Sub.PaperCode, Sub.SubsEndDate, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("SaveSubscriptionDetails() - failed", ex.ToString());
        //    }
        //}

        public void HandleCreditCardPayment()
        {
            string url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string goodsDescr = Settings.GetName_Product(Sub.PaperCode, Sub.ProductNo);
            string comment = Translate("/subscription/newsubscription");
            string consumerName = string.Format("Subscriber {0} {1}", Sub.Subscriber.FirstName, Sub.Subscriber.LastName).Trim();
            double vatPct = SubscriptionController.GetProductVat(Sub.PaperCode, Sub.ProductNo);
            PaymentMethod.TypeOfPaymentMethod payMet = IsAutowithdrawal ? PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal : PaymentMethod.TypeOfPaymentMethod.CreditCard;
            var prep = new NetsCardPayPrepare(Sub.TotalPriceExVat, null, vatPct, false, false, url, goodsDescr, comment, consumerName, Sub.Subscriber.Email, null, payMet, Sub);
        }


        public string GetTermsAndConditions()
        {
            string ret = string.Empty;
            PageReference termsAndConditionsPageLink = EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldTermsPage") as PageReference;
            if (termsAndConditionsPageLink != null)
            {
                PageData termsPageData = EPiServer.DataFactory.Instance.GetPage(termsAndConditionsPageLink);

                ret = string.Format(Translate("/dicampaign/readacceptedterms"),
                                                    EPiFunctions.GetFriendlyAbsoluteUrl(termsPageData.PageLink),
                                                    !string.IsNullOrEmpty((string)termsPageData["Heading"]) ? (string)termsPageData["Heading"] : termsPageData.PageName
                                                    );
            }
            return ret;
        }

        public string GetAutomaticDiGuldMembershipInformation()
        {
            string ret = string.Empty;
            PageReference termsAndConditionsPageLink = EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldTermsPage") as PageReference;
            if (termsAndConditionsPageLink != null)
            {
                PageData termsPageData = EPiServer.DataFactory.Instance.GetPage(termsAndConditionsPageLink);

                ret = string.Format(Translate("/common/message/digold/automaticmembership"),
                                                    EPiFunctions.GetFriendlyAbsoluteUrl(termsPageData.PageLink),
                                                    !string.IsNullOrEmpty((string)termsPageData["Heading"]) ? (string)termsPageData["Heading"] : termsPageData.PageName
                                                    );
            }
            return ret;
        }
        /*
        /// <summary>
        /// set dateVisitedCamp or dateBoughtCamp in campaign db
        /// </summary>
        private void TrySetVisitOrBoughtDate(bool setDateVisited)
        {
            string code = UrlCode;
            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    if (setDateVisited)
                        MsSqlHandler.SetDateVisitedCamp(code, CurrentPageLink.ID);
                    else
                        MsSqlHandler.SetDateBoughtCamp(code, CurrentPageLink.ID); 
                }
                catch (Exception ex)
                {
                    string s = setDateVisited ? "TrySetDateVisitedCamp()" : "TrySetDateBoughtCamp()";
                    new Logger(s + " failed for code=" + code, ex.ToString());
                }
            }
        }
        */
        /// <summary>
        /// Set timestamp on VisitDate
        /// </summary>
        private void TrySetVisitDate()
        {
            string code = UrlCode;
            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    MsSqlHandler.SetDateVisitedCamp(code, CurrentPageLink.ID);
                }
                catch (Exception ex)
                {
                    string s = "TrySetVisitDate()";
                    new Logger(s + " failed for code=" + code, ex.ToString());
                }
            }
        }


        /// <summary>
        /// Set timestamp on BoughtDate with the current cusno
        /// </summary>
        /// <param name="subsno"></param>
        //private void TrySetBoughtDate(long cusno)
        //{
        //    string code = UrlCode;
        //    if (!string.IsNullOrEmpty(code))
        //    {
        //        try
        //        {
        //            MsSqlHandler.SetDateBoughtCamp(code, CurrentPageLink.ID, cusno);
        //        }
        //        catch (Exception ex)
        //        {
        //            string s = "TrySetBoughtDate";
        //            new Logger(s + " failed for code=" + code, ex.ToString());
        //        }
        //    }
        //}

        public string GetTargetGroup()
        {
            //...tg=xxx in URL
            if (!string.IsNullOrEmpty(UrlTargetGroup))
                return UrlTargetGroup;
            
            
            string reg = null;
            string email = null;
            string postal = null;

            DataSet ds = MsSqlHandler.GetCampaignTargetGroups(CurrentPageLink.ID);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int id = int.Parse(dr["targetGroupTypeId"].ToString());
                    string tg = dr["targetGroup"].ToString();
                    if (id == Settings.TargetGroupType_Regular) reg = tg;
                    if (id == Settings.TargetGroupType_Email) email = tg;
                    if (id == Settings.TargetGroupType_Postal) postal = tg;
                }
            }

            if (reg == null) reg = string.Empty;   //reg targGr is mandatory, so this should not happen...
            if (email == null) email = reg;
            if (postal == null) postal = reg;

            if (IsUrlCodeEmail) return email;
            if (IsUrlCodePostal) return postal;

            return reg;
        }

        public void DisplayPlaceHolders(bool mainForm, bool otherPayerForm)
        {
            MainFormPlaceHolder.Visible = mainForm;
            OtherPayerPlaceHolder.Visible = otherPayerForm;
        }

        protected void TryPopulateForm()
        {
            string code = UrlCode;

            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    //DataSet ds = DIClassLib.DbHandlers.MsSqlHandler.GetCust(code, CurrentPageLink.ID);
                    DataSet ds = DIClassLib.DbHandlers.MsSqlHandler.GetCustByCode(code);

                    string firstName = ds.Tables[0].Rows[0]["firstName"].ToString().Trim() ?? string.Empty;
                    string lastName = ds.Tables[0].Rows[0]["lastName"].ToString().Trim() ?? string.Empty;
                    string email = ds.Tables[0].Rows[0]["email"].ToString().Trim() ?? string.Empty;
                    string streetName = ds.Tables[0].Rows[0]["streetName"].ToString().Trim() ?? string.Empty;
                    string streetNum = ds.Tables[0].Rows[0]["streetNo"].ToString().Trim() ?? string.Empty;
                    string door = ds.Tables[0].Rows[0]["streetLetter"].ToString().Trim() ?? string.Empty;
                    string stairs = GetFormattedStairs(ds);
                    string appartmentNum = string.Empty;
                    string zip = ds.Tables[0].Rows[0]["zipCode"].ToString().Replace(" ", "") ?? string.Empty;
                    string city = ds.Tables[0].Rows[0]["city"].ToString().Trim() ?? string.Empty;
                    string careOf = GetFormattedCoAddress(ds);
                    string phoneMob = GetFormattedPhoneMob(ds);
                    string company = ds.Tables[0].Rows[0]["company"].ToString().Trim().Trim() ?? string.Empty;
                    string companyNo = GetFormattedCompanyNo(ds);

                    if (!TrySetVariablesFromAvdStreetName(ref streetName, ref streetNum, ref door, ref stairs, ref appartmentNum))
                        TrySetStreetNumAndDoor(ds, ref streetNum, ref door);

                    CampForm.PopulateChildForm(firstName, lastName, email, streetName, streetNum, door, stairs, appartmentNum, zip, city, careOf, phoneMob, company);
                    OtherPayerForm.Populate(firstName, lastName, streetName, streetNum, zip, city, careOf, phoneMob, company, companyNo);

                }
                catch (Exception ex)
                {
                    new Logger("TryPopulateForm() failed for code:" + code, ex.ToString());
                }
            } else {
                // If we have a stored user in the session, use these fields. (From Gasell registration)
                UserFields uf = UserFields.GetFromSession();
                if (uf != null)
                {
                    CampForm.PopulateChildForm(uf.FirstName, uf.LastName, uf.Email, uf.StreetAddress, uf.StreetNumber, uf.Door, uf.Stairs, uf.Apartment, uf.Zip, uf.City, uf.CoAddress, uf.Telephone, uf.Company);
                    OtherPayerForm.Populate(uf.FirstName, uf.LastName, uf.StreetAddress, uf.StreetNumber, uf.Zip, uf.City, uf.CoAddress, uf.Telephone, uf.Company, "");
                }
            }
        }

        private string GetFormattedStairs(DataSet ds)
        {
            string s = ds.Tables[0].Rows[0]["stairs"].ToString().Trim() ?? string.Empty;
            
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            
            return (s.ToUpper().EndsWith("TR")) ? s.Substring(0, s.Length - 2) : s;
        }

        private string GetFormattedCoAddress(DataSet ds)
        {
            string s = ds.Tables[0].Rows[0]["coAddress"].ToString().Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (s.ToLower().StartsWith("c/o "))
                s = s.Remove(0, 4);

            return s;
        }

        private string GetFormattedPhoneMob(DataSet ds)
        {
            string ph = ds.Tables[0].Rows[0]["phoneMobile"].ToString().Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(ph))
                return string.Empty;
            
            //make sure that num starts with +46
            if (ph.StartsWith("0"))
                ph = "+46" + ph.Substring(1);

            if (ph.StartsWith("46"))
                ph = "+" + ph;

            //well formatted abroad num is ok
            if (!ph.StartsWith("+"))
                return string.Empty;

            return ph;
        }

        private string GetFormattedCompanyNo(DataSet ds)
        {
            string s = ds.Tables[0].Rows[0]["companyNo"].ToString().Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(s))
                return string.Empty;

            s = s.Replace("-", "");
            s = s.Replace(" ", "");

            return s;
        }

        #region -- TrySetVariablesFromAvdStreetName --
        //try set: streetName, streetNo, streetLetter
        //handle cases:
        //street name
        //street name    1
        //street name    1    b
        //street name    1    1tr
        //street name    1    place
        //street name    5-6
        //street name    1    b     kp
        //street name    1    lgh   3
        private bool TrySetVariablesFromAvdStreetName(ref string streetName, ref string streetNum, ref string door, ref string stairs, ref string appartmentNum)
        {
            if (string.IsNullOrEmpty(streetName))
                return false;

            string[] arr = streetName.Split(' ');

            //streetnum position in array is key for other operations
            int arrIdStreetNum = GetArrIdForStreetNum(arr);
            if (arrIdStreetNum == -1)
                return false;

            streetName = GetStreet(arr, arrIdStreetNum);
            streetNum  = arr[arrIdStreetNum];


            //handle case: "lgh" in arr
            int lghPos = TryGetLghPosition(arr);
            if (lghPos > 0)
            {
                int lghNumPos = lghPos + 1;
                if ((arr.Length - 1) >= lghNumPos)
                    appartmentNum = arr[lghNumPos];
            }
            else
            {
                door = GetDoor(arr, arrIdStreetNum);
                stairs = GetStairs(arr, arrIdStreetNum);
            }

            return true;
        }

        private int GetArrIdForStreetNum(string[] arr)
        {
            int i = 0;
            foreach (string s in arr)
            {
                if (MiscFunctions.IsNumeric(s))
                    return i;

                i++;
            }

            return -1;
        }

        private string GetStreet(string[] arrStreet, int idSteetNum)
        {
            //everything in front of streetnum is streetname
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < idSteetNum; i++)
                sb.Append(arrStreet[i] + " ");

            return sb.ToString().Trim();
        }

        private int TryGetLghPosition(string[] arr)
        {
            int i = 0;
            foreach (string s in arr)
            {
                if (s.ToUpper() == "LGH")
                    return i;

                i++;
            }

            return 0;
        }

        private string GetDoor(string[] arrStreet, int idSteetNum)
        {
            if (arrStreet.Length > idSteetNum + 1)
            {
                string door = arrStreet[idSteetNum + 1].ToUpper();
                if (!door.Contains("TR"))
                    return door;
            }
            return string.Empty;
        }

        private string GetStairs(string[] arrStreet, int idSteetNum)
        {
            if (arrStreet.Length > idSteetNum + 1)
            {
                for (int i = idSteetNum + 1; i < arrStreet.Length; i++)
                {
                    string stairs = arrStreet[i].ToUpper();
                    if (stairs.EndsWith("TR"))
                        return stairs.Substring(0, stairs.Length - 2);
                }
            }
            return string.Empty;
        }
        #endregion

        #region -- TrySetVariablesFromAvdStreetName = FALSE --
        private void TrySetStreetNumAndDoor(DataSet ds, ref string streetNum, ref string door)
        {
            string sn = ds.Tables[0].Rows[0]["streetno"].ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(sn) || MiscFunctions.IsNumeric(sn))
            {
                streetNum = sn;
                door = ds.Tables[0].Rows[0]["streetletter"].ToString() ?? string.Empty;
            }
            else
            {
                List<string> str = SplitStreetNoWithLetter(sn);
                streetNum = str[0];
                door = str[1].ToUpper();
            }
        }

        ///streetno=4a - do split - ret[0]=4, ret[1]=a
        private List<string> SplitStreetNoWithLetter(string streetNo)
        {
            List<string> ret = new List<string>(2);
            ret.Add(string.Empty);
            ret.Add(string.Empty);

            int i = 0;
            for (i = 0; i < streetNo.Length; i++)
            {
                if (!MiscFunctions.IsNumeric(streetNo[i].ToString()))
                {
                    ret[0] = streetNo.Substring(0, i);
                    ret[1] = streetNo.Substring(i);
                    break;
                }
            }

            return ret;
        }
        #endregion

        
    }
}