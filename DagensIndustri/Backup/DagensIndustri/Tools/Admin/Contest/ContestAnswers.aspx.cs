using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;
using EPiServer.DataAbstraction;
using EPiServer;
using EPiServer.Filters;
using EPiServer.Core;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.Contest;
using System.Text;

namespace DagensIndustri.Tools.Admin.Contest
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Tävlingssvar", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Tävlingssvar", UrlFromUi = "/Tools/Admin/Contest/ContestAnswers.aspx", SortIndex = 2050)]
    public partial class ContestAnswers : System.Web.UI.Page
    {
        protected String ErrorMessage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                DataBind();
        }

        protected PageDataCollection ContestPages
        {
            get
            {
                PropertyCriteriaCollection criterias = new PropertyCriteriaCollection();
                PropertyCriteria criteria = new PropertyCriteria();
                criteria.Condition = CompareCondition.Equal;
                criteria.Name = "PageTypeID";
                criteria.Type = PropertyDataType.PageType;
                criteria.Value = PageType.Load("[DI Gold] Contest").ID.ToString();
                criteria.Required = true;
                criterias.Add(criteria);

                PageDataCollection contests = DataFactory.Instance.FindPagesWithCriteria(PageReference.RootPage, criterias);
                return contests;
            }
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            AnswersGrid.DataBind();
        }

        protected void ExportToExcelButton_Click(object sender, EventArgs e)
        {
            if (ContestsDropDownList.SelectedIndex == -1)
            {
                ErrorMessage = "Välj tävling innan du exporterar";
                error_message.DataBind();
                return;
            }

            int epiPageId = Convert.ToInt32(ContestsDropDownList.SelectedValue);
            PageData contestPage;
            contestPage = DataFactory.Instance.GetPage(new PageReference(epiPageId));

            //WriteExcelToResponse(contestPage,Answers);
            Response.Redirect("ContestAnswersExcel.aspx?epiPageId=" + epiPageId);

                
        }

        private void WriteExcelToResponse(PageData contestPage,DataSet answers)
        {

        }

        protected DataSet Answers
        {
            get
            {
                int? epiPageId = null;
                if(ContestsDropDownList.SelectedIndex > -1)
                {
                    epiPageId = Convert.ToInt32(ContestsDropDownList.SelectedValue);
                }
                return ContestDbHandler.GetContestAnswers(epiPageId);
            }
        }

        
        
    }
}