using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DIClassLib.BonnierDigital
{
    public static class RequestHandler
    {
        private static Token _token;
        private static string _getAccessToken 
        {
            get
            {
                if (_token != null && _token.Expires > DateTime.Now)
                    return _token.access_token;

                _token = MakeTokenRequest();

                return _token.access_token;
            }
        }

        
        public static string WebReqPostJson(string url, string json)
        {
            try
            {
                string ret = "";
                string token = _getAccessToken;
                url = url + "?access_token=" + token;

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";

                Stream str = req.GetRequestStream();
                byte[] byteData = Encoding.UTF8.GetBytes(json);
                str.Write(byteData, 0, byteData.Length);
                str.Close();

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader rdr = new StreamReader(resp.GetResponseStream());
                ret = rdr.ReadToEnd();
                rdr.Close();
                
                return CleanUpJson(ret);
            }
            catch (Exception ex)
            {
                new Logger("WebReqPostJson() failed for url: " + url, ex.ToString());
            }

            return string.Empty;
        }


        public static string AddAccessTokenToUrl(string url)
        {
            string token = _getAccessToken;
            url = url + "?access_token=" + token;
            return url;
        }

        public static string WebReqPostParam(string url, string parameters)
        {
            try
            {
                string ret = "";
                string token = _getAccessToken;
                url = url + "?access_token=" + token;

                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";

                byte[] byteArray = Encoding.UTF8.GetBytes(parameters);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                string tmp = ((HttpWebResponse)response).StatusDescription;

                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                ret = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                return CleanUpJson(ret);
            }
            catch (Exception ex)
            {
                new Logger("WebReqPostParam() failed for url: " + url, "parameters: " + parameters + Environment.NewLine + ex.ToString());
                return null;
            }
        }

        public static SearchOutput SearchByEmail(string email)
        {
            SearchOutput ret = new SearchOutput();
            string url = ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString();
            string query = "q=email_s:" + email;
            
            string json = MakeRequestByGet(url, query);

            //something goes wrong or email not in BD-system - 404 is returned
            if (string.IsNullOrEmpty(json))
            {
                ret = new SearchOutput()
                {
                    httpResponseCode = "404",
                    numItems = "0",
                    query = query,
                    requestId = "",
                    startIndex = "",
                    totalItems = "0"
                };
                json = new JavaScriptSerializer().Serialize(ret);
            }
            
            return ret.GetSearchOutput(json);
        }
        /// <summary>
        /// Get a user's all entitlements
        /// </summary>
        /// <param name="servicePlusUserId">This ID can be retrieved by calling SearchByEmail() method with user's emailaddress</param>
        public static List<EntitlementOutputData2> GetUserEntitlements(string servicePlusUserId)
        {
            var url = ConfigurationManager.AppSettings["BonDigUrlCreateEntitlement"];
            var query = "q=userId_s:" + servicePlusUserId;
            var json = MakeRequestByGet(url, query);
            if (string.IsNullOrEmpty(json)) //No entitlements on user, or something went wrong
            {
                return new List<EntitlementOutputData2>();
            }
            var allEntitlements = new EntitlementOutput2();

            //TODO: Bug in S+ so make it a list (if only 1 item) so can use the more "correct" EntitlementOutput2() instead of EntitlementOutput()
            //Hack to fix S+ wrongly formatted JSON
            json = json.Replace("\"entitlements\":{", "\"entitlements\":[{");
            var checkChar = json.Substring(json.Length - 2, 1);
            if (checkChar != "]")
            {
                json = json.Insert(json.Length - 1, "]");
            }
            json = json.Replace("\"productTags\":\"", "\"productTags\":[\"");
            json = json.Replace("\",\"validFrom\":", "\"],\"validFrom\":");
            //End hack
            var entitlementOutput2Result = allEntitlements.GetEntitlementOutput(json);
            return entitlementOutput2Result.entitlements;
        }

        /// <summary>
        /// Extracts all external ids from a user's all entitlements
        /// </summary>
        /// <param name="servicePlusUserId">User's id in Service Plus</param>
        /// <returns>
        /// List of all unique external ids that are of type long. 
        /// If list contains more than 1 different, there is some problem as user should have same external ids on all entitlements!
        /// </returns>
        public static List<long> GetUserEntitlementsExternalIds(string servicePlusUserId)
        {
            var returnList = new List<long>();
            var entitlements = GetUserEntitlements(servicePlusUserId);
            foreach (var entitlement in entitlements)
            {
                long tmpCusno;
                if (!long.TryParse(entitlement.externalSubscriberId, out tmpCusno))
                {
                    continue; // externalSubscriberId was not of type long
                }
                if (!returnList.Contains(tmpCusno)) //Do not store same externalSubscriberId twice
                {
                    returnList.Add(tmpCusno);
                }
            }
            return returnList;
        }

        public static UserOutput GetUserByToken(string token)
        {
            //empty token - 404 is returned  
            if (string.IsNullOrEmpty(token))
            {
                return GetNoDataUserOutput();
            }

            UserOutput userOutput = new UserOutput();

            string url = ConfigurationManager.AppSettings["BonDigUrlUsersCurrent"].ToString() + "?access_token=" + token;
            
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/json";
                
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string json = sr.ReadToEnd();
                sr.Close();

                JavaScriptSerializer js = new JavaScriptSerializer();
                userOutput = js.Deserialize<UserOutput>(CleanUpJson(json));
                return userOutput;
            }
            catch (Exception ex)
            {
                new Logger("GetUserByToken() failed for url: " + url, ex.ToString());
                
                //not data in S+ - 404 is returned                
                return GetNoDataUserOutput();
            }
        }

        private static UserOutput GetNoDataUserOutput()
        {
            var userOutput = new UserOutput()
            {
                httpResponseCode = "404",
                requestId = "",
                user = null
            };

            return userOutput;
        }

        public static List<long> TryGetCirixCusnosFromBonDig(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                new Logger("TryGetCirixCusnosFromBonDig() called with empty token");
                return new List<long>();
            }

            HashSet<long> cusnos = new HashSet<long>();
            ExternalIdsOutput extOut = GetExternalIds(token);
            foreach (ExternalIds ext in extOut.externalIds)
            {
                //not all products in bonDig system has cirix cusnos in externalSubscriberId field
                if (Settings.BonDigExtProdIdsWithCirixCusnos.Contains(ext.externalProductId))
                {
                    long extCusno = 0;
                    long.TryParse(ext.externalSubscriberId, out extCusno);
                    if (extCusno > 0)
                        cusnos.Add(extCusno);
                }
            }

            return cusnos.ToList();
        }
        
        private static ExternalIdsOutput GetExternalIds(string token)
        {
            ExternalIdsOutput ret = new ExternalIdsOutput();
            string url = ConfigurationManager.AppSettings["BonDigUrlExternalIds"] + "?access_token=" + token;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/json";
                
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string json = sr.ReadToEnd();
                sr.Close();

                //make it a list (if only 1 item)
                if (!json.Contains("\"externalIds\":["))
                {
                    json = json.Replace("\"externalIds\":", "\"externalIds\":[");
                    json = json.Insert(json.Length - 1, "]");
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                ret = js.Deserialize<ExternalIdsOutput>(CleanUpJson(json));

                return ret;
            }
            catch (Exception ex)
            {
                string exStr = ex.ToString();
                
                //404 not interesting (thrown when no externalIds are found)
                if (!exStr.Contains("(404) Not Found") && !exStr.Contains("(401) Unauthorized"))
                    new Logger("GetExternalIds() failed for url: " + url, exStr);
                
                ret = new ExternalIdsOutput() 
                { 
                    httpResponseCode="404",
                    requestId = "",
                    totalItems = "0",
                    externalIds = new List<ExternalIds>()
                };
                return ret;
            }
        }


        private static string MakeRequestByGet(string url, string query)
        {
            try
            {
                string ret = "";
                string token = _getAccessToken;

                url = url + "?access_token=" + token + "&" + query;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/json";

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                ret = sr.ReadToEnd();
                sr.Close();

                return CleanUpJson(ret);
            }
            catch (Exception ex)
            {
                //new Logger("MakeRequestByGet() failed for url: " + url, ex.ToString());
                return null;
            }
        }

        private static Token MakeTokenRequest()
        {
            Token token = new Token();
            
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["BonDigUrlCreateToken"].ToString());
                request.Method = "POST";

                Stream sw = request.GetRequestStream();
                byte[] byteData = Encoding.ASCII.GetBytes(GetRequestArgs(token.RequestKV));
                sw.Write(byteData, 0, byteData.Length);
                sw.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string json = sr.ReadToEnd();
                sr.Close();

                JavaScriptSerializer js = new JavaScriptSerializer();
                token = js.Deserialize<Token>(CleanUpJson(json));
                token.Expires = DateTime.Now.AddSeconds(double.Parse(token.expires_in));
            }
            catch (Exception ex)
            {
                new Logger("MakeTokenRequest() failed", ex.ToString());
            }

            return token;
        }

        private static string GetRequestArgs(List<KeyVal> requestKV)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (KeyVal kv in requestKV)
            {
                if (i > 0)
                    sb.Append("&");

                sb.Append(kv.Key + "=" + kv.Value);
                i++;
            }

            return sb.ToString();
        }

        private static string CleanUpJson(string json)
        {
            json = json.Replace("@httpResponseCode", "httpResponseCode");
            json = json.Replace("@requestId", "requestId");
            return json;
        }


        public static Dictionary<String, Object> GetVerifyEntitlement(string externalResourceId, string token)
        {

            string url = BonDigMisc.GetVerifyEntitlementUrl(externalResourceId, token);

            try
            {
                #region example of returns from S+
                //string json = "{\"@httpResponseCode\":\"200\",\"@requestId\":\"0DDOkWMRv2sYbXWzUKO7ls\",\"entitled\":\"true\"}";
                //string json = "{\"@httpResponseCode\":\"401\",\"@errorCode\":\"UNAUTHORIZED\",\"@requestId\":\"6xH7XR5qwWXncrtli6EP6n\",\"errorMsg\":\"No valid entitlement for resource\",\"entitled\":\"true\"}";
                //string json = "{\"@httpResponseCode\":\"401\",\"@errorCode\":\"UNAUTHORIZED\",\"@requestId\":\"6xH7XR5qwWXncrtli6EP6n\",\"errorMsg\":\"Concurrent limit session of resource exceeded\",\"entitled\":\"true\",\"error\":\"concurrent_exceeded\"}";
                #endregion

                string json = "";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/json";

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                json = sr.ReadToEnd();
                sr.Close();

                return TryGetDicFromJson(json);
            }
            catch (WebException ex)
            {
                //needed to catch 401 returns
                string json = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return TryGetDicFromJson(json);
            }
            catch (Exception ex)
            {
                new Logger("VerifyEntitlementRequest() failed for url: " + url, ex.ToString());
                return null;
            }
        }

        private static Dictionary<string, object> TryGetDicFromJson(string json)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Object obj = serializer.DeserializeObject(json);
                Dictionary<String, Object> dic = (Dictionary<String, Object>)obj;
                return dic;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string TryGetDicValByKey(Dictionary<String, Object> dic, string key)
        {
            try
            {
                return dic[key].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        //private static string GetAccessToken()
        //{
        //    if (_token != null && _token.Expires > DateTime.Now)
        //        return _token.access_token;

        //    _token = MakeTokenRequest();

        //    return _token.access_token;
        //}

        //public static string SearchByEmail(string email)
        //{
        //    string url = ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString();
        //    string q = "q=email_s:" + email;
        //    return MakeRequestByGet(url, q);
        //}
    }
}
