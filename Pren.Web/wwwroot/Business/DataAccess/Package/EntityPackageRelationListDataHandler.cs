using System;
using System.Collections.Generic;
using System.Linq;
using Pren.Web.Business.Data;
using Pren.Web.Business.DataAccess.Package.Entities;

namespace Pren.Web.Business.DataAccess.Package
{
    public class EntityPackageRelationListDataHandler : IPackageRelationListDataHandler
    {
        public IEnumerable<PackageRelationListEntity> GetPackageRelationLists(int packageRelationId)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                return dbContext.PackageRelationList
                    .Where(list => list.fkPackageRelationId.Equals(packageRelationId))
                    .Select(ConvertToPackageRelationListEntity)
                    .ToList();
            }
        }

        public PackageRelationListEntity GetPackageRelationList(int packageListId)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                var packageRelationList = dbContext.PackageRelationList.SingleOrDefault(list => list.Id.Equals(packageListId));

                return packageRelationList != null ? ConvertToPackageRelationListEntity(packageRelationList) : null;
            }
        }


        public void AddPackageRelationList(string name, int relationTypeId)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                dbContext.PackageRelationList.Add(new PackageRelationList()
                {
                    fkPackageRelationId = relationTypeId,
                    Name =  name,
                    Created = DateTime.Now,
                });

                dbContext.SaveChanges();
            }
        }

        public void DeletePackageRelationList(int relationListId)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                var itemsToRemove = dbContext.PackageRelationItem.Where(item => item.fkPackageRelationListId.Equals(relationListId));
                var listToRemove = dbContext.PackageRelationList.FirstOrDefault(list => list.Id.Equals(relationListId));

                dbContext.PackageRelationItem.RemoveRange(itemsToRemove);
                dbContext.PackageRelationList.Remove(listToRemove);

                dbContext.SaveChanges();
            }
        }        

        private PackageRelationListEntity ConvertToPackageRelationListEntity(PackageRelationList relationList)
        {
            return new PackageRelationListEntity
            {
                Id = relationList.Id,
                Name = relationList.Name,
                Created = relationList.Created
            };
        }
    }
}