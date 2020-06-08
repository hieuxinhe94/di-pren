using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Pren.Web.Business.Data;
using Pren.Web.Business.DataAccess.Package.Entities;

namespace Pren.Web.Business.DataAccess.Package
{
    public class EntityPackageRelationItemDataHandler : IPackageRelationItemDataHandler
    {
        public IEnumerable<PackageRelationItemEntity> GetPackageRelationItems(int packageListId)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                return dbContext.PackageRelationItem
                    .Where(item => item.fkPackageRelationListId.Equals(packageListId))
                    .Select(ConvertToPackageRelationItemEntity)
                    .ToList();
            }
        }

        public IEnumerable<PackageRelationItemEntity> GetPackageRelationItems(int packageRelationId, string packageListName)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                return dbContext.PackageRelationItem
                    .Where(item => item.PackageRelationList.Name.Equals(packageListName) && item.PackageRelationList.fkPackageRelationId.Equals(packageRelationId))
                    .Select(ConvertToPackageRelationItemEntity)
                    .ToList();
            }
        }

        public PackageRelationItemEntity GetPackageRelationItem(int packageItemId)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                var packageRelationItem = dbContext.PackageRelationItem.SingleOrDefault(list => list.Id.Equals(packageItemId));

                return packageRelationItem != null ? ConvertToPackageRelationItemEntity(packageRelationItem) : null;
            }
        }

        public void AddPackageRelationItem(int relationListId, string name, bool wildCardBefore, string conditionBefore, bool wildCardAfter,
            string conditionAfter)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                var packageRelationList =
                    dbContext.PackageRelationList.SingleOrDefault(list => list.Id.Equals(relationListId));

                dbContext.PackageRelationItem.Add(new PackageRelationItem()
                {
                    fkPackageRelationListId = relationListId, 
                    Name = name,
                    ConditionBefore = conditionBefore,
                    WildcardBefore = wildCardBefore,
                    ConditionAfter = conditionAfter,
                    WildcardAfter = wildCardAfter,
                    Created = DateTime.Now                    
                });

                dbContext.SaveChanges();
            }
        }

        public void UpdatePackageRelationItem(int packageItemId, string name)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                var packageRelationItem = dbContext.PackageRelationItem.SingleOrDefault(item => item.Id.Equals(packageItemId));

                if (packageRelationItem == null)
                    return;

                packageRelationItem.Name = name;
                dbContext.PackageRelationItem.AddOrUpdate(packageRelationItem);

                dbContext.SaveChanges();
            }
        }

        public void DeletePackageRelationItem(int packageItemId)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                var packageRelationItem = dbContext.PackageRelationItem.SingleOrDefault(item => item.Id.Equals(packageItemId));

                dbContext.PackageRelationItem.Remove(packageRelationItem);

                dbContext.SaveChanges();
            }
        }

        private PackageRelationItemEntity ConvertToPackageRelationItemEntity(PackageRelationItem relationItem)
        {
            return new PackageRelationItemEntity
            {
                Id = relationItem.Id,
                Name = relationItem.Name,
                WildcardBefore = relationItem.WildcardBefore,
                ConditionBefore = relationItem.ConditionBefore,
                WildcardAfter = relationItem.WildcardAfter,
                ConditionAfter = relationItem.ConditionAfter,
                Created = relationItem.Created
            };
        }
    }
}