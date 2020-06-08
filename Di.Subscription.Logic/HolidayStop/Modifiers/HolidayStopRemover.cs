using System;
using Di.Subscription.DataAccess.HolidayStop;

namespace Di.Subscription.Logic.HolidayStop.Modifiers
{
    internal class HolidayStopRemover : IHolidayStopRemover
    {
        private readonly IHolidayStopRepository _holidayStopRepository;

        public HolidayStopRemover(IHolidayStopRepository holidayStopRepository)
        {
            _holidayStopRepository = holidayStopRepository;
        }

        public string DeleteHolidayStop(
            long subscriptionNumber,
            int externalNumber,
            DateTime dateStart)
        {
            return _holidayStopRepository.DeleteHolidayStop(
                SubscriptionConstants.DefaultUserId,
                subscriptionNumber,
                externalNumber,
                dateStart);
        }
    }
}