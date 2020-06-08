using System.Collections.Generic;
using EPiServer.Framework.Cache;

namespace Pren.Web.Business.Cache
{
    public interface IObjectCache
    {
        void AddToCache(string cacheKey, object objectToCache);
        void AddToCache(string cacheKey, object objectToCache, CacheEvictionPolicy cacheEvictionPolicy);
        object GetFromCache(string cacheKey);
        void RemoveFromCache(string cacheKey);
        Dictionary<string, CacheKeyInfo> GetSiteCacheInfo();
    }
}
