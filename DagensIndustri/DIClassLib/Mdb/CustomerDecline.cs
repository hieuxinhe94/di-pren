using System;

namespace DIClassLib.Mdb
{
    /// <summary>
    /// Based on table abmt_t_S2 in ActionBase database
    /// </summary>
    public class CustomerDecline
    {
        public long TpId { get; set; }
        public string CampaignId { get; set; }
        public string Outcome { get; set; }
        public DateTime DateTime { get; set; }
        public string Product { get; set; }
        public string Reason { get; set; }
        public string SalesCompany { get; set; }
        public string FileName { get; set; }
    }
}