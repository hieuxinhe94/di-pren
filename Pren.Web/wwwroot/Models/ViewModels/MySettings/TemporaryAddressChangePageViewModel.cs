using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Di.Subscription.Logic.Address.Types;
using EPiServer.Core;
using Pren.Web.Business.Messaging;
using Pren.Web.Models.CustomForms.MySettings;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class TemporaryAddressChangePageViewModel : PageViewModel<TemporaryAddressChangePage>
    {
        public TemporaryAddressChangePageViewModel(IContent currentPage)
            : base((TemporaryAddressChangePage)currentPage)
        {
            AddressForm = new AddressFormModel();
        }

        public AddressFormModel AddressForm { get; set; }
        public Message Message { get; set; }

        public DateTime EarliestChangeDate { get; set; }
        public IEnumerable<AddressChange> FutureTempAddresses { get; set; }
        public IEnumerable<SelectListItem> TempAddresses { get; set; }
    }
}
