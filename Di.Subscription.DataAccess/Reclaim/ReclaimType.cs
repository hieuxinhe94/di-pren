using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Reclaim
{
    public class ReclaimType : IDataSetObject
    {
        [DataSet(ColumnName = "RECLITEM")]
        public int ReclaimItem { get; set; }

        [DataSet(ColumnName = "RECLTEXT")]
        public string ReclaimText { get; set; }

        [DataSet(ColumnName = "LEVELNO")]
        public string LevelNumber { get; set; }

        [DataSet(ColumnName = "ORDERNO")]
        public int OrderNumber { get; set; }

        [DataSet(ColumnName = "RECLKIND")]
        public string ReclaimKind { get; set; }

        [DataSet(ColumnName = "RECLAIMPAPER")]
        public bool ReclaimPaper { get; set; }

        [DataSet(ColumnName = "COMPENSATION")]
        public bool Compensation { get; set; }

        [DataSet(ColumnName = "CARRIERMESSAGE")]
        public bool CarrierMessage { get; set; }

        [DataSet(ColumnName = "AXELWEBCODE")]
        public string AxelWebCode { get; set; }

        [DataSet(ColumnName = "ASO")]
        public bool Aso { get; set; }

        [DataSet(ColumnName = "LEAFNODE")]
        public bool LeafNode { get; set; }

        [DataSet(ColumnName = "KAYAKLITE")]
        public bool KayakLite { get; set; }

        [DataSet(ColumnName = "ADD_PRODUCT")]
        public bool AddProduct { get; set; }

        [DataSet(ColumnName = "ADD_PUBLDATE")]
        public bool AddPublDate { get; set; }

        [DataSet(ColumnName = "ADD_NAME")]
        public bool AddName { get; set; }

        [DataSet(ColumnName = "ADD_ADDRESS")]
        public bool AddAddress { get; set; }
    }
}
