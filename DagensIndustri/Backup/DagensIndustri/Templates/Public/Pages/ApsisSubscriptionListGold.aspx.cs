using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Security;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.EPiJobs.Apsis;
using DagensIndustri.Tools.Classes;
using DIClassLib.Subscriptions;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class ApsisSubscriptionListGold : DiTemplatePage
    {
        ApsisWsHandler _ws;
        string _urlCode = "code";

        public ApsisWsHandler Ws
        {
            get
            {
                if (_ws == null)
                    _ws = new ApsisWsHandler();

                return _ws;
            }
        }

        public bool IsSmsList
        {
            get { return EPiFunctions.HasValue(CurrentPage, "IsSmsList"); }
        }

        public string ApsisListId
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "ApsisListId"))
                    return CurrentPage["ApsisListId"].ToString();

                return string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //populate by code in url
                if (!string.IsNullOrEmpty(Request.QueryString[_urlCode]))
                {
                    MssqlCustomer mssqlCust = new MssqlCustomer(Request.QueryString[_urlCode]);
                    if (mssqlCust != null)
                    {
                        Subscriber = new SubscriptionUser2(mssqlCust.Cusno);
                        if (Subscriber != null)
                        {
                            if (DIClassLib.Misc.LoginUtil.ReLoginUserRefreshCookie(Subscriber.UserName, Subscriber.Password))
                                Response.Redirect(EPiFunctions.GetFriendlyUrl(CurrentPage));
                        }
                    }
                }

                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    //ShowMessage(string.Format(Translate("/promofferjoingold/loginforpromotionaloffer"), OfferName, EPiFunctions.GetLoginPageUrl(CurrentPage)), false, true);
                    UserMessageControl1.ShowMessage("/common/message/loginforservice", true, true);
                    SubscribeMultiView.Visible = false;
                    // LabelThankYou.Text = "<p>" + Translate("/common/message/loginforservice") + "</p>";
                    // LabelThankYou.Visible = true;
                    return;
                }

                DataBind();
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            PopulateWithUserInfo();
        }

        protected void ButtonSubscribe_Click(object sender, EventArgs e)
        {
            bool success = false;
            String apsisEmail = "";
            String apsisPhone = "";

            if (!User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiGold))
            {
                // create gold member
                //
                success = SaveData();
                apsisEmail = EmailInput.Text;
                apsisPhone = PhoneInput.Text;
            }
            else
            {
                if (IsSmsList)
                {
                    apsisPhone = ApsisInputPhone.Text;
                    apsisEmail = Subscriber.Email;
                }
                else
                {
                    apsisPhone = Subscriber.OPhone;
                    apsisEmail = ApsisInputEmail.Text;
                }
                success = true;
            }

            if (success && Subscriber != null)
            {
                ApsisListSubscriber apsisSubs = new ApsisListSubscriber(ApsisListId, Subscriber.Cusno.ToString(), Subscriber.RowText1, apsisEmail, apsisPhone);

                if (Ws.AddCustToApsisList(apsisSubs))
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                    {
                        // call apsis for short delay
                        //
                        ApsisWsHandler awh = new ApsisWsHandler();
                        ApsisListSubscriber sub = awh.TryGetApsisListSubscriberByCusno(CurrentPage["ApsisListId"].ToString(), Subscriber.Cusno.ToString());



                        String successMessage = Translate("/apsisSubsList/subsadded");
                        Session["SuccessMessage"] = successMessage;
                        Response.Redirect(Request.QueryString["ReturnUrl"]);
                        return;
                    }
                    else
                    {
                        UserMessageControl1.ShowMessage("/apsisSubsList/subsadded", true, false);
                    }

                }
                else
                {
                    UserMessageControl1.ShowMessage("/apsisSubsList/error", true, true);
                }
            }

            if (success)
            {
                SubscribeMultiView.SetActiveView(ThankYouView);
            }
            else
            {
            }
        }

        protected void ButtonUnSubscribe_Click(object sender, EventArgs e)
        {
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
                    ApsisInputPhone.Text = Subscriber.OPhone;
                    ApsisInputEmail.Text = Subscriber.Email;
                    //SocialSecurityNoInput.Text = Subscriber.BirthNo;
                    SocialSecurityNoInput.Text = Subscriber.SocialSecNo;
                    PhoneInput.Text = Subscriber.OPhone;

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
        #endregion
    }
}