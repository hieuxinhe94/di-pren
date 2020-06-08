using System;

namespace Di.Subscription.Logic.HolidayStop.Modifiers
{
    internal interface IHolidayStopRemover
    {
        string DeleteHolidayStop(
           long subscriptionNumber,
           int externalNumber,
           DateTime dateStart);
    }
}