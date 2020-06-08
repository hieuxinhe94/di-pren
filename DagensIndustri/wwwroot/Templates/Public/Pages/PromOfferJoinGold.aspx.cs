using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using System.Text;
using DIClassLib.DbHelpers;
using System.Data;
using DIClassLib.Membership;
using DIClassLib.GoldMember;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class PromOfferJoinGold : DiTemplatePage
    {
        string _urlCode = "code";
        string _viewStateSubUsr = "subUsr";
        string _viewStateMssqlUsr = "mssqlUsr";


        public string OfferName
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "OfferName"))
                    return CurrentPage["OfferName"].ToString();

                return string.Empty;
            }
        }

        public string IntroText
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "MainIntro2"))
                    return CurrentPage["MainIntro2"].ToString();

                return string.Empty;
            }
        }

        public string BreadText
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "MainBody2"))
                    return CurrentPage["MainBody2"].ToString();

                return string.Empty;
            }
        }

        public string ThankYouText
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "ThankYouText"))
                    return CurrentPage["ThankYouText"].ToString();

                return string.Empty;
            }
        }

        public SubscriptionUser2 SubUser 
        {
            get 
            {
                if (ViewState[_viewStateSubUsr] != null)
                    return (SubscriptionUser2)ViewState[_viewStateSubUsr];

                return null;
            }
            set
            {
                ViewState[_viewStateSubUsr] = value;
            }
        }

        public MssqlCustomer MssqlCust
        {
            get
            {
                if (ViewState[_viewStateMssqlUsr] != null)
                    return (MssqlCustomer)ViewState[_viewStateMssqlUsr];

                return null;
            }
            set
            {
                ViewState[_viewStateMssqlUsr] = value;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserMessageControl.ClearMessage();

            if (!IsPostBack)
            {
                TryPopulateViewStateObjects();

                if (MssqlCust == null || MssqlCust.Cusno == 0 || SubUser == null)
                {
                    SetFormVisibility(false, false);

                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        //ShowMessage(string.Format(Translate("/promofferjoingold/loginforpromotionaloffer"), OfferName, EPiFunctions.GetLoginPageUrl(CurrentPage)), false, true);
                        LabelThankYou.Text = "<p>" + string.Format(Translate("/promofferjoingold/loginforpromotionaloffer"), OfferName, EPiFunctions.GetLoginPageUrl(CurrentPage)) + "</p>";
                        LabelThankYou.Visible = true;
                        return;
                    }
                    
                    ShowMessage("/promofferjoingold/custnotfound", true, true);
                    return;
                }

                if (!GoldRuleEnforcer.UserPassesGoldRules(MssqlCust.Cusno))
                {
                    SetFormVisibility(false, false);
                    ShowMessage("/digold/missingsubscriptionsdetails2", true, false);
                    return;
                }
                
                if (MssqlCust.DateTookGoldOffer > DateTime.MinValue)
                {
                    SetFormVisibility(false, false);
                    ShowMessage("/promofferjoingold/usedoffer", true, true);
                    return;
                }

                if (MssqlCust.IsGoldMember)
                    SetFormVisibility(false, true);

                PopulateFormFields();

                SetDiGoldOfferCustDate(true, false);
            }
        }

        private void TryPopulateViewStateObjects()
        {
            //populate by code in url
            if (!string.IsNullOrEmpty(Request.QueryString[_urlCode]))
                MssqlCust = new MssqlCustomer(Request.QueryString[_urlCode]);

            //populate by logged in user
            if(MssqlCust == null || MssqlCust.Cusno == 0)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    MssqlCust = new MssqlCustomer(User.Identity.Name, CurrentPage.PageLink.ID);
            }

            if (MssqlCust != null && MssqlCust.Cusno > 0)   //&& !string.IsNullOrEmpty(MssqlCust.UserId)
                SubUser = new SubscriptionUser2(MssqlCust.Cusno);
        }

        private void PopulateFormFields()
        {
            GoldForm.FirstNameInput.Text = MssqlCust.FirstName;
            GoldForm.LastNameInput.Text = MssqlCust.LastName;
            GoldForm.EmailInput.Text = MssqlCust.Email;
        }

        private void SetFormVisibility(bool goldFormVisible, bool addrAndBtnVisible)
        {
            //PlaceHolderTexts.Visible = introAndBreadVisible;
            GoldForm.Visible = goldFormVisible;
            PlaceHolderAddressAndButton.Visible = addrAndBtnVisible;
        }

        

        protected void OrderButton_Click(object sender, EventArgs e)
        {
            if (MssqlCust == null || SubUser == null)
            {
                SetFormVisibility(false, false);
                ShowMessage("/promofferjoingold/error", true, true);
                return;
            }

            bool doSave = true;

            if (!MssqlCust.IsGoldMember)
            {
                string email = GoldForm.EmailInput.Text.Trim();
                string phone = MiscFunctions.FormatPhoneNumber(GoldForm.PhoneInput.Text.Trim(), Settings.PhoneMaxNoOfDigits, true);
                string socSec = MiscFunctions.FormatSocialSecurityNo(GoldForm.SocialSecurityNoInput.Text.Trim());

                string err = ValidateGoldForm(email, phone, socSec);
                if (!string.IsNullOrEmpty(err))
                {
                    ShowMessage(err, false, true);
                    return;                           //give user new chance to fill out form
                }

                SubUser.UpdateUserOnJoinGold(email, phone, socSec);
                
                if (doSave)
                    SaveNameChange();
            }

            //hide all texts and forms
            SetFormVisibility(false, false);

            if (doSave)
            {
                InsertDiGoldOffer();
                SetDiGoldOfferCustDate(false, true);
                LabelThankYou.Text = ThankYouText;
                LabelThankYou.Visible = true;
                return;
            }

            ShowMessage("/promofferjoingold/error", true, true);
        }

        private string ValidateGoldForm(string email, string phone, string socSec)
        {
            if (!MiscFunctions.IsValidEmail(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(socSec))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Ändra följande och försök igen:<br>");

                if (!MiscFunctions.IsValidEmail(email))
                    sb.Append("- Felaktigt formaterad e-postadress<br>");

                if (string.IsNullOrEmpty(phone))
                    sb.Append("- Ogiltigt mobilnummer<br>");

                if (string.IsNullOrEmpty(socSec))
                    sb.Append("- Ogiltigt födelsedatum<br>");

                return sb.ToString();
            }

            return string.Empty;
        }

        private void SaveNameChange()
        {
            string newFirstName = GoldForm.FirstNameInput.Text.Trim();
            string newLastName = GoldForm.LastNameInput.Text.Trim();

            if (newFirstName != MssqlCust.FirstName || newLastName != MssqlCust.LastName)
                MsSqlHandler.SaveDiGoldNameChange(SubUser.Cusno, newFirstName, newLastName);
        }

        private void InsertDiGoldOffer()
        {
            MsSqlHandler.InsertDiGoldOffer(MssqlCust.Cusno,
                                            CurrentPage.PageLink.ID,
                                            OfferName,
                                            AddressForm.CompanyInput.Text.Trim(),
                                            AddressForm.NameInput.Text.Trim(),
                                            AddressForm.StreetAddressInput.Text.Trim(),
                                            AddressForm.HouseNoInput.Text.Trim(),
                                            AddressForm.ZipCodeInput.Text.Trim(),
                                            AddressForm.CityInput.Text.Trim(),
                                            AddressForm.CareOfInput.Text.Trim());
        }

        private void SetDiGoldOfferCustDate(bool setDateVisitedGoldOffer, bool setDateTookGoldOffer)
        {
            if (MssqlCust.PopulatedByUrlCode)
                MsSqlHandler.SetDiGoldOfferCustDate(MssqlCust.Code, setDateVisitedGoldOffer, setDateTookGoldOffer);
        }

    }

    [Serializable]
    public class MssqlCustomer
    {
        public bool PopulatedByUrlCode { get; set; }
        public string Code { get; set; }
        public int EpiPageId = 0;
        public long Cusno = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneMob { get; set; }
        public DateTime DateVisitedGoldOffer = DateTime.MinValue;
        public DateTime DateTookGoldOffer = DateTime.MinValue;
        public bool IsGoldMember = false;
        //public string UserId { get; set; }

        public MssqlCustomer() { }


        public MssqlCustomer(string codeInUrl)
        {
            if(!string.IsNullOrEmpty(codeInUrl))
                PopulateByCodeInUrl(codeInUrl);
        }

        private void PopulateByCodeInUrl(string codeInUrl)
        {
            try
            {
                Guid g = new Guid(codeInUrl);

                DataSet ds = MsSqlHandler.GetDiGoldOfferCust(g);
                if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    PopulatedByUrlCode = true;
                    Code = codeInUrl;
                    int.TryParse(dr["epiPageId"].ToString(), out EpiPageId);
                    long.TryParse(dr["cusno"].ToString(), out Cusno);
                    FirstName = dr["firstName"].ToString();
                    LastName = dr["lastName"].ToString();
                    Email = dr["email"].ToString();
                    PhoneMob = dr["phoneMob"].ToString();
                    DateTime.TryParse(dr["dateVisitedGoldOffer"].ToString(), out DateVisitedGoldOffer);
                    //DateTime.TryParse(dr["dateTookGoldOffer"].ToString(), out DateTookGoldOffer);
                }

                if (Cusno > 0)
                {
                    IsGoldMember = MembershipDbHandler.IsInRole(Cusno, DiRoleHandler.RoleDiGold);
                    DateTookGoldOffer = GetDateTookGoldOffer(Cusno, EpiPageId);
                    //UserId = MembershipDbHandler.GetUserid(Cusno);
                }
            }
            catch (Exception ex)
            {
                new Logger("PopulateByCodeInUrl() failed", ex.ToString());
            }
        }


        public MssqlCustomer(string userId, int epiPageId)
        {
            if (!string.IsNullOrEmpty(userId))
                PopulateByUserId(userId, epiPageId);
        }
        
        private void PopulateByUserId(string userId, int epiPageId)
        {
            PopulatedByUrlCode = false;
            //UserId = userId;
            EpiPageId = epiPageId;
            int i = MembershipDbHandler.GetCusno(userId);
            Cusno = (i > 0) ? i : 0;
            if (Cusno > 0)
            {
                IsGoldMember = MembershipDbHandler.IsInRole(Cusno, DiRoleHandler.RoleDiGold);
                DateTookGoldOffer = GetDateTookGoldOffer(Cusno, EpiPageId);
            }
        }

        private DateTime GetDateTookGoldOffer(long cusno, int epiPageId)
        {
            DataSet ds = MsSqlHandler.GetDateTookDiGoldOffer(cusno, epiPageId);

            if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                return DateTime.Parse(ds.Tables[0].Rows[0]["date"].ToString());

            return DateTime.MinValue;
        }
    }

}