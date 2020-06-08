using Di.Subscription.Logic.Parameters.Retrievers;

namespace Di.Subscription.Logic.Parameters
{
    public class ParametersHandler : IParametersHandler
    {
        public IParametersRetriever ParametersRetriever { get; private set; }

        public ParametersHandler(IParametersRetriever parametersRetriever)
        {
            ParametersRetriever = parametersRetriever;
        }        
    }
}
