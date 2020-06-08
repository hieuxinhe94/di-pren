namespace Di.Subscription.Logic.Campaign.Types
{
    public class Campaign
    {
        public int CampaignNumber { get; set; }
        public string CampaignId { get; set; }
        public decimal TotalPriceIncludningVat { get; set; }
        public decimal TotalPriceExcludningVat { get; set; }
        public decimal PriceForCustomerToPay { get; set; }
        public decimal PriceExludingVatForCustomerToPay { get; set; }
        public decimal VatAmount { get; set; }
        public decimal VatPercent { get; set; }
        public string PaperCode { get; set; }
        public string ProductNumber { get; set; }
        public string PackageId { get; set; }
        public string SubsKind { get; set; }
        public string PriceGroup { get; set; }
        public bool SubsKindOngoingEnabled { get; set; }
        public bool SubsKindTimedEnabled { get; set; }
    }
}
