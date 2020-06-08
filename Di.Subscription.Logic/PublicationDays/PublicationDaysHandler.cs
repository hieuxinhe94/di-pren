using System;
using System.Collections.Generic;
using Di.Subscription.Logic.PublicationDays.Retrievers;
using Di.Subscription.Logic.PublicationDays.Types;

namespace Di.Subscription.Logic.PublicationDays
{
    class PublicationDaysHandler : IPublicationDaysHandler
    {
        private readonly IPublicationDaysRetriever _publicationDaysRetriever;

        public PublicationDaysHandler(IPublicationDaysRetriever publicationDaysRetriever)
        {
            _publicationDaysRetriever = publicationDaysRetriever;
        }

        public IEnumerable<PublicationDay> GetPublicationDays(string productNumber, DateTime startDate, DateTime endDate)
        {
            return _publicationDaysRetriever.GetPublicationDays(productNumber, startDate, endDate);
        }
    }
}