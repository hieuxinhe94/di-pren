using System;
using System.Collections.Generic;
using System.Linq;

namespace DIClassLib.Campaign
{
    public class CampaignInfo
    {
        public int CampNo { get; set; }
        public string CampId { get; set; }
        public string CampName { get; set; }
        public string PackageId { get; set; }
        public string PaperCode { get; set; }
        public string ProductNo { get; set; }
        public DateTime CampStartDate { get; set; }
        public DateTime CampEndDate { get; set; }
        public decimal PerDiscount { get; set; }
        public decimal StandDiscount { get; set; }
        public decimal Discpercent { get; set; }
        public decimal TotalPrice { get; set; }
    }
}