using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;

namespace DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipFlow
{
    public partial class AdditionalUserDetails : UserControlBase
    {
        #region Properties
        /// <summary>
        /// Page heading
        /// </summary>
        public string Heading
        {
            get
            {
                return Translate("/digold/supplyadditionaldetails");
            }
        }

        /// <summary>
        /// Name of the promotional offer
        /// </summary>
        public string PromotionalOfferName 
        {
            get
            {
                return ViewState["PromotionalOfferName"] as string;
            }
            set
            {
                ViewState["PromotionalOfferName"] = value;
            }
        }

        /// <summary>
        /// Get whether the user has accepted the promotional offer
        /// </summary>
        public bool AcceptedPromotionalOffer
        {
            get
            {
                return DiGoldControl.AcceptedPromotionalOffer;
            }
        }

        public SubscriptionUser2 Subscriber
        {
            get
            {
                return Session["Subscriber"] as SubscriptionUser2;
            }
            set
            {
                Session["Subscriber"] = value;
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                PopulateWithUserInfo();
            }

            DataBind();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Populate input fields with user info
        /// </summary>
        private void PopulateWithUserInfo()
        {
            try
            {
                if (Subscriber == null)
                {
                    //Create new subscriber object
                    Subscriber = new SubscriptionUser2();
                }

                //if no valid cusno
                if (Subscriber.Cusno > 0)
                {
                    //Get customer's firstname and lastname
                    string firstName;
                    string lastName;
                    GetCustomerInfo(out firstName, out lastName);

                    FirstNameInput.Text = firstName;
                    LastNameInput.Text = lastName;
                    EmailInput.Text = Subscriber.Email;
                    //SocialSecurityNoInput.Text = Subscriber.BirthNo;
                    SocialSecurityNoInput.Text = Subscriber.SocialSecNo;
                    PhoneInput.Text = Subscriber.OPhone;

                    DiGoldControl.ShowPromotionalOffer = ShowPromotionalOffer();
                }
            }
            catch (Exception ex)
            {
                new Logger("PopulateWithUserInfo() - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/common/errors/error", true, true);
            }         
        }

        /// <summary>
        /// Check if data is valid. If not, show message to the user
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
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
        
        /// <summary>
        /// Save data to db
        /// </summary>
        /// <returns></returns>
        public bool SaveData()
        {
            bool saved = false;

            try
            {
                if (!IsValid())
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
        /// If there exists a promotional offer and the user has not used it yet, show the promotional offer
        /// </summary>
        /// <returns></returns>
        private bool ShowPromotionalOffer()
        {
            bool showPromotionalOffer = false;

            if (EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldPromotionalOfferPage") != null)
            {                
                if (!string.IsNullOrEmpty(PromotionalOfferName))
                {
                    DataSet diGoldOfferDs = MsSqlHandler.GetDiGoldOfferByCusno(Subscriber.Cusno, PromotionalOfferName);

                    //If no rows where returned from the db, then user has not used the offer yet.
                    if (diGoldOfferDs == null || diGoldOfferDs.Tables[0].Rows.Count == 0)
                        showPromotionalOffer = true;
                }
            }
            
            return showPromotionalOffer;
        }
        #endregion
    }
}