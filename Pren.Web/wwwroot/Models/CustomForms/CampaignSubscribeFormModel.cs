using System;
using System.Text;

namespace Pren.Web.Models.CustomForms
{
    [Serializable]
    public class CampaignSubscribeFormModel
    {
        public int CampaignContentId { get; set; }
        public string ExtraInfo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Ssn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string StreetNo { get; set; }
        public string StairCase { get; set; }
        public string Stairs { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Co { get; set; }

        public string Company { get; set; }

        public string OriginalInfo { get; set; }
        public string OriginalInfoInvoice { get; set; }        

        public string PrenStart { get; set; }

        public string FirstNameDigital { get; set; }
        public string LastNameDigital { get; set; }

        public string CompanyInvoice { get; set; }
        public string AttentionInvoice { get; set; }
        public string SsnInvoice { get; set; }
        //public string FirstNameInvoice { get; set; }
        //public string LastNameInvoice { get; set; }
        public string StreetAddressInvoice { get; set; }
        public string StreetNoInvoice { get; set; }
        //public string StairCaseInvoice { get; set; }
        //public string StairsInvoice { get; set; }
        public string ZipInvoice { get; set; }
        public string CityInvoice { get; set; }
        public string PhoneInvoice { get; set; }

        public long CampNo { get; set; }
        public string CampId { get; set; }
        public string PaymentMethod { get; set; }
        public bool InvoiceOtherPayer { get; set; }
        public string TargetGroup { get; set; }

        public bool IsDigital { get; set; }
        public bool IsStudent { get; set; }
        public bool IsTrial { get; set; }
        public bool IsTrialFree { get; set; }
        public bool IsPayWall { get; set; }
        public bool IsServicePlusUser { get; set; }

        public string ToLogString()
        {
            var sb = new StringBuilder();

            foreach (var propertyInfo in typeof(CampaignSubscribeFormModel).GetProperties())
            {
                sb.AppendLine(String.Format("{0}: '{1}'", propertyInfo.Name, propertyInfo.GetValue(this, null) ?? "[NULL]"));
            }

            return sb.ToString();
        }
    }
}
