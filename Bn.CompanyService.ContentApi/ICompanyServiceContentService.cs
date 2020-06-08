using System.Collections.Generic;
using Bn.CompanyService.ContentApi.Models;

namespace Bn.CompanyService.ContentApi
{
    public interface ICompanyServiceContentService
    {
        IEnumerable<Message> GetMessages(string brand);
    }
}