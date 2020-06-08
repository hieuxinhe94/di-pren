using System.Collections.Generic;
using Pren.Web.Business.DataAccess.CodePortal.Entities;

namespace Pren.Web.Business.DataAccess.CodePortal
{
    public interface ICodePortalListDataHandler
    {
        IEnumerable<CodeListEntity> GetAllCodeLists();
        CodeListEntity GetCodeList(int listId);
        int AddCodeList(CodeListEntity codeList, List<CodeEntity> codes);
    }
}