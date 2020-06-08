using System;
using System.Text;

namespace Pren.Web.Models.CustomForms
{
    public class BusinessSubscriptionActivationFormModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BizSubscriptionId { get; set; }

        public string ToLogString()
        {
            var sb = new StringBuilder();

            foreach (var propertyInfo in typeof(BusinessSubscriptionActivationFormModel).GetProperties())
            {
                sb.AppendLine(String.Format("{0}: '{1}'", propertyInfo.Name, propertyInfo.GetValue(this, null) ?? "[NULL]"));
            }

            return sb.ToString();
        }
    }    
}
