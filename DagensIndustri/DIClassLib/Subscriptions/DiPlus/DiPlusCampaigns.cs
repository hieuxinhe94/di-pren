using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using System.Data;
using DIClassLib.CardPayment;


namespace DIClassLib.Subscriptions.DiPlus
{
    public static class DiPlusCampaigns
    {
        private static DiPlusCampaign _standAlone;
        public static DiPlusCampaign StandAlone
        {
            get
            {
                if(_standAlone == null)
                    _standAlone = new DiPlusCampaign(DiPlusSubscriptionType.PlusSubsType.StandAlonePlusSubs, long.Parse(MiscFunctions.GetAppsettingsValue("diPlusSubsnoNewStandAlone")));

                return _standAlone;
            }
        }

        private static DiPlusCampaign _upgrade6Days;
        public static DiPlusCampaign Upgrade6Days
        {
            get
            {
                if (_upgrade6Days == null)
                    _upgrade6Days = new DiPlusCampaign(DiPlusSubscriptionType.PlusSubsType.UpgradeDi6DaySubs, long.Parse(MiscFunctions.GetAppsettingsValue("diPlusSubsnoUpgrade6Day")));

                return _upgrade6Days;
            }
        }

        private static DiPlusCampaign _upgradeWeekend;
        public static DiPlusCampaign UpgradeWeekend
        {
            get
            {
                if (_upgradeWeekend == null)
                    _upgradeWeekend = new DiPlusCampaign(DiPlusSubscriptionType.PlusSubsType.UpgradeWeekendSubs, long.Parse(MiscFunctions.GetAppsettingsValue("diPlusSubsnoUpgradeWeekend")));

                return _upgradeWeekend;
            }
        }
    }

    public class DiPlusCampaign
    {
        public DiPlusSubscriptionType.PlusSubsType SubsType { get; set; }
        public long Campno { get; set; }
        public double PriceExVat { get; set; }
        public double PriceIncVat { get; set; }


        public DiPlusCampaign(DiPlusSubscriptionType.PlusSubsType subsType, long campno)
        {
            Campno = campno;
            SubsType = subsType;
            PriceExVat = GetPriceExVat();
            PriceCalculator pc = new PriceCalculator(null, PriceExVat, Settings.VatIpad, 1);
            PriceIncVat = (double)pc.PriceIncVat;
        }

        private double GetPriceExVat()
        {
            double price = 0;
            if (Campno > 0)
            {
                DataSet ds = SubscriptionController.GetCampaign(Campno);
                if(ds != null && ds.Tables != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows[0] != null)
                    double.TryParse(ds.Tables[0].Rows[0]["TOTALPRICE"].ToString(), out price);
            }

            return price;
        }

        
    }
}
