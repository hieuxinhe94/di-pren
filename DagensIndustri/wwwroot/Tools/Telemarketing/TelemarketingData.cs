using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DIClassLib.Subscriptions;

namespace DagensIndustri.Tools.Telemarketing
{
    public class TelemarketingData
    {
        public bool EmailExistAtServicePlus { get; set; }

        public string ServicePlusUserId { get; set; }

        public List<Subscription> CustomerSubscriptions { get; set; }

        public CirixCustomerSearchResult CirixCustomerSearchResult { get; set; }

        public IEnumerable<ProductConfig> ProductConfig { get; set; }

        public long ExternalSubscriberId { get; set; }
    }
}