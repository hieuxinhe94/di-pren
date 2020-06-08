using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.DataAccess.CodePortal.Entities;
using Pren.Web.Business.Mail;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;

namespace Pren.Web.Business.CodePortal
{
    public class CodePortalService : ICodePortalService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly IExternalMailTrigger _externalMailTrigger;
        private readonly ISessionData _sessionData;

        public CodePortalService(IDataAccess dataAccess, IServicePlusFacade servicePlusFacade, IExternalMailTrigger externalMailTrigger, ISessionData sessionData)
        {
            _dataAccess = dataAccess;
            _servicePlusFacade = servicePlusFacade;
            _externalMailTrigger = externalMailTrigger;
            _sessionData = sessionData;
        }

        public bool HasAccessToCode(string token, int listId)
        {
            Contract.Assume(!string.IsNullOrEmpty(token), "token cannot be null or empty");
            Contract.Assume(listId > 0, "listId must be > 0");

            var list = _dataAccess.CodePortalListDataHandler.GetCodeList(listId);

            if (list == null)
            {
                return false;
            }

            var resourceId = list.ResourceId;

            return _servicePlusFacade.VerifyEntitlement(resourceId, token);
        }

        public string GetNewCodeAndSetAsUsed(string token, int listId)
        {
            var code = GetCodeAndSetAsUsed(token, listId);

            return code == null ? "" : code.Code;
        }

        public string GetExistingCode(string userId, int listId)
        {
            var usedCode = GetCode(userId, listId);

            if (usedCode == null)
            {
                return string.Empty;
            }

            return usedCode.Code;
        }

        public int AddCodesToCodeList(int codeListId, List<CodeEntity> codes)
        {
            Contract.Assume(codeListId > 0, "codeListId must be larger than 0");
            Contract.Assume(codes != null, "codes cannot be null");           
 
            return _dataAccess.CodePortalCodeDataHandler.AddCodesToCodeList(codeListId, codes);
        }

        public async Task<string> CreateNewGiveAway(string token, int listId, string giveAwayTo)
        {
            var giveAwayCode = GetCodeAndSetAsUsed(token, listId);

            if (giveAwayCode == null)
            {
                return "";
            }

            var list = _dataAccess.CodePortalListDataHandler.GetCodeList(listId);
            var isDigital = list.Name.ToLower().StartsWith("[digital]");
            var workflowId = isDigital ? "DI_Give_Away_Campaign_Extra" : "DI_Give_Away_Campaign_Total";
            var parameters = GetGiveAwayParameters(giveAwayCode, giveAwayTo);

            var result = await _externalMailTrigger.InvokeExternalMailAsync(parameters, workflowId);

            if (!result.Success) return "Ett fel uppstod - inbjudan kan inte skickas";

            _dataAccess.CodePortalGiveAwayDatahandler.AddGiveAway(
                new GiveAwayEntity
                {
                    CodeId = giveAwayCode.Id,
                    GiveAwayTo = giveAwayTo
                });

            return giveAwayTo;
        }

        public string GetExistingGiveAway(string userId, int listId)
        {
            var usedCode = GetCode(userId, listId);

            if (usedCode == null)
            {
                return string.Empty;
            }

            var giveAway = _dataAccess.CodePortalGiveAwayDatahandler.GetGiveAway(usedCode.Id);

            if (giveAway == null)
            {
                return string.Empty;
            }

            return giveAway.GiveAwayTo;
        }

        public int AddCodeList(CodeListEntity codeList, List<CodeEntity> codes)
        {
            Contract.Assume(codeList != null, "codeList cannot be null");
            Contract.Assume(codes != null, "codes cannot be null");
            Contract.Assume(codes.Any(), "codes must contain at least one CodeEntity");

            return _dataAccess.CodePortalListDataHandler.AddCodeList(codeList, codes);
        }

        public IEnumerable<CodeEntity> GetCodes(int listId)
        {
            Contract.Assume(listId > 0, "listId must be > 0");

            return _dataAccess.CodePortalCodeDataHandler.GetCodes(listId);
        }

        public CodeListEntity GetCodeList(int listId)
        {
            Contract.Assume(listId > 0, "listId must be > 0");

            return _dataAccess.CodePortalListDataHandler.GetCodeList(listId);
        }

        public IEnumerable<CodeListEntity> GetAllCodeLists()
        {
            return _dataAccess.CodePortalListDataHandler.GetAllCodeLists();
        }

        private CodeEntity GetCode(string userId, int listId)
        {
            Contract.Assume(!string.IsNullOrEmpty(userId), "userid cannot be null or empty");
            Contract.Assume(listId > 0, "listId must be > 0");

            return _dataAccess.CodePortalCodeDataHandler.GetUsedCode(listId, userId);
        }

        private CodeEntity GetCodeAndSetAsUsed(string token, int listId)
        {
            var user = _servicePlusFacade.GetUserByToken(token);

            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return null;
            }

            var code = _dataAccess.CodePortalCodeDataHandler.GetAvailableCode(listId);
            if (code == null)
            {
                return null;
            }

            var updatedCode = _dataAccess.CodePortalCodeDataHandler.SetCodeAsUsed(code.Id, DateTime.Now, user.Id, user.Email);

            return updatedCode;
        }

        private Dictionary<string, string> GetGiveAwayParameters(CodeEntity giveAwayCode, string giveAwayTo)
        {
            var parameters = new Dictionary<string, string>
            {
                {"email", giveAwayTo.Trim()},
                {"url", giveAwayCode.Code},
                {"systemid", ""} //mandatory parameter, can be empty
            };

            var subscriber = (Subscriber)_sessionData.Get(SessionConstants.SubscriberSessionKey);

            if (subscriber?.SelectedSubscription?.KayakCustomer == null) return parameters;

            var customer = subscriber.SelectedSubscription.KayakCustomer;
            parameters.Add("firstname", customer.FirstName);
            parameters.Add("lastname", customer.LastName);

            return parameters;
        }
    }
}