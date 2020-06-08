using System.Collections.Generic;
using System.Threading.Tasks;
using Pren.Web.Business.DataAccess.CodePortal.Entities;

namespace Pren.Web.Business.CodePortal
{
    public interface ICodePortalService
    {
        bool HasAccessToCode(string token, int listId);
        string GetNewCodeAndSetAsUsed(string token, int listId);
        string GetExistingCode(string userId, int listId);
        int AddCodeList(CodeListEntity codeList, List<CodeEntity> codes);
        IEnumerable<CodeEntity> GetCodes(int listId);
        CodeListEntity GetCodeList(int listId);
        IEnumerable<CodeListEntity> GetAllCodeLists();
        int AddCodesToCodeList(int codeListId, List<CodeEntity> codes);

        Task<string> CreateNewGiveAway(string token, int listId, string giveAwayTo);
        string GetExistingGiveAway(string userId, int listId);
    }
}