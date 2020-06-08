using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WS.Foretagarna
{
    /// <summary>
    /// Summary description for Foretagarna
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Foretagarna : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "HelloWorld";
        }
    }
}
