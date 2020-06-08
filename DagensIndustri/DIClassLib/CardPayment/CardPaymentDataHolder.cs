using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.CardPayment
{
    [Serializable]
    public class CardPaymentDataHolder
    {
        public int EpiPageId { get; set; }
        public string DiDepartment { get; set; }
        public string ItemDescr { get; set; }
        public int NumItems { get; set; }
        public bool IsStreetAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneMobile { get; set; }
        public string Email { get; set; }
        public string CareOf { get; set; }
        public string Company { get; set; }
        public string CompanyNum { get; set; }
        public string Street { get; set; }
        public string StreetNum { get; set; }
        public string Entrance { get; set; }
        public string StairsNum { get; set; }
        public string ApartmentNum { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string StopOrBox { get; set; }
        public string StopOrBoxNum { get; set; }
        public PriceCalculator PriceCalc { get; set; }


        public CardPaymentDataHolder(int epiPageId, string diDepartment, string itemDescr, int numItems, bool isStreetAddress, string firstName, string lastName, string phoneMob, string email, PriceCalculator priceCalc)
        {
            EpiPageId = epiPageId;
            DiDepartment = diDepartment;
            ItemDescr = itemDescr;
            NumItems = numItems;
            IsStreetAddress = isStreetAddress;
            FirstName = firstName;
            LastName = lastName;
            PhoneMobile = phoneMob;
            Email = email;
            PriceCalc = priceCalc;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>Kortköpsfakta</b><br>");
            sb.Append("EpiPageId: " + EpiPageId.ToString() + "<br>");
            sb.Append("DiDepartment: " + DiDepartment + "<br>");
            sb.Append("ItemDescr: " + ItemDescr + "<br>");
            sb.Append("NumItems: " + NumItems.ToString() + "<br>");
            sb.Append("IsStreetAddress: " + IsStreetAddress.ToString() + "<br>");
            sb.Append("FirstName: " + FirstName + "<br>");
            sb.Append("LastName: " + LastName + "<br>");
            sb.Append("PhoneMobile: " + PhoneMobile + "<br>");
            sb.Append("Email: " + Email + "<br>");
            sb.Append("CareOf: " + CareOf + "<br>");
            sb.Append("Company: " + Company + "<br>");
            sb.Append("CompanyNum: " + CompanyNum + "<br>");
            sb.Append("Street: " + Street + "<br>");
            sb.Append("StreetNum: " + StreetNum + "<br>");
            sb.Append("Entrance: " + Entrance + "<br>");
            sb.Append("StairsNum: " + StairsNum + "<br>");
            sb.Append("ApartmentNum: " + ApartmentNum + "<br>");
            sb.Append("Zip: " + Zip + "<br>");
            sb.Append("City: " + City + "<br>");
            sb.Append("StopOrBox: " + StopOrBox + "<br>");
            sb.Append("StopOrBoxNum: " + StopOrBoxNum + "<hr>");

            return sb.ToString();
        }
    }
}
