using System;

namespace Di.Subscription.Logic.HolidayStop.Modifiers
{
    internal interface IHolidayStopChanger
    {
        string ChangeHolidayStop(
           long subscriptionNumber,
           int externalNumber,
           DateTime dateStartOld,
           DateTime dateStartNew,
           DateTime dateEndOld,
           DateTime dateEndNew);
    }
}