using System;
using System.ComponentModel.DataAnnotations;

namespace Pren.Web.Models.CustomForms.MySettings
{
    public class SubscriptionSleepFormModel
    {
        public DateTime StartDateOrg { get; set; }
        public DateTime EndDateOrg { get; set; }
        public DateTime DateMinAddrChange { get; set; }
        public DateTime DateMaxAddrChange { get; set; }
        public string SubscriptionSleepId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Ongoing { get; set; }
    }
}
