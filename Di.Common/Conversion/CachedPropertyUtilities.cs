using System.Reflection;
using Di.Common.Cache;

namespace Di.Common.Conversion
{
    public class CachedPropertyUtilities : IPropertyUtilities
    {
        private readonly IPropertyUtilities _propertyUtilities;
        private readonly IObjectCache _objectCache;

        public CachedPropertyUtilities(IPropertyUtilities propertyUtilities, IObjectCache objectCache)
        {
            _propertyUtilities = propertyUtilities;
            _objectCache = objectCache;
        }

        public PropertyInfo[] GetPropertyInfos<T>(T obj)
        {
            var objectType = obj.GetType();

            var cacheKey = objectType.Name;

            var propertyInfos = (PropertyInfo[])_objectCache.Get(cacheKey);

            if (propertyInfos != null)
            {
                return propertyInfos;
            }

            propertyInfos = _propertyUtilities.GetPropertyInfos(obj);

            _objectCache.Insert(cacheKey, propertyInfos);

            return propertyInfos;
        }
    }
}
