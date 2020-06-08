using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Core;

namespace DIClassLib.Competition
{
    public class CompetitionUtility
    {
        public static List<string> GetAnswers(PageData page)
        {
            var answers = new List<string>();

            if (page["Answers"] != null)
            {
                foreach (var answer in page["Answers"].ToString().Split('#'))
                    answers.Add(answer);
            }

            return answers;
        }
    }
}
