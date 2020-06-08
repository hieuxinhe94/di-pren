using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using EPiServer;
using System.Text;
using DIClassLib.Contest;
using DIClassLib.DbHandlers;
using System.Data;
using System.Web.Script.Serialization;

namespace DagensIndustri.Tools.Admin.Contest
{
    public partial class ContestAnswersExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int epiPageId = 0;
            if (!Int32.TryParse(Request.QueryString["epiPageId"], out epiPageId))
            {
                Response.Write("Fel! Parametrar saknas");
                return;
            }

            
            PageData contestPage;
            contestPage = DataFactory.Instance.GetPage(new PageReference(epiPageId));

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Contest.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write("<meta http-equiv=Content-Type content=text/html;charset=UTF-8>");
            /*
            Response.Write("<table>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("Hello");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.End();
            */
            StringBuilder sb = new StringBuilder();
            sb.Append("<table><tr>");
            sb.Append("<th>Kundnr</th>");
            sb.Append("<th>Namn</th>");
            sb.Append("<th>Epost</th>");
            sb.Append("<th>Tel</th>");
            sb.Append("<th>Datum</th>");
            // questions
            //
            List<ContestQuestion> questions = ContestQuestion.GetQuestionsFromContestPage(contestPage);
            foreach (ContestQuestion q in questions)
            {
                sb.Append("<th>" + q.QuestionText + "</th>");
            }
            sb.Append("</tr>");

            // answers
            //

            DataSet ds = ContestDbHandler.GetContestAnswers(epiPageId);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<tr>");
                string cusno = dr["cusno"].ToString();
                string date = dr["date"].ToString();
                sb.Append("<td>" + cusno + "</td>");
                sb.Append("<td>" + dr["name"].ToString() + "</td>");
                sb.Append("<td>" + dr["mail"].ToString() + "</td>");
                sb.Append("<td>" + dr["phone"].ToString() + "</td>");
                sb.Append("<td>" + date + "</td>");
                string answerData = dr["answerData"].ToString();
                List<String> answers = jss.Deserialize<List<String>>(answerData);
                for (int i = 0; i < questions.Count(); ++i)
                {
                    sb.Append("<td>");
                    if (i < answers.Count())
                    {
                        sb.Append(answers[i]);
                    }
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }

            
            sb.Append("</table>");
            Response.Write(sb.ToString());
            Response.End();

        }
    }
}
