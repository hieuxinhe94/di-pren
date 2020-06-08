using System;
using System.Collections.Generic;
using Pren.Web.Business.DataAccess.CodePortal.Entities;

namespace Pren.Web.Business.DataAccess.CodePortal
{
    public interface ICodePortalCodeDataHandler
    {
        IEnumerable<CodeEntity> GetCodes(int listId);
        int AddCodesToCodeList(int listId, List<CodeEntity> codes);
        CodeEntity GetAvailableCode(int listId);
        CodeEntity SetCodeAsUsed(int id, DateTime usedDateTime, string usedById);
        CodeEntity SetCodeAsUsed(int id, DateTime usedDateTime, string usedById, string usedByEmail);
        CodeEntity GetUsedCode(int listId, string userId);
    }
}