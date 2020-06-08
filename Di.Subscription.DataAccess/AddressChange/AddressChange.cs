using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.AddressChange
{
    public class AddressChange : IDataSetObject
    {
        [DataSet(ColumnName = "CUSNO")]
        public int CustomerNumber { get; set; }

        [DataSet(ColumnName = "STARTDATE")]
        public DateTime StartDate { get; set; }

        [DataSet(ColumnName = "ENDDATE")]
        public DateTime EndDate { get; set; }

        [DataSet(ColumnName = "CHANGETYPE")]
        public string ChangeType { get; set; }

        [DataSet(ColumnName = "ADDRNO")]
        public int AddrNo { get; set; }

        [DataSet(ColumnName = "DOORNO")]
        public long DoorNo { get; set; }

        [DataSet(ColumnName = "ADDRSTATE")]
        public string AddressState { get; set; }

        [DataSet(ColumnName = "STREET1")]
        public string Street1 { get; set; }

        [DataSet(ColumnName = "STREETNAME")]
        public string StreetAddress { get; set; }

        [DataSet(ColumnName = "HOUSENO")]
        public string StreetNumber { get; set; }

        [DataSet(ColumnName = "STAIRCASE")]
        public string StairCase { get; set; }

        [DataSet(ColumnName = "APARTMENT")]
        public string Apartment { get; set; }

        [DataSet(ColumnName = "STREET2")]
        public string Street2 { get; set; }

        [DataSet(ColumnName = "STREET3")]
        public string Street3 { get; set; }

        [DataSet(ColumnName = "COUNTRYCODE")]
        public string CountryCode { get; set; }

        [DataSet(ColumnName = "ZIPCODE")]
        public string Zip { get; set; }

        [DataSet(ColumnName = "POSTNAME")]
        public string City { get; set; }

        [DataSet(ColumnName = "TEMPNAME1")]
        public string TemporaryName1 { get; set; }

        [DataSet(ColumnName = "TEMPNAME2")]
        public string TemporaryName2 { get; set; }

        [DataSet(ColumnName = "SA_DAYS")]
        public string SaDays { get; set; }

    }
}
