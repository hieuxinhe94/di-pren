using System;

namespace Di.Subscription.Logic.HolidayStop.Types
{
    internal class HolidayStop
    {
        public HolidayStop(
            DateTime startDate,
            DateTime endDate,
            int externalNumber,
            long subscriptionNumber)
        {
            StartDate = startDate;
            EndDate = endDate;
            ExternalNumber = externalNumber;
            SubscriptionNumber = subscriptionNumber;

            // Default values
            SleepType = HolidayStopConstants.DefaultSleepType;
            CreditType = HolidayStopConstants.DefaultCreditType;
            AllowWebPaper = HolidayStopConstants.DefaultAllowWebPaper;
            RenewSubscription = false;
            RecieveType = SubscriptionConstants.DefaultRecieveType;
            SleepReason = string.Empty;
            SleepLimit = string.Empty;
            UserId = SubscriptionConstants.DefaultUserId;
        }

        internal string UserId { get; set; }
        internal long SubscriptionNumber { get; set; }
        internal int ExternalNumber { get; set; }
        internal DateTime StartDate { get; set; }
        internal DateTime EndDate { get; set; }
        internal string SleepType { get; set; }
        internal string CreditType { get; set; }
        internal string AllowWebPaper { get; set; }
        internal bool RenewSubscription { get; set; }
        internal string SleepReason { get; set; }
        internal string RecieveType { get; set; }
        internal string SleepLimit { get; set; }
    }
}
