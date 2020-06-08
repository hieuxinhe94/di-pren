using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using DIClassLib.SignUp;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.DbHelpers;
using DagensIndustri.Templates.Public.Pages.SignUpNoPay;
using DIClassLib.Misc;
using System.Text;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class SignUpPersons : DiTemplatePage
    {
        #region statics
        protected static readonly String ACCESS_ALL = "all";
        protected static readonly String ACCESS_SUBSCRIBERS = "subscribers";
        protected static readonly String ACCESS_DIGOLD = "digold";
        #endregion

        #region properties
        protected String SignUpAccess
        {
            get
            {
                String access = (String)CurrentPage["SignUpAccess"];
                if (!String.IsNullOrEmpty(access))
                {
                    return access;
                }
                else
                {
                    return "all";
                }
            }
        }

        protected bool UserIsGoldMember
        {
            get
            {
                
               return System.Web.HttpContext.Current.User.IsInRole("DiGold");
            }
        }

        protected SubscriptionUser2 Subscriber { get; set; }

        protected SignUpPerson CurrentPerson { get; set; }

        protected List<PersonName> CurrentFriends { get; set; }

        protected String UrlCode
        {
            get
            {
                return Request.QueryString["code"];
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

        public List<PersonName> FriendNames
        {
            get
            {
                return GetFriendNames(CurrentPerson);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);


 
            // get current subscriber and/or signup person depending on context
            GetUserFromContext();
            if (CurrentPerson == null)
            {
                int placesLeft = SignUpPerson.GetPlacesLeft(CurrentPage);
                if (placesLeft == 0)
                {
                    PlaceHolderSignUpForm.Visible = false;
                    String fullyBookedMessage = (String)CurrentPage["MaxParticipantsText"];
                    if (String.IsNullOrEmpty(fullyBookedMessage))
                    {
                        fullyBookedMessage = Translate("/signupgoldfriend/fullybooked");
                    }

                    UserMessageControl.ShowMessage(fullyBookedMessage, false, false);
                    return;
                }
            }
            else
            {
                DateTime dtlastCancel = DateTime.MinValue;
                if (CurrentPage["MaxDateTimeCancelBooking"] != null)
                {
                    dtlastCancel = (DateTime)CurrentPage["MaxDateTimeCancelBooking"];
                    if (dtlastCancel <= DateTime.Now)
                    {
                        UserMessageControl.ShowMessage("Tiden för att avboka eller ändra bokningen har löpt ut. Vänligen kontakta oss vid eventuella frågor.", false, false);
                        PlaceHolderSignUpForm.Visible = false;
                        return;
                    }
                }
            }

            if ((SignUpAccess == ACCESS_DIGOLD || SignUpAccess == ACCESS_SUBSCRIBERS) && !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                PlaceHolderSignUpForm.Visible = false;
                return;
            }

            PopulateForm();
            SetLabelHeaderFormCustText();
        }

        private void SetLabelHeaderFormCustText()
        {
            string s = "<p><b>Fyll i bokningsformuläret";
            
            if (NumFriends > 0)
                s += " för dig och ditt sällskap";

            LabelHeaderFormCust.Text = s + ":</b></p>";
        }

        private void PopulateForm()
        {
            // If we have a registered user
            if (CurrentPerson != null)
            {
                FirstNameInput.Text = CurrentPerson.FirstName;
                LastNameInput.Text = CurrentPerson.LastName;
                EmailInput.Text = CurrentPerson.Email;
                PhoneInput.Text = CurrentPerson.Phone;
                if(Subscriber != null)
                    SocialSecurityNoInput.Text = Subscriber.SocialSecNo;
            }
            else if (Subscriber != null)
            {
                try
                {
                    string[] customerInfo = MdbDbHandler.GetCustomerName(Subscriber.Cusno);
                    if (customerInfo != null && customerInfo.Length == 2)
                    {
                        FirstNameInput.Text = customerInfo[0];
                        LastNameInput.Text = customerInfo[1];
                    }

                    EmailInput.Text = Subscriber.Email;
                    PhoneInput.Text = Subscriber.OPhone;
                    SocialSecurityNoInput.Text = Subscriber.SocialSecNo;
                }
                catch (Exception ex)
                {
                    new Logger("PopulateForm() failed for cusno:" + Subscriber.Cusno, ex.ToString());
                }
            }
            
        }

        private void GetUserFromContext()
        {
            // 1. Check for code in url
            //
            if (!String.IsNullOrEmpty(UrlCode))
            {
                Guid guidCode = Guid.Empty;
                try{
                    guidCode = new Guid(UrlCode);
                }catch(Exception){}

                
                if (guidCode != Guid.Empty)
                {
                    // 1.1 If we have a code in the query string, check if the user has a registration already if not see if we can sign in the subscriber
                    //
                    GetSignUpPersonAndSubscriberFromUrlCode(guidCode);
                    
                    if (Subscriber != null)
                    {
                        if (DIClassLib.Misc.LoginUtil.ReLoginUserRefreshCookie(Subscriber.UserName, Subscriber.Password))
                            Response.Redirect(EPiFunctions.GetFriendlyUrl(CurrentPage));
                    }
                    
                }              
            }

            // 2. See if we have an authenticated user
            //
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)                
            {
                Subscriber = new SubscriptionUser2(HttpContext.Current.User.Identity.Name);

                // 2.1 Check if the subscriber has registered for the event
                //
                if (Subscriber != null && Subscriber.Cusno > 0)
                {
                    CurrentPerson = SignUpPerson.GetRegisteredSignUpPerson(Subscriber.Cusno);
                }
            }

        }

        private void GetSignUpPersonAndSubscriberFromUrlCode(Guid guidCode)
        {
            SignUpPerson person = SignUpPerson.GetRegisteredSignUpPerson(guidCode);                    
            if (person != null)
            {
                CurrentPerson = person;
                // We have an existing registration, get the subscriber if we have cusno
                //
                if (person.Cusno > 0)
                {
                    string userId = MembershipDbHandler.GetUserid(person.Cusno);
                    if (!String.IsNullOrEmpty(userId))
                    {
                        Subscriber = new SubscriptionUser2(userId);
                    }
                }
            }
            else
            {
                
                // See if we have a post in separate code table
                //
                long cusno = SignUpPerson.GetCusnoFromCodeTable(guidCode);
                if (cusno > 0)
                {
                    string userId = MembershipDbHandler.GetUserid(cusno);
                    if (!String.IsNullOrEmpty(userId))
                    {
                        Subscriber = new SubscriptionUser2(userId);
                    }
                    // CurrentPerson = SignUpPerson.GetRegisteredSignUpPerson(cusno);
                }

            }
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

        #region events
        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            // delete current registration and insert new
            GetUserFromContext();
            SignUpPerson.RemoveSignUpPerson(CurrentPerson.Code);
            
            Guid guidCode = CurrentPerson.Code;
            List<PersonName> friends = GetFriendsFromForm();
            
            String errorMessage = "";
            SignUpPerson createdPerson = SignUpOnEvent(FirstNameInput.Text, LastNameInput.Text, PhoneInput.Text, EmailInput.Text, friends.ToArray(), guidCode, out errorMessage);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                UserMessageControl.ShowMessage(errorMessage, false, true);
                return;
            }
            if (createdPerson != null)
            {
                // send mail
                List<PersonName> friendNames = GetFriendNames(createdPerson);

                // show success message
                StringBuilder sb = new StringBuilder();
                sb.Append(Translate("/signupgoldfriend/thanks"));
                
                if (SendConfMail(createdPerson))
                    sb.Append("<br>" + Translate("/signupgoldfriend/mailsent") + " " + EmailInput.Text.Trim());
                
                ShowSuccessMessage(sb.ToString());
            }
            else
            {
                UserMessageControl.ShowMessage(Translate("/signupgoldfriend/error"), false, true);
            }
        }

        /// <summary>
        /// Register new person (with friends) to event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonInsert_Click(object sender, EventArgs e)
        {
            GetUserFromContext();

            // If this a gold event check if the user is a gold member
            //
            if (this.SignUpAccess == ACCESS_DIGOLD)
            {
                if (!User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiGold))
                {
                    if (!SaveGoldData())
                    {
                        return;
                    }
                }
            }

            // check so that the event still has places available
            //
            //int currentParticipantCount = MsSqlHandler.GetSignUpNumParticipants(CurrentPage.PageLink.ID);
            int placesLeft = SignUpPerson.GetPlacesLeft(CurrentPage);



            List<PersonName> friends = GetFriendsFromForm();

            // check so that the person and all friends can sign up
            //
            int signUpCount = 1 + friends.Count();
            if (signUpCount > placesLeft)
            {
                UserMessageControl.ShowMessage("Det finns inte tillräckligt med plastser kvar", false, true);
                return;
            }

            Guid guidCode = Guid.Empty;
            if (UrlCode != null)
            {
                try { guidCode = new Guid(UrlCode); }
                catch { }
            }
            
            if (guidCode == Guid.Empty)
                guidCode = Guid.NewGuid();

            String errorMessage = "";
            SignUpPerson createdPerson = SignUpOnEvent(FirstNameInput.Text, LastNameInput.Text, PhoneInput.Text, EmailInput.Text, friends.ToArray(), guidCode, out errorMessage);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                UserMessageControl.ShowMessage(errorMessage, false, true);
                return;
            }

            if (createdPerson != null)
            {
                // send mail
                List<PersonName> friendNames = GetFriendNames(createdPerson);

                // show success message
                StringBuilder sb = new StringBuilder();
                sb.Append(Translate("/signupgoldfriend/thanks"));
                
                if (SendConfMail(createdPerson))
                    sb.Append("<br>" + Translate("/signupgoldfriend/mailsent") + " " + EmailInput.Text.Trim());
                
                ShowSuccessMessage(sb.ToString());
            }
            else
            {
                UserMessageControl.ShowMessage(Translate("/signupgoldfriend/error"), false, true);
            }
        }

        /// <summary>
        /// Cancel a persons signup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            GetUserFromContext();
            try
            {
                SignUpPerson.CancelSignUpPerson(CurrentPerson.PayerId);
                ShowSuccessMessage("Din registrering är nu avbokad");
            }
            catch (Exception ex)
            {
                UserMessageControl.ShowMessage(Translate("/signupgoldfriend/error"), false, true);
            }           
        }

        #endregion


        #region methods
        /// <summary>
        /// Get the entered friends from the form.
        /// </summary>
        /// <returns></returns>
        private List<PersonName> GetFriendsFromForm()
        {
            List<PersonName> friends = new List<PersonName>();

            for (int i = 0; i < NumFriends; ++i)
            {
                String firstname = Request.Form["friend_firstname_" + i];
                String lastname = Request.Form["friend_lastname_" + i];
                if (!String.IsNullOrEmpty(firstname) || !String.IsNullOrEmpty(lastname))
                {
                    friends.Add(new PersonName(firstname, lastname));
                }
            }

            return friends;
        }

        /// <summary>
        /// Sign up the user for the event
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="friends"></param>
        /// <param name="code"></param>
        /// <returns>Created signup person</returns>
        private SignUpPerson SignUpOnEvent(String firstname, String lastname, String phone, String email, PersonName[] friends, Guid code, out String errorMessage)
        {
            
            errorMessage = "";
            int personCount = 1 + friends.Count();
            // Check if there are enough places left
            //
            //
            int currentParticipantCount = MsSqlHandler.GetSignUpNumParticipants(CurrentPage.PageLink.ID);
            if (currentParticipantCount + personCount > NumMaxParticipants)
            {
                String fullyBookedMessage = (String)CurrentPage["MaxParticipantsText"];
                if (String.IsNullOrEmpty(fullyBookedMessage))
                {
                    fullyBookedMessage = Translate("/signupgoldfriend/fullybooked");
                }

                UserMessageControl.ShowMessage(fullyBookedMessage, false, false);            
                return null;
            }

            
            SignUpPerson insertedPerson = null;
            long cusno = 0;
            if (Subscriber != null)
            {
                cusno = Subscriber.Cusno;
            }

            try
            {
                // Insert main person
                //
                int payerId = SignUpPerson.InsertSignUpPerson(cusno, CurrentPage.PageLink.ID, 0, "", firstname, lastname, "", "", "", phone, email, code);

                // Insert friends
                //
                foreach (PersonName friend in friends)
                {
                    MsSqlHandler.InsertSignUpUserGuid(cusno, CurrentPage.PageLink.ID, payerId, "", friend.FirstName, friend.LastName, "", "", "", "", "",null);
                }

                // Get the person
                //
                insertedPerson = SignUpPerson.GetRegisteredSignUpPerson(code);


                
            }
            catch (Exception ex)
            {
                new Logger("SignUpOnEvent() - failed", ex.ToString());
            }

            return insertedPerson;
        }

        private bool SendConfMail(SignUpPerson person)
        {
            string email = person.Email;

            //if (SubUser == null || !MiscFunctions.IsValidEmail(email))
            if (!MiscFunctions.IsValidEmail(email))
                return false;

            try
            {
                string fromAdr = TryGetEpiKey("ConfMailFrom") ?? "no-reply@di.se";
                string headline = TryGetEpiKey("ConfMailHeadline") ?? Translate("/signupgoldfriend/bookingconf");
                StringBuilder body = new StringBuilder();
                body.Append(TryGetEpiKey("ConfMailText") ?? Translate("/signupgoldfriend/thanks"));

                List<PersonName> signedUpNames = new List<PersonName>();
                signedUpNames.Add(new PersonName(person.FirstName, person.LastName));
                if (person.Friends != null && person.Friends.Count() > 0)
                {
                    foreach (SignUpPerson friend in person.Friends)
                        signedUpNames.Add(new PersonName(friend.FirstName, friend.LastName));
                }
                
                body.Append(GetSignedUpNamesInHtml(signedUpNames.ToArray()));

                if (person.Code != Guid.Empty)
                {
                    String cancelUrl = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
                    string urlChr = (cancelUrl.Contains('?')) ? "&" : "?";
                    cancelUrl += urlChr + "code=" + person.Code.ToString();
                    body.Append("<br><a href='" + cancelUrl + "'>Avboka eller ändra din bokning</a>");
                }

                body.Append(Translate("/signupgoldfriend/mailend"));

                MiscFunctions.SendMail(fromAdr, email, headline, body.ToString(), true);
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

        private string GetSignedUpNamesInHtml(PersonName[] friends)
        {
            if (friends.Count() == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("<br>" + Translate("/signupgoldfriend/confpers") + "<br>");

            foreach (PersonName n in friends)
                sb.Append("- " + n.FirstName + " " + n.LastName + "<br>");

            return sb.ToString();
        }

        private List<PersonName> GetFriendNames(SignUpPerson person)
        {
            List<PersonName> names = new List<PersonName>();
            List<SignUpPerson> friends = null;

            if (person != null && person.Friends != null)
                friends = person.Friends;


            // hide fields if there are not enough places left
            //
            int fieldsToShow = NumFriends;
            int placesLeft = SignUpPerson.GetPlacesLeft(CurrentPage);
            if (placesLeft < NumFriends)
            {
                fieldsToShow = placesLeft - 1; // subtract one for the main person
                if (person != null && person.Friends.Count > placesLeft)
                {
                    // make sure that even if there are not enough places left, an existing signup can edit friends
                    //
                    fieldsToShow = person.Friends.Count;
                }
                else if (CurrentFriends != null)
                {
                    fieldsToShow = person.Friends.Count;
                }
            }


            for (int i = 0; i < fieldsToShow; ++i)
            {
                if (friends != null && friends.Count > i)
                {
                    names.Add( new PersonName(friends[i].FirstName, friends[i].LastName));
                }
                else if (CurrentFriends != null && CurrentFriends.Count > i)
                {
                    names.Add(new PersonName(CurrentFriends[i].FirstName, CurrentFriends[i].LastName));
                }
                else
                {
                    names.Add(new PersonName("", ""));
                }
            }
            return names;
        }

        private void ShowSuccessMessage(String msg)
        {
            UserMessageControl.ShowMessage(msg, false, false);
            PlaceHolderSignUpForm.Visible = false;
        }


        /// <summary>
        /// Save data to db
        /// </summary>
        /// <returns></returns>
        public bool SaveGoldData()
        {
            bool saved = false;

            try
            {
                if (!GoldIsValid())
                    return false;

                //Get customer's firstname and lastname
                string existingFirstName;
                string existingLastName;
                GetCustomerInfo(out existingFirstName, out existingLastName);

                bool nameHasChanged = FirstNameInput.Text.Trim() != existingFirstName || LastNameInput.Text.Trim() != existingLastName;
                if (nameHasChanged)
                {
                    //If firstname and last name differ from what is stored, store the new first and last name in another database
                    MsSqlHandler.SaveDiGoldNameChange(Subscriber.Cusno, FirstNameInput.Text.Trim(), LastNameInput.Text.Trim());
                }

                //Format the entered mobile number
                string mobilePhoneNo = FormatMobilePhoneNumber();

                //Format the entered social security number
                string socSecNo = MiscFunctions.FormatSocialSecurityNo(SocialSecurityNoInput.Text);

                //If everything was OK, store data
                saved = Subscriber.UpdateUserOnJoinGold(EmailInput.Text.Trim(), mobilePhoneNo, socSecNo);
            }
            catch (Exception ex)
            {
                new Logger("SaveData() - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/common/errors/error", true, true);
            }
            return saved;
        }

        /// <summary>
        /// Check if data is valid. If not, show message to the user
        /// </summary>
        /// <returns></returns>
        public bool GoldIsValid()
        {
            //Validate the entered mobile number
            if (string.IsNullOrEmpty(FormatMobilePhoneNumber()))
            {
                ((DiTemplatePage)Page).ShowMessage("/common/validation/mobilephonenumberrequired", true, true);
                return false;
            }

            //Validate the entered social security number
            if (string.IsNullOrEmpty(MiscFunctions.FormatSocialSecurityNo(SocialSecurityNoInput.Text)))
            {
                ((DiTemplatePage)Page).ShowMessage("/common/validation/socialsecuritynumberrequired", true, true);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get firstname and lastname for the subscriber
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        private void GetCustomerInfo(out string firstName, out string lastName)
        {
            firstName = string.Empty;
            lastName = string.Empty;

            if (Subscriber != null)
            {
                string[] customerInfo = MdbDbHandler.GetCustomerName(Subscriber.Cusno);
                if (customerInfo != null && customerInfo.Length == 2)
                {
                    firstName = customerInfo[0];
                    lastName = customerInfo[1];
                }
            }
        }

        /// <summary>
        /// Format MobileNumber
        /// </summary>
        /// <returns></returns>
        private string FormatMobilePhoneNumber()
        {
            int mobilePhoneMaxValue;
            if (!int.TryParse(PhoneInput.MaxValue, out mobilePhoneMaxValue))
                mobilePhoneMaxValue = Settings.PhoneMaxNoOfDigits;

            return MiscFunctions.FormatPhoneNumber(PhoneInput.Text, mobilePhoneMaxValue, true);
        }
        #endregion
    }
}