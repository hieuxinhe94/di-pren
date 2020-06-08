using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DIClassLib.Subscriptions.CirixMappers
{
    [Serializable]
    public class AddressMap : IComparable<AddressMap>
    {

        public string Id { get { return Addrno + "_" + Doorno + "_" + StartDate.ToString() + "_" + EndDate.ToString(); } }
        //public string AddressOneLine { get { return StreetName; } }

        public string BasicAddr { get; set; }       //Y / N
        public string ChangeType { get; set; }      //Settings.AdressChangeType_Current
        public string SaDays { get; set; }          //YYYYYYN
        
        public long Cusno { get; set; }
        public int Addrno { get; set; }             //1 for perm address
        public string Doorno { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AddrState { get; set; }
        public string Street1 { get; set; }         //Adamsgatan 4B 3TR
        public string StreetName { get; set; }      //Adamsgatan
        public string Houseno { get; set; }         //39
        public string Staircase { get; set; }       //B
        public string Apartment { get; set; }       //2TR
        public string Street2 { get; set; }         //co-address LGH2001
        public string Street3 { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public string Postname { get; set; }
      
        public bool CanBeEdited
        {
            get
            {
                if (ChangeType == Settings.AdressChangeType_Splitted)
                    return false;

                return !IsInProgress;
                //return true;
            }
        }
        public bool CanBeDeleted
        {
            get
            {
                if (ChangeType == Settings.AdressChangeType_Splitted)
                    return false;

                return !IsInProgress;
                //return true;
            }
        }
        
        private string _comment = string.Empty;
        public string Comment 
        {
            get
            {
                _comment = string.Empty;
                
                if (ChangeType == Settings.AdressChangeType_Splitted)
                    _comment = "Delad adress. ";

                if (IsInProgress)
                    _comment += "Period påbörjad. ";

                if (_comment.Length > 0)
                    _comment += "Kontakta kundtjänst för ändringar.";

                //if (!CanBeEdited && !CanBeDeleted)
                //    _comment += "Kontakta kundtjänst för ändringar.";
                
                return _comment;
            }
            set 
            {
                _comment = value;
            }
        }
        public bool HasComment
        {
            get { return (Comment.Length > 0); }
        }
        
        private bool IsInProgress
        {
            get
            {
                DateTime now = DateTime.Now.Date;
                return (now >= StartDate && now <= EndDate);
            }
        }



        public AddressMap() { }

        /// <summary>
        /// constructor for dummy objects
        /// </summary>
        public AddressMap(string changeType, string saDays, long cusno, int addrno, string doorno, DateTime startDate, DateTime endDate, 
                            string addrState, string street1, string streetName, string houseno, string staircase, string apartment, string street2,
                            string street3, string countryCode, string zipCode, string postname) 
        { 
            ChangeType = changeType;
            SaDays = saDays;
            Cusno = cusno;
            Addrno = addrno;
            Doorno = doorno;
            StartDate = startDate;
            EndDate = endDate;
            AddrState = addrState;
            Street1 = street1;
            StreetName = streetName;
            Houseno = houseno;
            Staircase = staircase;
            Apartment = apartment;
            Street2 = street2;
            Street3 = street3;
            CountryCode = countryCode;
            ZipCode = zipCode;
            Postname = postname;
        }

        public AddressMap(DataRow dr)
        {
            
            //try
            //{
                if (dr.Table.Columns.Contains("BASICADDR"))
                    BasicAddr = dr["BASICADDR"].ToString();

                if (dr.Table.Columns.Contains("CHANGETYPE"))
                    ChangeType = dr["CHANGETYPE"].ToString();

                if (dr.Table.Columns.Contains("SA_DAYS"))
                    SaDays = dr["SA_DAYS"].ToString();

                Cusno = long.Parse(dr["CUSNO"].ToString());
                Addrno = int.Parse(dr["ADDRNO"].ToString());
                Doorno = dr["DOORNO"].ToString();
                StartDate = (DateTime)dr["STARTDATE"];
                EndDate = (DateTime)dr["ENDDATE"];
                AddrState = dr["ADDRSTATE"].ToString();
                Street1 = dr["STREET1"].ToString();
                StreetName = dr["STREETNAME"].ToString();
                Houseno = dr["HOUSENO"].ToString();
                Staircase = dr["STAIRCASE"].ToString();
                Apartment = dr["APARTMENT"].ToString();
                Street2 = dr["STREET2"].ToString();
                Street3 = dr["STREET3"].ToString();
                CountryCode = dr["COUNTRYCODE"].ToString();
                ZipCode = dr["ZIPCODE"].ToString();
                Postname = dr["POSTNAME"].ToString();

                Comment = "";
            //}
            //catch (Exception ex)
            //{
            //    new Logger("AddressMap() failed for cusno=" + Cusno, ex.ToString());
            //}
        }


        /// <summary>
        /// used when List<AddressMap>.Sort() is called
        /// </summary>
        public int CompareTo(AddressMap other)
        {
            return StartDate.CompareTo(other.StartDate);  //ASC
            //return -StartDate.CompareTo(other.StartDate);   //DESC
        }

        public bool AreEqual(AddressDataHolder dh)
        {
            if (CompareStrings(dh.CirixStreet2, Street2) &&       //co-address LGH2001
                CompareStrings(dh.StreetName, StreetName) &&      //Adamsgatan
                CompareStrings(dh.HouseNum, Houseno) &&           //39
                CompareStrings(dh.Staircase, Staircase) &&        //B
                CompareStrings(dh.CirixApartment, Apartment) &&   //2TR
                CompareStrings(dh.Zip, ZipCode) &&                //12345
                CompareStrings(dh.City, Postname))                //staden
                return true;

            return false;
        }

        public bool AreEqual(AddressMap am)
        {
            if (CompareStrings(am.Street2, Street2) &&          //co-address LGH2001
                CompareStrings(am.StreetName, StreetName) &&    //Adamsgatan
                CompareStrings(am.Houseno, Houseno) &&          //39
                CompareStrings(am.Staircase, Staircase) &&      //B
                CompareStrings(am.Apartment, Apartment) &&      //2TR
                CompareStrings(am.ZipCode, ZipCode) &&          //12345
                CompareStrings(am.Postname, Postname))          //staden
                return true;

            return false;
        }

        private bool CompareStrings(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1))
                s1 = "";

            if (string.IsNullOrEmpty(s2))
                s2 = "";

            return (s1.ToUpper().Trim() == s2.ToUpper().Trim());
        }

    }

}
