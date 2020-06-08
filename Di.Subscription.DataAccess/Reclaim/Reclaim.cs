using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Reclaim
{
    public class Reclaim : IDataSetObject
    {
        [DataSet(ColumnName = "RECLAIMNO")]
        public int ReclaimNumber { get; set; }

        [DataSet(ColumnName = "RECLDATE")]
        public DateTime ReclaimDate { get; set; }

        [DataSet(ColumnName = "PUBLDATE")]
        public DateTime PublDate { get; set; }

        [DataSet(ColumnName = "PAPERCODE")]
        public string PaperCode { get; set; }

        [DataSet(ColumnName = "PAPERNAME")]
        public string PaperName { get; set; }

        [DataSet(ColumnName = "DELIVERYTYPE")]
        public string DeliveryType { get; set; }

        [DataSet(ColumnName = "BUNDLENO")]
        public int BundleNumber { get; set; }

        [DataSet(ColumnName = "BUNDLENAME")]
        public string BundleName { get; set; }

        [DataSet(ColumnName = "STACKNO")]
        public int StackNumber { get; set; }

        [DataSet(ColumnName = "STACKNAME")]
        public string StackName { get; set; }

        [DataSet(ColumnName = "NAME1")]
        public string Name1 { get; set; }

        [DataSet(ColumnName = "NAME2")]
        public string Name2 { get; set; }

        [DataSet(ColumnName = "STREET1")]
        public string Street1 { get; set; }

        [DataSet(ColumnName = "STREET2")]
        public string Street2 { get; set; }

        [DataSet(ColumnName = "COUNTRYCODE")]
        public string CountryCode { get; set; }

        [DataSet(ColumnName = "ZIPCODE")]
        public string ZipCode { get; set; }

        [DataSet(ColumnName = "POSTNAME")]
        public string PostName { get; set; }

        [DataSet(ColumnName = "RECLPAPER")]
        public string ReclaimPaper { get; set; }

        [DataSet(ColumnName = "RECLPAPERNAME")]
        public string ReclaimPaperName { get; set; }

        [DataSet(ColumnName = "RECLMESG")]
        public string ReclaimMessage { get; set; }

        [DataSet(ColumnName = "RECLCODE")]
        public string ReclaimCode { get; set; }

        [DataSet(ColumnName = "RECLTEXT")]
        public string ReclaimText { get; set; }

        [DataSet(ColumnName = "PRINTED")]
        public string Printed { get; set; }

        [DataSet(ColumnName = "PRINTDATE")]
        public DateTime PrintDate { get; set; }

        [DataSet(ColumnName = "MSGSENT")]
        public string MessageSent { get; set; }

        [DataSet(ColumnName = "DELMESSAGENO")]
        public int DelMessageNumber { get; set; }

        [DataSet(ColumnName = "DELMESGDATE")]
        public DateTime DelMessgeDate { get; set; }

        [DataSet(ColumnName = "DOORNO")]
        public long DoorNumber { get; set; }

        [DataSet(ColumnName = "SUBSNO")]
        public long SubscriptionNumber { get; set; }

        [DataSet(ColumnName = "EXTNO")]
        public int ExternalNumber { get; set; }

        [DataSet(ColumnName = "MARK")]
        public string Mark { get; set; }

        [DataSet(ColumnName = "RECLITEM")]
        public int ReclaimItem { get; set; }

        [DataSet(ColumnName = "RECLITEMNAME")]
        public string ReclaimItemName { get; set; }

        [DataSet(ColumnName = "RECLKIND")]
        public int ReclaimKind { get; set; }

        [DataSet(ColumnName = "RECLCHNL")]
        public string ReclaimChannel { get; set; }

        [DataSet(ColumnName = "RECLCHNLTEXT")]
        public string ReclaimChannelText { get; set; }

        [DataSet(ColumnName = "RESPPERSON")]
        public string RespPerson { get; set; }

        [DataSet(ColumnName = "REQUESTTEXT")]
        public string RequestText { get; set; }

        [DataSet(ColumnName = "ACTIONTEXT")]
        public string ActionText { get; set; }

        [DataSet(ColumnName = "COMPLETED")]
        public string Completed { get; set; }

        [DataSet(ColumnName = "COMMUNECODE")]
        public string CommuneCode { get; set; }

        [DataSet(ColumnName = "COMDELPAPER")]
        public string ComdelPaper { get; set; }

        [DataSet(ColumnName = "CREDITSUBSCRIBER")]
        public string CreditSubscriber { get; set; }

        [DataSet(ColumnName = "CREDITDAYS")]
        public decimal CreditDays { get; set; }

        [DataSet(ColumnName = "CREDITMONEY")]
        public decimal CreditMoney { get; set; }
    }
}
