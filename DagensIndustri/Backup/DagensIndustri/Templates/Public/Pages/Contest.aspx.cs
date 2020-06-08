using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes;
using System.Web.Script.Serialization;
using DIClassLib.Contest;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

using EPiServer.XForms.Implementation;

using Microsoft.VisualBasic.Logging;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class Contest : DiTemplatePage
    {
        protected List<ContestQuestion> Questions { get; set; }
        protected String ErrorMessage { get; set; }
        private bool IsValid { get; set; }

        public string UrlCode
        {
            get
            {
                var code = Request.QueryString["code"];

                if (code == null)
                    return string.Empty;

                code = MiscFunctions.REC(code).Trim();

                try   { new Guid(code); }
                catch { return string.Empty; }

                return code;
            }
        }
        public long Cusno { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            LoadQuestions();
        }

        
        private void LoadQuestions()
        {
            IsValid = true;
            Questions = ContestQuestion.GetQuestionsFromContestPage(CurrentPage);
            foreach (ContestQuestion q in Questions)
            {
                String answer = Request.Form[q.FormInputName];
                if (String.IsNullOrEmpty(answer) )
                {
                    IsValid = false;

                }
                q.Answer = answer;
                if (q.QuestionType == QuestionType.Alternatives)
                {
                    int selectedIndex = -1;
                    if (Int32.TryParse(answer, out selectedIndex))
                    {
                        --selectedIndex;
                        if (selectedIndex > -1 && selectedIndex < q.Alternatives.Count)
                        {
                            q.Alternatives[selectedIndex].IsSelected = true;
                        }
                    }
                }
 
                //        Response.Write(q.Id + ": " + answer + "<br/>");

            }

            #region questions with alternatives
            /*
            // Questions with alternatives
            //
            for (int i = 1; i <= 10; ++i)
            {
                if (EPiFunctions.HasValue(CurrentPage, "Question" + i))
                {
                    ContestQuestion q = new ContestQuestion()
                    {
                        Id = i.ToString(),
                        QuestionText = (String)CurrentPage["Question" + i],
                        FormInputName = "question_" + i,
                        QuestionType = QuestionType.Alternatives
                    };

                    for (int a = 1; a <= 3; ++a)
                    {
                        if (EPiFunctions.HasValue(CurrentPage, "Q" + i + "Alt" + a))
                        {
                            ContestQuestionAlternative alt = new ContestQuestionAlternative()
                            {
                                Id = i + "_" + a,
                                Text = (String)CurrentPage["Q" + i + "Alt" + a],
                                QuestionId = q.Id,
                                FormInputName = "question_" + i
                            };
                            q.Alternatives.Add(alt);
                        }
                    }
                    Questions.Add(q);
                }

                
            }
            // Questions with text answers
            //
            for (int i = 1; i <= 3; ++i)
            {
                if (EPiFunctions.HasValue(CurrentPage, "TextQuestion" + i))
                {
                    ContestQuestion q = new ContestQuestion()
                    {
                        Id = "text_question_" + i.ToString(),
                        QuestionText = (String)CurrentPage["TextQuestion" + i],
                        FormInputName = "text_question_" + i,
                        QuestionType = QuestionType.Text
                    };
                    Questions.Add(q);
                }
            }
             */
            #endregion
        }

        protected void SubmitContestButton_Click(object sender, EventArgs e)
        {
            LoadQuestions();

            if (!IsValid)
            {
                ErrorMessage = "Var god svara på alla frågor.";
                QuestionRepeater.DataBind();
                server_error_div.DataBind();
            }
            else
            {
                SetCustProps();

                List<String> answers = Questions.Select(x => x.Answer).ToList();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                String jsonAnswers = jss.Serialize(answers);

                ContestDbHandler.InsertContestAnswer(CurrentPage.PageLink.ID, Cusno, Name, Mail, Phone, jsonAnswers);

                ContestMultiView.SetActiveView(ThankYouView);
            }
           
        }

        private void SetCustProps()
        {
            //code in url
            if (!string.IsNullOrEmpty(UrlCode))
            {
                var ds = ContestDbHandler.GetContestCusnoByCode(new Guid(UrlCode));
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    long tmp;
                    if (long.TryParse(ds.Tables[0].Rows[0]["cusno"].ToString(), out tmp))
                        Cusno = tmp;
                }
            }
            else  //no code in url, try get cusno from logged in user (if user is logged in)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    Cusno = MembershipDbHandler.GetCusno(HttpContext.Current.User.Identity.Name);
            }

            if (Cusno > 0)
            {
                try
                {
                    var ds = CirixDbHandler.GetCustomer(Cusno);
                    if (DbHelpMethods.DataSetHasRows(ds))
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        Name = (dr["ROWTEXT1"].ToString() + " " + dr["ROWTEXT2"].ToString()).Trim();
                        Mail = dr["EMAILADDRESS"].ToString();
                        Phone = dr["O_PHONE"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    new Logger("SetCustProps() failed for cusno: " + Cusno, ex.ToString());
                }
                
            }
        }

    }

}