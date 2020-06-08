using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.PublicationDays
{
    public class PublicationDay : IDataSetObject
    {
        [DataSet(ColumnName = "ISSUEDATE")]
        public DateTime IssueDate { get; set; }
        [DataSet(ColumnName = "DELIVERYNO")]
        public string DeliveryNumber { get; set; }
        [DataSet(ColumnName = "PRODUCTNO")]
        public string ProductNumber { get; set; }
        [DataSet(ColumnName = "ADDRESSDAY")]
        public string AddressDay { get; set; }
    }
}
