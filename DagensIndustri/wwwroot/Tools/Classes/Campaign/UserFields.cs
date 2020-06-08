using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DagensIndustri.Tools.Classes.Campaign
{
    public class UserFields
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String CoAddress { get; set; }
        public String Company { get; set; }
        public String StreetAddress { get; set; }
        public String StreetNumber { get; set; }
        public String Door { get; set; }
        public String Stairs { get; set; }
        public String Apartment { get; set; }
        public String Zip { get; set; }
        public String City { get; set; }
        public String Telephone { get; set; }
        public String Email { get; set; }

        public static void StoreInSession(UserFields uf)
        {
            HttpContext.Current.Session["USER_FIELDS"] = uf;
        }

        public static UserFields GetFromSession()
        {
            return HttpContext.Current.Session["USER_FIELDS"] as UserFields;
        }
    }
}