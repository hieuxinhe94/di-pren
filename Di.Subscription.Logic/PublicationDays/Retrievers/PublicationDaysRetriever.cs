using System;
using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.PublicationDays;
using PublicationDay = Di.Subscription.Logic.PublicationDays.Types.PublicationDay;

namespace Di.Subscription.Logic.PublicationDays.Retrievers
{
    internal class PublicationDaysRetriever : IPublicationDaysRetriever
    {
        private readonly IPublicationDaysRepository _publicationDaysRepository;

        public PublicationDaysRetriever(IPublicationDaysRepository publicationDaysRepository)
        {
            _publicationDaysRepository = publicationDaysRepository;
        }

        public IEnumerable<PublicationDay> GetPublicationDays(string productNumber, DateTime startDate, DateTime endDate)
        {
            return _publicationDaysRepository.GetPublicationDays(
                SubscriptionConstants.PaperCodeDi, 
                productNumber,
                startDate, 
                endDate)
                .Select(GetPublicationDay);
        }

        private PublicationDay GetPublicationDay(DataAccess.PublicationDays.PublicationDay publicationDay)
        {
            return new PublicationDay
            {
                IssueDate = publicationDay.IssueDate
            };
        } 
    }
}