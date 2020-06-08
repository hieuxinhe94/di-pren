using System;
using System.Collections.Generic;

namespace Di.Subscription.DataAccess.HolidayStop
{
    public interface IHolidayStopRepository
    {
        IEnumerable<HolidayStop> GetHolidayStops(
            string userId, 
            long subscriptionNumber, 
            int externalNumber);

        long CreateHolidayStop(
            string userId,
            long subscriptionNumber,
            DateTime startDate,
            DateTime endDate,
            string sleepType,
            string creditType,
            string allowWebPaper,
            string sleepReason,
            string receiveType,
            string sleepLimit);

        string DeleteHolidayStop(
            string userId, 
            long subscriptionNumber, 
            int externalNumber, 
            DateTime dateStart);

        string ChangeHolidayStop(
            string userId, 
            long subscriptionNumber, 
            int externalNumber, 
            DateTime dateStartOld,
            DateTime dateStartNew, 
            DateTime dateEndOld, 
            DateTime dateEndNew, 
            string sleepType, 
            string creditType,
            string allowWebPaper, 
            bool renewSubscription, 
            string sleepReason, 
            string receiveType, 
            string sleepLimit);
    }
}