using System;
using System.Collections.Generic;

namespace Di.Subscription.Logic.HolidayStop
{
    public interface IHolidayStopHandler
    {
        IEnumerable<Types.HolidayStopItem> GetHolidayStops(
           long subscriptionNumber,
           int externalNumber);

        string CreateHolidayStop(
            DateTime startDate,
            DateTime endDate,
            long subscriptionNumber);

        string DeleteHolidayStop(
           long subscriptionNumber,
           int externalNumber,
           DateTime dateStart);

        string ChangeHolidayStop(
            long subscriptionNumber,
            int externalNumber,
            DateTime dateStartOld,
            DateTime dateStartNew,
            DateTime dateEndOld,
            DateTime dateEndNew);
    }
}