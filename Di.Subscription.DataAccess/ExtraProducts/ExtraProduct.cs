using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.ExtraProducts
{
    public class ExtraProduct : IDataSetObject
    {
        [DataSet(ColumnName = "PRODUCTCODE")]
        public string ProductCode { get; set; }

        [DataSet(ColumnName = "PRODUCTNAME")]
        public string ProductName { get; set; }

        [DataSet(ColumnName = "PRICE")]
        public string Price { get; set; }

        [DataSet(ColumnName = "VATCODE")]
        public string VatCode { get; set; }

        [DataSet(ColumnName = "CATEGORY")]
        public string Category { get; set; }

        [DataSet(ColumnName = "ST_RECEIVER")]
        public string StReciever { get; set; }

        [DataSet(ColumnName = "TEXTALLOWED")]
        public string TextAllowed { get; set; }

        [DataSet(ColumnName = "PRICINGTYPE")]
        public string PricingType { get; set; }
    }
}
