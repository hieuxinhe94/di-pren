using System;
using System.Web;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHandlers;
using DIClassLib.Membership;
using DIClassLib.Misc;

namespace DagensIndustri
{
    public partial class tmp2 : DiTemplatePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IssueUtil issueDI = new IssueUtil(HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiGold), Settings.PaperCode_DI, Settings.ProductNo_Regular, DateTime.Now);
            litOutput.Text = issueDI.NextIssueDate.ToString();
            //var kh = new KayakHandler();
            //var pc = "DI";
            //var pr = "01";
            //var issue = EnumIssue.Issue.InDateOrFirstAfterInDate;
            //var d = new DateTime(2015, 2, 1);
            //var issuDateCirix = CirixDbHandler.GetIssueDate(pc, pr, d, issue);
            //var issueDateKayak = kh.GetIssueDate(pc, pr,d ,issue);
            //litOutput.Text = string.Format("Använder Cirix: {0}<br />Cirix: {1} == Kayak: {2}", Settings.UseCirixHandler, issuDateCirix,
            //    issueDateKayak);
        }
    }
}