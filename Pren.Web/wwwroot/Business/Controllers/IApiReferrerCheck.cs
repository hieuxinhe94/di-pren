using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pren.Web.Business.Controllers
{
    public interface IApiReferrerCheck
    {
        bool VerifyDomain(string domainReferrer);
    }
}