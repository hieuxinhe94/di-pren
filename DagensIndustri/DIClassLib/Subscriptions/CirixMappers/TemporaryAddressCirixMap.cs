using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DIClassLib.DbHelpers;

namespace DIClassLib.Subscriptions.CirixMappers
{
    [Serializable]
    public class TemporaryAddressCirixMap
    {

        public long Cusno { get; set; }
        public string Addrno { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Doorno { get; set; }
        public string BasicAddr { get; set; }
        public string AddrState { get; set; }
        public string Street1 { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string StairCase { get; set; }
        public string Apartment { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public string ZipCode { get; set; }
        public string PostName { get; set; }
        public string CountryCode { get; set; }


        public TemporaryAddressCirixMap(DataRow dr)
        {
            try
            {
                Cusno = long.Parse(dr["CUSNO"].ToString());
                Addrno = dr["ADDRNO"].ToString();
                StartDate = (DateTime)dr["STARTDATE"];
                EndDate = (DateTime)dr["ENDDATE"];
                Doorno = dr["DOORNO"].ToString();
                BasicAddr = dr["BASICADDR"].ToString();
                AddrState = dr["ADDRSTATE"].ToString();
                Street1 = dr["STREET1"].ToString();
                StreetName = dr["STREETNAME"].ToString();
                HouseNo = dr["HOUSENO"].ToString();
                StairCase = dr["STAIRCASE"].ToString();
                Apartment = dr["APARTMENT"].ToString();
                Street2 = dr["STREET2"].ToString();
                Street3 = dr["STREET3"].ToString();
                ZipCode = dr["ZIPCODE"].ToString();
                PostName = dr["POSTNAME"].ToString();
                CountryCode = dr["COUNTRYCODE"].ToString();
            }
            catch (Exception ex)
            {
                new Logger("TemporaryAddressCirixMap failed for cusno=" + Cusno, ex.ToString());
            }

        }

    }
}
