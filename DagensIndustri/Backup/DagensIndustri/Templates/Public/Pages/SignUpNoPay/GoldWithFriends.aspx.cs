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


namespace DagensIndustri.Templates.Public.Pages.SignUpNoPay
{
    public partial class GoldWithFriends : DiTemplatePage
    {
        public int MaxNumParticipants
        {
            get
            {
                int i = 500;
                if (EPiFunctions.HasValue(CurrentPage, "MaxParticipants"))
                    int.TryParse(CurrentPage["MaxParticipants"].ToString(), out i);

                return i;
            }
        }
        public int MaxNumFriends = 10;
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
        public SubscriptionUser2 SubUser 
        {
            get 
            { 
                if(ViewState["SubUser"] != null)
                    return (SubscriptionUser2)ViewState["SubUser"];

                return null;
            }
            set 
            { 
                ViewState["SubUser"] = value; 
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
        public bool SocialSecIsVisible 
        {
            get { return !EPiFunctions.HasValue(CurrentPage, "GoldHideSocSec"); }
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
                if (MsSqlHandler.GetSignUpNumParticipants(CurrentPage.PageLink.ID) >= MaxNumParticipants)
                {
                    Mainbody1.Text = EPiFunctions.HasValue(CurrentPage, "MaxParticipantsText") ?
                        CurrentPage["MaxParticipantsText"].ToString() : Translate("/signupgoldfriend/fullybooked");

                    Mainintro1.Visible = false;
                    PlaceHolderForm.Visible = false;
                    return;
                }

                PopulateMembers();
                PlaceHolderSocSec.Visible = SocialSecIsVisible;
                DisplayFriendForms();

                if (NumFriends == 0)
                    LabelFormHeader.Text = Translate("/signupgoldfriend/bookingone");
                else if (NumFriends > 0)
                    LabelFormHeader.Text = Translate("/signupgoldfriend/bookingfriends");
            }
        }

        private void PopulateMembers()
        {
            if (User.IsInRole(DiRoleHandler.RoleDiGold))
            {
                try
                {
                    SubUser = new SubscriptionUser2();

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
                    new Logger("PopulateMembers() failed for user:" + User.Identity.Name, ex.ToString());
                }
            }
        }

        private void DisplayFriendForms()
        {
            for (int i = 1; i <= NumFriends; i++)
                GetFriendForm(i).Visible = true;
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
            Mainintro1.Visible = false;
            Mainbody1.Visible = false;
            PlaceHolderForm.Visible = false;

            TryUpdateGoldMember();
            if (SignUpOnEvent())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Translate("/signupgoldfriend/thanks"));
                if(SendConfMail())
                    sb.Append("<br>" + Translate("/signupgoldfriend/mailsent") + " " + EmailInput.Text.Trim());

                ShowMessage(sb.ToString(), false, false);
            }
            else
            {
                ShowMessage(Translate("/signupgoldfriend/error"), false, true);
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
                    if (SocialSecIsVisible)
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
            try
            {
                long cusno = 0;
                if (SubUser != null && SubUser.Cusno > 0)
                    cusno = SubUser.Cusno;

                int payerId = MsSqlHandler.TryUpdateSignUpUser(cusno, CurrentPage.PageLink.ID, "", FirstNameInput.Text, LastNameInput.Text, "", "", "", PhoneInput.Text, EmailInput.Text);
                
                if(payerId == 0)
                    payerId = MsSqlHandler.InsertSignUpUser(cusno, CurrentPage.PageLink.ID, 0, "", FirstNameInput.Text, LastNameInput.Text, "", "", "", PhoneInput.Text, EmailInput.Text);

                SignedUpNames.Add(new PersonName(FirstNameInput.Text, LastNameInput.Text));

                for (int i = 1; i <= MaxNumFriends; i++)
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

            //if (SubUser == null || !MiscFunctions.IsValidEmail(email))
            if (!MiscFunctions.IsValidEmail(email))
                return false;

            try
            {
                string fromAdr = TryGetEpiKey("ConfMailFrom") ?? Translate("/signupgoldfriend/dimail");
                string headline = TryGetEpiKey("ConfMailHeadline") ?? Translate("/signupgoldfriend/bookingconf");
                string body = TryGetEpiKey("ConfMailText") ?? Translate("/signupgoldfriend/thanks");
                body += GetSignedUpNamesInHtml() + Translate("/signupgoldfriend/mailend");
                
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
            sb.Append("<br>" + Translate("/signupgoldfriend/confpers") + "<br>");
            
            foreach (PersonName n in SignedUpNames)
                sb.Append("- " + n.FirstName + " " + n.LastName + "<br>");
            
            return sb.ToString();
        }

    }


    


    //could not find controls in nested contentPlaceHolders
    //private void DisplayFriendForms()
    //{
    //    ContentPlaceHolder cp1 = (ContentPlaceHolder)Master.FindControl("WideMainContentPlaceHolder");
    //    if (cp1 != null)
    //    {
    //        ContentPlaceHolder cp2 = (ContentPlaceHolder)cp1.FindControl("MainContentPlaceHolder1");
    //        if (cp2 != null)
    //        {
    //            for (int i = 1; i <= NumFriends; i++)
    //            {
    //                //ff = FindControl("FriendForm" + i.ToString()) as Units.Placeable.SignUpNoPay.FriendForm;
    //                Units.Placeable.SignUpNoPay.FriendForm ff = cp2.FindControl("FriendForm" + i.ToString()) as Units.Placeable.SignUpNoPay.FriendForm;
    //                if (ff != null)
    //                    ff.Visible = true;
    //            }
    //        }
    //    }
    //}

}