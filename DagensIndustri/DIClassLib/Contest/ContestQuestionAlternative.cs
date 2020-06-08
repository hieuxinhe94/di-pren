using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.Contest
{
    public class ContestQuestionAlternative
    {
        public String Id { get; set; }
        public String Text { get; set; }
        public String QuestionId { get; set; }
        public String FormInputName { get; set; }
        public bool IsSelected { get; set; }
    }
}
