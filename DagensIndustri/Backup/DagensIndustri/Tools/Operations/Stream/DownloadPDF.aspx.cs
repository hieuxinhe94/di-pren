using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.ServiceModel.Description;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Data.SqlClient;

using DIClassLib.DbHandlers;
using DIClassLib.DocTrackr;

using EPiServer.Core;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Membership;

namespace DagensIndustri.Tools.Operations.Stream
{
    public partial class DownloadPDF : EPiServer.TemplatePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string issueID = Request["issueid"];
            bool isAppendix = Request["appendix"] != null ? Request["appendix"].Equals(bool.TrueString.ToLower()) : false;
            bool isTomorrow = Request["tomorrow"] != null ? Request["tomorrow"].Equals(bool.TrueString.ToLower()) : false;

            string path = ConfigurationManager.AppSettings["PDFArchivePath"];

            //no need to go to special dir for tomorrows issue - all files should be in place at 22.00
            //if (isTomorrow)
            //    path = ConfigurationManager.AppSettings["PDFPaperPathTomorrow"];

            if (isAppendix)
            {
                MiscFunctions.StreamPdf(path + "Bilagor\\" + issueID + ".pdf", issueID);
            }
            else if (CheckAccess(issueID))
            {
                //140227 - replaced
                //MiscFunctions.StreamPdf(path + issueID + ".pdf", issueID);

                DateTime dt = DateTime.MinValue;
                try
                {
                    //remove text in file name, eg: 'morgondagen-20140303' => '20140303'
                    int i = issueID.LastIndexOf('-');
                    if (i > 0)
                        issueID = issueID.Substring(i + 1);
                    
                    dt = new DateTime(int.Parse(issueID.Substring(0, 4)), int.Parse(issueID.Substring(4, 2)), int.Parse(issueID.Substring(6, 2)));
                }
                catch (Exception ex) 
                {
                    new Logger("OnLoad - failed to parse date for issueID: " + issueID, ex.ToString());
                    return;
                }
                
                var user = HttpContext.Current.User.Identity.Name;
                var cusno = MembershipDbHandler.GetCusno(user);
                if (cusno > 0) 
                    user = cusno.ToString();

                //stream docTrackr file if available, else orgFile will be used
                var util = new DocTrackrUtil();
                util.StreamFile(dt, user, HttpContext.Current.Request.UserHostAddress, "dagensindustri.se");
            }
            else
            {
                ShowMessage(Translate("/download/nodownloadleft"));
            }
        }

        private bool CheckAccess(string issueId)
        {
            //Only gain access if user is authenticated
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiSms24Hour))
                {
                    SqlDataReader DR = null;

                    try
                    {
                        //Get username and remove prefix
                        string userName = HttpContext.Current.User.Identity.Name.Replace(MiscFunctions.GetAppsettingsValue("SmsUser") + "__", "");
                        DR = SqlHelper.ExecuteReader("DisePren", "DISESubtractDIDownload", new SqlParameter("@userid", userName));

                        while (DR.Read())
                        {
                            if (DR["result"] != System.DBNull.Value && (int)DR["result"] > 0)
                                return true;
                            else
                                return false;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        new Logger("checkAccess(string issueId) - failed", ex.ToString());
                        return false;
                    }
                    finally
                    {
                        if (DR != null)
                            DR.Close();
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                //Log if not authorized user trying to download pdf
                new Logger("Not authorized user trying to download pdf. IP: " + Request.UserHostAddress, "");
                return false;
            }
        }

        private void ShowMessage(string messageText)
        {
            LblMessage.Text = messageText;
            contentHolder.Visible = true;
        }
    }
}