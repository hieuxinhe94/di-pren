using System.Collections.Generic;
using Pren.Web.Business.DataAccess.Package.Entities;

namespace Pren.Web.Business.DataAccess.Package
{
    public interface IPackageRelationDataHandler
    {
        IEnumerable<PackageRelationEntity> GetAllPackageRelations();

        PackageRelationEntity GetPackageRelation(int relationId);
    }
}
