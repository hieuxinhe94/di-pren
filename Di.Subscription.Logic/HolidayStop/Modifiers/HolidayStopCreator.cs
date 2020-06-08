using System;
using Di.Subscription.DataAccess.HolidayStop;

namespace Di.Subscription.Logic.HolidayStop.Modifiers
{
    internal class HolidayStopCreator : IHolidayStopCreator
    {
        private readonly IHolidayStopRepository _holidayStopRepository;

        public HolidayStopCreator(IHolidayStopRepository holidayStopRepository)
        {
            _holidayStopRepository = holidayStopRepository;
        }

        public string CreateHolidayStop(
            DateTime startDate, 
            DateTime endDate, 
            long subscriptionNumber)
        {
            var result = _holidayStopRepository.CreateHolidayStop(
                SubscriptionConstants.DefaultUserId,
                subscriptionNumber,
                startDate,
                endDate,
                HolidayStopConstants.DefaultSleepType,
                HolidayStopConstants.DefaultCreditType,
                HolidayStopConstants.DefaultAllowWebPaper,
                string.Empty,
                SubscriptionConstants.DefaultRecieveType,
                string.Empty
            );

            return result == 0 ? "OK" : "FAILED " + result;
        }
    }
}