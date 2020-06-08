using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Parameters
{
    public class Parameter : IDataSetObject
    {
        [DataSet(ColumnName = "CODEVALUE")]
        public string CodeValue { get; set; }

        [DataSet(ColumnName = "CODENO")]
        public string CodeNumber { get; set; }

        [DataSet(ColumnName = "DESCRIPTION")]
        public string Description { get; set; }
    }
}
