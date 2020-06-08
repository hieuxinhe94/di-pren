using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DIClassLib.DbHandlers;

namespace DIClassLib.Misc
{
    [Serializable]
    public class OfferCode
    {

        public OfferCode(DataRow row)
        {
            //GetCampaign from Cirix to get more info
            DataSet dsCampaign = SubscriptionController.GetCampaign(long.Parse(row["campNo"].ToString()));

            Price = double.Parse(dsCampaign.Tables[0].Rows[0]["TOTALPRICE"].ToString());
            PaperCode = dsCampaign.Tables[0].Rows[0]["PAPERCODE"].ToString();
            SubsLength = int.Parse(dsCampaign.Tables[0].Rows[0]["SUBSLENGTH"].ToString());
            SubsLengthUnit = dsCampaign.Tables[0].Rows[0]["LENGTHUNIT"].ToString();
            DiscPercent = int.Parse(dsCampaign.Tables[0].Rows[0]["DISCPERCENT"].ToString());

            CampStartDate = DateTime.Parse(dsCampaign.Tables[0].Rows[0]["CAMPSTARTDATE"].ToString());
            StartDateFirst = DateTime.Parse(dsCampaign.Tables[0].Rows[0]["STARTDATE_FRST"].ToString());
            SubsEndDate = DateTime.Parse(dsCampaign.Tables[0].Rows[0]["SUBSENDDATE"].ToString());

            //Set MSSQL properties
            OfferCodeId = int.Parse(row["offerCodeId"].ToString());
            Text = row["offerText"].ToString();
            CampNo = long.Parse(row["campNo"].ToString());
            CampId = row["campId"].ToString();
            IsAutogiro = bool.Parse(row["isAutoGiro"].ToString());
            IsStudent = bool.Parse(row["isStudent"].ToString());
            ProcuctName = row["productName"].ToString();
            ProductNo = row["productNo"].ToString();
            Subskind = row["subsKind"].ToString();
            PriceGroup = row["priceGroup"].ToString();
        }

        public DateTime CampStartDate { get; set; }
        public DateTime StartDateFirst { get; set; }

        public DateTime SubsEndDate { get; set; }

        public int OfferCodeId { get; set; }

        public string Text { get; set; }

        public string CampId { get; set; }

        public long CampNo { get; set; }

        public bool IsAutogiro { get; set; }

        public bool IsStudent { get; set; }

        public string ProcuctName { get; set; }

        public string ProductNo { get; set; }

        public double Price { get; set; }

        public string PaperCode { get; set; }

        public int SubsLength { get; set; }

        public string SubsLengthUnit { get; set; }

        public int DiscPercent { get; set; }

        public string PriceGroup { get; set; }

        //Subskind. 01 = tills vidare, 02 = tidsbestämd
        public string Subskind { get; set; }

        //Rabattkategori, always 01
        public string Substype
        {
            get
            {
                return "01";
            }
        }

        //TODO: check if 0 is proper GrossPrice default value
        public double GrossPrice
        {
            get { return 0; }
        }

        //if exists discount is applied
        public double TotalPrice
        {
            get
            {
                double returnValue = Price;

                if (returnValue > 0)
                {
                    if (DiscPercent > 0)
                        returnValue = Math.Round(returnValue * ((100 - DiscPercent) * 0.01), MidpointRounding.AwayFromZero);
                }

                return returnValue;
            }
        }

        public double TotalPriceIncVAT
        {
            get
            {
                double returnValue = 0;

                if (TotalPrice > 0)
                    returnValue = Math.Round(TotalPrice * 1.06, MidpointRounding.AwayFromZero);

                return returnValue;
            }
        }


        /// <summary>
        /// TODO: Must check unit to know if price is per week/month
        /// </summary>
        public double ItemPrice
        {
            get
            {
                double returnValue = TotalPrice;

                if (IsAutogiro)
                    returnValue = Math.Round(TotalPrice / SubsLength, MidpointRounding.AwayFromZero);

                return returnValue;
            }
        }

        /// <summary>
        /// TODO: Must check unit to know if quantity is week/month
        /// </summary>
        public int ItemQuantity
        {
            get
            {
                int returnValue = 1;
                if (IsAutogiro)
                    returnValue = SubsLength;

                return returnValue;
            }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("OfferCodeId: " + OfferCodeId + "<br />");
            sb.Append("Text: " + Text + "<br />");
            sb.Append("CampId: " + CampId + "<br />");
            sb.Append("CampNo: " + CampNo + "<br />");
            sb.Append("IsAutogiro: " + IsAutogiro + "<br />");
            sb.Append("ProcuctName: " + ProcuctName + "<br />");
            sb.Append("ProductNo: " + ProductNo + "<br />");
            sb.Append("Price: " + Price + "<br />");
            sb.Append("PaperCode: " + PaperCode + "<br />");
            sb.Append("SubsLength: " + SubsLength + "<br />");
            sb.Append("SubsLengthUnit: " + SubsLengthUnit + "<br />");
            sb.Append("DiscPercent: " + DiscPercent + "<br />");
            sb.Append("PriceGroup: " + PriceGroup + "<br />");
            sb.Append("Substype: " + Substype + "<br />");
            sb.Append("Subskind: " + Subskind + "<br />");
            sb.Append("GrossPrice: " + GrossPrice + "<br />");
            sb.Append("TotalPrice: " + TotalPrice + "<br />");
            sb.Append("TotalPriceIncVAT: " + TotalPriceIncVAT + "<br />");
            sb.Append("ItemPrice: " + ItemPrice + "<br />");
            sb.Append("ItemQuantity: " + ItemQuantity + "<br />");

            return sb.ToString();

        }


    }
}
