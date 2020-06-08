using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Tools.Admin.Conference
{
    /// <summary>
    /// used in epi tool to save admin comments for persons who have downloaded a conference PDF
    /// </summary>
    public class PdfCommentSaver : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = TryGetActionValueFromPostedForm(context);
            if (string.IsNullOrEmpty(action))
            {
                context.Response.Write("Tekniskt fel. Action-argument saknas.");
                context.Response.End();
            }

            if (action == "savePdfComment")
            {
                var cmtId = 0;
                int.TryParse(context.Request.Form["commentId"], out cmtId);
                
                var cmt = context.Request.Form["comment"];
                if (string.IsNullOrEmpty(cmt))
                    cmt = string.Empty;

                if (cmtId > 0)
                {
                    MsSqlHandler.UpdateConfPdfComment(cmtId, cmt);
                    context.Response.Write("Kommentaren har sparats.");
                    context.Response.End();
                }
                else
                {
                    context.Response.Write("Tekniskt fel. Id-argument saknas.");
                    context.Response.End();
                }
            }
            else
            {
                context.Response.Write("Tekniskt fel. Okänt action-argument.");
                context.Response.End();
            }
        }

        private string TryGetActionValueFromPostedForm(HttpContext context)
        {
            string s = context.Request.Form["action"];
            if (!string.IsNullOrEmpty(s))
                return s;

            return string.Empty;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}