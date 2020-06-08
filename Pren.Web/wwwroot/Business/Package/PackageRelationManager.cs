using System.Linq;
using Pren.Web.Business.DataAccess.Package;

namespace Pren.Web.Business.Package
{
    public class PackageRelationManager : IPackageRelationManager
    {
        private readonly IPackageRelationItemDataHandler _packageRelationItemDataHandler;

        public PackageRelationManager(IPackageRelationItemDataHandler packageRelationItemDataHandler)
        {
            _packageRelationItemDataHandler = packageRelationItemDataHandler;
        }

        public string GetParameters(PackageRelationTypeEnum relationType, string packageId)
        {
            var relationItems = _packageRelationItemDataHandler.GetPackageRelationItems((int)relationType, packageId);

            return string.Join(",", relationItems.Select(item =>
                (item.WildcardBefore ? "*" : item.ConditionBefore) +
                "-" + item.Name + "-" +
                (item.WildcardAfter ? "*" : item.ConditionAfter)
                ).ToArray());
        }
    }
}