using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NuGet;
using Pren.Web.Business.Data;
using Pren.Web.Business.DataAccess.CodePortal.Entities;

namespace Pren.Web.Business.DataAccess.CodePortal
{
    public class EntityCodePortalListDataHandler : ICodePortalListDataHandler
    {
        public IEnumerable<CodeListEntity> GetAllCodeLists()
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                return dbContext.CodePortalList.Select(ConvertToCodeListEntity).ToList();
            }
        }

        public CodeListEntity GetCodeList(int listId)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                var codeList = dbContext.CodePortalList.SingleOrDefault(cl => cl.Id.Equals(listId));

                return codeList == null ? null : ConvertToCodeListEntity(codeList);
            }
        }

        public int AddCodeList(CodeListEntity codeList, List<CodeEntity> codes)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                var codesToAdd = new Collection<CodePortalCode>();
                
                codesToAdd.AddRange(codes.Select(c => 
                    new CodePortalCode
                    {
                        Code = c.Code
                    }));

                var listToAdd = dbContext.CodePortalList.Add(new CodePortalList
                {
                    Name = codeList.Name,
                    ResourceId = codeList.ResourceId,
                    ValidFrom = codeList.ValidFrom,
                    ValidTo = codeList.ValidTo,
                    CodePortalCode = codesToAdd,
                    Created = DateTime.Now
                });

                dbContext.SaveChanges();
                
                return listToAdd.Id;
            }                        
        }

        private CodeListEntity ConvertToCodeListEntity(CodePortalList codeList)
        {
            return new CodeListEntity
            {
                Id = codeList.Id,
                Name = codeList.Name,
                ValidFrom = codeList.ValidFrom,
                ValidTo = codeList.ValidTo,
                ResourceId = codeList.ResourceId,
                Created = codeList.Created
            };
        }
    }
}