using System;
using System.Collections.Generic;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.HolidayStop
{
    internal class HolidayStopRepository : IHolidayStopRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public HolidayStopRepository(
            ISubscriptionDataAccess subscriptionDataAccess, 
            IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public IEnumerable<HolidayStop> GetHolidayStops(
            string userId, 
            long subscriptionNumber, 
            int externalNumber)
        {
            var holidayStopsDataSet = _subscriptionDataAccess.GetHolidayStops(userId, subscriptionNumber, externalNumber);

            return _objectConverter.ConvertFromDataSet<HolidayStop>(holidayStopsDataSet);
        }

        public long CreateHolidayStop(
            string userId,
            long subscriptionNumber,  
            DateTime startDate,
            DateTime endDate,
            string sleepType,
            string creditType,
            string allowWebPaper,
            string sleepReason,
            string receiveType,
            string sleepLimit)
        {
            return _subscriptionDataAccess.CreateHolidayStop(
                userId,
                subscriptionNumber,
                startDate,
                endDate,
                sleepType,
                creditType,
                allowWebPaper,
                sleepReason,
                receiveType,
                sleepLimit);
        }

        public string DeleteHolidayStop(
            string userId, 
            long subscriptionNumber, 
            int externalNumber, 
            DateTime dateStart)
        {
            return _subscriptionDataAccess.DeleteHolidayStop(
                userId,
                subscriptionNumber,
                externalNumber,
                dateStart);
        }

        public string ChangeHolidayStop(
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
            string sleepLimit)
        {
            return _subscriptionDataAccess.ChangeHolidayStop(
                userId,
                subscriptionNumber,
                externalNumber,
                dateStartOld,
                dateStartNew,
                dateEndOld,
                dateEndNew,
                sleepType,
                creditType,
                allowWebPaper,
                renewSubscription,
                sleepReason,
                receiveType,
                sleepLimit);
        }
    }
}