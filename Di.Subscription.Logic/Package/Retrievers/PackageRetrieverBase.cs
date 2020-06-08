using System;
using System.Collections.Generic;
using System.Linq;
using Di.Subscription.Logic.Package.Types;

namespace Di.Subscription.Logic.Package.Retrievers
{
    public abstract class PackageRetrieverBase
    {
        /// <summary>
        /// Gets a list of all products and packages.
        /// </summary>
        /// <param name="getProductPackages">GetProductPackages Func</param>
        /// <returns>
        /// A list of all packageIds. For backward compablility a combination of PaperCode and Product number is also included in list.
        /// </returns>
        protected List<string> GetAllProductPackageIds(Func<IEnumerable<ProductPackage>> getProductPackages)
        {
            var packages = getProductPackages().ToList();

            var packageIds = packages.Select(package => package.PackageId).Distinct().ToArray();
            var paperCodes = packages.Select(package => package.PaperCode + "-" + package.ProductNumber).Distinct().ToArray();

            return packageIds.Concat(paperCodes).ToList();
        }
    }
}
