using System.ComponentModel.DataAnnotations;

namespace Pren.Web.Models.CustomForms.MySettings
{
    public class StopOrBoxAddressFormModel
    {
        [Required]
        public string StopOrBoxAddress { get; set; }
        [Required]
        public string StopOrBoxNumber { get; set; }
        [Required]
        public string StopOrBoxZip { get; set; }
        [Required]
        public string StopOrBoxCity { get; set; }
        [Required]
        public string StopOrBoxFromDate { get; set; }
        public string StopOrBoxToDate { get; set; }
    }
}
