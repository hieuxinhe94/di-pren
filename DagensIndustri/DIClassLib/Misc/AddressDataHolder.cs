using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;


namespace DIClassLib.Misc
{
    
    [Serializable]
    public class AddressDataHolder
    {
        public bool IsStreetAddress { get; set; }
        public string FirstName = "";
        public string LastName = "";
        public string CareOf = "";
        public string Company = "";
        public string StreetName = "";
        public string HouseNum = "";
        public string Staircase = "";
        public string Stairs = "";
        public string ApartmentNo = "";
        public string Zip = "";
        public string City = "";
        public DateTime Date1 = DateTime.MinValue;
        public DateTime Date2 = DateTime.MinValue;

        /// <summary>
        /// 3TR
        /// </summary>
        public string CirixApartment
        {
            get
            {
                if (!string.IsNullOrEmpty(Stairs) && !Stairs.ToUpper().EndsWith("TR"))
                    return Stairs + "TR";

                return string.Empty;
            }
        }
        
        /// <summary>
        /// Care-Of-Address LGH2001
        /// </summary>
        public string CirixStreet2 
        { 
            get
            {
                //return (CareOf + " " + ApartmentNo).Trim();
                StringBuilder sb = new StringBuilder();
                sb.Append(CareOf);

                if (!string.IsNullOrEmpty(ApartmentNo))
                    sb.Append(" LGH" + ApartmentNo);

                return sb.ToString().Trim();
            }
        }


        public AddressDataHolder() { }


        public string GetAddressAsHtml(DIClassLib.Subscriptions.SubscriptionUser2 Subscriber)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Subscriber.RowText1);

            if (Subscriber.IsCompanyCust)
                sb.Append(", " + Subscriber.RowText2);

            sb.Append("<br>");


            if (!string.IsNullOrEmpty(CareOf))
                sb.Append("C/O " + CareOf.ToUpper() + "<br>");


            sb.Append(StreetName.ToUpper());

            if (!string.IsNullOrEmpty(HouseNum))
                sb.Append(" " + HouseNum);

            if (!string.IsNullOrEmpty(Staircase))
                sb.Append(" " + Staircase.ToUpper());

            if (!string.IsNullOrEmpty(Stairs))
                sb.Append(" " + Stairs + "TR");

            if (!string.IsNullOrEmpty(ApartmentNo))
                sb.Append(" LGH" + ApartmentNo);

            sb.Append("<br>");


            sb.Append(Zip + " " + City.ToUpper());

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>Address</b><br>");
            sb.Append("Är gatuadress (ej box): " + IsStreetAddress.ToString() + "<br>");
            sb.Append("Förnamn: " + FirstName + "<br>");
            sb.Append("Efternamn: " + LastName + "<br>");
            sb.Append("CO: " + CareOf + "<br>");
            sb.Append("Företag: " + Company + "<br>");
            sb.Append("Gata: " + StreetName + "<br>");
            sb.Append("Gatunummer: " + HouseNum + "<br>");
            sb.Append("Uppgång: " + Staircase + "<br>");
            sb.Append("Trappor: " + Stairs + "<br>");
            sb.Append("Lägenhetsnummer: " + ApartmentNo + "<br>");
            sb.Append("Postnummer: " + Zip + "<br>");
            sb.Append("Stad: " + City + "<br>");
            sb.Append("Apartment i Cirix: " + CirixApartment + "<br>");
            sb.Append("Street2 i Cirix: " + CirixStreet2 + "<br>");
            sb.Append("Datum 1: " + Date1.ToShortDateString() + "<hr>");
            return sb.ToString();
        }
    }
    
}