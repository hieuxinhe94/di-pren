using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;

namespace WS.Autowithdrawal
{
    /// <summary>
    /// Summary description for PayTrans
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Autowithdrawal : WebService
    {
        [WebMethod]
        public bool InsertAutoWithdrawal(string accessToken, string aurigaSubsId, long cusno, long subsno, long campno, int includeInBatch, string pageGuid, DateTime? subsEndDate, DateTime dateSaved, DateTime? dateStopped)
        {
            if (!ValidateRequest(accessToken))
            {
                return false;
            }

            return InsertAwd(aurigaSubsId, cusno, subsno, campno,includeInBatch,pageGuid, subsEndDate,dateSaved,dateStopped);
        }

        private bool InsertAwd(string aurigaSubsId, long cusno, long subsno, long campno, int includeInBatch, string pageGuid, DateTime? subsEndDate, DateTime dateSaved, DateTime? dateStopped)        
        {
            try
            {
                var sqlParameters = new[]{
                    new SqlParameter("@aurigaSubsId", aurigaSubsId), 
                    new SqlParameter("@cusno", cusno), 
                    new SqlParameter("@subsno", subsno), 
                    new SqlParameter("@campno", campno),                     
                    new SqlParameter("@includeInBatch", includeInBatch),
                    new SqlParameter("@pageGuid", pageGuid),
                    new SqlParameter("@dateSubsEnd", subsEndDate ?? (object)DBNull.Value),
                    new SqlParameter("@dateSaved", dateSaved),
                    new SqlParameter("@dateStopped", dateStopped ?? (object)DBNull.Value)
                    };
                
                SqlHelper.ExecuteNonQuery("DisePren", "awd_InsertPushedAwd", sqlParameters);

                return true;
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append("aurigaSubsId:" + aurigaSubsId);
                sb.Append(", cusno:" + cusno);
                sb.Append(", subsno:" + subsno);
                sb.Append(", dateSubsEnd:" + subsEndDate);

                new Logger("WS - awd_InsertPushedAwd failed for " + sb + "<hr>", ex.ToString());

                return false;
            }
        }

        /// <summary>
        /// Make sure this logic is used for building accessToken in applications that calls any webservice that using this method
        /// </summary>
        /// <returns>True if accessToken is valid</returns>
        private bool ValidateRequest(string accessToken)
        {
            var secret = ConfigurationManager.AppSettings["InsertPayTransSecret"];
            if (string.IsNullOrEmpty(secret))
            {
                return false;
            }
            
            //The logic for correct accessToken
            var stringToProcess = string.Format("{0}_{1}_{2}_{3}", DIClassLib.Misc.MiscFunctions.GetMd5Hash(secret), DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var token = DIClassLib.Misc.MiscFunctions.GetMd5Hash(stringToProcess);
            return accessToken == token;
        }
    }
}