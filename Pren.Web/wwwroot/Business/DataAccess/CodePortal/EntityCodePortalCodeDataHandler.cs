using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NuGet;
using Pren.Web.Business.Data;
using Pren.Web.Business.DataAccess.CodePortal.Entities;

namespace Pren.Web.Business.DataAccess.CodePortal
{
    public class EntityCodePortalCodeDataHandler : EntityDataHandlerBase, ICodePortalCodeDataHandler
    {
        public IEnumerable<CodeEntity> GetCodes(int listId)
        {
            return GetEntities<IEnumerable<CodeEntity>>(dbContext =>
                dbContext.CodePortalCode
                    .Where(code => code.fkListId.Equals(listId))
                    .Select(ConvertToCodeEntity)
                    .ToList()
            );
        }

        public int AddCodesToCodeList(int listId, List<CodeEntity> codes)
        {
            return AddOrUpdateEntities(dbContext =>
            {
                var codesToAdd = new Collection<CodePortalCode>();

                codesToAdd.AddRange(codes.Select(c =>
                    new CodePortalCode
                    {
                        fkListId = listId,
                        Code = c.Code
                    }));

                var existingCodes = dbContext.CodePortalCode.Where(code => code.CodePortalList.Id.Equals(listId));
                var uniqueCodes = codesToAdd.Where(t => !existingCodes.Any(code => code.Code.Equals(t.Code)));

                var addedCodes = dbContext.CodePortalCode.AddRange(uniqueCodes);
                var nrOfCodesAddes = addedCodes.Count();

                dbContext.SaveChanges();

                return nrOfCodesAddes;  
            });
        }

        public CodeEntity GetAvailableCode(int listId)
        {
            return GetEntities(dbContext =>
            {
                var availableCode = dbContext.CodePortalCode
                   .FirstOrDefault(code => code.fkListId.Equals(listId)
                       && code.UsedTime.Equals(null)
                       && code.UsedById.Equals(null));

                return ConvertToCodeEntity(availableCode);   
            });
        }

        public CodeEntity SetCodeAsUsed(int id, DateTime usedDateTime, string usedById, string usedByEmail)
        {
            return AddOrUpdateEntities(dbContext =>
            {
                var codeToUpdate = dbContext.CodePortalCode.SingleOrDefault(code => code.Id.Equals(id));

                if (codeToUpdate != null)
                {
                    codeToUpdate.UsedTime = usedDateTime;
                    codeToUpdate.UsedById = usedById;
                    codeToUpdate.UsedByEmail = usedByEmail;

                    dbContext.SaveChanges();

                    return ConvertToCodeEntity(codeToUpdate);
                }

                return null;
            });
        }

        public CodeEntity SetCodeAsUsed(int id, DateTime usedDateTime, string usedById)
        {
            return SetCodeAsUsed(id, usedDateTime, usedById, null);
        }

        public CodeEntity GetUsedCode(int listId, string userId)
        {
            return GetEntities(dbContext =>
            {
                var usedCode =
                    dbContext.CodePortalCode.Where(code => code.fkListId.Equals(listId) && code.UsedById.Equals(userId))
                        .OrderByDescending(t => t.Id)
                        .FirstOrDefault();

                if (usedCode == null)
                {
                    return null;
                }

                return ConvertToCodeEntity(usedCode);                
            });
        }

        private CodeEntity ConvertToCodeEntity(CodePortalCode code)
        {
            return new CodeEntity
                {
                    Code = code.Code,
                    Id = code.Id,           
                    ListId = code.fkListId,
                    UsedById = code.UsedById,
                    UsedTime = code.UsedTime,
                    UsedByEmail = code.UsedByEmail
                };
        }
    }
}