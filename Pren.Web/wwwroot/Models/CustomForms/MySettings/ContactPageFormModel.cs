using System.Web;

namespace Pren.Web.Models.CustomForms.MySettings
{
    public class ContactPageFormModel 
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public string CustomerNumber { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
    }
}
