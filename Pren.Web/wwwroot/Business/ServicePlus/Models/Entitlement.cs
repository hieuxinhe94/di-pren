using System;
using Pren.Web.Business.ServicePlus.Logic;

namespace Pren.Web.Business.ServicePlus.Models
{
    public class Entitlement
    {
        public EntitlementState State { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public long SubscriberId { get; set; }
        public DateTime? Updated { get; set; }
    }
}
