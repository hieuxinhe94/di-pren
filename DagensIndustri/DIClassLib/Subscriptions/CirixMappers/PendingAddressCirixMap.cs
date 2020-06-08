using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DIClassLib.DbHelpers;

namespace DIClassLib.Subscriptions.CirixMappers
{
    [Serializable]
    public class PendingAddressCirixMap
    {

        public string ChangeType { get; set; }
        public long Cusno { get; set; }
        public string Addrno { get; set; }
        public string Doorno { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AddrState { get; set; }
        public string Street1 { get; set; }
        public string StreetName { get; set; }
        public string Houseno { get; set; }
        public string Staircase { get; set; }
        public string Apartment { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public string Postname { get; set; }


        public PendingAddressCirixMap(DataRow dr)
        {
            try
            {
                ChangeType = dr["CHANGETYPE"].ToString();
                Cusno = long.Parse(dr["CUSNO"].ToString());
                Addrno = dr["ADDRNO"].ToString();
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
                //<SA_DAYS>YYYYYYN</SA_DAYS>
            }
            catch (Exception ex)
            {
                new Logger("PendingAddressCirixMap() failed for cusno=" + Cusno, ex.ToString());
            }
        }
    }
}
