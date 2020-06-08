using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using EPiServer;
using DagensIndustri.BusinessCheck;
using DagensIndustri.OneByOne;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch
{
    public partial class CompanyBusiness : UserControlBase
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
        protected XmlDocument BusinessDescriptionXmlDocument { get; set; }
        protected XmlDocument WorksiteXmlDocument { get; set; }
        protected DataImport2Result BusinessCheckResult  
        { 
            get
            {
                return ViewState["BusinessCheckResult "] as DataImport2Result;
            }
            set
            {
                ViewState["BusinessCheckResult "] = value;
            }
        }

        protected string BusinessDescriptionXml
        {
            get
            {
                return ViewState["BusinessDescriptionXml"] as string;
            }
            set
            {
                ViewState["BusinessDescriptionXml"] = value;
            }
        }
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
            LoadBusinessDescription();
            LoadBusinessCheck();
            DataBind();

            this.Visible = BusinessDescriptionXmlDocument.HasChildNodes || WorksiteXmlDocument.HasChildNodes;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load xml document for business description and populate necessary fields
        /// </summary>
        private void LoadBusinessDescription()
        {
            BusinessDescriptionXmlDocument = new XmlDocument();

            string companyNumber = !string.IsNullOrEmpty(Request.QueryString["companynumber"])
                                    ? Request.QueryString["companynumber"]
                                    : MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization/CompanyNumber").Replace("-", "");

            if (string.IsNullOrEmpty(companyNumber) || DocumentFactory == null)
                return;

            try
            {
                //If it is not a postback or xml stored in viewstate is empty (due to prior error), retrieve the xml with a webservice call. 
                //Otherwise, just load the xml from viewstate.
                if (!IsPostBack || string.IsNullOrEmpty(BusinessDescriptionXml))
                {
                    RequestParameter[] parameters = new RequestParameter[] { new RequestParameter() };

                    parameters[0].name = "companyNumber";
                    parameters[0].value = companyNumber;

                
                        string xml = DocumentFactory.getDocument(ConfigurationManager.AppSettings["OBOBusinessDescription"], parameters);
                        BusinessDescriptionXmlDocument.LoadXml(xml);
                        BusinessDescriptionXml = xml;
                }
                else
                {
                    BusinessDescriptionXmlDocument.LoadXml(BusinessDescriptionXml);
                }

                PopulateBiCompanies();
            }
            catch (Exception ex)
            {
                new Logger("CompanyBusiness LoadBusinessDescription() - failed", ex.ToString());
                CompanyDetailsPage.ShowMessage("/common/errors/error", true, true);
            }
        }

        /// <summary>
        /// Load xml document for worksite details and populate necessary fields
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

                PopulateBusinesses();
            }
            catch (Exception ex)
            {
                new Logger("CompanyBusiness LoadWorksite() - failed", ex.ToString());
                CompanyDetailsPage.ShowMessage("/common/errors/error", true, true);
            }
        }

        /// <summary>
        /// Load information about the boardmembers
        /// </summary>
        private void LoadBusinessCheck()
        {
            string companyNumber = !string.IsNullOrEmpty(Request.QueryString["companynumber"])
                                    ? Request.QueryString["companynumber"]
                                    : MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization/CompanyNumber").Replace("-", "");

            if (string.IsNullOrEmpty(companyNumber))
                return;

            try
            {
                //If it is not a postback, retrieve the xml with a webservice call. 
                //Otherwise, just load the xml from viewstate.
                if (!IsPostBack)
                {
                    DataImport2 dataImport = new DataImport2();
                    BusinessCheckResult = dataImport.DataImport2Company(ConfigurationManager.AppSettings["BusinessCheckCustomerLoginName"],
                                                                        ConfigurationManager.AppSettings["BusinessCheckUserLoginName"],
                                                                        ConfigurationManager.AppSettings["BusinessCheckPassword"],
                                                                        ConfigurationManager.AppSettings["BusinessCheckLanguage"],
                                                                        ConfigurationManager.AppSettings["BusinessCheckPackageName"],
                                                                        companyNumber);
                }

                PopulateBoardMembers();
            }
            catch (Exception ex)
            {
                new Logger("CompanyBusiness LoadBusinessDescription() - failed", ex.ToString());
                CompanyDetailsPage.ShowMessage("/common/errors/error", true, true);
            }
        }

        /// <summary>
        /// Populate bi companies
        /// </summary>
        private void PopulateBiCompanies()
        {
            if (BusinessDescriptionXmlDocument != null)
            {
                XmlNodeList biCompaniesXmlNodeList = BusinessDescriptionXmlDocument.SelectNodes("Bizbook-PRV/Company/BiCompany");
                BiCompanyRepeater.Visible = biCompaniesXmlNodeList != null && biCompaniesXmlNodeList.Count > 0;
                HasNoBiCompaniesLiteral.Visible = biCompaniesXmlNodeList == null || biCompaniesXmlNodeList.Count == 0;

                if (BiCompanyRepeater.Visible)
                {
                    BiCompanyRepeater.DataSource = biCompaniesXmlNodeList;
                    BiCompanyRepeater.DataBind();
                }
            }
        }

        /// <summary>
        /// Populate different businesses
        /// </summary>
        private void PopulateBusinesses()
        {
            if (WorksiteXmlDocument != null)
            {
                List<Business> snBusinessesList = new List<Business>();
                List<Business> otherBusinessesList = new List<Business>();
                
                XmlNodeList businessesXmlNodeList = WorksiteXmlDocument.SelectNodes("Bizbook-Show-W/BlockCRMBusiness/Businesses/Business");
                foreach (XmlNode xmlNode in businessesXmlNodeList)
                {
                    Business business = new Business 
                    {
                        Code = MiscFunctions.GetXmlNodeText(xmlNode, "Code"),
                        Text = MiscFunctions.GetXmlNodeText(xmlNode, "Text")
                    };

                    string code = !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(xmlNode, "Code"))
                                ? MiscFunctions.GetXmlNodeText(xmlNode, "Code") 
                                : string.Empty;
                    if (code.ToUpper().StartsWith("SN"))
                    {
                        snBusinessesList.Add(business);
                    }
                    else if (code.ToUpper().StartsWith("OV"))
                    {
                        otherBusinessesList.Add(business);
                    }
                }
                
                SNBusinessRepeater.DataSource = snBusinessesList;
                SNBusinessRepeater.DataBind();

                OtherBusinessRepeater.DataSource = otherBusinessesList;
                OtherBusinessRepeater.DataBind();
            } 
        }

        /// <summary>
        /// Populate the board members table
        /// </summary>
        private void PopulateBoardMembers()
        {
            if (BusinessCheckResult != null)
            {
                List<BoardMember> boardMemberList = new List<BoardMember>();
                foreach (BusinessCheck.Block block in BusinessCheckResult.Blocks)
                {
                    //A block with segment is a board member block. Add it to board table.
                    if (block.SegmentSpecified)
                    {
                        BoardMember boardMember = CreateBoardMember(block);
                        if (!string.IsNullOrEmpty(boardMember.Name) && !string.IsNullOrEmpty(boardMember.SocialSecurityNumber))
                        {
                            //The order of the board member list is: The president, the external chief executive officer and then all the other members.
                            //Is this the correct order?
                            if (boardMember.IsPresident)
                            {
                                boardMemberList.Insert(0, boardMember);
                            }
                            else if (boardMember.IsExternalChiefExecutiveOfficer)
                            {
                                if (boardMemberList.Count > 0 && boardMemberList[0].IsPresident)
                                    boardMemberList.Insert(1, boardMember);
                                else
                                    boardMemberList.Insert(0, boardMember);
                            }
                            else
                            {
                                boardMemberList.Add(boardMember);
                            }
                        }
                    }
                }

                foreach (BoardMember boardMember in boardMemberList)
                {
                    TableRow tableRow = new TableRow();
                    tableRow.TableSection = TableRowSection.TableBody;
                    tableRow.Cells.Add(CreateTableCell(boardMember.Name));
                    tableRow.Cells.Add(CreateTableCell(boardMember.GetBoardFunctions()));
                    tableRow.Cells.Add(CreateTableCell(boardMember.SocialSecurityNumber));
                    tableRow.Cells.Add(CreateTableCell(boardMember.AdmittanceDate));

                    BoardTable.Rows.Add(tableRow);
                }
            }
        }

        /// <summary>
        /// Create a board member from a given block
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private BoardMember CreateBoardMember(BusinessCheck.Block block)
        {
            BoardMember boardMember = new BoardMember();
            foreach (Field field in block.Fields)
            {
                switch (field.Code)
                {
                    case "PNr":
                        boardMember.SocialSecurityNumber = field.Value;
                        break;
                    case "Namn":
                        boardMember.Name = field.Value;
                        break;
                    case "Funk1":
                        boardMember.Funk1 = field.Value;
                        break;
                    case "Funk1Text":
                        boardMember.Funk1Text = field.Value;
                        break;
                    case "Funk1Datum":
                        boardMember.Funk1Datum = field.Value;
                        break;
                    case "Funk2":
                        boardMember.Funk2 = field.Value;
                        break;
                    case "Funk2Text":
                        boardMember.Funk2Text = field.Value;
                        break;
                    case "Funk2Datum":
                        boardMember.Funk2Datum = field.Value;
                        break;
                    case "Funk3":
                        boardMember.Funk3 = field.Value;
                        break;
                    case "Funk3Text":
                        boardMember.Funk3Text = field.Value;
                        break;
                    case "Funk3Datum":
                        boardMember.Funk3Datum = field.Value;
                        break;
                    case "Funk4":
                        boardMember.Funk4 = field.Value;
                        break;
                    case "Funk4Text":
                        boardMember.Funk4Text = field.Value;
                        break;
                    case "Funk4Datum":
                        boardMember.Funk4Datum = field.Value;
                        break;
                    default:
                        break;
                }                
            }
            return boardMember;
        }        

        /// <summary>
        /// Create a table cell with value
        /// </summary>
        /// <param name="cellValue"></param>
        /// <returns></returns>
        private TableCell CreateTableCell(string cellValue)
        {
            TableCell tableCell = new TableCell();
            tableCell.Text = cellValue;
            return tableCell;
        }

        /// <summary>
        /// Get a certain field value given the block name and the field code
        /// </summary>
        /// <param name="blockName"></param>
        /// <param name="fieldCode"></param>
        /// <returns></returns>
        protected string GetBusinessCheckValue(string blockName, string fieldCode)
        {
            string fieldValue = string.Empty;
            if (BusinessCheckResult != null)
            {
                foreach (BusinessCheck.Block block in BusinessCheckResult.Blocks)
                {
                    if (block.Code.Equals(blockName))
                    {
                        foreach (Field field in block.Fields)
                        {
                            if (field.Code.Equals(fieldCode))
                            {
                                fieldValue = field.Value;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(fieldValue))
                            break;
                    }
                }
            }
            return fieldValue;
        }

        protected string GetCommentsForAnnualAccounting() 
        {
            StringBuilder annualAccountCommentBuilder = new StringBuilder();
            if (BusinessCheckResult != null)
            {
                foreach (BusinessCheck.Block block in BusinessCheckResult.Blocks)
                {
                    string period = string.Empty;
                    string comment = string.Empty;
                    if (block.Code.Equals("Bokslut"))
                    {
                        foreach (Field field in block.Fields)
                        {
                            if (field.Code.Equals("BokPer"))
                            {
                                period = field.Value;
                            }
                            else if (field.Code.Equals("RevKomText"))
                            {
                                comment = field.Value;
                            }
                        }

                        if (!string.IsNullOrEmpty(comment))
                        {
                            annualAccountCommentBuilder.AppendFormat("<p>{0}<br />{1}</p>", period, comment);
                        }
                    }
                }
            }
            return annualAccountCommentBuilder.ToString();
        }
        #endregion

        #region Class Business
        /// <summary>
        /// An internal class used to show information about different businesses
        /// </summary>
        protected class Business
        {
            public string Code { get; set; }
            public string Text { get; set; }
        }
        #endregion

        #region Class BoardMember
        /// <summary>
        /// An internal class used to show information about a board member
        /// </summary>
        private class BoardMember
        {
            #region Constants
            private const string PRESIDENT = "OF";
            private const string EXTERNAL_CHIEF_EXECUTIVE_OFFICER = "EVD";

            #endregion
            #region Properties
            public string Name { get; set; }
            public string SocialSecurityNumber { get; set; }            
            
            public string Funk1 { get; set; }
            public string Funk1Text { get; set; }
            public string Funk1Datum { get; set; }

            public string Funk2 { get; set; }
            public string Funk2Text { get; set; }
            public string Funk2Datum { get; set; }

            public string Funk3 { get; set; }
            public string Funk3Text { get; set; }
            public string Funk3Datum { get; set; }

            public string Funk4 { get; set; }
            public string Funk4Text { get; set; }
            public string Funk4Datum { get; set; }

            public string AdmittanceDate
            {
                get
                {
                    string admittanceDate = string.Empty;

                    if (!string.IsNullOrEmpty(Funk1Datum))
                        admittanceDate = Funk1Datum;
                    else if (!string.IsNullOrEmpty(Funk2Datum))
                        admittanceDate = Funk2Datum;
                    else if (!string.IsNullOrEmpty(Funk3Datum))
                        admittanceDate = Funk3Datum;
                    else if (!string.IsNullOrEmpty(Funk4Datum))
                        admittanceDate = Funk4Datum;

                    return admittanceDate;
                }
            }

            public bool IsPresident
            {
                get
                {
                    return (Funk1 == PRESIDENT || Funk2 == PRESIDENT || Funk3 == PRESIDENT || Funk4 == PRESIDENT);
                }
            }

            public bool IsExternalChiefExecutiveOfficer
            {
                get
                {
                    return (Funk1 == EXTERNAL_CHIEF_EXECUTIVE_OFFICER || Funk2 == EXTERNAL_CHIEF_EXECUTIVE_OFFICER || 
                            Funk3 == EXTERNAL_CHIEF_EXECUTIVE_OFFICER || Funk4 == EXTERNAL_CHIEF_EXECUTIVE_OFFICER);
                }
            }
            #endregion

            #region Methods
            /// <summary>
            /// Get a comma seperated list of board functions. First in the list has to be the president function (OF) and then 
            /// external chiefexecutive officer function (EVD). 
            /// </summary>
            /// <returns></returns>
            public string GetBoardFunctions()
            {                
                List<string> boardFunctionList = new List<string>();

                //Add first the president function if it exists
                if (Funk1 == PRESIDENT)
                    boardFunctionList.Add(Funk1Text);
                else if (Funk2 == PRESIDENT)
                    boardFunctionList.Add(Funk2Text);
                else if (Funk3 == PRESIDENT)
                    boardFunctionList.Add(Funk3Text);
                else if (Funk4 == PRESIDENT)
                    boardFunctionList.Add(Funk4Text);

                //Then add first the external chiefexecutive officer function if it exists
                if (Funk1 == EXTERNAL_CHIEF_EXECUTIVE_OFFICER)
                    boardFunctionList.Add(Funk1Text);
                else if (Funk2 == EXTERNAL_CHIEF_EXECUTIVE_OFFICER)
                    boardFunctionList.Add(Funk2Text);
                else if (Funk3 == EXTERNAL_CHIEF_EXECUTIVE_OFFICER)
                    boardFunctionList.Add(Funk3Text);
                else if (Funk4 == EXTERNAL_CHIEF_EXECUTIVE_OFFICER)
                    boardFunctionList.Add(Funk4Text);

                //Finally add all the other functions
                if (!string.IsNullOrEmpty(Funk1Text) && Funk1 != PRESIDENT && Funk1 != EXTERNAL_CHIEF_EXECUTIVE_OFFICER)
                    boardFunctionList.Add(Funk1Text);
                if (!string.IsNullOrEmpty(Funk2Text) && Funk2 != PRESIDENT && Funk2 != EXTERNAL_CHIEF_EXECUTIVE_OFFICER)
                    boardFunctionList.Add(Funk2Text);
                if (!string.IsNullOrEmpty(Funk3Text) && Funk3 != PRESIDENT && Funk3 != EXTERNAL_CHIEF_EXECUTIVE_OFFICER)
                    boardFunctionList.Add(Funk3Text);
                if (!string.IsNullOrEmpty(Funk4Text) && Funk4 != PRESIDENT && Funk4 != EXTERNAL_CHIEF_EXECUTIVE_OFFICER)
                    boardFunctionList.Add(Funk4Text);

                //Create a comma separated string with the functions
                StringBuilder functionsStringBuilder = new StringBuilder();
                foreach (string function in boardFunctionList)
                {
                    functionsStringBuilder.AppendFormat("{0}, ", function);
                }

                string functions = functionsStringBuilder.ToString().Trim();
                return functions.Remove(functions.LastIndexOf(","));
            }
            #endregion
        }
        #endregion
    }    
}