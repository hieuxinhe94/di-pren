using System;
using System.Collections.Generic;
using Di.Subscription.Logic.Parameters.Retrievers;
using Di.Subscription.Logic.Parameters.Types;

namespace Pren.Web.Business.Cache.Parameters.Retrievers
{
    public class CachedParametersRetriever : ParametersRetrieverBase, IParametersRetriever
    {
        private readonly IParametersRetriever _parametersRetriever;
        private readonly IObjectCache _objectCache;

        public CachedParametersRetriever(IParametersRetriever parametersRetriever, IObjectCache objectCache)
        {
            _parametersRetriever = parametersRetriever;
            _objectCache = objectCache;
        }

        public IEnumerable<ReceiveType> GetAllReceiveTypes()
        {
            return GetAll(GetReceiveTypes);
        }

        public IEnumerable<ReceiveType> GetReceiveTypes(string paperCode)
        {
            var cacheKey = "subscriptionReceiveTypes" + paperCode;

            return GetFromCache(cacheKey, paperCode, _parametersRetriever.GetReceiveTypes);
        }

        public IEnumerable<TargetGroup> GetAllTargetGroups()
        {
            return GetAll(GetTargetGroups);
        }

        public IEnumerable<TargetGroup> GetTargetGroups(string paperCode)
        {
            var cacheKey = "subscriptionTargetGroups" + paperCode;

            return GetFromCache(cacheKey, paperCode, _parametersRetriever.GetTargetGroups);
        }

        private IEnumerable<T> GetFromCache<T>(string cacheKey, string paperCode, Func<string, IEnumerable<T>> getFunc)
        {
            var parameters = (IEnumerable<T>)_objectCache.GetFromCache(cacheKey);

            if (parameters != null)
            {
                return parameters;
            }

            parameters = getFunc(paperCode);
            _objectCache.AddToCache(cacheKey, parameters);

            return parameters;
        }
    }
}
