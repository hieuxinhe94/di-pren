using System.Collections.Generic;
using EPiServer.Core;
using Pren.Web.Business.Subscription;
using Pren.Web.Controllers.MySettings;

namespace Pren.Web.Models.Partials.MySettings
{
    public class MySettingsPageMenu
    {

        public IEnumerable<IContent> MenuPages { get; set; }

        public ContentReference CurrentPageReference { get; set; }

        public List<SubscriptionMenuItem> SubscriptionMenuItems { get; set; }

        public string ServicePlusEmail { get; set; }
        public string CustomerNumber { get; set; }

        public bool IsPrivateSubscriber { get; set; }
        public bool IsBusinessAdminSubscriber { get; set; }
        public SubscriptionType SelectedSubscriptionType { get; set; }

        public string ChangePasswordUrl { get; set; }

    }
}