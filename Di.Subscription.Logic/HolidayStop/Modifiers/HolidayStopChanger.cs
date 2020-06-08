using System;
using Di.Subscription.DataAccess.HolidayStop;

namespace Di.Subscription.Logic.HolidayStop.Modifiers
{
    internal class HolidayStopChanger : IHolidayStopChanger
    {
        private readonly IHolidayStopRepository _holidayStopRepository;

        public HolidayStopChanger(IHolidayStopRepository holidayStopRepository)
        {
            _holidayStopRepository = holidayStopRepository;
        }

        public string ChangeHolidayStop(
            long subscriptionNumber,
            int externalNumber,
            DateTime dateStartOld,
            DateTime dateStartNew,
            DateTime dateEndOld,
            DateTime dateEndNew)
        {
            var holidayStop = new Types.HolidayStop(dateStartOld, dateEndOld, externalNumber, subscriptionNumber);

            return _holidayStopRepository.ChangeHolidayStop(
                holidayStop.UserId,
                holidayStop.SubscriptionNumber,
                holidayStop.ExternalNumber,
                holidayStop.StartDate,
                dateStartNew,
                holidayStop.EndDate,
                dateEndNew,
                holidayStop.SleepType,
                holidayStop.CreditType,
                holidayStop.AllowWebPaper,
                holidayStop.RenewSubscription,
                holidayStop.SleepReason,
                holidayStop.RecieveType,
                holidayStop.SleepLimit);
        }
    }
}