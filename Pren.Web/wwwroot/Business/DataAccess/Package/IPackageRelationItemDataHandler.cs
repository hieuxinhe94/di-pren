using System.Collections.Generic;
using Pren.Web.Business.DataAccess.Package.Entities;

namespace Pren.Web.Business.DataAccess.Package
{
    public interface IPackageRelationItemDataHandler
    {
        IEnumerable<PackageRelationItemEntity> GetPackageRelationItems(int packageListId);

        IEnumerable<PackageRelationItemEntity> GetPackageRelationItems(int packageRelationId, string packageListName);

        PackageRelationItemEntity GetPackageRelationItem(int packageItemId);

        void AddPackageRelationItem(int relationListId, string name, bool wildCardBefore, string conditionBefore, bool wildCardAfter, string conditionAfter);

        void UpdatePackageRelationItem(int packageItemId, string name);

        void DeletePackageRelationItem(int packageItemId);
    }
}
