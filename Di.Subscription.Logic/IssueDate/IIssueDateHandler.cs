using Di.Subscription.Logic.IssueDate.Retrievers;

namespace Di.Subscription.Logic.IssueDate
{
    public interface IIssueDateHandler
    {
        IIssueDateRetriever Retriever { get; }
    }
}