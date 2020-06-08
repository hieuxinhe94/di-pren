using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Package
{
    public class ProductPackage : IDataSetObject
    {
        [DataSet(ColumnName = "PACKAGEID")]
        public string PackageId { get; set; }

        [DataSet(ColumnName = "PACKAGENAME")]
        public string PackageName { get; set; }

        [DataSet(ColumnName = "STARTDATE")]
        public DateTime StartDate { get; set; }

        [DataSet(ColumnName = "ENDDATE")]
        public DateTime EndDate { get; set; }

        [DataSet(ColumnName = "PAPERCODE")]
        public string PaperCode { get; set; }

        [DataSet(ColumnName = "PAPERNAME")]
        public string PaperName { get; set; }

        [DataSet(ColumnName = "PAPERGROUPID")]
        public string PaperGroupId { get; set; }

        [DataSet(ColumnName = "PRODUCTNO")]
        public string ProductNumber { get; set; }

        [DataSet(ColumnName = "MAINPRODUCT")]
        public string MainProduct { get; set; }

        [DataSet(ColumnName = "INVOICINGPRODUCT")]
        public string InvoicingProduct { get; set; }

        [DataSet(ColumnName = "PRODUCTNAME")]
        public string ProductName { get; set; }

        [DataSet(ColumnName = "MO_ACT")]
        public string MondayAct { get; set; }

        [DataSet(ColumnName = "TU_ACT")]
        public string TuesdayAct { get; set; }

        [DataSet(ColumnName = "WE_ACT")]
        public string WednesdayAct { get; set; }

        [DataSet(ColumnName = "TH_ACT")]
        public string ThursdayAct { get; set; }

        [DataSet(ColumnName = "FR_ACT")]
        public string FridayAct { get; set; }

        [DataSet(ColumnName = "SA_ACT")]
        public string SaturdayAct { get; set; }

        [DataSet(ColumnName = "SU_ACT")]
        public string SundayAct { get; set; }

        [DataSet(ColumnName = "SPECIAL")]
        public string Special { get; set; }
    }
}
