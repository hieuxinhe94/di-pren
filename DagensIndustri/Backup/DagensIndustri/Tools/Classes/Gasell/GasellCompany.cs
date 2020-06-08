using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DagensIndustri.Templates.Public.Units.Placeable.GasellFlow;
using System.Text;

namespace DagensIndustri.Tools.Classes.Gasell
{
    [Serializable]
    public class GasellCompany
    {
        #region Properties

        public string InvoiceAddress { get; set; }

        public string InvoiceReference { get; set; }

        public string OrgNumber { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        #endregion

        public GasellCompany(PayWithInvoice companyDetails)
        {
            InvoiceAddress = companyDetails.InvoiceAddress;
            InvoiceReference = companyDetails.InvoiceReference;
            OrgNumber = companyDetails.OrgNumber;
            Zip = companyDetails.Zip;
            City = companyDetails.City;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("InvoiceAddress:" + InvoiceAddress + "<br>");
            sb.Append("InvoiceReference:" + InvoiceReference + "<br>");
            sb.Append("OrgNumber:" + OrgNumber + "<br>");
            sb.Append("Zip:" + Zip + "<br>");
            sb.Append("City:" + City + "<hr>");
            return sb.ToString();
        }
    }
}