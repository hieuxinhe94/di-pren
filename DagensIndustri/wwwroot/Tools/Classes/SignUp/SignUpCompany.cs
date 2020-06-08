using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow;

namespace DagensIndustri.Tools.Classes.SignUp
{
    [Serializable]
    public class SignUpCompany
    {
        #region Properties

        public string InvoiceAddress { get; set; }

        public string InvoiceReference { get; set; }

        public string OrgNumber { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        #endregion

        public SignUpCompany(PayWithInvoice companyDetails)
        {
            InvoiceAddress = companyDetails.InvoiceAddress;
            InvoiceReference = companyDetails.InvoiceReference;
            OrgNumber = companyDetails.OrgNumber;
            Zip = companyDetails.Zip;
            City = companyDetails.City;
        }
    }
}