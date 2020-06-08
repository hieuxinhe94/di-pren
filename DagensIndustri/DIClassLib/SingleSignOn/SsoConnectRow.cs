using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.Misc;
using DIClassLib.BonnierDigital;
using DIClassLib.DbHelpers;


namespace DIClassLib.SingleSignOn
{
    [Serializable]
    public class SsoConnectRow
    {
        public string CodeStrFromGui;   //[123][-][XXXX]
        public string CodeFirstFour;    //[XXXX]
        public string TryPopulareErr;

        public int Id;                  //[123]
        public string Code;             //[XXXXYYYYY-YYYYYYYY-YYYYYYYY-YYYYYY]
        public long CirixCusno;
        public string CirixEmail;
        public string PlusToken;
        public string PlusUserId;
        public string PlusFirstName;
        public string PlusLastName;
        public string PlusRemembered;
        public DateTime DateConnAccounts;
        public DateTime DateSaved;
        public string CustomerCode { get { return Id.ToString() + "-" + CodeFirstFour; } }

        private List<string> _cirix_rt1_rt2 = null;
        /// <summary>
        /// Returns: [Comp] [LastName FistName] -- OR -- [LastName FistName] []
        /// </summary>
        public List<string> Cirix_RowText1_RowText2
        {
            get 
            {
                if (_cirix_rt1_rt2 == null)
                {
                    _cirix_rt1_rt2 = new List<string>();
                    _cirix_rt1_rt2.Add("");
                    _cirix_rt1_rt2.Add("");

                    DataSet ds = SubscriptionController.GetCustomer(CirixCusno);
                    if (DbHelpMethods.DataSetHasRows(ds))
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        _cirix_rt1_rt2[0] = dr["ROWTEXT1"].ToString();        //FÖRETAGET             ENAMN FNAMN
                        _cirix_rt1_rt2[1] = dr["ROWTEXT2"].ToString();        //LastName FirstName    ''
                    }
                }

                return _cirix_rt1_rt2;
            }
        }

        public SsoConnectRow() { }

        public SsoConnectRow(DataSet ds)
        {
            SetPropsFromDataSet(ds);
        }

        public SsoConnectRow(long cusno)
        {
            CirixCusno = cusno;
            TryPopulateByCirixCusno();
        }

        private void TryPopulateByCirixCusno()
        {
            DataSet ds = MsSqlHandler.SsoGetCustRow(CirixCusno);
            if (!DbHelpers.DbHelpMethods.DataSetHasRows(ds))
            {
                TryPopulareErr = "Ingen kund hittades för kundnummer: " + CirixCusno.ToString();
                return;
            }

            SetPropsFromDataSet(ds);
        }


        /// <summary>
        /// codeStr format: [int][-][4chrs]
        /// </summary>
        public SsoConnectRow(string codeStrFromGui)
        {
            CodeStrFromGui = MiscFunctions.REC(codeStrFromGui);
            TryPopulateByCodeStr();
        }

        private void TryPopulateByCodeStr()
        {
            TryPopulareErr = ValidateCodeStr();
            if (!string.IsNullOrEmpty(TryPopulareErr))
                return;

            DataSet ds = MsSqlHandler.SsoGetCustRow(Id, CodeFirstFour);
            if (!DbHelpers.DbHelpMethods.DataSetHasRows(ds))
            {
                TryPopulareErr = "Ingen kund hittades för kod: " + Id.ToString() + "-" + CodeFirstFour;
                return;
            }

            SetPropsFromDataSet(ds);
        }

        private string ValidateCodeStr()
        {
            string format = " Giltigt format: 123-ABCD (ett tal, bindestreck, 4 tecken).";

            if (string.IsNullOrEmpty(CodeStrFromGui))
                return "Var god ange kod." + format;

            int idxFirstDash = CodeStrFromGui.IndexOf("-");
            if (idxFirstDash < 1)
                return "Felaktig kod." + format;

            if (idxFirstDash != CodeStrFromGui.LastIndexOf("-"))
                return "Koden får bara innehålla ett bindestreck." + format;

            if (CodeStrFromGui.EndsWith("-"))
                return "Felaktig kod." + format;

            int.TryParse(CodeStrFromGui.Substring(0, idxFirstDash), out Id);
            CodeFirstFour = CodeStrFromGui.Substring(idxFirstDash + 1).Trim();

            if (Id < 1 || CodeFirstFour.Length != 4)
                return "Felaktig kod." + format;

            return string.Empty;
        }

        private void SetPropsFromDataSet(DataSet ds)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            Id = int.Parse(dr["Id"].ToString());
            Code = dr["Code"].ToString();
            CirixCusno = long.Parse(dr["CirixCusno"].ToString());
            CirixEmail = dr["CirixEmail"].ToString();
            PlusToken = dr["PlusToken"].ToString();
            PlusUserId = dr["PlusUserId"].ToString();
            PlusFirstName = dr["PlusFirstName"].ToString();
            PlusLastName = dr["PlusLastName"].ToString();
            PlusRemembered = dr["PlusRemembered"].ToString();
            DateTime.TryParse(dr["DateConnAccounts"].ToString(), out DateConnAccounts);
            DateTime.TryParse(dr["DateSaved"].ToString(), out DateSaved);

            if (!string.IsNullOrEmpty(Code))
                CodeFirstFour = Code.Substring(0, 4);
        }


        //if (string.IsNullOrEmpty(PlusEmail))
        //    PopulateByEmailInPlus();

        //private void PopulateByEmailInPlus()
        //{
        //    string json = RequestHandler.SearchByEmail(CirixEmail);
        //    SearchOutput searchRes = new SearchOutput();
        //    searchRes = searchRes.GetSearchOutput(json);
        //    bool userExists = (int.Parse(searchRes.totalItems) > 0) ? true : false;
        //    if (userExists)
        //    {
        //        PlusEmail = CirixEmail;
        //        PlusCustId = searchRes.users.id;
        //        PlusFirstName = searchRes.users.firstName;
        //        PlusLastName = searchRes.users.lastName;
        //    }
        //}

        
    }
}
