using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DIClassLib.Subscriptions
{
    /// <summary>
    /// campaign info class
    /// holds "all" important campaign properites
    /// </summary>
    [Serializable]
    public class CampaignObject
    {
        #region Properties
        private DataRow CampaignDataRow { get; set; }
        private DataRow CampaignDetailsDataRow { get; set; }
        private DataRow SubsChoices2DataRow { get; set; }

        //01 = DI (normal subscriptions), 05 = DI weekend
        public string ProductNo { get; set; }
        public bool IsDirectDebit { get; set; }

        //1050
        public long CampNo
        {
            get
            {
                return Convert.ToInt64(CampaignDataRow["CAMPNO"]);
            }
        }

        //N10120000E 
        public string CampId
        {
            get
            {
                return (string)CampaignDataRow["CAMPID"];
            }
        }

        //DI
        public string Papercode
        {
            get
            {
                return (string)CampaignDataRow["PAPERCODE"];
            }
        }

        //01  (most often)
        public string Substype
        {
            get
            {
                //return SubsChoices2DataRow != null ? (string)SubsChoices2DataRow["SUBSKIND"] : "01";
                return "01";
            }
        }

        //Subskind. 01 = tills vidare, 02 = tidsbestämd
        public string SubsKind
        {
            get
            {
                return SubsChoices2DataRow != null ? (string)SubsChoices2DataRow["SUBSKIND"] : "01";
            }
        }

        //12
        public int SubslenMons
        {
            get
            {
                return Convert.ToInt32(CampaignDetailsDataRow["SUBSLENGTH"]);
            }
        }

        //00
        public string Pricegroup
        {
            get
            {

                string priceGroup = "00";

                if (SubsChoices2DataRow != null)
                    priceGroup = (string)SubsChoices2DataRow["PRICEGROUP"];

                if (DirectDebit == "Y")
                    priceGroup = "25";

                return priceGroup;
            }
        }

        //todo: check if 0 is proper GrossPrice default value
        //public double GrossPrice
        //{
        //    get { return 0; }
        //}

        //if exists discount is applied
        public double TotalPrice
        {
            get
            {
                //todo: confirm prio for prices
                double price = Convert.ToDouble(CampaignDetailsDataRow["TOTALPRICE"]);
              
                if (price > 0)
                {
                    int discPct = Convert.ToInt32(CampaignDetailsDataRow["DISCPERCENT"]);
                    if (discPct > 0)
                        price = Math.Round(price * ((100 - discPct) * 0.01));
                }

                return price;
            }
        }

        //public double TotalPriceIncVAT
        //{
        //    get
        //    {
        //        double ret = 0;
        //        if (TotalPrice > 0)
        //            ret = Math.Round(TotalPrice * 1.06);

        //        return ret;
        //    }
        //}


        public double ItemPrice
        {
            get
            {
                //todo: check logic for ItemPrice
                double ret = TotalPrice;

                if (DirectDebit == "Y")
                    ret = Math.Round(TotalPrice / SubslenMons);

                return ret;
            }
        }


        public string DirectDebit
        {
            get
            {
                return IsDirectDebit ? "Y" : "N";
            }
        }


        public int ItemQty
        {
            get
            {
                int ret = 1;
                if (DirectDebit == "Y")
                    ret = SubslenMons;

                return ret;
            }
        }

        public int DiscPercent
        {
            get
            {
                int ret = 0;
                if (CampaignDetailsDataRow != null)
                    ret = Convert.ToInt32(CampaignDetailsDataRow["DISCPERCENT"]);

                return ret;
            }
        }


        #endregion

        #region Constructors
        public CampaignObject(DataRow drCampaign, DataRow drCampaignDetails, DataRow drSubsChoices2, string productNo, bool isDirectDebit)
        {
            CampaignDataRow = drCampaign;
            CampaignDetailsDataRow = drCampaignDetails;
            SubsChoices2DataRow = drSubsChoices2;
            ProductNo = productNo;
            IsDirectDebit = isDirectDebit;
        }
        #endregion
    }
}
