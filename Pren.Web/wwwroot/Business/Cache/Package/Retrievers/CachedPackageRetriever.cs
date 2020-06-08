using System.Collections.Generic;
using Di.Subscription.Logic.Package.Retrievers;
using Di.Subscription.Logic.Package.Types;

namespace Pren.Web.Business.Cache.Package.Retrievers
{
    public class CachedPackageRetriever : PackageRetrieverBase, IPackageRetriever
    {
        private readonly IPackageRetriever _packageRetriever;
        private readonly IObjectCache _objectCache;

        public CachedPackageRetriever(IPackageRetriever packageRetriever, IObjectCache objectCache)
        {
            _packageRetriever = packageRetriever;
            _objectCache = objectCache;
        }

        public IEnumerable<ProductPackage> GetProductPackages()
        {
            const string cacheKey = "subscriptionProductPackages";
            var productPackages = (IEnumerable<ProductPackage>)_objectCache.GetFromCache(cacheKey);

            if (productPackages != null)
            {
                return productPackages;
            }

            productPackages = _packageRetriever.GetProductPackages();
            _objectCache.AddToCache(cacheKey, productPackages);

            return productPackages;  
        }

        public List<string> GetAllProductPackageIds()
        {
            return GetAllProductPackageIds(GetProductPackages);
        }
    }
}