using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DagensIndustri.Templates.Public.Pages.SignUpNoPay
{
    public class PersonName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public PersonName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}