using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pren.Web.Models.CustomForms.MySettings
{
    public class StreetAddressFormModel
    {
        public string Co { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        public string StreetNo { get; set; }
        public string StairCase { get; set; }
        public string Stairs { get; set; }
        public string ApartmentNumber { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("<b>Address</b><br>");
            sb.Append("CO: " + Co + "<br>");
            sb.Append("Gata: " + StreetAddress + "<br>");
            sb.Append("Gatunummer: " + StreetNo + "<br>");
            sb.Append("Uppgång: " + StairCase + "<br>");
            sb.Append("Trappor: " + Stairs + "<br>");
            sb.Append("Lägenhetsnummer: " + ApartmentNumber + "<br>");
            sb.Append("Postnummer: " + Zip + "<br>");
            sb.Append("Stad: " + City + "<br>");
            sb.Append("Fråndatum: " + FromDate.ToShortDateString() + "<br>");
            sb.Append("Tilldatum: " + ToDate.ToShortDateString() + "<hr>");
            return sb.ToString();
        }
    }
}
