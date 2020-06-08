using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Customer
{
    public class CustomerSimple : IDataSetObject
    {
        [DataSet(ColumnName = "CUSNO")]
        public long CustomerNumber { get; set; }

        [DataSet(ColumnName = "CUSSTATE")]
        public string CustomerState { get; set; }

        [DataSet(ColumnName = "ROWTEXT1")]
        public string RowText1 { get; set; }

        [DataSet(ColumnName = "ROWTEXT2")]
        public string RowText2 { get; set; }

        [DataSet(ColumnName = "ROWTEXT3")]
        public string RowText3 { get; set; }

        [DataSet(ColumnName = "STREET1")]
        public string Street1 { get; set; }

        [DataSet(ColumnName = "HOUSENO")]
        public string HouseNumber { get; set; }

        [DataSet(ColumnName = "STREET2")]
        public string Street2 { get; set; }

        [DataSet(ColumnName = "STREET3")]
        public string Street3 { get; set; }

        [DataSet(ColumnName = "COUNTRYCODE")]
        public string CountryCode { get; set; }

        [DataSet(ColumnName = "ZIPCODE")]
        public string ZipCode { get; set; }

        [DataSet(ColumnName = "POSTNAME")]
        public string PostName { get; set; }

        [DataSet(ColumnName = "H_PHONE")]
        public string HomePhone { get; set; }

        [DataSet(ColumnName = "W_PHONE")]
        public string WorkPhone { get; set; }

        [DataSet(ColumnName = "O_PHONE")]
        public string OfficePhone { get; set; }

        [DataSet(ColumnName = "EMAILADDRESS")]
        public string EmailAddress { get; set; }

        [DataSet(ColumnName = "PROTECTEDIDENTITY")]
        public string ProtectedIdentity { get; set; }

        [DataSet(ColumnName = "TRAFFICLIGHT")]
        public int TrafficLight { get; set; }

        [DataSet(ColumnName = "PRODSTRING")]
        public string ProdString { get; set; }

        [DataSet(ColumnName = "ADDRSTARTDATE")]
        public DateTime AddressStartDate { get; set; }

        [DataSet(ColumnName = "ADDRENDDATE")]
        public DateTime AddressEndDate { get; set; }

        [DataSet(ColumnName = "SOCIALSECNO")]
        public string SocialSecurityNumber { get; set; }

        [DataSet(ColumnName = "ACCOUNTNO")]
        public string AccountNumber { get; set; }

    }
}
