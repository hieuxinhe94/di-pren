using System;

namespace Pren.Web.Models.CustomForms
{
    [Serializable]
    public class BusinessCampaignSubscribeFormModel
    {
        public string Email { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string StreetAddress { get; set; }
        public string StreetNo { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }

        public string BizSubscriptionDefinitionId { get; set; }
        public string CampaignNumber { get; set; }

        public string ServicePlusUserId { get; set; }
    }
}
