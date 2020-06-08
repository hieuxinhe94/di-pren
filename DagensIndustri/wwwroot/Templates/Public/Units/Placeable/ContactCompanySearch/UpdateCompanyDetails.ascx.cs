using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using EPiServer;
using DagensIndustri.OneByOne;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch
{
    public partial class UpdateCompanyDetails : UserControlBase
    {
        #region Properties
        /// <summary>
        /// Gets a reference to the page instance that contains the control
        /// </summary>
        private Pages.ContactCompanySearch.UpdateCompanyDetails UpdateCompanyDetailsPage
        {
            get
            {
                return (Pages.ContactCompanySearch.UpdateCompanyDetails)Page;
            }
        }

        protected XmlDocument WorksiteXmlDocument { get; set; }
        protected string WorksiteXml
        {
            get
            {
                return ViewState["WorksiteXml"] as string;
            }
            set
            {
                ViewState["WorksiteXml"] = value;
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadWorksite();
            if (!IsPostBack)
            {
                InitializeFields();
            }
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
                    UpdateCompanyDetailsPage.ShowMessage("/contactcompany/email/mailsentconfirmation", true, false);
                }
            }
            catch (Exception ex)
            {
                new Logger("SendButton_Click() - failed", ex.ToString());
                UpdateCompanyDetailsPage.ShowMessage("/contactcompany/email/failedtosendemail", true, true);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load xml document for worksite details
        /// </summary>
        private void LoadWorksite()
        {
            WorksiteXmlDocument = new XmlDocument();

            if (string.IsNullOrEmpty(Request.QueryString["worksiteid"]))
                return;

            DocumentFactoryService DocumentFactory = EPiFunctions.SetUpOneByOneDocumentFactory();
            if (DocumentFactory == null)
            {
                UpdateCompanyDetailsPage.ShowMessage("/common/errors/error", true, true);
                return;
            }

            try
            {
                //If it is not a postback or xml stored in viewstate is empty (due to prior error), retrieve the xml with a webservice call. 
                //Otherwise, just load the xml from viewstate.
                if (!IsPostBack || string.IsNullOrEmpty(WorksiteXml))
                {
                    RequestParameter[] parameters = new RequestParameter[] { new RequestParameter() };

                    parameters[0].name = "worksiteId";
                    parameters[0].value = Request.QueryString["worksiteid"];

                    string xml = DocumentFactory.getDocument(ConfigurationManager.AppSettings["OBOShowWorksiteDetails"], parameters);
                    WorksiteXmlDocument.LoadXml(xml);
                    WorksiteXml = xml;
                }
                else
                {
                    WorksiteXmlDocument.LoadXml(WorksiteXml);
                }
            }
            catch (Exception ex)
            {
                new Logger("CompanyWorksiteDetails LoadWorksite() - failed", ex.ToString());
                UpdateCompanyDetailsPage.ShowMessage("/common/errors/error", true, true);
            }
        }

        /// <summary>
        /// Initialize fields with company details retrieved from the webservice
        /// </summary>
        private void InitializeFields()
        {
            if (WorksiteXmlDocument != null)
            {
                CompanyNameLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/Name");
                CompanyNameInput.Text = CompanyNameLabel.Text;

                DeliveryAddressLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/PostalAddress/Address");
                DeliveryAddressInput.Text = DeliveryAddressLabel.Text;

                DeliveryZipCodeLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/PostalAddress/Zipcode").Replace(" ", "");
                DeliveryZipCodeInput.Text = DeliveryZipCodeLabel.Text;

                DeliveryCityLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/PostalAddress/City");
                DeliveryCityInput.Text = DeliveryCityLabel.Text;

                VisitorStreetAddressLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/VisitAddress/Address");
                VisitorStreetAddressInput.Text = VisitorStreetAddressLabel.Text;

                VisitorZipCodeLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/VisitAddress/Zipcode").Replace(" ", "");
                VisitorZipCodeInput.Text = VisitorZipCodeLabel.Text;

                VisitorCityLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/VisitAddress/City");
                VisitorCityInput.Text = VisitorCityLabel.Text;

                ContactInfoPhoneLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/Phone");
                ContactInfoPhoneInput.Text = ContactInfoPhoneLabel.Text;

                ContactInfoFaxLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/Fax");
                ContactInfoFaxInput.Text = ContactInfoFaxLabel.Text;

                ContactInfoEmailLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockPlusCommunication/Email");
                ContactInfoEmailInput.Text = ContactInfoEmailLabel.Text;

                HomepageUrlLabel.Text = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockPlusCommunication/WWW");
                HomePageUrlInput.Text = HomepageUrlLabel.Text;
            }
        }

        /// <summary>
        /// Create email body
        /// </summary>
        /// <returns></returns>
        private string CreateMailBody()
        {
            StringBuilder bodyBuilder = new StringBuilder();
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/companyname"), CompanyNameInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/organizationnumber"), MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization/CompanyNumber"));
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/careof"), CompanyCareOfInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/address"), DeliveryAddressInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/zipcode"), MiscFunctions.FormatZipCode(DeliveryZipCodeInput.Text.Trim()));
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/city"), DeliveryCityInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/visitoraddress"), VisitorStreetAddressInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/visitorzipcode"), MiscFunctions.FormatZipCode(VisitorZipCodeInput.Text.Trim()));
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/visitorcity"), VisitorCityInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/companyemail"), ContactInfoEmailInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/companyphone"), ContactInfoPhoneInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/companyfax"), ContactInfoFaxInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/homepage"), HomePageUrlInput.Text.Trim());
            //bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/decisionmakerfirstname"), "");
            //bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/decisionmakerlastname"), "");
            //bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/positions"), "");
            //bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/mainposition"), "");
            //bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/otherpositions"), "");
            //bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/decisionmakeremail"), "");
            //bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/decisionmakerphone"), "");
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/changestobedone"), ChangeCommentInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/informationproviderfirstname"), InfoProviderFirstNameInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/informationproviderlastname"), InfoProviderLastNameInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/informationprovideremail"), InfoProviderEmailInput.Text.Trim());
            bodyBuilder.AppendFormat("{0} {1}<br/>", Translate("/contactcompany/email/informationproviderphone"), InfoProviderPhoneInput.Text.Trim());

            return bodyBuilder.ToString();
        }

        protected string GetScript(string inputClientID)
        {
            return string.Format("$(function() {{$('#{0}').focus();}});", inputClientID);
        }
        #endregion
    }
}