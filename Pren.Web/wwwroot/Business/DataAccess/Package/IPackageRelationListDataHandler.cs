using System.Collections.Generic;
using Pren.Web.Business.DataAccess.Package.Entities;

namespace Pren.Web.Business.DataAccess.Package
{
    public interface IPackageRelationListDataHandler
    {
        IEnumerable<PackageRelationListEntity> GetPackageRelationLists(int packageRelationId);

        PackageRelationListEntity GetPackageRelationList(int packageListId);

        void AddPackageRelationList(string name, int relationTypeId);

        void DeletePackageRelationList(int relationListId);
    }
}
