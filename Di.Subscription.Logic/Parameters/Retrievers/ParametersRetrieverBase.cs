using System;
using System.Collections.Generic;
using System.Linq;

namespace Di.Subscription.Logic.Parameters.Retrievers
{
    public abstract class ParametersRetrieverBase
    {
        protected IEnumerable<T> GetAll<T>(Func<string, IEnumerable<T>> getFunc)
        {
            var result = getFunc(SubscriptionConstants.PaperCodeAll);
            result = result.Union(getFunc(SubscriptionConstants.PaperCodeDi));
            result = result.Union(getFunc(SubscriptionConstants.PaperCodeIpad));
            result = result.Union(getFunc(SubscriptionConstants.PaperCodeDise));

            return result;
        }
    }
}
