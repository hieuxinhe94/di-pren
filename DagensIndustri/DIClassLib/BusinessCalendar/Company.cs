using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.BusinessCalendar
{
    public class Company
    {

        public int CompanyId;
        public string Name;
        public string Isin;


        public Company(int companyId, string name, string isin)
        {
            CompanyId = companyId;
            Name = name;
            Isin = isin;
        }


    }
}
