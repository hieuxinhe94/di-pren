using System;
using System.Collections.Generic;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.PublicationDays
{
    class PublicationDaysRepository : IPublicationDaysRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public PublicationDaysRepository(ISubscriptionDataAccess subscriptionDataAccess, IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public IEnumerable<PublicationDay> GetPublicationDays(string paperCode, string productNumber, DateTime firstDate, DateTime lastDate)
        {
            var publicationDaysDataSet = _subscriptionDataAccess.GetPublicationDays(paperCode, productNumber, firstDate, lastDate);
            return _objectConverter.ConvertFromDataSet<PublicationDay>(publicationDaysDataSet);
        }
    }
}