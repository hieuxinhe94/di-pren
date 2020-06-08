using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    public partial class CompanyWorksiteDetails : UserControlBase
    {
        #region Properties
        /// <summary>
        /// Gets a reference to the page instance that contains the control
        /// </summary>
        private Pages.ContactCompanySearch.CompanyDetails CompanyDetailsPage
        {
            get
            {
                return (Pages.ContactCompanySearch.CompanyDetails)Page;
            }
        }
        public DocumentFactoryService DocumentFactory { get; set; }
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
            DataBind();

            this.Visible = WorksiteXmlDocument.HasChildNodes;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load xml document for worksite details
        /// </summary>
        private void LoadWorksite()
        {
            WorksiteXmlDocument = new XmlDocument();

            if (string.IsNullOrEmpty(Request.QueryString["worksiteid"]) || DocumentFactory == null)
                return;

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
                CompanyDetailsPage.ShowMessage("/common/errors/error", true, true);
            }
        }

        /// <summary>
        /// Format the URL so that it contains either http://  or https://. If none exists, add http:// to the url.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        protected string FormatURL(XmlDocument xmlDoc, string xpath)
        {
            string nodeText = MiscFunctions.GetXmlNodeText(xmlDoc, xpath);
            if (!string.IsNullOrEmpty(nodeText))
            {
                if (!nodeText.ToLower().StartsWith("http://") && !nodeText.ToLower().StartsWith("https://"))
                {
                    nodeText = "http://" + nodeText;
                }
            }
            return nodeText;
        }

        /// <summary>
        /// Checks if there is any kind of communication ways to contact the company (phone, email, fax and www)
        /// </summary>
        /// <returns></returns>
        protected bool HasCommunicationDetails()
        {
            return !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/Phone")) ||
                   !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockPlusCommunication/Email")) ||
                   !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/Fax")) ||
                   !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockPlusCommunication/WWW"));
        }


        protected string GetMapsUrl()
        {
            string address = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/VisitAddress/Address");
            string city = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/VisitAddress/City");
            return EPiFunctions.GetMapsUrl(address, city);
        }

        protected string GetCompanyNumber()
        {
            string organisationNumber = string.Empty;
            if (MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization/LegalForm/Text").ToLower() == "fysisk person")
                organisationNumber = Translate("/contactcompanysearch/companydetails/company/detailsnotshown");
            else
                organisationNumber = MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization/CompanyNumber");

            return organisationNumber;
        }
        #endregion
    }
}