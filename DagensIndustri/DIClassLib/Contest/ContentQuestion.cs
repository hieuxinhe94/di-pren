using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Core;

namespace DIClassLib.Contest
{
    public enum QuestionType
    {
        Alternatives,
        Text
    }


    public class ContestQuestion
    {
        public String Id { get; set; }
        public String FormInputName { get; set; }
        public String QuestionText { get; set; }
        public List<ContestQuestionAlternative> Alternatives { get; set; }
        public QuestionType QuestionType { get; set; }
        public String Answer { get; set; }

        public ContestQuestion()
        {
            Alternatives = new List<ContestQuestionAlternative>();
            QuestionType = QuestionType.Alternatives;
        }

        /// <summary>
        /// Get List of ContestQuestion object from ContestPage properties
        /// </summary>
        /// <param name="contestPage">The page from which to get data</param>
        /// <returns>List of ContestQuestions</returns>
        public static List<ContestQuestion> GetQuestionsFromContestPage(PageData contestPage)
        {
            List<ContestQuestion> questions = new List<ContestQuestion>();
            for (int i = 1; i <= 10; ++i)
            {
                if (contestPage.Property["Question" + i] != null && contestPage.Property["Question" + i].Value != null)
                {
                    ContestQuestion q = new ContestQuestion()
                    {
                        QuestionText = (String)contestPage["Question" + i],
                        FormInputName = "question_" + i,
                        QuestionType = QuestionType.Alternatives
                    };

                    for (int a = 1; a <= 3; ++a)
                    {
                        if (contestPage.Property["Q" + i + "Alt" + a] != null && contestPage.Property["Q" + i + "Alt" + a].Value != null)                        
                        {
                            ContestQuestionAlternative alt = new ContestQuestionAlternative()
                            {
                                Id = i + "_" + a,
                                Text = (String)contestPage["Q" + i + "Alt" + a],
                                QuestionId = q.Id,
                                FormInputName = "question_" + i
                            };

                           
                            q.Alternatives.Add(alt);
                        }
                    }
                    questions.Add(q);
                }

            }
            // Questions with text answers
            //
            for (int i = 1; i <= 3; ++i)
            {
                if (contestPage.Property["TextQuestion" + i] != null && contestPage.Property["TextQuestion" + i].Value != null)                                        
                {
                    ContestQuestion q = new ContestQuestion()
                    {
                        Id = "text_question_" + i.ToString(),
                        QuestionText = (String)contestPage["TextQuestion" + i],
                        FormInputName = "text_question_" + i,
                        QuestionType = QuestionType.Text
                    };
                    questions.Add(q);
                }
            }
            return questions;
        }


    }
}
