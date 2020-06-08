using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using System.Data;


namespace DIClassLib.Misc
{
    [Serializable]
    public class CampaignUser
    {
        public CampaignUser(string activeTab, string firstName, string lastName, string streetName, string streetNo, string entrance,
                            string stairs, string apartment, string zip, string city, string phone, string email, string co, string birthNo,
                            string company, string orgNo, OfferCode offerCode, string targetGroup, bool isSubscriber)
        {
            ActiveTab = activeTab;
            FirstName = firstName.ToUpper();
            LastName = lastName.ToUpper();
            StreetName = streetName.ToUpper();
            HouseNo = streetNo.ToUpper();
            Staircase = entrance.ToUpper();
            Apartment = !string.IsNullOrEmpty(stairs) && !stairs.Trim().ToUpper().EndsWith("TR")
                ? stairs + "TR"
                : stairs;
            ApartmentNo = apartment.ToUpper();
            ZipCode = zip.ToUpper();
            City = city.ToUpper();
            OtherPhone = phone.ToUpper();
            Email = email.ToLower();
            CareOf = co.ToUpper();
            BirthNo = birthNo.ToUpper();
            Company = company.ToUpper();
            OrgNo = orgNo.ToUpper();
            IsSubscriber = isSubscriber;

            CirixName1 = (LastName + " " + FirstName).Trim();
            //TODO: No attention field in form            
            //CirixName2 = Attention.Trim();
            CirixName2 = string.Empty;
            CirixStreet2 = (CareOf + " " + ApartmentNo).Trim();

            OfferCode = offerCode;
            TargetGroup = targetGroup;
        }

        public string CirixName1 { get; set; }
        public string CirixName2 { get; set; }
        public string CirixStreet2 { get; set; }

        public bool IsSubscriber { get; set; }

        public CampaignUser Payer { get; set; }

        public string TargetGroup { get; set; }

        public OfferCode OfferCode { get; set; }

        public string ActiveTab { get; set; }

        public DateTime DateStart
        {
            get
            {
                DateTime subsMinDate = SubscriptionController.GetNextIssueDateIncDiRules(DateTime.Now.Date, OfferCode.PaperCode, OfferCode.ProductNo);

                if (OfferCode.CampStartDate != OfferCode.StartDateFirst)
                {
                    if (subsMinDate > OfferCode.StartDateFirst)
                        return subsMinDate;

                    return OfferCode.StartDateFirst;
                }

                return subsMinDate;
            }
        }

        public DateTime DateEnd
        {
            get
            {
                if (OfferCode.SubsLength > 0)
                {
                    if (OfferCode.SubsLengthUnit.Equals("WW"))
                        return DateStart.AddDays(OfferCode.SubsLength * 7);

                    return DateStart.AddMonths(OfferCode.SubsLength);
                }

                //If OfferCode.SubsLength is 0. Then we must set DateEnd to SubsEndDate collected from OfferCode.
                //This occurs for some campaigns. Example, "Betala 399:- så får du DI Weekend varje helg resten av året".
                return OfferCode.SubsEndDate;
            }
        }

        public long CusNo { get; set; }

        public long SubsNo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StreetName { get; set; }

        public string HouseNo { get; set; }

        public string Staircase { get; set; }

        public string Apartment { get; set; }

        public string ApartmentNo { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string OtherPhone { get; set; }

        public string Email { get; set; }

        public string CareOf { get; set; }

        public string BirthNo { get; set; }

        public string Company { get; set; }

        public string OrgNo { get; set; }

        //use for logging
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (IsSubscriber)
                sb.Append("<b>Prenumerant</b><br>");
            else
                sb.Append("<b>Person som ska betala prenumerationen</b><br>");

            sb.Append("FirstName: " + FirstName + "<br>");
            sb.Append("LastName: " + LastName + "<br>");
            sb.Append("CareOf: " + CareOf + "<br>");
            sb.Append("StreetName: " + StreetName + "<br>");
            sb.Append("HouseNo: " + HouseNo + "<br>");
            sb.Append("Staircase: " + Staircase + "<br>");
            sb.Append("Apartment: " + Apartment + "<br>");
            sb.Append("ApartmentNo: " + ApartmentNo + "<br>");
            sb.Append("ZipCode: " + ZipCode + "<br>");
            sb.Append("City: " + City + "<br>");
            sb.Append("Email: " + Email + "<br>");
            sb.Append("Phone: " + OtherPhone + "<br>");
            sb.Append("BirthNo: " + BirthNo + "<br>");
            sb.Append("CirixName1: " + CirixName1 + "<br>");
            sb.Append("CirixName2: " + CirixName2 + "<br>");
            sb.Append("CirixStreet2: " + CirixStreet2 + "<hr>");
            sb.Append("Målgrupp: " + TargetGroup + "<hr>");

            if (Payer != null)
                sb.Append(Payer.ToString());

            if (OfferCode != null)
                sb.Append(OfferCode.ToString());


            return sb.ToString();
        }

        //public void CreateExtraExpenseItem()
        //{
        //    //If campId ends with "P", a premium item is associated with the subscription
        //    if (OfferCode.CampId.ToUpper().EndsWith("P"))
        //    {
        //        //Get premium item from Cirix
        //        DataSet ds = SubscriptionController.GetCampaignCoProducts(OfferCode.CampId);
        //        //Get expCode and discPercent from premium item
        //        string expCode = ds.Tables[0].Rows[0]["EXPCODE"].ToString();
        //        string discPercent = ds.Tables[0].Rows[0]["DISCPERCENT"].ToString();

        //        //Associate item with subscription
        //        SubscriptionController.CreateExtraExpenseItem(CusNo,
        //            Payer != null ? Payer.CusNo : 0,
        //            SubsNo,
        //            0,
        //            OfferCode.PaperCode,
        //            OfferCode.ProductNo,
        //            expCode,
        //            1,
        //            DateStart,
        //            DateStart,
        //            "01",
        //            double.Parse(discPercent));
        //    }
        //}
    }
}
