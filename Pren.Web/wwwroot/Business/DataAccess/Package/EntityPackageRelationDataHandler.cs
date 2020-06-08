using System.Collections.Generic;
using System.Linq;
using Pren.Web.Business.Data;
using Pren.Web.Business.DataAccess.Package.Entities;

namespace Pren.Web.Business.DataAccess.Package
{
    public class EntityPackageRelationDataHandler : IPackageRelationDataHandler
    {
        public IEnumerable<PackageRelationEntity> GetAllPackageRelations()
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                return dbContext.PackageRelation
                    .Select(ConvertToPackageRelationEntity)
                    .ToList();
            }
        }

        public PackageRelationEntity GetPackageRelation(int relationId)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                var packageRelation = dbContext.PackageRelation.SingleOrDefault(relation => relation.Id.Equals(relationId));

                return packageRelation != null ? ConvertToPackageRelationEntity(packageRelation) : null;
            }
        }

        private PackageRelationEntity ConvertToPackageRelationEntity(PackageRelation relation)
        {
            return new PackageRelationEntity
            {
                Id = relation.Id,
                Name = relation.Name,
                Created = relation.Created
            };
        }
    }
}