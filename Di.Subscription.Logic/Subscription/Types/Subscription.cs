using System;

namespace Di.Subscription.Logic.Subscription.Types
{
    public class Subscription
    {
        public long SubscriptionNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string PriceGroup { get; set; }

        public string SubscriptionState { get; set; }

        public string SubscriptionKind { get; set; }

        public string PackageId { get; set; }

        public string PaperCode { get; set; }

        public string ProductNumber { get; set; }

        public int ExternalNumber { get; set; }
    }
}
