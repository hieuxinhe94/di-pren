using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pren.Web.Business.Mail
{
    public interface IExternalMailTrigger
    {
        Task<TriggerExternalMailResult> InvokeExternalMailAsync(Dictionary<string, string> parameters, string workflowId);
    }
}