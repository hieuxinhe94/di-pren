namespace Pren.Web.Models.CustomForms.MySettings
{
    public class AddressFormModel
    {
        public AddressFormModel()
        {
            StreetAddressForm = new StreetAddressFormModel();
            //StopOrBoxAddressForm = new StopOrBoxAddressFormModel();
        }

        public StreetAddressFormModel StreetAddressForm { get; set; }
        //public StopOrBoxAddressFormModel StopOrBoxAddressForm { get; set; }
        public string ShowFormLinkText { get; set; }
        public bool HideToDates { get; set; }
        public string SubscriptionNumber { get; set; }
    }
}
