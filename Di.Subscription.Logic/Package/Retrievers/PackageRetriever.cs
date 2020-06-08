using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.Package;
using ProductPackage = Di.Subscription.Logic.Package.Types.ProductPackage;

namespace Di.Subscription.Logic.Package.Retrievers
{
    public class PackageRetriever : PackageRetrieverBase, IPackageRetriever
    {
        private readonly IPackageRepository _packageRepository;

        public PackageRetriever(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public List<string> GetAllProductPackageIds()
        {
            return GetAllProductPackageIds(GetProductPackages);
        }

        public IEnumerable<ProductPackage> GetProductPackages()
        {
            return _packageRepository.GetProductPackages(SubscriptionConstants.DefaultUserId).Select(GetProductPackage);
        }

        private ProductPackage GetProductPackage(DataAccess.Package.ProductPackage package)
        {
            return new ProductPackage
            {
                StartDate = package.StartDate,
                EndDate = package.EndDate,
                PackageId = package.PackageId,
                PaperCode = package.PaperCode,
                ProductNumber = package.ProductNumber,
                MainProduct = package.MainProduct.Equals("Y")
            };
        }
    }
}