using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.DiMBA;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using System.Configuration;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class MBAForm : EPiServer.UserControlBase
    {
        //public static string mbaResponse { get; set; }
        public bool RegisterDiGoldMembershipOnSubmit { get; set; }

        protected DagensIndustri.Templates.Public.Pages.DiGoldMBA MbaPage
        {
            get { return (DagensIndustri.Templates.Public.Pages.DiGoldMBA)Page; }
        }

        protected SubscriptionUser2 SubUser
        {
            get { return MbaPage.SubUser; }
        }

        protected bool UserIsDiGoldMember 
        {
            get { return HttpContext.Current.User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiGold); }
        }

        

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
                PopulateWithUserInfo();

            DataBind();
        }


        protected void MBAFormButton_Click(object sender, EventArgs e)
        {
            try
            {
                //DiMBA.DIMBA ws = new DiMBA.DIMBA();
                bool isPersonalApplication = (NominationRadioButtonList.SelectedItem.Value == "0") ? true : false;
                string personalNumber = PersonalInput.Text;
                string firstname = FirstNameInput.Text;
                string lastname = LastNameInput.Text;
                string address = AddressInput.Text;
                string zip = ZipInput.Text;
                string city = CityInput.Text;
                string mobile = TelephoneInput.Text;
                string email = EmailInput.Text;
                string linkedin = LinkedInInput.Text;
                string company = CompanyInput.Text;
                string position = PositionInput.Text;
                string education = AcademicEducationInput.Text;
                string educationPoints = AcademicPointsInput.Text;
                string workYears = WorkYearsInput.Text;
                int englishLevel = int.Parse(DdlEnglishLevel.SelectedItem.Text);
                string motivation = MotivationInput.Text;
                //string nomination = NominationRadioButtonList.SelectedItem.Text;
                

                //mbaResponse = ws.InsertContestant(nomination, email, firstname, lastname, personalNumber, company, position, address, zip, city, mobile, education, educationPoints, workYears, englishLevel, linkedin, motivation);
                //new Logger("mbaResponse: " + mbaResponse);

                long cusno = 0;
                if (SubUser != null && SubUser.Cusno > 0)
                    cusno = SubUser.Cusno;

                bool applicationOk = MsSqlHandler.InsertMbaApplication(cusno, isPersonalApplication, personalNumber, firstname, lastname, address, zip, city, mobile,
                                                                        email, linkedin, company, position, education, educationPoints, workYears, englishLevel, motivation);

                //if (mbaResponse == "true")
                if (applicationOk)
                {
                    ((DiTemplatePage)Page).ShowMessage("/mba/message/success", true, false);
                    SendMails();
                    ClearForm();
                }
                else
                {
                    ((DiTemplatePage)Page).ShowMessage("/mba/message/error", true, true);
                }

                if (RegisterDiGoldMembershipOnSubmit && !UserIsDiGoldMember)
                {
                    string mobilePhoneNo = FormatMobilePhoneNumber();
                    string socSecNo = MiscFunctions.FormatSocialSecurityNo(GoldSocialSecurityNoInput.Text);
                    
                    if (SubUser != null)
                        SubUser.UpdateUserOnJoinGold(GoldEmailInput.Text.Trim(), mobilePhoneNo, socSecNo);
                }
            }
            catch (Exception ex)
            {
                new Logger("MBAForm() - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/mba/message/error", true, true);
            }

            PlaceHolderAllContent.Visible = false;
        }

        protected void ClearForm()
        {
            FirstNameInput.Text = string.Empty;
            LastNameInput.Text = string.Empty;
            AddressInput.Text = string.Empty;
            ZipInput.Text = string.Empty;
            CityInput.Text = string.Empty;
            TelephoneInput.Text = string.Empty;
            EmailInput.Text = string.Empty;
            LinkedInInput.Text = string.Empty;
            CompanyInput.Text = string.Empty;
            PositionInput.Text = string.Empty;
            PersonalInput.Text = string.Empty;
            AcademicEducationInput.Text = string.Empty;
            AcademicPointsInput.Text = string.Empty;
            WorkYearsInput.Text = string.Empty;
            MotivationInput.Text = string.Empty;
        }

        protected void SendMails()
        {
            try
            {
                if (Page.IsValid)
                {
                    SubscriptionUser2 DiGoldMember = new SubscriptionUser2();

                    string mailFrom = "no-reply@di.se";

                    if (NominationRadioButtonList.SelectedItem.Value == "0")
                    {
                        #region DiGold like nominee mail

                        string diGoldNominationMailTo = EmailInput.Text;
                        string diGoldNominationSubject = Translate("/mba/mail/digold.nominee/subject");
                        string diGoldNominationBody = Translate("/mba/mail/digold.nominee/body");

                        MailMessage diGoldNominationMail = new MailMessage(mailFrom,diGoldNominationMailTo)
                        { 
                            IsBodyHtml = true,
                            Subject = diGoldNominationSubject,
                            Body = diGoldNominationBody
                        };

                        MiscFunctions.SendMail(diGoldNominationMail);

                        #endregion
                    }
                    else
                    {
                        #region Recommendation mail
                        //users email unknown if not logged in gold member
                        if (MiscFunctions.IsValidEmail(DiGoldMember.Email))
                        {
                            string recommendationMailTo = DiGoldMember.Email;
                            string recommendationMailSubject = string.Format(Translate("/mba/mail/recommendation/subject"), FirstNameInput.Text, LastNameInput.Text);
                            string recommendationMailBody = string.Format(Translate("/mba/mail/recommendation/body"), FirstNameInput.Text, LastNameInput.Text, FirstNameInput.Text);

                            MailMessage recommendationMail = new MailMessage(mailFrom, recommendationMailTo)
                            {
                                IsBodyHtml = true,
                                Subject = recommendationMailSubject,
                                Body = recommendationMailBody
                            };

                            MiscFunctions.SendMail(recommendationMail);
                        }
                        #endregion

                        #region Nomination mail

                        string nominationMailTo = EmailInput.Text;
                        //string nominationMailSubject = string.Format(Translate("/mba/mail/nominee/subject"), DiGoldMember.Name);
                        string nominationMailSubject = string.IsNullOrEmpty(DiGoldMember.RowText1) ? 
                                                           string.Format(Translate("/mba/mail/nominee/subject"), "") :
                                                           string.Format(Translate("/mba/mail/nominee/subject"), " av " + DiGoldMember.RowText1); 

                        string nominationMailBody = Translate("/mba/mail/nominee/body");

                        MailMessage nominationMail = new MailMessage(mailFrom, nominationMailTo)
                        {
                            IsBodyHtml = true,
                            Subject = nominationMailSubject,
                            Body = nominationMailBody
                        };

                        MiscFunctions.SendMail(nominationMail);

                        #endregion
                    }

                    #region (130325 - commented out) Mail to utbildning.se

                    //string utbildningMailSubject = Translate("/mba/mail/utbildning.se/subject");
                    //string utbildningMailBody = string.Format(Translate("/mba/mail/utbildning.se/body"), FirstNameInput.Text, LastNameInput.Text);
                    //string utbildningMailTo = "info@utbildning.se";

                    //if (IsValue("EducationMailTo"))
                    //    utbildningMailTo = CurrentPage.Property["EducationMailTo"].ToString();

                    //MailMessage utbildningMail = new MailMessage(mailFrom, utbildningMailTo)
                    //{
                    //    IsBodyHtml = true,
                    //    Subject = utbildningMailSubject,
                    //    Body = utbildningMailBody + "<br />" + "<br />" +
                    //           "<b>" + FirstNameInput.Title + ": " + "</b>" + FirstNameInput.Text + "<br />" +
                    //           "<b>" + LastNameInput.Title + ": " + "</b>" + LastNameInput.Text + "<br />" +
                    //           "<b>" + EmailInput.Title + ": " + "</b>" + EmailInput.Text + "<br />" +
                    //           "<b>" + TelephoneInput.Title + ": " + "</b>" + TelephoneInput.Text + "<br />" +
                    //           "<b>" + PersonalInput.Title + ": " + "</b>" + PersonalInput.Text + "<br />" +
                    //           "<b>" + AddressInput.Title + ": " + "</b>" + AddressInput.Text + "<br />" +
                    //           "<b>" + ZipInput.Title + ": " + "</b>" + ZipInput.Text + "<br />" +
                    //           "<b>" + CityInput.Title + ": " + "</b>" + CityInput.Text + "<br />" +
                    //           "<b>" + LinkedInInput.Title + ": " + "</b>" + LinkedInInput.Text + "<br />" +
                    //           "<b>" + CompanyInput.Title + ": " + "</b>" + CompanyInput.Text + "<br />" +
                    //           "<b>" + PositionInput.Title + ": " + "</b>" + PositionInput.Text + "<br />" +
                    //           "<b>" + AcademicEducationInput.Title + ": " + "</b>" + AcademicEducationInput.Text + "<br />" +
                    //           "<b>" + AcademicPointsInput.Title + ": " + "</b>" + AcademicPointsInput.Text + "<br />" +
                    //           "<b>" + WorkYearsInput.Title + ": " + "</b>" + WorkYearsInput.Text + "<br />" +
                    //           "<b>" + Translate("/mba/form/englishlevel") + ": " + "</b>" + DdlEnglishLevel.SelectedItem.Text + "<br />" +
                    //           "<b>" + MotivationInput.Title + ": " + "</b>" + MotivationInput.Text + "<br />"
                    //           //"<b>" + "MBA WS Response" + ": " + "</b>" + mbaResponse 
                    //};

                    //MiscFunctions.SendMail(utbildningMail);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                new Logger("SendMails() - failed", ex.ToString());
            }
        }
        
        /// <summary>
        /// Populate input fields with user info
        /// </summary>
        private void PopulateWithUserInfo()
        {
            try
            {
                if (SubUser != null && SubUser.Cusno > 0)
                {
                    //Get customer's firstname and lastname
                    string firstName;
                    string lastName;
                    GetCustomerInfo(out firstName, out lastName);

                    GoldFirstNameInput.Text = firstName;
                    GoldLastNameInput.Text = lastName;
                    GoldEmailInput.Text = MbaPage.SubUser.Email;
                    FirstNameInput.Text = firstName;
                    LastNameInput.Text = lastName;
                    EmailInput.Text = MbaPage.SubUser.Email;
                    TelephoneInput.Text = MbaPage.SubUser.OPhone;
                    GoldPhoneInput.Text = SubUser.OPhone;
                    //SocialSecurityNoInput.Text = Subscriber.BirthNo;
                    //SocialSecurityNoInput.Text = _mbaPage.Subscriber.SocialSecNo;
                    

                    //DiGoldControl.ShowPromotionalOffer = ShowPromotionalOffer();
                }
            }
            catch (Exception ex)
            {
                new Logger("PopulateWithUserInfo() - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/common/errors/error", true, true);
            }
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

            if (SubUser != null && SubUser.Cusno > 0)
            {
                string[] customerInfo = MdbDbHandler.GetCustomerName(SubUser.Cusno);
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
            if (!int.TryParse(GoldPhoneInput.MaxValue, out mobilePhoneMaxValue))
                mobilePhoneMaxValue = Settings.PhoneMaxNoOfDigits;

            return MiscFunctions.FormatPhoneNumber(GoldPhoneInput.Text, mobilePhoneMaxValue, true);
        }
    }
}