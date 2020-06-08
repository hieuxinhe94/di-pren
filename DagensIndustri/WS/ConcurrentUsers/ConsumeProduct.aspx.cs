using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using DIClassLib.DbHelpers;


namespace WS.ConcurrentUsers
{
    public partial class ConsumeProduct : System.Web.UI.Page
    {
        public string AppId                 { get { return DropDownListAppIds.SelectedItem.Value; } }
        public string ExtResId              { get { return DropDownListExtResIds.SelectedItem.Value; } }
        public string UrlToThisPage         { get; set; } //url to current page without url parameters
        public string UserToken             { get; set; }

        public string UrlCheckLoggedIn      { get { return "http://account.qa.newsplus.se/check-logged-in?callback=" + UrlToThisPage; } }
        public string UrlVerifyEntitlement  { get { return "http://api.qa.newsplus.se/v1/resources/verify-entitlement?externalResourceId=" + ExtResId + "&access_token=" + UserToken; } }
        public string UrlLogin              { get { return "http://account.qa.newsplus.se/login" + "?appId=" + AppId + "&lc=sv&callback=" + UrlToThisPage; } }
        public string UrlLogout             { get { return "http://account.qa.newsplus.se/logout?callback=" + UrlToThisPage; } }



        protected void Page_Load(object sender, EventArgs e)
        {
            UrlToThisPage = HttpUtility.UrlEncode(Request.Url.GetLeftPart(UriPartial.Path));  //address to this page, without URL params
            UserToken = Request["token"];
            ButtonLogout.Visible = false;
            HandlePanels(false, false, false);


            //todo: might be better to get token from S+ API instead of going to check-logged-in page
            //token does not exist. Go to S+ check-logged-in page. Will return token in url if user is logged in.
            if (UserToken == null)
                Response.Redirect(UrlCheckLoggedIn);

            //token exists but is empty: user is not logged in
            if (UserToken == "")
                HandlePanels(true, false, false);   //show log in

            //token has value: user is logged in
            if (!string.IsNullOrEmpty(UserToken))
            {
                ButtonLogout.Visible = true;

                //check if user is allowed to consume resource
                Dictionary<String, Object> servicePlusDic = VerifyEntitlementRequest();
                if (servicePlusDic == null)
                {
                    LiteralNotEntitledMess.Text = "Cannot verify that user should be allowed to consume resource. No valid return from S+.";
                    HandlePanels(false, true, false);   //show not entitled
                    return;
                }

                string respCode = TryGetDicValByKey(servicePlusDic, "@httpResponseCode");
                if (respCode == "200")
                {
                    HandlePanels(false, false, true);   //show entitled
                    return;
                }
                if (respCode == "401")
                {
                    string error = TryGetDicValByKey(servicePlusDic, "error");
                    string errorMsg = TryGetDicValByKey(servicePlusDic, "errorMsg");
                    if (error == "")
                    {
                        //user does not have entitlement to consume resource
                        LiteralNotEntitledMess.Text = errorMsg;
                    }
                    else
                    {
                        //to many users online with same account
                        LiteralNotEntitledMess.Text = errorMsg + "<br>You need to log out and then log in again to take control of your account";
                    }

                    HandlePanels(false, true, false);   //show not entitled
                }
            }
        }

        /// <summary>
        /// Makes call to S+ verify-entitlement service using current externalResourceId and access_token.
        /// </summary>
        /// <returns>Dictionary[String, Object] object. If ["error", "concurrent_exceeded"] exist in return object: notify user that logout + login is required to retain access to resource.</returns>
        private Dictionary<String, Object> VerifyEntitlementRequest()
        {
            #region example of returns from S+
            //"{\"@httpResponseCode\":\"200\",\"@requestId\":\"0DDOkWMRv2sYbXWzUKO7ls\",\"entitled\":\"true\"}";
            //"{\"@httpResponseCode\":\"401\",\"@errorCode\":\"UNAUTHORIZED\",\"@requestId\":\"6xH7XR5qwWXncrtli6EP6n\",\"errorMsg\":\"No valid entitlement for resource\",\"entitled\":\"true\"}";
            //"{\"@httpResponseCode\":\"401\",\"@errorCode\":\"UNAUTHORIZED\",\"@requestId\":\"6xH7XR5qwWXncrtli6EP6n\",\"errorMsg\":\"Concurrent limit session of resource exceeded\",\"entitled\":\"true\",\"error\":\"concurrent_exceeded\"}";
            #endregion

            try
            {
                string json = "";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(UrlVerifyEntitlement);
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
                new Logger("VerifyEntitlementRequest() failed for url: " + UrlVerifyEntitlement, ex.ToString());
                return null;
            }
        }

        private Dictionary<string, object> TryGetDicFromJson(string json)
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

        private string TryGetDicValByKey(Dictionary<String, Object> dic, string key)
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

        private void HandlePanels(bool showNotLoggedIn, bool showNotEntitled, bool showEntitled)
        {
            PlaceHolderNotLoggedIn.Visible = showNotLoggedIn;
            PlaceHolderNotEntitled.Visible = showNotEntitled;
            PlaceHolderEntitled.Visible = showEntitled;

            if (showEntitled)
                LabelConsumeTime.Text = DateTime.Now.ToString();
        }


        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlLogin);
        }

        protected void ButtonLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlLogout);
        }

        protected void ButtonConsume_Click(object sender, EventArgs e)
        {
            //will cause postback (run logic in Page_Load)
        }
    }
}