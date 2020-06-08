using System;

namespace Di.Subscription.DataAccess.IssueDate
{
    public interface IIssueDateRepository
    {
        string GetNextIssuedate(string papercode, string productNumber, DateTime minDate);
    }
}