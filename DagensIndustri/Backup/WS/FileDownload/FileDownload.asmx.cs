using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

using DIClassLib.CardPayment;
using DIClassLib.DbHelpers;
using DIClassLib.DocTrackr;

namespace WS.FileDownload
{
    /// <summary>
    /// FileDownload: allows download of files if user is authenticated
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[System.Web.Script.Services.ScriptService]
    public class FileDownload : System.Web.Services.WebService
    {
        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}


        /// <summary>
        /// Will stream a file to the end user if provided 'issueDate' matches a file on the file server.
        /// On error: nothing is returned.
        /// </summary>
        /// <param name="issueDate">Date for Di-issue</param>
        /// <param name="servicePlusUserId">End users Service+ user id</param>
        /// <param name="ipNumber">End users IP-number</param>
        /// <param name="siteProvidedDownload">Site URL or system that end user downloads from</param>
        [WebMethod]
        public void DownloadFile(DateTime issueDate, string servicePlusUserId, string ipNumber, string siteProvidedDownload)
        {
            var util = new DocTrackrUtil();
            util.StreamFile(issueDate, servicePlusUserId, ipNumber, siteProvidedDownload);
        }


        //todo: create method for returning paper issue dates
        //public List<DateTime> GetIssuesInDateInterval(DateTime intervalStart, DateTime intervalEnd, Enum productName)
        //{
            
        //}

    }
}
