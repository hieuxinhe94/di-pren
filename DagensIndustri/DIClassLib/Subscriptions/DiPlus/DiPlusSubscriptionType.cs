using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace DIClassLib.Subscriptions.DiPlus
{
    public class DiPlusSubscriptionType
    {

        public enum PlusSubsType
        {
            [DescriptionAttribute("StandAlonePlusSubs")]
            StandAlonePlusSubs = 0,
            [DescriptionAttribute("UpgradeDi6DaySubs")]
            UpgradeDi6DaySubs,
            [DescriptionAttribute("UpgradeWeekendSubs")]
            UpgradeWeekendSubs
        }

    }
}
