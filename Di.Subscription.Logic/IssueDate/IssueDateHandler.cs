using Di.Subscription.Logic.IssueDate.Retrievers;

namespace Di.Subscription.Logic.IssueDate
{
    internal class IssueDateHandler : IIssueDateHandler
    {
        public IIssueDateRetriever Retriever { get; private set; }

        //public IssueDateHandler() : this(new IssueDateRetriever())
        //{
        //}

        public IssueDateHandler(IIssueDateRetriever issueDateRetriever)
        {
            Retriever = issueDateRetriever;
        }

    }
}