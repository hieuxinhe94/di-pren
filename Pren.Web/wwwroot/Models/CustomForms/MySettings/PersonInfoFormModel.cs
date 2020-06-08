using System.ComponentModel.DataAnnotations;

namespace Pren.Web.Models.CustomForms.MySettings
{
    public class PersonInfoFormModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
