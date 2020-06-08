using System;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.IssueDate
{
    internal class IssueDateRepository : IIssueDateRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;

        public IssueDateRepository(ISubscriptionDataAccess subscriptionDataAccess)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
        }

        public string GetNextIssuedate(string papercode, string productNumber, DateTime minDate)
        {
            return _subscriptionDataAccess.GetNextIssuedate(papercode, productNumber, minDate);
        }
    }
}