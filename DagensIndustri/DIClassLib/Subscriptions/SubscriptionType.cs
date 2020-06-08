using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DIClassLib.Subscriptions
{
    public class SubscriptionType
    {
        #region Enums
        public enum TypeOfSubscription
        {
            [DescriptionAttribute("Di Premium")]
            DiPremium = 0,
            [DescriptionAttribute("Di")]
            Di,
            [DescriptionAttribute("Di via Autogiro")]
            DiDirectDebit,
            [DescriptionAttribute("Di Weekend")]
            DiWeekend,
            [DescriptionAttribute("Dagens industri i läsplatta")]
            DiPlus
        }
        #endregion
    }
}
