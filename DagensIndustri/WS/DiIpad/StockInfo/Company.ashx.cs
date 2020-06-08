using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS.DiIpad.StockInfo
{
    /// <summary>
    /// Summary description for Company
    /// </summary>
    public class Company : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            //? get specific company info by compnay name ?
            //string comp = context.Request.QueryString["comp"];

            context.Response.ContentType = "text/plain";
            context.Response.Write(RandomNumber().ToString());
        }

        private int RandomNumber()
        {
            int min = -100;
            int max = 100;
            Random random = new Random();
            return random.Next(min, max);
        }


        public bool IsReusable
        {
            get { return false; }
        }
    }
}