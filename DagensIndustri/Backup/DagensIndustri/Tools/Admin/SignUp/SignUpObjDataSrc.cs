using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Tools.Admin.SignUp
{
    public class SignUpObjDataSrc
    {
        public DataSet GetSignUpPersons(int epiPageId)
        {
            return MsSqlHandler.GetSignUpPersons(epiPageId);
        }


        public void UpdateSignUpPerson(string cusno, string epiPageId, string payMethod, string firstName, string lastName, string address,
                                            string zip, string city, string phone, string email, bool canceled, int Id)
        {
            MsSqlHandler.UpdateSignUpPerson(Id, cusno, epiPageId, NullToEmptyStr(payMethod), NullToEmptyStr(firstName), NullToEmptyStr(lastName), NullToEmptyStr(address), 
                                            NullToEmptyStr(zip), NullToEmptyStr(city), NullToEmptyStr(phone), NullToEmptyStr(email), canceled);
        }

        private string NullToEmptyStr(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return s;
        }

        public void DeleteSignUpPerson(int id)
        {
            MsSqlHandler.DeleteSignUpPerson(id);
        }

    }
}