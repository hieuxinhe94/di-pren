using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.BonnierDigital;

namespace WS.SPlusProxy
{
    public partial class CreateToken : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Token token = new Token();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.bonnier.se/v1/oauth/token");
            request.Method = "POST";

            Stream sw = request.GetRequestStream();
            byte[] byteData = Encoding.ASCII.GetBytes(GetRequestArgs(token.RequestKV));
            sw.Write(byteData, 0, byteData.Length);
            sw.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string json = sr.ReadToEnd();
            sr.Close();

            litOutPut.Text = json;
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
    }

    public class Token
    {
        public List<KeyVal> RequestKV
        {
            get
            {
                List<KeyVal> urlKeyVals = new List<KeyVal>();
                urlKeyVals.Add(new KeyVal("client_id", "di"));
                urlKeyVals.Add(new KeyVal("client_secret", "L5atI4XY"));
                urlKeyVals.Add(new KeyVal("grant_type", "client_credentials"));
                return urlKeyVals;
            }
        }
        public DateTime Expires = DateTime.MinValue;

        public string httpResponseCode;
        public string requestId;
        public string access_token;
        public string token_type;
        public string expires_in;

        public Token() { }
    }
}