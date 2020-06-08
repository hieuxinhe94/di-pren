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
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch
{
    public partial class CompanyFinancialDetails : UserControlBase
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
        protected XmlDocument AnnualAccountsXmlDocument { get; set; }
        protected string AnnualAccountsXml
        {
            get
            {
                return ViewState["AnnualAccountsXml"] as string;
            }
            set
            {
                ViewState["AnnualAccountsXml"] = value;
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadAnnualAccounts();
            DataBind();

            this.Visible = AnnualAccountsXmlDocument.HasChildNodes;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load xml document for annual accounts and populate necessary fields
        /// </summary>
        private void LoadAnnualAccounts()
        {
            AnnualAccountsXmlDocument = new XmlDocument();

            if (string.IsNullOrEmpty(Request.QueryString["worksiteid"]) || DocumentFactory == null)
                return;

            try
            {
                //If it is not a postback or xml stored in viewstate is empty (due to prior error), retrieve the xml with a webservice call. 
                //Otherwise, just load the xml from viewstate.
                if (!IsPostBack || string.IsNullOrEmpty(AnnualAccountsXml))
                {
                    RequestParameter[] parameters = new RequestParameter[] { new RequestParameter() };

                    parameters[0].name = "worksiteId";
                    parameters[0].value = Request.QueryString["worksiteid"];

                    string xml = DocumentFactory.getDocument(ConfigurationManager.AppSettings["OBOAnnualAccounts"], parameters);
                    AnnualAccountsXmlDocument.LoadXml(xml);
                    AnnualAccountsXml = xml;
                }
                else
                {
                    AnnualAccountsXmlDocument.LoadXml(AnnualAccountsXml);
                }

                PopulateAnnualAccounts();
            }
            catch (Exception ex)
            {
                new Logger("CompanyFinancialDetails LoadAnnualAccounts() - failed", ex.ToString());
                CompanyDetailsPage.ShowMessage("/common/errors/error", true, true);
            }
        }

        private void PopulateAnnualAccounts()
        {
            string translateStart = "/contactcompanysearch/companydetails/companyfinancialdetails/";
            AnnualAccountsTable.Rows.Add(CreateTableHeaderRow(string.Concat(translateStart, "annualaccountingstart"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod", "start"));
            AnnualAccountsTable.Rows.Add(CreateTableHeaderRow(string.Concat(translateStart, "annualaccountingend"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod", "stop"));

            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "numberofemployees"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/NumberOfEmployees"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "sharecapital"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/ShareCapital"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "netsales"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/IncomeStatement/NetSales"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "changeinnetsales"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/ChangeInNetSales"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "profitafterfinanceincomeandexpense"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/IncomeStatement/ProfitAfterFinanceIncomeAndExpense"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "profitlossfortheyear2"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/ProfitLossForTheYear2"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "totalequityandliabilities"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/TotalEquityAndLiabilities"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "profitmargin"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/ProfitMargin"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "returnonequity"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/BalanceSheet/ReturnOnEquity"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "quickratio"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/QuickRatio"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "equityassetsratio"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/EquityAssetsRatio"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "totalequity"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/TotalEquity"));
            AnnualAccountsTable.Rows.Add(CreateTableRow(string.Concat(translateStart, "totalliabilities"), "Bizbook-Show-Bsl/BslMainBlock/AccountingPeriod/EquityAndLiabilities/TotalLiabilities"));
        }

        /// <summary>
        /// Create a table row in the header section of the table, containing of header cells and add data from a specific xpath to it
        /// </summary>
        /// <param name="firstCellText"></param>
        /// <param name="xpath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private TableRow CreateTableHeaderRow(string firstCellText, string xpath, string attributeName)
        {
            TableRow tableRow = new TableRow();
            tableRow.TableSection = TableRowSection.TableHeader;
            TableHeaderCell tableCell = new TableHeaderCell();
            tableCell.Text = Translate(firstCellText);
            tableRow.Cells.Add(tableCell);

            foreach (XmlNode accountingPeriodNode in AnnualAccountsXmlDocument.SelectNodes(xpath))
            {
                tableCell = new TableHeaderCell();
                tableCell.Text = MiscFunctions.GetXmlAttributeText(accountingPeriodNode, attributeName);
                tableRow.Cells.Add(tableCell);
            }

            return tableRow;
        }

        /// <summary>
        /// Create a table row in the body section of the table, containing of cells and add data from a specific xpath to it
        /// </summary>
        /// <param name="firstCellText"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private TableRow CreateTableRow(string firstCellText, string xpath)
        {
            TableRow tableRow = new TableRow();
            tableRow.TableSection = TableRowSection.TableBody;
            TableCell tableCell = new TableCell();
            tableCell.Text = Translate(firstCellText);
            tableRow.Cells.Add(tableCell);

            foreach (XmlNode xmlNode in AnnualAccountsXmlDocument.SelectNodes(xpath))
            {
                tableCell = new TableCell();
                tableCell.Text = MiscFunctions.GetXmlAttributeText(xmlNode, "value");
                tableRow.Cells.Add(tableCell);
            }

            return tableRow;
        }
        #endregion

    }
}