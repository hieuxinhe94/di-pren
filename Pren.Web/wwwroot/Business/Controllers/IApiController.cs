using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Pren.Web.Business.Controllers
{
    public interface IApiController
    {
        bool VerifyDomain();
    }
}