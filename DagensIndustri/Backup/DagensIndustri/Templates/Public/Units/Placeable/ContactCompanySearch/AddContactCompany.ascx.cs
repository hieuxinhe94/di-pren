using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch
{
    public partial class AddContactCompany : UserControlBase
    {
        #region Properties
        /// <summary>
        /// Gets a reference to the page instance that contains the control
        /// </summary>
        private Pages.ContactCompanySearch.AddContactCompany AddCompanyDetailsPage
        {
            get
            {
                return (Pages.ContactCompanySearch.AddContactCompany)Page;
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                string from = EPiFunctions.SettingsPageSetting(CurrentPage, "ContactCompanyFromEmail") as string;
                string to = EPiFunctions.SettingsPageSetting(CurrentPage, "ContactCompanyToEmail") as string;
                if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                {
                    string body = CreateMailBody();
                    MiscFunctions.SendMail(from, to, Translate("/contactcompany/email/subject"), body, true);
                    AddCompanyDetailsPage.ShowMessage("/contactcompany/email/mailsentconfirmation", true, false);
                }
            }
            catch (Exception ex)
            {
                new Logger("SendButton_Click() - failed", ex.ToString());
                AddCompanyDetailsPage.ShowMessage("/contactcompany/email/failedtosendemail", true, true);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create email body
        /// </summary>
        /// <returns></returns>
        private string CreateMailBody()
        {
            StringBuilder bodyBuilder = new StringBuilder();
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/companyname"), CompanyNameInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/organizationnumber"), OrganizationNumberInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/address"), DeliveryAddressInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/zipcode"), MiscFunctions.FormatZipCode(DeliveryZipCodeInput.Text.Trim()));
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/city"), DeliveryCityInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/visitoraddress"), VisitorStreetAddressInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/visitorzipcode"), MiscFunctions.FormatZipCode(VisitorZipCodeInput.Text.Trim()));
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/visitorcity"), VisitorCityInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/companyemail"), ContactInfoEmailInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/companyphone"), ContactInfoPhoneInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/companyfax"), ContactInfoFaxInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/decisionmakerfirstname"), DecisionMakerFirstNameInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/decisionmakerlastname"), DecisionMakerLastNameInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/positions"), DecisionMakerPositionsInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/mainposition"), "");
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/otherpositions"), "");
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/decisionmakeremail"), DecisionMakerEmailInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/decisionmakerphone"), DecisionMakerPhoneInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/changestobedone"), DecisionMakerChangesInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/informationproviderfirstname"), InfoProviderFirstNameInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/informationproviderlastname"), InfoProviderLastNameInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/informationprovideremail"), InfoProviderEmailInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/informationproviderphone"), InfoProviderPhoneInput.Text.Trim());

            return bodyBuilder.ToString();
        }
        #endregion
    }
}