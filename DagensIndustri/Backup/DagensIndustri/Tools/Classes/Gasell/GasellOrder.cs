using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DagensIndustri.Tools.Classes.Gasell;
using DagensIndustri.Templates.Public.Pages;
using DIClassLib.DbHandlers;
using System.Text;

namespace DagensIndustri.Tools.Classes.Gasell
{
    [Serializable]
    public class GasellOrder
    {
        #region Properties
        public List<int> OrderIDList;
        public List<GasellUser> GasellUsersList = new List<GasellUser>();
        public GasellCompany Company;
        public string DiscountCode { get; set; }
        public int GasellId { get; set; }
        public string PaymentMethod { get; set; }
        #endregion

        public GasellOrder(List<GasellUser> gasselUsersList, GasellCompany gasellCompany, string discountCode, int gasellId, string paymentMethod)
        {
            GasellUsersList = gasselUsersList;
            DiscountCode = (discountCode == null) ? string.Empty : discountCode;
            Company = gasellCompany;
            GasellId = gasellId;
            PaymentMethod = paymentMethod;

            string paymentMethodName = string.Empty;

            if (PaymentMethod == "1")
                paymentMethodName = "Credit Card - NOT OK";
            else if (PaymentMethod == "2")
                paymentMethodName = "Invoice";
            else if (PaymentMethod == "3")
                paymentMethodName = "Discount Code";

            OrderIDList = new List<int>();     

            if (Company != null)
            {
                foreach (GasellUser gasellUser in GasellUsersList)
                {
                    int i = InsertGasellPerson(gasellId, gasellUser.FirstName, gasellUser.LastName, gasellUser.Title, gasellUser.Company, gasellUser.Address, gasellUser.Zip, gasellUser.City,
                            gasellUser.Phone, gasellUser.Email, gasellUser.Branch, gasellUser.NumberOfEmployees, gasellUser.IsGasellCompany, DiscountCode, Company.InvoiceAddress,
                            Company.Zip, Company.City, Company.InvoiceReference, Company.OrgNumber, paymentMethodName);

                    OrderIDList.Add(i);
                }
            }
            else
            {
                foreach (GasellUser gasellUser in GasellUsersList)
                {
                    int i = InsertGasellPerson(gasellId, gasellUser.FirstName, gasellUser.LastName, gasellUser.Title, gasellUser.Company, gasellUser.Address, gasellUser.Zip, gasellUser.City,
                            gasellUser.Phone, gasellUser.Email, gasellUser.Branch, gasellUser.NumberOfEmployees, gasellUser.IsGasellCompany, DiscountCode, string.Empty,
                            string.Empty, string.Empty, string.Empty, string.Empty, paymentMethodName);

                    OrderIDList.Add(i);
                }
            }
        }

        public int InsertGasellPerson(int gasellId, string firstname, string lastsname, string title, string company, string address,
            string zipcode, string city, string phone, string mail, string bransch, string employees, bool isGasellCompany, string code,
            string invoiceAddress, string invoiceZipCode, string invoiceCity, string invoiceRef, string orgNo, string payInfo)
        {
            return (MsSqlHandler.InsertGasellPerson(gasellId, firstname, lastsname, title, company, address, zipcode, city, phone, mail, bransch, employees,
                                                    isGasellCompany, code, invoiceAddress, invoiceZipCode, invoiceCity, invoiceRef, orgNo, payInfo));
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("EpiPageId" + GasellId + "<br>");
            sb.Append("DiscountCode" + DiscountCode + "<br>");
            sb.Append("PaymentMethod" + PaymentMethod + "<br>");

            if (OrderIDList != null && OrderIDList.Count > 0)
            {
                sb.Append("OrderIDList:");
                foreach (int i in OrderIDList)
                    sb.Append(i.ToString() + ", ");

                sb.Append("<br>");
            }
            
            foreach (GasellUser gu in GasellUsersList)
                sb.Append(gu.ToString());

            if(Company != null)
                sb.Append(Company.ToString());

            return sb.ToString();
        }
    
    }
}