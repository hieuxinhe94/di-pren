using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using EPiServer.Framework.Cache;

namespace Pren.Web.Business.Cache
{
    public class ObjectCache : IObjectCache
    {
        private const string CachePrefix = "Pren.Web_";

        private readonly ISynchronizedObjectInstanceCache _cache;

        public ObjectCache(ISynchronizedObjectInstanceCache cache)
        {
            _cache = cache;
        }

        public void AddToCache(string cacheKey, object objectToCache)
        {
            cacheKey = GetCacheKeyWithPrefix(cacheKey);
            _cache.Insert(cacheKey, objectToCache, CacheEvictionPolicy.Empty);
        }

        public void AddToCache(string cacheKey, object objectToCache, CacheEvictionPolicy cacheEvictionPolicy)
        {
            cacheKey = GetCacheKeyWithPrefix(cacheKey);
            _cache.Insert(cacheKey, objectToCache, cacheEvictionPolicy);
        }

        public object GetFromCache(string cacheKey)
        {
            cacheKey = GetCacheKeyWithPrefix(cacheKey);
            return _cache.Get(cacheKey);
        }

        public void RemoveFromCache(string cacheKey)
        {
            var cacheKeyWithPrefix = GetCacheKeyWithPrefix(cacheKey);
            _cache.Remove(cacheKeyWithPrefix);
        }

        public Dictionary<string, CacheKeyInfo> GetSiteCacheInfo()
        {
            var cacheItems = new Dictionary<string, CacheKeyInfo>();

            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var key = enumerator.Key as string;
                if (key != null && key.StartsWith(CachePrefix))
                {
                    var expires = GetCacheUtcExpiryDateTime(key);
                    var cacheItemInfo = new CacheKeyInfo
                    {
                        Key = key,
                        Expires = expires
                    };
                    cacheItems.Add(key, cacheItemInfo);
                }
            }

            return cacheItems;
        }

        private string GetCacheKeyWithPrefix(string cacheKey)
        {
            if (cacheKey.StartsWith(CachePrefix))
            {
                return cacheKey;
            }

            return CachePrefix + cacheKey;
        }

        private DateTime GetCacheUtcExpiryDateTime(string cacheKey)
        {
            var cacheEntry = HttpRuntime.Cache.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(HttpRuntime.Cache, new object[] { cacheKey, 1 });
            var utcExpiresProperty = cacheEntry.GetType().GetProperty("UtcExpires", BindingFlags.NonPublic | BindingFlags.Instance);
            var utcExpiresValue = (DateTime)utcExpiresProperty.GetValue(cacheEntry, null);

            return utcExpiresValue;
        }
    }
}
