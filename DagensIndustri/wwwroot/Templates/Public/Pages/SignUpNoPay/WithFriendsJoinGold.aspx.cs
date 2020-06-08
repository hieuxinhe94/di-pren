using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.Membership;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.DbHelpers;
using System.Text;
using System.Data;
using DIClassLib.GoldMember;
using DIClassLib.EPiJobs.SyncSubs;


namespace DagensIndustri.Templates.Public.Pages.SignUpNoPay
{
    public partial class WithFriendsJoinGold : DiTemplatePage
    {
        public string UrlCode
        {
            get
            {
                if (Request.QueryString["code"] == null)
                    return string.Empty;

                return Request.QueryString["code"].ToString();
            }
        }

        public int NumMaxParticipants
        {
            get
            {
                int i = 500;
                if (EPiFunctions.HasValue(CurrentPage, "MaxParticipants"))
                    int.TryParse(CurrentPage["MaxParticipants"].ToString(), out i);

                return i;
            }
        }
        public int NumMaxFriends = 10;
        public int NumFriends
        {
            get
            {
                int i = 0;
                if (EPiFunctions.HasValue(CurrentPage, "NumFriends"))
                    int.TryParse(CurrentPage["NumFriends"].ToString(), out i);

                return i;
            }
        }

        private int _numTotSignedUp = -100;
        public int NumTotSignedUp 
        { 
            get 
            {
                if (_numTotSignedUp != -100)
                    return _numTotSignedUp;

                _numTotSignedUp = MsSqlHandler.GetSignUpNumParticipants(CurrentPage.PageLink.ID);

                return _numTotSignedUp;
            } 
        }
        
        private int _numSignUpsLeft = -100;
        public int NumSignUpsLeftForUser 
        {
            get
            {
                if (!SubUserIsPopulated)
                    return 0;

                if (_numSignUpsLeft != -100)
                    return _numSignUpsLeft;
                
                _numSignUpsLeft = 1 + NumFriends - NumTotInvitesByCust;

                return _numSignUpsLeft;
            }
        }

        private int _numFriendInvLeft = -100;
        public int NumFriendInvitesLeftForUser
        {
            get
            {
                if (!SubUserIsPopulated)
                    return 0;

                if (_numFriendInvLeft != -100)
                    return _numFriendInvLeft;

                if (NumTotInvitesByCust == 0)  
                    _numFriendInvLeft = NumSignUpsLeftForUser - 1;  //user has not invited anyone yet
                else  
                    _numFriendInvLeft = NumSignUpsLeftForUser;      //user has invited himself (and x friends)

                return _numFriendInvLeft;
            }
        }

        private int _numTotInvByCust = -100;
        public int NumTotInvitesByCust
        {
            get
            {
                if (!SubUserIsPopulated)
                    return 0;

                if (_numTotInvByCust != -100)
                    return _numTotInvByCust;

                _numTotInvByCust = MsSqlHandler.GetNumSignUpPersonsForCust(SubUser.Cusno, CurrentPage.PageLink.ID);

                return _numTotInvByCust;
            }
        }


        public SubscriptionUser2 SubUser
        {
            get
            {
                if (ViewState["SubUser"] != null)
                    return (SubscriptionUser2)ViewState["SubUser"];

                return null;
            }
            set
            {
                ViewState["SubUser"] = value;
            }
        }
        public bool SubUserIsPopulated
        {
            get
            {
                if (SubUser != null && SubUser.Cusno > 0)
                    return true;

                return false;
            }
        }

        public bool UserIsLoggedIn 
        { 
            get 
            { 
                return HttpContext.Current.User.Identity.IsAuthenticated; 
            } 
        }

        private bool? _isGold = null;
        public bool UserIsGoldMember
        {
            get
            {
                if (!SubUserIsPopulated)
                    return false;

                if (_isGold != null)
                    return (bool)_isGold;

                _isGold = MembershipDbHandler.IsInRole(SubUser.Cusno, DiRoleHandler.RoleDiGold);

                return (bool)_isGold;
            }
        }

        public string OldFirstName
        {
            get
            {
                if (ViewState["OldFirstName"] != null)
                    return (string)ViewState["OldFirstName"];

                return string.Empty;
            }
            set
            {
                ViewState["OldFirstName"] = value;
            }
        }
        public string OldLastName
        {
            get
            {
                if (ViewState["OldLastName"] != null)
                    return (string)ViewState["OldLastName"];

                return string.Empty;
            }
            set
            {
                ViewState["OldLastName"] = value;
            }
        }
        public List<PersonName> SignedUpNames = new List<PersonName>();
        


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserIsLoggedIn && !PopSubUserByUrlCode())
                    return;                

                if (UserIsLoggedIn)
                    SubUser = new SubscriptionUser2(HttpContext.Current.User.Identity.Name);

                if (SubUserIsNotPopulated())
                    return;

                if (UserCannotBecomeGoldMember())
                    return;

                if (EventIsFullyBooked())
                    return;

                if (NoInvitaionsLeft())
                    return;
                
                if (UserIsGoldMember)
                    PlaceHolderCheckBoxGoldTerms.Visible = false;

                PopulateGoldForm();
                DisplayFriendForms();
            }
        }

        
        private bool PopSubUserByUrlCode()
        {
            if (!TryPopulateSubUserByUrlCode())
            {
                //Mainbody1.Text = "<p>" + string.Format(Translate("/withfriendsjoingold/loginforpromotionaloffer"), EPiFunctions.GetLoginPageUrl(CurrentPage)) + "</p>";
                //HandleFormVisibility(false, true, false);

                //ShowMessage(string.Format(Translate("/withfriendsjoingold/loginforpromotionaloffer"), EPiFunctions.GetLoginPageUrl(CurrentPage)), true, true);
                //HandleFormVisibility(true, true, false);

                LiteralDoLoginLink.Text = "<p><a href='" + EPiFunctions.GetLoginPageUrl(CurrentPage) + "'>Klicka här för att logga in och boka plats</a></p>";
                LiteralDoLoginLink.Visible = true;
                HandleFormVisibility(true, true, false);

                return false;
            }

            return true;
        }

        private bool UserCannotBecomeGoldMember()
        {
            if (!GoldRuleEnforcer.UserPassesGoldRules(SubUser.Cusno))
            {
                ShowMessage("/digold/missingsubscriptionsdetails2", true, false);
                HandleFormVisibility(false, false, false);
                return true;
            }

            return false;
        }
 
        private bool SubUserIsNotPopulated()
        {
            if (!SubUserIsPopulated)
            {
                ShowMessage("/withfriendsjoingold/custnotfound", true, true);
                HandleFormVisibility(false, false, false);
                return true;
            }

            return false;
        }

        private bool EventIsFullyBooked()
        {
            if (NumTotSignedUp >= NumMaxParticipants)
            {
                Mainbody1.Text = EPiFunctions.HasValue(CurrentPage, "MaxParticipantsText") ?
                    CurrentPage["MaxParticipantsText"].ToString() : Translate("/withfriendsjoingold/fullybooked");

                HandleFormVisibility(false, true, false);
                return true;
            }

            return false;
        }

        private bool NoInvitaionsLeft()
        {
            if (NumSignUpsLeftForUser <= 0)
            {
                ShowMessage("/withfriendsjoingold/usedoffer", true, true);
                HandleFormVisibility(false, false, false);
                return true;
            }

            return false;
        }


        private void HandleFormVisibility(bool mainIntro, bool mainBody, bool placeHolderForm)
        {
            Mainintro1.Visible = mainIntro;
            Mainbody1.Visible = mainBody;
            PlaceHolderForm.Visible = placeHolderForm;
        }

        private bool TryPopulateSubUserByUrlCode()
        {
            if (string.IsNullOrEmpty(UrlCode))
                return false;

            try
            {
                Guid g = new Guid(UrlCode);
                DataSet ds = MsSqlHandler.GetSignUpPersonCust(g);
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    long cusno = long.Parse(dr["cusno"].ToString());

                    //sync cust to mssql login tables: 1=ok, -1=no active subs, -2=no cust info in cirix
                    if (cusno > 0 && SyncSubsHandler.SyncCustToMssqlLoginTables((int)cusno) == 1)
                    {
                        //return LoginUtil.TryLoginUserToDagensIndstri(cusno);
                        //string userId = MembershipDbHandler.GetUserid(cusno);
                        SubUser = new SubscriptionUser2(cusno);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("TryPopSubUserFromUrlCode() failed for urlCode: " + UrlCode, ex.ToString());
            }
        
            return false;
        }

        private void PopulateGoldForm()
        {
            try
            {
                string[] customerInfo = MdbDbHandler.GetCustomerName(SubUser.Cusno);
                if (customerInfo != null && customerInfo.Length == 2)
                {
                    OldFirstName = customerInfo[0];
                    OldLastName = customerInfo[1];
                    FirstNameInput.Text = OldFirstName;
                    LastNameInput.Text = OldLastName;
                }

                EmailInput.Text = SubUser.Email;
                PhoneInput.Text = SubUser.OPhone;
                SocialSecurityNoInput.Text = SubUser.SocialSecNo;
            }
            catch (Exception ex)
            {
                new Logger("PopulateGoldForm() failed for cusno:" + SubUser.Cusno, ex.ToString());
            }
        }

        private void DisplayFriendForms()
        {
            int numFr = NumFriendInvitesLeftForUser;

            int totLeft = NumMaxParticipants - NumTotSignedUp;
            if (numFr > totLeft)
                numFr = totLeft;

            for (int i = 1; i <= numFr; i++)
                GetFriendForm(i).Visible = true;


            if (numFr > 0)
            {
                LabelHeaderFriends.Visible = true;
                LabelFormHeader.Text = Translate("/withfriendsjoingold/bookingfriends");
            }
            else
                LabelFormHeader.Text = Translate("/withfriendsjoingold/bookingone");
        }

        private Units.Placeable.SignUpNoPay.FriendForm GetFriendForm(int i)
        {
            switch (i)
            {
                case 1:
                    return FriendForm1;
                case 2:
                    return FriendForm2;
                case 3:
                    return FriendForm3;
                case 4:
                    return FriendForm4;
                case 5:
                    return FriendForm5;
                case 6:
                    return FriendForm6;
                case 7:
                    return FriendForm7;
                case 8:
                    return FriendForm8;
                case 9:
                    return FriendForm9;
                case 10:
                    return FriendForm10;
                default:
                    return null;
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            HandleFormVisibility(false, false, false);
            
            TryUpdateGoldMember();
            
            if (SignUpOnEvent())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Translate("/withfriendsjoingold/thanks"));
                if(SendConfMail())
                    sb.Append("<br>" + Translate("/withfriendsjoingold/mailsent") + " " + EmailInput.Text.Trim());

                ShowMessage(sb.ToString(), false, false);
            }
            else
            {
                ShowMessage(Translate("/withfriendsjoingold/error"), true, true);
            }
        }

        private void TryUpdateGoldMember()
        {
            if (SubUser == null)
                return;

            string email = "";
            string phoneMob = "";
            string socSec = null;

            try
            {
                email = EmailInput.Text.Trim();
                phoneMob = MiscFunctions.FormatPhoneNumber(PhoneInput.Text.Trim(), Settings.PhoneMaxNoOfDigits, true);

                if (MiscFunctions.IsValidEmail(email) && !string.IsNullOrEmpty(phoneMob))
                {
                    socSec = MiscFunctions.FormatSocialSecurityNo(SocialSecurityNoInput.Text.Trim());
                    SubUser.UpdateUserOnJoinGold(email, phoneMob, socSec);
                }

                if (FirstNameInput.Text.Trim() != OldFirstName || LastNameInput.Text.Trim() != OldLastName)
                    MsSqlHandler.SaveDiGoldNameChange(SubUser.Cusno, FirstNameInput.Text.Trim(), LastNameInput.Text.Trim());
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("cusno:" + SubUser.Cusno.ToString() + ", ");
                sb.Append("email:" + email + ", ");
                sb.Append("phoneMob:" + phoneMob + ", ");
                sb.Append("socialSecNo:" + SubUser.SocialSecNo);
                new Logger("TryUpdateGoldMember() - failed for " + sb.ToString(), ex.ToString());
            }
        }

        private bool SignUpOnEvent()
        {
            if (!SubUserIsPopulated)
                return false;
            
            try
            {
                long cusno = SubUser.Cusno;

                int payerId = MsSqlHandler.TryUpdateSignUpUser(cusno, CurrentPage.PageLink.ID, "", FirstNameInput.Text, LastNameInput.Text, "", "", "", PhoneInput.Text, EmailInput.Text);
                
                if(payerId == 0)
                    payerId = MsSqlHandler.InsertSignUpUser(cusno, CurrentPage.PageLink.ID, 0, "", FirstNameInput.Text, LastNameInput.Text, "", "", "", PhoneInput.Text, EmailInput.Text);
                
                SignedUpNames.Add(new PersonName(FirstNameInput.Text, LastNameInput.Text));

                for (int i = 1; i <= NumMaxFriends; i++)
                {
                    Units.Placeable.SignUpNoPay.FriendForm ff = GetFriendForm(i);
                    string fn = ff.FirstName;
                    string ln = ff.LastName;
                    if (!string.IsNullOrEmpty(fn) || !string.IsNullOrEmpty(ln))
                    {
                        MsSqlHandler.InsertSignUpUser(cusno, CurrentPage.PageLink.ID, payerId, "", fn, ln, "", "", "", "", "");
                        SignedUpNames.Add(new PersonName(fn, ln));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                new Logger("SignUpOnEvent() - failed", ex.ToString());
                return false;
            }
        }
        
        private bool SendConfMail()
        {
            string email = EmailInput.Text.Trim();

            if (!SubUserIsPopulated || !MiscFunctions.IsValidEmail(email))
                return false;

            try
            {
                string fromAdr = TryGetEpiKey("ConfMailFrom") ?? Translate("/withfriendsjoingold/dimail");
                string headline = TryGetEpiKey("ConfMailHeadline") ?? Translate("/withfriendsjoingold/bookingconf");
                string body = TryGetEpiKey("ConfMailText") ?? Translate("/withfriendsjoingold/thanks");
                body += GetSignedUpNamesInHtml() + Translate("/withfriendsjoingold/mailend");
                
                MiscFunctions.SendMail(fromAdr, email, headline, body, true);
                return true;
            }
            catch (Exception ex)
            {
                new Logger("SendConfMail() failed", ex.ToString());
                return false;
            }
        }

        private string TryGetEpiKey(string key)
        {
            if (EPiFunctions.HasValue(CurrentPage, key))
            {
                string s = CurrentPage[key].ToString();
                if (!string.IsNullOrEmpty(s))
                    return s;
            }

            return null;
        }

        private string GetSignedUpNamesInHtml()
        {
            if (SignedUpNames.Count == 0)
                return string.Empty;
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<br>" + Translate("/withfriendsjoingold/confpers") + "<br>");
            
            foreach (PersonName n in SignedUpNames)
                sb.Append("- " + n.FirstName + " " + n.LastName + "<br>");
            
            return sb.ToString();
        }

        protected string GetGoldTerms()
        {
            string termsAndConditions = string.Empty;
            PageReference termsAndConditionsPageLink = EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldTermsPage") as PageReference;
            if (termsAndConditionsPageLink != null)
            {
                PageData termsPageData = EPiServer.DataFactory.Instance.GetPage(termsAndConditionsPageLink);

                termsAndConditions = string.Format(Translate("/digold/readacceptedterms"),
                                                    EPiFunctions.GetFriendlyAbsoluteUrl(termsPageData.PageLink),
                                                    !string.IsNullOrEmpty((string)termsPageData["Heading"]) ? (string)termsPageData["Heading"] : termsPageData.PageName
                                                    );
            }
            return termsAndConditions;
        } 

    }
}