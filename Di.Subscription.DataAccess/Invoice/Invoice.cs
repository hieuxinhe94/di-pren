using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Invoice
{
    public class Invoice : IDataSetObject
    {
        [DataSet(ColumnName = "INVNO")]
        public int InvoiceNumber { get; set; }

        [DataSet(ColumnName = "SUBSNO")]
        public long SubscriptionNumber { get; set; }

        [DataSet(ColumnName = "INVDATE")]
        public DateTime InvoiceDate { get; set; }

        [DataSet(ColumnName = "EXPDATE")]
        public DateTime ExpirationDate { get; set; }

        [DataSet(ColumnName = "INVTYPE")]
        public string InvoiceType { get; set; }

        [DataSet(ColumnName = "CODEVALUE")]
        public string CodeValue { get; set; }

        [DataSet(ColumnName = "INVAMOUNT")]
        public decimal InvoiceAmount { get; set; }

        [DataSet(ColumnName = "VATAMOUNT")]
        public decimal VatAmount { get; set; }

        [DataSet(ColumnName = "INVTEXT")]
        public string InvoiceText { get; set; }

        [DataSet(ColumnName = "OPENAMOUNT")]
        public decimal OpenAmount { get; set; }

        [DataSet(ColumnName = "EXPENSES")]
        public float Expenses { get; set; }

        [DataSet(ColumnName = "INTEREST")]
        public float Interest { get; set; }

        [DataSet(ColumnName = "INVCLASS")]
        public string InvoiceClass { get; set; }

        [DataSet(ColumnName = "PAPERCODE")]
        public string PaperCode { get; set; }

        [DataSet(ColumnName = "REFERENCENO")]
        public string ReferenceNumber { get; set; }

        [DataSet(ColumnName = "INVSTATE")]
        public string InvoiceState { get; set; }
    }
}
