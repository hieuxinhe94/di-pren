using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.HolidayStop;
using Di.Subscription.Logic.HolidayStop.Types;

namespace Di.Subscription.Logic.HolidayStop.Retrievers
{
    internal class HolidayStopRetriever : IHolidayStopRetriever
    {
        private readonly IHolidayStopRepository _holidayStopRepository;

        public HolidayStopRetriever(IHolidayStopRepository holidayStopRepository)
        {
            _holidayStopRepository = holidayStopRepository;
        }

        public IEnumerable<HolidayStopItem> GetHolidayStops(long subscriptionNumber, int externalNumber)
        {
            var holidayStopsExt = _holidayStopRepository.GetHolidayStops(SubscriptionConstants.DefaultUserId, subscriptionNumber, externalNumber);
            var holidayStopItems = holidayStopsExt.Select(holidayStopExt => new HolidayStopItem
            {
                Id = holidayStopExt.SubscriptionNumber + "_" + holidayStopExt.StartDate + "_" + holidayStopExt.EndDate,
                StartDate = holidayStopExt.StartDate,
                EndDate = holidayStopExt.EndDate
            });

            return holidayStopItems;
        }
    }
}