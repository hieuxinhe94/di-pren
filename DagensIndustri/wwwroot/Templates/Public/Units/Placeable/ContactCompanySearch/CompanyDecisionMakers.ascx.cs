using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using EPiServer;
using DagensIndustri.OneByOne;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using System.Globalization;

namespace DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch
{
    public partial class CompanyDecisionMakers : UserControlBase
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
        protected XmlDocument DecisionMakersXmlDocument { get; set; }
        protected string DecisionMakersXml
        {
            get
            {
                return ViewState["DecisionMakersXml"] as string;
            }
            set
            {
                ViewState["DecisionMakersXml"] = value;
            }
        }
        /// <summary>
        /// Id of the contact that is being viewed
        /// </summary>
        protected string SelectedContactId
        {
            get
            {
                return ViewState["SelectedContactId"] as string;
            }
            set
            {
                ViewState["SelectedContactId"] = value;
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadDecisionMakers();

            //If a certain contact was selected, load its details
            if (!IsPostBack)
            {
                LoadContact();
            }

            this.Visible = DecisionMakersXmlDocument.HasChildNodes;
        }
        
        /// <summary>
        /// Load xml document for a certain decision maker and populate its detail fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DecisionMakerLinkButton_Click(object sender, EventArgs e)
        {
            if (DocumentFactory == null)
                return;

            string contactId = ((LinkButton)sender).CommandArgument;
            RepeaterItem contactRepeaterItem = FindContactRepeaterItem(contactId);
            LoadContact(contactId, contactRepeaterItem);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load xml document for decision makers and populate decision makers' repeater 
        /// </summary>
        private void LoadDecisionMakers()
        {
            DecisionMakersXmlDocument = new XmlDocument();

            if (string.IsNullOrEmpty(Request.QueryString["worksiteid"]) || DocumentFactory == null)
                return;

            try
            {
                //If it is not a postback or xml stored in viewstate is empty (due to prior error), retrieve the xml with a webservice call. 
                //Otherwise, just load the xml from viewstate.
                if (!IsPostBack || string.IsNullOrEmpty(DecisionMakersXml))
                {
                    RequestParameter[] parameters = new RequestParameter[] { new RequestParameter() };

                    parameters[0].name = "worksiteId";
                    parameters[0].value = Request.QueryString["worksiteid"];

                    string xml = DocumentFactory.getDocument(ConfigurationManager.AppSettings["OBODecisionMakers"], parameters);
                    DecisionMakersXmlDocument.LoadXml(xml);
                    DecisionMakersXml = xml;
                }
                else
                {
                    DecisionMakersXmlDocument.LoadXml(DecisionMakersXml);
                }

                PopulateDecisionMakers();
                DataBind();
            }
            catch (Exception ex)
            {
                new Logger("CompanyDecisionMakers LoadDecisionMakers() - failed", ex.ToString());
                CompanyDetailsPage.ShowMessage("/common/errors/error", true, true);
            }
        }

        /// <summary>
        /// If a certain contact was selected, load its details
        /// </summary>
        private void LoadContact()
        {
            if (string.IsNullOrEmpty(Request.QueryString["contactid"]))
                return;

            string contactId = Request.QueryString["contactid"];
            RepeaterItem contactRepeaterItem = FindContactRepeaterItem(contactId);
            LoadContact(contactId, contactRepeaterItem);
        }

        /// <summary>
        /// Load xml document for a certain decision maker and populate the details section
        /// </summary>
        /// <param name="contactId"></param>
        private void LoadContact(string contactId, object sender)
        {
            if (string.IsNullOrEmpty(contactId))
                return;

            RequestParameter[] parameters = new RequestParameter[] { new RequestParameter() };

            parameters[0].name = "contactId";
            parameters[0].value = contactId;

            try
            {
                string xml = DocumentFactory.getDocument(ConfigurationManager.AppSettings["OBOShowContact"], parameters);
                XmlDocument contactXmlDocument = new XmlDocument();
                contactXmlDocument.LoadXml(xml);

                PopulateDecisionMakerDetails(contactXmlDocument, sender, contactId);
            }
            catch (Exception ex)
            {
                new Logger("CompanyDecisionMakers LoadContact() - failed", ex.ToString());
                CompanyDetailsPage.ShowMessage("/common/errors/error", true, true);
            }
        }

        /// <summary>
        /// Populate decision makers' repeater after sorting the xml according to MainJobTitle
        /// </summary>
        private void PopulateDecisionMakers()
        {
            XPathNavigator navigator = DecisionMakersXmlDocument.CreateNavigator();
            XPathExpression expression = navigator.Compile("Bizbook-Fetch-C/Contacts/Contact");
            
            expression.AddSort("MainJobTitle", XmlSortOrder.Ascending, XmlCaseOrder.None, "sv", XmlDataType.Text);
            expression.AddSort("LastName", XmlSortOrder.Ascending, XmlCaseOrder.None, "sv", XmlDataType.Text);
            
            DecisionMakerRepeater.DataSource = navigator.Select(expression);
            DecisionMakerRepeater.DataBind();
        }

        /// <summary>
        /// Populate details about a certain decision maker
        /// </summary>
        /// <param name="contactXmlDocument"></param>
        /// <param name="sender"></param>
        private void PopulateDecisionMakerDetails(XmlDocument contactXmlDocument, object sender, string contactId)
        {
            RepeaterItem contactRepeaterItem = sender as RepeaterItem;

            //Set the decision maker's main position
            Label mainPositionLabel = contactRepeaterItem.FindControl("MainPositionLabel") as Label;
            mainPositionLabel.Text = MiscFunctions.GetXmlNodeText(contactXmlDocument, "Bizbook-Show-C/BlockCRMPosition/MainPosition/Text");

            //Set the decision maker's email address
            HyperLink emailHyperLink = contactRepeaterItem.FindControl("EmailHyperLink") as HyperLink;
            emailHyperLink.NavigateUrl = string.Format("mailto:{0}", MiscFunctions.GetXmlNodeText(contactXmlDocument, "Bizbook-Show-C/BlockPlusCommunication/Email"));
            emailHyperLink.Text = MiscFunctions.GetXmlNodeText(contactXmlDocument, "Bizbook-Show-C/BlockPlusCommunication/Email");

            //Add the decision maker's remaining positions in a list
            List<string> positions = new List<string>(); 
            string mainPositionCode = MiscFunctions.GetXmlNodeText(contactXmlDocument, "Bizbook-Show-C/BlockCRMPosition/MainPosition/Code");
            foreach (XmlNode xmlNode in contactXmlDocument.SelectNodes("Bizbook-Show-C/BlockCRMPosition/Positions/Position"))
            {
                if (MiscFunctions.GetXmlNodeText(xmlNode, "Code") != mainPositionCode)
                {
                    positions.Add(MiscFunctions.GetXmlNodeText(xmlNode, "Text"));
                }
            }

            PlaceHolder otherPositionPlaceHolder = contactRepeaterItem.FindControl("OtherPositionPlaceHolder") as PlaceHolder;
            otherPositionPlaceHolder.Visible = positions.Count > 0;

            //Populate the repeater for remaining positions if exists
            Repeater otherPositionsRepeater = otherPositionPlaceHolder.FindControl("OtherPositionsRepeater") as Repeater;
            otherPositionsRepeater.DataSource = positions;
            otherPositionsRepeater.DataBind();

            //Set 'open' class to the list item 
            HtmlGenericControl decisionMakerListItem = contactRepeaterItem.FindControl("DecisionMakerListItem") as HtmlGenericControl;
            string existingAttributes = decisionMakerListItem.Attributes["class"];
            if (SelectedContactId != contactId)
            {
                SelectedContactId = contactId;
                decisionMakerListItem.Attributes.Add("class", string.Format("{0} open", existingAttributes));
            }
        }

        /// <summary>
        /// Find the repeater item for a certain contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        private RepeaterItem FindContactRepeaterItem(string contactId)
        {
            RepeaterItem contactRepeaterItem = null;
            foreach (RepeaterItem repeaterItem in DecisionMakerRepeater.Items)
            {
                HiddenField contactIdHiddenField = repeaterItem.FindControl("ContactIdHiddenField") as HiddenField;
                if (contactIdHiddenField.Value == contactId)
                {
                    contactRepeaterItem = repeaterItem;
                    break;
                }
            }
            return contactRepeaterItem;
        }

        /// <summary>
        /// Get a value from the xpath navigator
        /// </summary>
        /// <param name="xpathNavigator"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetValue(XPathNavigator xpathNavigator, string name)
        {
            return xpathNavigator != null && xpathNavigator.SelectSingleNode(name) != null
                ? xpathNavigator.SelectSingleNode(name).InnerXml
                : string.Empty;
        }
        #endregion
    }
}