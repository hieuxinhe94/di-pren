using System.Web;

namespace Di.Common.Cache
{
    public class ObjectCache : IObjectCache
    {
        public object Get(string cacheKey)
        {
            return HttpContext.Current != null ? HttpContext.Current.Cache.Get(cacheKey) : null;
        }

        public void Insert(string cacheKey, object value)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, value);   
            }            
        }
    }
}