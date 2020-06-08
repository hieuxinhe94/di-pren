using System;
using System.Collections.Generic;
using Di.Subscription.Logic.Address.Types;
using DIClassLib.Subscriptions;
using EPiServer.Core;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.CustomForms.MySettings;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class PermanentAddressChangePageViewModel : PageViewModel<PermanentAddressChangePage>
    {
        public PermanentAddressChangePageViewModel(IContent currentPage)
            : base((PermanentAddressChangePage)currentPage)
        {
            AddressForm = new AddressFormModel
            {
                HideToDates = true
            };
        }

        public Subscriber Subscriber { get; set; }
        public AddressFormModel AddressForm { get; set; }
        public Message Message { get; set; }
        public DateTime EarliestChangeDate { get; set; }
        public IEnumerable<AddressChange> FuturePermAddresses { get; set; }
    }
}
