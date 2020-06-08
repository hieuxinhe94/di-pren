using System;

namespace Di.Subscription.Logic.IssueDate.Retrievers
{
    public interface IIssueDateRetriever
    {
        DateTime GetNextIssuedate(string papercode, string productNumber, DateTime minDate);
    }
}