using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Subscription
{
    public class Subscription : IDataSetObject
    {
        [DataSet(ColumnName = "PAPER")]
        public string Paper { get; set; }

        [DataSet(ColumnName = "PAPERCODE")]
        public string PaperCode { get; set; }

        [DataSet(ColumnName = "PAPERNAME")]
        public string PaperName { get; set; }

        [DataSet(ColumnName = "PRODUCT")]
        public string Product { get; set; }

        [DataSet(ColumnName = "PRODUCTNO")]
        public string ProductNumber { get; set; }

        [DataSet(ColumnName = "PRODUCTNAME")]
        public string ProductName { get; set; }

        [DataSet(ColumnName = "PACKAGE")]
        public string Package { get; set; }

        [DataSet(ColumnName = "PACKAGEID")]
        public string PackageId { get; set; }

        [DataSet(ColumnName = "PACKAGENAME")]
        public string PackageName { get; set; }

        [DataSet(ColumnName = "SUBSKIND")]
        public string SubscriptionKind { get; set; }

        [DataSet(ColumnName = "SUBSSTARTDATE")]
        public DateTime SubscriptionStartDate { get; set; }

        [DataSet(ColumnName = "INVSTARTDATE")]
        public DateTime InvoiceStartDate { get; set; }

        [DataSet(ColumnName = "SUBSENDDATE")]
        public DateTime SubscriptionEndDate { get; set; }

        [DataSet(ColumnName = "SUSPENDDATE")]
        public DateTime SuspEndDate { get; set; }

        [DataSet(ColumnName = "SUBSSTATE")]
        public string SubscriptionState { get; set; }

        [DataSet(ColumnName = "CANCELREASON")]
        public string CancelReason { get; set; }

        [DataSet(ColumnName = "SOURCE")]
        public string Source { get; set; }

        [DataSet(ColumnName = "SUBSNO")]
        public string SubscriptionNumber { get; set; }

        [DataSet(ColumnName = "SUBSCUSNO")]
        public int SubscriptionCustomerNumber { get; set; }

        [DataSet(ColumnName = "SUBSCUSNAME")]
        public string SubscriptionCustomerName { get; set; }

        [DataSet(ColumnName = "PAYCUSNO")]
        public int PaymerCustomerNumber { get; set; }

        [DataSet(ColumnName = "PAYCUSNAME")]
        public string PayerCustomerName { get; set; }

        [DataSet(ColumnName = "CAMPID")]
        public string CampaignId { get; set; }

        [DataSet(ColumnName = "ORDERID")]
        public string OrderId { get; set; }

        [DataSet(ColumnName = "OTHER_SUBSNO")]
        public string OtherSubscriptionNumber { get; set; }

        [DataSet(ColumnName = "SALESNO")]
        public string SalesNumber { get; set; }

        [DataSet(ColumnName = "ORDER_CUSNO")]
        public int OrderCustomerNumber { get; set; }

        [DataSet(ColumnName = "ORDER_SUBSNO")]
        public int OrderSubscriptionNumber { get; set; }

        [DataSet(ColumnName = "ORDER_EXTNO")]
        public int OrderExtNumber { get; set; }

        [DataSet(ColumnName = "ORDER_MAINPAPER")]
        public string OrderMainPaper { get; set; }

        [DataSet(ColumnName = "PACKAGESUBSNO")]
        public int PackageSubscriptionNumber { get; set; }

        [DataSet(ColumnName = "UNPBREAKDATE")]
        public DateTime UnpBreakDate { get; set; }

        [DataSet(ColumnName = "PRICEGROUP")]
        public string PriceGroup { get; set; }

        [DataSet(ColumnName = "SUBSTYPE")]
        public string SubscriptionType { get; set; }

        [DataSet(ColumnName = "SUBSLEN_MONS")]
        public int SubscriptionLenMons { get; set; }

        [DataSet(ColumnName = "SUBSLEN_DAYS")]
        public int SubscriptionLenDays { get; set; }

        [DataSet(ColumnName = "PAIDUNTILDATE")]
        public DateTime PaidUntilDate { get; set; }

        [DataSet(ColumnName = "EXTSUBSEXISTS")]
        public string ExtSubscriptionExists { get; set; }

        [DataSet(ColumnName = "GIFTSUBS")]
        public string GiftSubscription { get; set; }

        [DataSet(ColumnName = "NOTES")]
        public string Notes { get; set; }

        [DataSet(ColumnName = "ENTRYDATE")]
        public DateTime EntryDate { get; set; }

        [DataSet(ColumnName = "SALESDATE")]
        public DateTime SalesDate { get; set; }

        [DataSet(ColumnName = "TARGETGROUP")]
        public string TargetGroup { get; set; }

        [DataSet(ColumnName = "TARGETGROUP_CODVAL")]
        public string TargetGroupCodVal { get; set; }

        [DataSet(ColumnName = "INVMODE")]
        public string InvoiceMode { get; set; }

        [DataSet(ColumnName = "RECEIVETYPE")]
        public string ReceiveType { get; set; }

        [DataSet(ColumnName = "RECEIVETYPE_CODVAL")]
        public string ReceiveTypeCodVal { get; set; }

        [DataSet(ColumnName = "CRITER1")]
        public string Criteria1 { get; set; }

        [DataSet(ColumnName = "CRITER2")]
        public string Criteria2 { get; set; }

        [DataSet(ColumnName = "CRITER3")]
        public string Criteria3 { get; set; }

        [DataSet(ColumnName = "CRITER4")]
        public string Criteria4 { get; set; }

        [DataSet(ColumnName = "CRITER5")]
        public string Criteria5 { get; set; }

        [DataSet(ColumnName = "CRITER6")]
        public string Criteria6 { get; set; }

        [DataSet(ColumnName = "PRICEGROUP_CODVAL")]
        public string PriceGroupCodVal { get; set; }

        [DataSet(ColumnName = "SUBSTYPE_CODVAL")]
        public string SuscriptionTypeCodVal { get; set; }

        [DataSet(ColumnName = "CAMPNO")]
        public int CampaignNumber { get; set; }

        [DataSet(ColumnName = "CAMPNAME")]
        public string CampaignName { get; set; }

        [DataSet(ColumnName = "CAMPGROUPID")]
        public string CampaignGroupId { get; set; }

        [DataSet(ColumnName = "CAMPGROUPNAME")]
        public string CampaignGroupName { get; set; }

        [DataSet(ColumnName = "SALESNO_CODVAL")]
        public string SalesNumberCodVal { get; set; }

        [DataSet(ColumnName = "SALESGROUP")]
        public string SalesGroup { get; set; }

        [DataSet(ColumnName = "SALESGROUP_CODVAL")]
        public string SalesGroupCodVal { get; set; }

        [DataSet(ColumnName = "MAINPRODUCT")]
        public string MainProduct { get; set; }

        [DataSet(ColumnName = "ITEMQTY")]
        public int ItemQuantity { get; set; }

        [DataSet(ColumnName = "PRICEAMOUNT")]
        public double PriceAmount { get; set; }

        [DataSet(ColumnName = "PRICEAMOUNTINCLUDINGVAT")]
        public double PriceAmountIncludingVat { get; set; }

        [DataSet(ColumnName = "PRICELISTNO")]
        public int PriceListNumber { get; set; }

        [DataSet(ColumnName = "VATAMOUNT")]
        public double VatAmount { get; set; }

        [DataSet(ColumnName = "VATPERCENT")]
        public double VatPercent { get; set; }

        [DataSet(ColumnName = "EXTRAMODE")]
        public string ExtraMode { get; set; }

    }
}
