using System.Collections.Generic;

namespace Di.Subscription.DataAccess.Package
{
    public interface IPackageRepository
    {
        IEnumerable<ProductPackage> GetProductPackages(string userId);
    }
}