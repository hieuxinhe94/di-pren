using System.Collections.Generic;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.Package
{
    class PackageRepository : IPackageRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public PackageRepository(ISubscriptionDataAccess subscriptionDataAccess, IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public IEnumerable<ProductPackage> GetProductPackages(string userId)
        {
            var productPackagesDataSet = _subscriptionDataAccess.GetPapersAndProducts(userId);
            return _objectConverter.ConvertFromDataSet<ProductPackage>(productPackagesDataSet);
        }
    }
}