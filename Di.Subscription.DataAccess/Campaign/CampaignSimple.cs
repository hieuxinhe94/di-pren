using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Campaign
{
    public class CampaignSimple : IDataSetObject
    {
        [DataSet(ColumnName = "PAPERCODE")]
        public string PaperCode { get; set; }

        [DataSet(ColumnName = "CAMPID")]
        public string CampaignId { get; set; }

        [DataSet(ColumnName = "CAMPNAME")]
        public string CampaignName { get; set; }

        [DataSet(ColumnName = "CAMPSTARTDATE")]
        public DateTime CampaignStartDate { get; set; }

        [DataSet(ColumnName = "CAMPENDDATE")]
        public DateTime CampaignEndDate { get; set; }

        [DataSet(ColumnName = "PACKAGEID")]
        public string PackageId { get; set; }

        [DataSet(ColumnName = "CAMPNO")]
        public int CampaignNumber { get; set; }

        [DataSet(ColumnName = "PRODUCTNO")]
        public string ProductNumber { get; set; }

        [DataSet(ColumnName = "PRICEGROUP")]
        public string PriceGroup { get; set; }

        [DataSet(ColumnName = "SUBSLENGTH")]
        public int SubscriptionLength { get; set; }

        [DataSet(ColumnName = "PRICELISTNO")]
        public int PriceListNumber { get; set; }

        [DataSet(ColumnName = "PRICEDETTYPE")]
        public string PriceDetType { get; set; }

        [DataSet(ColumnName = "STARTDATE_FRST")]
        public DateTime StartDateFirst { get; set; }

        [DataSet(ColumnName = "STARTDATE_LAST")]
        public DateTime StartDateLast { get; set; }

        [DataSet(ColumnName = "SUBSENDDATE")]
        public DateTime SubscriptionEndDate { get; set; }

        [DataSet(ColumnName = "LENGTHUNIT")]
        public string LengthUnit { get; set; }

        [DataSet(ColumnName = "STAND_ITEMQTY")]
        public int StandItemQuantity { get; set; }

        [DataSet(ColumnName = "PER_ITEMQTY")]
        public int PerItemQuantity { get; set; }

        [DataSet(ColumnName = "NOTES")]
        public string Notes { get; set; }

        [DataSet(ColumnName = "DISCTYPE")]
        public string DiscountType { get; set; }

        [DataSet(ColumnName = "DISCPERCENT")]
        public decimal DiscountPercent { get; set; }

        [DataSet(ColumnName = "UNPBREAK_INCR")]
        public int UnpBreakIncr { get; set; }

        [DataSet(ColumnName = "SAMPLE")]
        public string Sample { get; set; }

        [DataSet(ColumnName = "FREESUBS")]
        public string FreeSubs { get; set; }

        [DataSet(ColumnName = "VATINCLUDED")]
        public string VatIncluded { get; set; }

        [DataSet(ColumnName = "SUBSCRITER1")]
        public string SubsCriteria1 { get; set; }

        [DataSet(ColumnName = "SUBSCRITER2")]
        public string SubsCriteria2 { get; set; }

        [DataSet(ColumnName = "SUBSCRITER3")]
        public string SubsCriteria3 { get; set; }

        [DataSet(ColumnName = "SUBSCRITER4")]
        public string SubsCriteria4 { get; set; }

        [DataSet(ColumnName = "SUBSCRITER5")]
        public string SubsCriteria5 { get; set; }

        [DataSet(ColumnName = "SUBSCRITER6")]
        public string SubsCriteria6 { get; set; }

        [DataSet(ColumnName = "SUBSTYPE")]
        public string SubsType { get; set; }

        [DataSet(ColumnName = "PRICEGUARANTEE")]
        public string PriceGuarantee { get; set; }

        [DataSet(ColumnName = "CAMPGROUPNAME")]
        public string CampaignGroupName { get; set; }

        [DataSet(ColumnName = "SUBSKIND")]
        public string SubsKind { get; set; }

        [DataSet(ColumnName = "PERIOD_MM")]
        public int PeriodMm { get; set; }

        [DataSet(ColumnName = "ISSUEQTY")]
        public int IssueQuantity { get; set; }

        [DataSet(ColumnName = "PERIODAMOUNT")]
        public double PeriodAmount { get; set; }

        [DataSet(ColumnName = "VATAMOUNT")]
        public double VatAmount { get; set; }

        [DataSet(ColumnName = "PERAMOUNTWITHVAT")]
        public double PerAmountWithVat { get; set; }

        [DataSet(ColumnName = "ITEMQTY")]
        public int ItemQuantity { get; set; }

        [DataSet(ColumnName = "VATPRCENT")]
        public int VatPercent { get; set; }
                
    }
}
