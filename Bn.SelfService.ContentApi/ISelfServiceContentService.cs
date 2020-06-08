using System.Collections.Generic;
using Bn.SelfService.ContentApi.Models;

namespace Bn.SelfService.ContentApi
{
    public interface ISelfServiceContentService
    {
        IEnumerable<Alert> GetAlerts(string brand);
        IEnumerable<Teaser> GetTeasers(string brand);
        IEnumerable<Code> GetCodes(string brand);
        IEnumerable<RightColumnTeaser> GetRightColumnTeasers(string brand);
        string GetText(string brand, string type);
    }
}