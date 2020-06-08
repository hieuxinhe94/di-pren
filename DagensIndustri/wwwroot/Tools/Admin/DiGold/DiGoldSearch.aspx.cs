using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;
using DIClassLib.GoldMember;
using DIClassLib.DbHandlers;
using System.Web.Security;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Tools.Admin.DiGold
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Di Gold Search", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Di Gold Search", UrlFromUi = "/Tools/Admin/DiGold/DigoldSearch.aspx", SortIndex = 2060)]
    public partial class DiGoldSearch : System.Web.UI.Page
    {
        #region Properties

        public SubscriptionUser2 Subscriber
        {
            get
            {
                return ViewState["Subscriber"] as SubscriptionUser2;
                //return Session["Subscriber"] as SubscriptionUser;
            }
            set
            {
                ViewState["Subscriber"] = value;
                //Session["Subscriber"] = value;
            }
        }

        public string SubsName { get; set; }
        public string SubsCustomerNumber { get; set; }
        public string SubsAddress { get; set; }
        public string SubsZipCode { get; set; }
        public string SubsEmail { get; set; }

        #endregion Properties

        #region Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LiteralErr.Visible = false;
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            SocialSecurityResultPlaceHolder.Visible = false;
            SearchResults.Visible = false;

            List<long> customer = new List<long>();
            
            if(!string.IsNullOrEmpty(SocialSecurityInput.Text))
                customer = GoldMemberHandler.GetCusnosByBirthNo(SocialSecurityInput.Text);

            if (customer.Count > 0)
            {
                //string userID = MembershipDbHandler.GetUserid(customer[0]);

                Subscriber = new SubscriptionUser2(customer[0]);

                SubsCustomerNumber = Subscriber.Cusno.ToString();
                SubsName = Subscriber.RowText1 ?? string.Empty;
                SubsAddress = Subscriber.Address ?? string.Empty;
                SubsZipCode = Subscriber.Zip ?? string.Empty;
                SubsEmail = Subscriber.Email ?? string.Empty;

                ResultsLinkButton3.CommandArgument = SubsCustomerNumber + " | " + UserIsDIGoldMemberByCusNo(Subscriber.Cusno.ToString()).ToString();

                SocialSecurityResultPlaceHolder.Visible = true;
            }
            else
            {
                DataSet ds = GoldMemberHandler.FindCustomer("", LastNameInput.Text, FirstNameInput.Text, EmailInput.Text, TelephoneInput.Text, StreetInput.Text, StreetNoInput.Text, ZipInput.Text, "");

                try
                {
                    if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = ds.Tables[0].Rows[i];

                            if (!IsActive(long.Parse(dr["CUSNO"].ToString())))
                            {
                                ds.Tables[0].Rows.RemoveAt(i);
                            }

                            ds.Tables[0].AcceptChanges();
                        }
                    }

                }
                catch (Exception ex)
                {
                    new Logger("IsActive() - failed", ex.ToString());
                }

                SearchResults.DataSource = ds;
                SearchResults.DataBind();

                SearchResults.Visible = true;
            }

            ClearForm();
        }

        protected void ResultsLinkButton_Click(object sender, EventArgs e)
        {
            string commandArgument = ((LinkButton)sender).CommandArgument;

            if (!string.IsNullOrEmpty(commandArgument))
            {
                string[] args = commandArgument.Split('|');

                long cusno = long.Parse(args[0]);
                bool isGold = Convert.ToBoolean(args[1].ToString());

                SearchFormPlaceHolder.Visible = false;
                SocialSecurityResultPlaceHolder.Visible = false;
                SearchResults.Visible = false;
                HasTicketPlaceHolder.Visible = false;
                TicketFormPlaceHolder.Visible = false;
                TicketPlaceHolder.Visible = false;

                Subscriber = new SubscriptionUser2(cusno);
                //Session["Subscriber"] = Subscriber;

                if (isGold)
                {
                    if (HasWineTicket(cusno))
                    {
                        HasTicketPlaceHolder.Visible = true;
                    }
                    else
                    {
                        TicketFormPlaceHolder.Visible = true;
                    }
                }
                else
                {

                    if (Subscriber.Cusno > 0)
                    {
                        DiGoldFormPlaceHolder.Visible = true;
                    }
                }


                string oldFirstName = "";
                string oldLastName = "";
                GetCustomerInfo(out oldFirstName, out oldLastName);
                DiGoldFirstnameInput.Text = oldFirstName;
                DiGoldLastnameInput.Text = oldLastName;
            }
        }

        protected void DiGoldSaveButton_Click(object sender, EventArgs e)
        {
            //if (DiGoldFirstnameInput.Text == "" || DiGoldLastnameInput.Text == "" || DiGoldEmailInput.Text == "" || 
            //    DiGoldTelephoneInput.Text == "" || DiGoldSocialInput.Text == "" || !IsValid())
            if (DiGoldEmailInput.Text == "" || DiGoldTelephoneInput.Text == "" || DiGoldSocialInput.Text == "" || !IsValid())
            {
                LiteralErr.Visible = true;
                return;
            }


            bool saved = false;

            if (TermsAcceptedInput.Checked)
            {
                saved = SaveData();

                if (saved)
                {
                    string userID = MembershipDbHandler.GetUserid(Subscriber.Cusno);
                    DiRoleHandler.AddUserToRoles(userID, new string[] { DiRoleHandler.RoleDiGold });
                }

                if (TicketAcceptInput.Checked)
                {
                    try
                    {
                        MsSqlHandler.InsertTicketEntry(Subscriber.Cusno);
                        TicketPlaceHolder.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        new Logger("InsertTicketEntry() - failed", ex.ToString());
                    }
                }
                else
                {
                    Response.Redirect("/Tools/Admin/DiGold/DigoldSearch.aspx");                
                }
            }            
        }

        protected void TicketLinkButton_Click(object sender, EventArgs e)
        {
            MsSqlHandler.InsertTicketEntry(Subscriber.Cusno);
            TicketFormPlaceHolder.Visible = false;
            TicketPlaceHolder.Visible = true;
        }

        #endregion Events

        #region Methods


        protected void ClearForm()
        {
            FirstNameInput.Text = string.Empty;
            LastNameInput.Text = string.Empty;
            SocialSecurityInput.Text = string.Empty;
            StreetInput.Text = string.Empty;
            StreetNoInput.Text = string.Empty;
            ZipInput.Text = string.Empty;
            TelephoneInput.Text = string.Empty;
            EmailInput.Text = string.Empty;            
        }

        /// <summary>
        /// Check whether the user has DiGold role
        /// </summary>
        protected bool UserIsDIGoldMember(string userID)
        {
            bool isUserDIGoldMember = false;
            if (!string.IsNullOrEmpty(userID))
            {
                isUserDIGoldMember = Roles.IsUserInRole(userID, DiRoleHandler.RoleDiGold);
            }
            return isUserDIGoldMember;
        }

        protected bool UserIsDIGoldMemberByCusNo(string cusno)
        {
            bool isDiGold = false;
            try
            {
                string userID = MembershipDbHandler.GetUserid(long.Parse(cusno));

                isDiGold = UserIsDIGoldMember(userID);
            }
            catch (Exception ex)
            {
                new Logger("UserIsDIGoldMemberByCusNo() - failed for cusno:" + cusno, ex.ToString());
            }

            return isDiGold;
        }

        private static bool IsActive(long cusno)
        {
            var isActive = false;
            var dsSubs = SubscriptionController.GetSubscriptions(cusno, false);

            foreach (DataRow dr in dsSubs.Tables[0].Rows)
            {
                if (Settings.SubsStateActiveValues.Contains(dr["SUBSSTATE"].ToString())) // && subsType.Contains(dr["PRODUCTNO"].ToString()))
                    isActive = true;   //has active subs
            }
            return isActive;
        }

        protected static bool IsDiWeekend(long cusno)
        {
            var isDiWeekend = false;
            var subsType = new List<string> { "05" };
            var dsSubs = SubscriptionController.GetSubscriptions(cusno, false);

            foreach (DataRow dr in dsSubs.Tables[0].Rows)
            {
                if (subsType.Contains(dr["PRODUCTNO"].ToString()))
                    isDiWeekend = true;
            }
            return isDiWeekend;
        }

        protected bool HasWineTicket(long cusno)
        {
            return MsSqlHandler.IsCustomerInTicketEntries(cusno);
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
                //((DiTemplatePage)Page).ShowMessage("/common/validation/mobilephonenumberrequired", true, true);
                return false;
            }

            //Validate the entered social security number
            if (string.IsNullOrEmpty(MiscFunctions.FormatSocialSecurityNo(DiGoldSocialInput.Text)))
            {
                //((DiTemplatePage)Page).ShowMessage("/common/validation/socialsecuritynumberrequired", true, true);
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
            if (!int.TryParse(DiGoldTelephoneInput.MaxValue, out mobilePhoneMaxValue))
                mobilePhoneMaxValue = Settings.PhoneMaxNoOfDigits;

            return MiscFunctions.FormatPhoneNumber(DiGoldTelephoneInput.Text, mobilePhoneMaxValue, true);
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

                if (DiGoldFirstnameInput.Text.Trim() != "" && DiGoldLastnameInput.Text.Trim() != "")
                {
                    //Get customer's firstname and lastname
                    string existingFirstName;
                    string existingLastName;
                    GetCustomerInfo(out existingFirstName, out existingLastName);

                    bool nameHasChanged = DiGoldFirstnameInput.Text.Trim() != existingFirstName || DiGoldLastnameInput.Text.Trim() != existingLastName;
                    if (nameHasChanged)
                    {
                        //If firstname and last name differ from what is stored, store the new first and last name in another database
                        MsSqlHandler.SaveDiGoldNameChange(Subscriber.Cusno, DiGoldFirstnameInput.Text.Trim(), DiGoldLastnameInput.Text.Trim());
                    }
                }

                //Format the entered mobile number
                string mobilePhoneNo = FormatMobilePhoneNumber();

                //Format the entered social security number
                string socSecNo = MiscFunctions.FormatSocialSecurityNo(DiGoldSocialInput.Text);

                //If everything was OK, store data
                saved = Subscriber.UpdateUserOnJoinGold(DiGoldEmailInput.Text.Trim(), mobilePhoneNo, socSecNo);
            }
            catch (Exception ex)
            {
                new Logger("SaveData() - failed", ex.ToString());
                //((DiTemplatePage)Page).ShowMessage("/common/errors/error", true, true);
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

        protected string GetTermsAndConditions()
        {
            EPiServer.Core.PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiServer.Core.PageReference.StartPage);
            string termsAndConditions = string.Empty;
            EPiServer.Core.PageReference termsAndConditionsPageLink = EPiFunctions.SettingsPageSetting(pd, "DiGoldTermsPage") as EPiServer.Core.PageReference;
            if (termsAndConditionsPageLink != null)
            {
                EPiServer.Core.PageData termsPageData = EPiServer.DataFactory.Instance.GetPage(termsAndConditionsPageLink);

                termsAndConditions = string.Format("Jag har läst och godkänt <a href=\"{0}\" onclick=\"window.open(this.href, '{1}', 'toolbar=yes, location=yes, directories=yes, status=yes, menubar=yes, scrollbars=yes, copyhistory=yes, resizable=yes'); return false\">medlemsvillkoren</a>",
                                                    EPiFunctions.GetFriendlyAbsoluteUrl(termsPageData.PageLink),
                                                    !string.IsNullOrEmpty((string)termsPageData["Heading"]) ? (string)termsPageData["Heading"] : termsPageData.PageName
                                                    );
            }
            return termsAndConditions;
        }        

        #endregion Methods
    }
}