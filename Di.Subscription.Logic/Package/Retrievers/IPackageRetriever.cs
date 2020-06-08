using System.Collections.Generic;
using ProductPackage = Di.Subscription.Logic.Package.Types.ProductPackage;

namespace Di.Subscription.Logic.Package.Retrievers
{
    public interface IPackageRetriever
    {
        IEnumerable<ProductPackage> GetProductPackages();

        List<string> GetAllProductPackageIds();
    }
}
