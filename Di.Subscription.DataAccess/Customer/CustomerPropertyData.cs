using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Customer
{
    public class CustomerPropertyData: IDataSetObject
    {
        [DataSet(ColumnName = "PROPERTYCODE")]
        public string PropertyCode { get; set; }

        [DataSet(ColumnName = "PROPERTYNAME")]
        public string PropertyName { get; set; }

        [DataSet(ColumnName = "PROPERTYTYPE")]
        public string PropertyType { get; set; }

        [DataSet(ColumnName = "READONLY")]
        public string ReadOnly { get; set; }

        [DataSet(ColumnName = "USERVALUE")]
        public string UserValue { get; set; }
    }
}
