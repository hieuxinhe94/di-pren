using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Tools.Admin.Gasell
{
    public class GasellObjDataSrc
    {
        public DataSet GetGasellPersons(int epiPageId)
        {
            return MsSqlHandler.GetGasellPersons(epiPageId);
        }


        public void UpdateGasellPerson(string firstname, string lastname, string title, string company, string address,
                                            string zipcode, string city, string phone, string mail, string bransch, string employees,
                                            bool gasellCompany, string code, string invoiceAddress, string invoiceZipCode, string invoiceCity,
                                            string invoiceRef, string orgNo, string payInfo, bool canceled, int id)
        {
            MsSqlHandler.UpdateGasellPerson(id, NullToEmptyStr(firstname), NullToEmptyStr(lastname), NullToEmptyStr(title), NullToEmptyStr(company), NullToEmptyStr(address),
                                            NullToEmptyStr(zipcode), NullToEmptyStr(city), NullToEmptyStr(phone), NullToEmptyStr(mail), NullToEmptyStr(bransch), NullToEmptyStr(employees),
                                            gasellCompany, NullToEmptyStr(code), NullToEmptyStr(invoiceAddress), NullToEmptyStr(invoiceZipCode), NullToEmptyStr(invoiceCity),
                                            NullToEmptyStr(invoiceRef), NullToEmptyStr(orgNo), NullToEmptyStr(payInfo), canceled);
        }

        private string NullToEmptyStr(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return s;
        }

        public void DeleteGasellPerson(int id)
        {
            MsSqlHandler.DeleteGasellPerson(id);
        }
    }
}