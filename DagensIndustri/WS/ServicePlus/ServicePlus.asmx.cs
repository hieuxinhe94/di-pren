using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services;
using DIClassLib.BonnierDigital;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace WS.ServicePlus
{
    /// <summary>
    /// Summary description for PayTrans
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServicePlus : WebService
    {

        [WebMethod]
        public List<long> TryGetCirixCusnosFromBonDig(string token)
        {
            return RequestHandler.TryGetCirixCusnosFromBonDig(token);
        }

        [WebMethod]
        public UserOutput GetUserByToken(string token)
        {
            return RequestHandler.GetUserByToken(token);
        }

        [WebMethod]
        public string GetUserByTokenJson(string token)
        {
            string url = "https://api.bonnier.se/v1/users/current" + "?access_token=" + token;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/json";

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string json = sr.ReadToEnd();
                sr.Close();

                return json;

                //JavaScriptSerializer js = new JavaScriptSerializer();
                //userOutput = js.Deserialize<UserOutput>(CleanUpJson(json));
                //return userOutput;
            }
            catch (Exception ex)
            {                
                new Logger("GetUserByToken() failed for url: " + url, ex.ToString());

                return "";

                ////not data in S+ - 404 is returned
                //userOutput = new UserOutput()
                //{
                //    httpResponseCode = "404",
                //    requestId = "",
                //    user = null
                //};

                //return userOutput;
                //new JavaScriptSerializer().Serialize(userOutput);  //return userOutput as a string
            }
        }

        [WebMethod]
        public string GetVerifyEntitlement(string externalResourceId, string token)
        {
            string respCode = string.Empty;
            //return RequestHandler.GetVerifyEntitlement(externalResourceId, token);
            Dictionary<String, Object> sPlusDic = RequestHandler.GetVerifyEntitlement(externalResourceId, token);

            //no valid return from S+
            if (sPlusDic == null)
            {
                //LiteralMessage.Text = "Ett tekniskt fel uppstod. Vi kunde tyvärr inte verifiera att du har rätt att läsa PDF-tidningen.";                
                return "null";
            }

            return RequestHandler.TryGetDicValByKey(sPlusDic, "@httpResponseCode");
        }
    }
}