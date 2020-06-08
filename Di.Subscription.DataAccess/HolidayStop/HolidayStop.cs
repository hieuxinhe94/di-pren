using System;
using Di.Common.Conversion;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.HolidayStop
{
    public class HolidayStop : IDataSetObject
    {
        [DataSet(ColumnName = "SUBSNO")]
        public long SubscriptionNumber { get; set; }

        [DataSet(ColumnName = "SLEEPSTARTDATE")]
        public DateTime StartDate { get; set; }

        [DataSet(ColumnName = "SLEEPENDDATE")]
        public DateTime EndDate { get; set; }

        [DataSet(ColumnName = "CREDITTYPE")]
        public string CreditType { get; set; }

        [DataSet(ColumnName = "CREDITAMOUNT")]
        public decimal CreditAmount { get; set; }

        [DataSet(ColumnName = "SLEEPTYPE")]
        public string SleepType { get; set; }

        [DataSet(ColumnName = "CREDITED")]
        public string Credited { get; set; }

        [DataSet(ColumnName = "CREDIT_INVNO")]
        public int CreditInvoiceNumber { get; set; }

        [DataSet(ColumnName = "BOOKINGDATE")]
        public DateTime BookingDate { get; set; }

        [DataSet(ColumnName = "ALLOW_WEBPAPER")]
        public string AllowWebPaper { get; set; }

        [DataSet(ColumnName = "SLEEPREASON")]
        public string SleepReason { get; set; }

        [DataSet(ColumnName = "RECEIVETYPE")]
        public string RecieveType { get; set; }

        [DataSet(ColumnName = "STAMP_USER")]
        public string StampUser { get; set; }  
    }
}
