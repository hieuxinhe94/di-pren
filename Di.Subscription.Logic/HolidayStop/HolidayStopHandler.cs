using System;
using System.Collections.Generic;
using Di.Subscription.Logic.HolidayStop.Modifiers;
using Di.Subscription.Logic.HolidayStop.Retrievers;
using Di.Subscription.Logic.HolidayStop.Types;

namespace Di.Subscription.Logic.HolidayStop
{
    internal class HolidayStopHandler : IHolidayStopHandler
    {
        private readonly IHolidayStopCreator _holidayStopCreator;
        private readonly IHolidayStopRemover _holidayStopRemover;
        private readonly IHolidayStopChanger _holidayStopChanger;
        private readonly IHolidayStopRetriever _holidayStopRetriever;

        public HolidayStopHandler(
            IHolidayStopCreator holidayStopCreator,
            IHolidayStopRemover holidayStopRemover,
            IHolidayStopChanger holidayStopChanger,
            IHolidayStopRetriever holidayStopRetriever)
        {
            _holidayStopCreator = holidayStopCreator;
            _holidayStopRemover = holidayStopRemover;
            _holidayStopChanger = holidayStopChanger;
            _holidayStopRetriever = holidayStopRetriever;
        }

        public IEnumerable<HolidayStopItem> GetHolidayStops(
            long subscriptionNumber, 
            int externalNumber)
        {
            return _holidayStopRetriever.GetHolidayStops(subscriptionNumber, externalNumber);
        }

        public string CreateHolidayStop(
            DateTime startDate, 
            DateTime endDate, 
            long subscriptionNumber)
        {
            return _holidayStopCreator.CreateHolidayStop(
                startDate,
                endDate,
                subscriptionNumber);
        }

        public string DeleteHolidayStop(
            long subscriptionNumber,
            int externalNumber,
            DateTime dateStart)
        {
            return _holidayStopRemover.DeleteHolidayStop(
                subscriptionNumber,
                externalNumber,
                dateStart);
        }

        public string ChangeHolidayStop(
            long subscriptionNumber,
            int externalNumber,
            DateTime dateStartOld,
            DateTime dateStartNew,
            DateTime dateEndOld,
            DateTime dateEndNew)
        {
            return _holidayStopChanger.ChangeHolidayStop(
                subscriptionNumber,
                externalNumber,
                dateStartOld,
                dateStartNew,
                dateEndOld,
                dateEndNew);
        }
    }
}