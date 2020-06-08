using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.Misc
{
    public class AddCustsToCirixReturn
    {
        
        public long CampNo { get; set; }
        public string CampId { get; set; }

        public long Subsno { get; set; }

        public long SubscriberCusno { get; set; }
        public long PayerCusno { get; set; }

        public string ErrorDetails { get; set; }

        public enum ErrorTypes
        {
            //person related
            FirstName = 100,
            LastName = 101,
            PhoneMobile = 102,
            PhoneDayTime = 103,
            Email = 104,
            StreetName = 105,
            ZipCode = 106,

            //campaign related
            CampNotPopulated = 200,

            //cirix related
            SaveToCirixFailed = 300
        }

        /// <summary>
        /// List of errors: FirstName=100, LastName=101, PhoneMobile=102, PhoneDayTime=103, Email=104, StreetName=105, ZipCode=106, CampNotPopulated=200, SaveToCirixFailed=300
        /// </summary>
        public List<int> Errors = new List<int>();


        public AddCustsToCirixReturn()
        { 
            
        }


        //sb.Append("FirstName missing<br>");
        //sb.Append("LastName missing<br>");
        //sb.Append("MobilePhone not valid Swedish format (start with +46 and contain 7-20 chars)<br>");
        //sb.Append("Email not valid<br>");
        //sb.Append("StreetName missing<br>");
        //sb.Append("ZipCode not valid Swedish format (numeric and 5 chars)<br>");
        //
        //retObj.Message = "Campaign was not populated. Check that campaign exists in Cirix for provided campId/campNo";
    }
}
