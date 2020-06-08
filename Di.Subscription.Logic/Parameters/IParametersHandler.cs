using Di.Subscription.Logic.Parameters.Retrievers;

namespace Di.Subscription.Logic.Parameters
{
    public interface IParametersHandler
    {
        IParametersRetriever ParametersRetriever { get; }
    }
}