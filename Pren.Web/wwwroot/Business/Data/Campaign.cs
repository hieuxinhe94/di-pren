//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pren.Web.Business.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Campaign
    {
        public string CampaignId { get; set; }
        public int CampaignNumber { get; set; }
        public decimal TotalPriceIncludningVat { get; set; }
        public decimal TotalPriceExcludningVat { get; set; }
        public decimal VatAmount { get; set; }
        public decimal VatPercent { get; set; }
        public string PaperCode { get; set; }
        public string ProductNumber { get; set; }
        public string PackageId { get; set; }
        public System.DateTime LastUpdated { get; set; }
        public string SubsKind { get; set; }
        public string PriceGroup { get; set; }
        public Nullable<decimal> PriceForCustomerToPay { get; set; }
    }
}
