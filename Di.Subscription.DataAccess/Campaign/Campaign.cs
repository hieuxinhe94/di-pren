using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Campaign
{
    public class Campaign : IDataSetObject
    {

        [DataSet(ColumnName = "CAMPNO")]
        public int CampaignNumber { get; set; }

        [DataSet(ColumnName = "CAMPID")]
        public string CampaignId { get; set; }

        [DataSet(ColumnName = "CAMPSTARTDATE")]
        public DateTime CampaignStartDate { get; set; }

        [DataSet(ColumnName = "PRICEDETTYPE")]
        public string PriceDetType { get; set; }

        [DataSet(ColumnName = "CAMPNAME")]
        public string CampaignName { get; set; }

        [DataSet(ColumnName = "CAMPENDDATE")]
        public DateTime CampaignEndDate { get; set; }

        [DataSet(ColumnName = "STARTDATE_FRST")]
        public DateTime StartDateFirst { get; set; }

        [DataSet(ColumnName = "STARTDATE_LAST")]
        public DateTime StartDateLast { get; set; }

        [DataSet(ColumnName = "SUBSENDDATE")]
        public DateTime SubscriptionEndDate { get; set; }

        [DataSet(ColumnName = "LENGTHUNIT")]
        public string LengthUnit { get; set; }

        [DataSet(ColumnName = "SUBSLENGTH")]
        public int SubscriptionLength { get; set; }

        [DataSet(ColumnName = "STAND_ITEMQTY")]
        public int StandItemQuantity { get; set; }

        [DataSet(ColumnName = "PER_ITEMQTY")]
        public int PerItemQuantity { get; set; }

        [DataSet(ColumnName = "EXT_CAMPNO")]
        public int ExternalCampaignNumber { get; set; }

        [DataSet(ColumnName = "SUBSCHNL_DIR")]
        public string SubsChnlDir { get; set; }

        [DataSet(ColumnName = "SUBSCHNL_SAL")]
        public string SubsChnlSal { get; set; }

        [DataSet(ColumnName = "REMINDERQTY")]
        public int ReminderQuantity { get; set; }

        [DataSet(ColumnName = "COUNTIN_CONT")]
        public string CountingCont { get; set; }

        [DataSet(ColumnName = "COUNTIN_ORDB")]
        public string CountingOrdb { get; set; }

        [DataSet(ColumnName = "COUNTIN_CIRC")]
        public string CountingCirc { get; set; }

        [DataSet(ColumnName = "TOTALPRICE")]
        public decimal TotalPrice { get; set; }

        [DataSet(ColumnName = "ADDRSRCNOTES")]
        public string AddressSrcNotec { get; set; }

        [DataSet(ColumnName = "INVOICINGTYPE")]
        public string InvoicingType { get; set; }

        [DataSet(ColumnName = "DUNQTY")]
        public int DunQuantity { get; set; }

        [DataSet(ColumnName = "STAND_XTRAWKS")]
        public int StandExtraWks { get; set; }

        [DataSet(ColumnName = "PER_XTRAWKS")]
        public int PerExtraWks { get; set; }

        [DataSet(ColumnName = "XTRAINSUBS")]
        public string ExtraInSubs { get; set; }

        [DataSet(ColumnName = "XTRAINITEM")]
        public string ExtraInItem { get; set; }

        [DataSet(ColumnName = "STANDDISCOUNT")]
        public decimal StandDiscount { get; set; }

        [DataSet(ColumnName = "PERDISCOUNT")]
        public decimal PerDiscount { get; set; }

        [DataSet(ColumnName = "DISCINSUBS")]
        public string DiscInSubs { get; set; }

        [DataSet(ColumnName = "DELIMTYPE")]
        public string DelimType { get; set; }

        [DataSet(ColumnName = "INVTEXT")]
        public string InvoiceText { get; set; }

        [DataSet(ColumnName = "INVDELAY")]
        public int InvoiceDelay { get; set; }

        [DataSet(ColumnName = "NOTES")]
        public string Notes { get; set; }

        [DataSet(ColumnName = "CAMPGROUPID")]
        public string CampaignGroupId { get; set; }

        [DataSet(ColumnName = "COPRODINCL")]
        public string CoProdIncl { get; set; }

        [DataSet(ColumnName = "CAMPINVINCL")]
        public string CampaignInvIncl { get; set; }

        [DataSet(ColumnName = "STICKERQTY")]
        public int StickerQuantity { get; set; }

        [DataSet(ColumnName = "EXTRADAYS")]
        public int ExtraDays { get; set; }

        [DataSet(ColumnName = "STEXACTITEMS")]
        public string StExactItems { get; set; }

        [DataSet(ColumnName = "PEREXACTITEMS")]
        public string PerExactItems { get; set; }

        [DataSet(ColumnName = "XTRADAYS_ON_INV")]
        public string ExtraDaysOnInvoice { get; set; }

        [DataSet(ColumnName = "XTRALENUNIT")]
        public string ExtraLengthUnit { get; set; }

        [DataSet(ColumnName = "INVSTARTDATE")]
        public DateTime InvoiceStartDate { get; set; }

        [DataSet(ColumnName = "DISCTYPE")]
        public string DiscountType { get; set; }

        [DataSet(ColumnName = "DISCPERCENT")]
        public decimal DiscountPercent { get; set; }

        [DataSet(ColumnName = "DUNINV")]
        public string DunInvoice { get; set; }

        [DataSet(ColumnName = "DUN1_TEXT")]
        public string Dun1Text { get; set; }

        [DataSet(ColumnName = "DUN1_EXPDTEINCR")]
        public int Dun1ExpdteIncr { get; set; }

        [DataSet(ColumnName = "DUN2_TEXT")]
        public string Dun2Text { get; set; }

        [DataSet(ColumnName = "DUN2_EXPDTEINCR")]
        public int Dun2ExpdIncr { get; set; }

        [DataSet(ColumnName = "UNPBREAK_INCR")]
        public int UnpBreakIncr { get; set; }

        [DataSet(ColumnName = "EXT_CAMP_PERC")]
        public int ExtCampaignPercent { get; set; }

        [DataSet(ColumnName = "EXT_CAMPNO2")]
        public int ExtCampaignNumber2 { get; set; }

        [DataSet(ColumnName = "EXT_CAMP2_PERC")]
        public int ExtCampaignPercent2 { get; set; }

        [DataSet(ColumnName = "EXT_CAMPNO3")]
        public int ExtCampaignNumber3 { get; set; }

        [DataSet(ColumnName = "EXT_CAMP3_PERC")]
        public int ExtCampaignPercent3 { get; set; }

        [DataSet(ColumnName = "EXT_CAMPNO4")]
        public int ExtCampaignNumber4 { get; set; }

        [DataSet(ColumnName = "EXT_CAMP4_PERC")]
        public int ExtCampaignPercent4 { get; set; }

        [DataSet(ColumnName = "EXT_CAMPNO5")]
        public int ExtCampaignNumber5 { get; set; }

        [DataSet(ColumnName = "EXT_CAMP5_PERC")]
        public int ExtCampaignPercent5 { get; set; }

        [DataSet(ColumnName = "ALT_INVOICE_REQUEST")]
        public string AltInvoiceRequest { get; set; }

        [DataSet(ColumnName = "ALT_SUBSKIND")]
        public string AltSubsKind { get; set; }

        [DataSet(ColumnName = "ALT_SUBSLEN")]
        public int AltSubsLen { get; set; }

        [DataSet(ColumnName = "ALT_SUBSPRICEGROUP")]
        public string AltSubsPriceGroup { get; set; }

        [DataSet(ColumnName = "ALT_SUBSITEMQTY")]
        public int AltSubsItemQuantity { get; set; }

        [DataSet(ColumnName = "ALT_CAMPNO")]
        public int AltCampaignNumber { get; set; }

        [DataSet(ColumnName = "NORM_EXPDTEINCR")]
        public int NormExpteIncr { get; set; }

        [DataSet(ColumnName = "SPECIAL_CAMP")]
        public string SpecialCampaign { get; set; }

        [DataSet(ColumnName = "PACKAGEID")]
        public string PackageId { get; set; }

        [DataSet(ColumnName = "PRICELISTNO")]
        public int PriceListNumber { get; set; }

        [DataSet(ColumnName = "SUBSSLEEP_CREDIT_ALLOWED")]
        public string SubsSleepCreditAllowed { get; set; }

        [DataSet(ColumnName = "SAMPLE")]
        public string Sample { get; set; }

        [DataSet(ColumnName = "FREESUBS")]
        public string FreeSubs { get; set; }

        [DataSet(ColumnName = "CANCELREASON")]
        public string CancelReason { get; set; }

        [DataSet(ColumnName = "VATPERCENT")]
        public decimal VatPercent { get; set; }

        [DataSet(ColumnName = "VATINCLUDED")]
        public string VatIncluded { get; set; }

        [DataSet(ColumnName = "ALLOWDAYS")]
        public int AllowDays { get; set; }

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

        [DataSet(ColumnName = "PRICEGROUP")]
        public string PriceGroup { get; set; }
    }
}
