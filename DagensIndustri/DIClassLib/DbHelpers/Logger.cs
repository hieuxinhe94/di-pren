using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;


namespace DIClassLib.DbHelpers
{
    public class Logger
    {

        private string _connStr = "DagensIndustriMISC";



        /// <summary>
        /// log ERRORS
        /// </summary>
        /// <param name="description"></param>
        /// <param name="exception"></param>
        public Logger(string description, string exception)
        {
            try
            {
                string fileName = string.Empty;
                try
                {
                    fileName = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.FilePath);
                }
                catch (Exception e)
                {
                    //do nothing
                }

                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new SqlParameter("@errorFileName", fileName),
                    new SqlParameter("@description", description),
                    new SqlParameter("@exception", exception),
                };

                SqlHelper.ExecuteNonQuery(_connStr, "DoLog", sqlParameters);
            }
            catch (Exception ex)
            {
                //Do nothing
                //System.Web.HttpContext.Current.Response.Write("ERROR: " + ex.ToString());
            }
        }


        /// <summary>
        /// log INFO - set appsettingsValue writeToInfoLog=true to activate
        /// </summary>
        public Logger(string info)
        {
            bool writeLog = false;
            bool.TryParse(Misc.MiscFunctions.GetAppsettingsValue("writeToInfoLog"), out writeLog);
            if (!writeLog)
                return;

            string fileName = string.Empty;
            try
            {
                fileName = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.FilePath);
            }
            catch (Exception e)
            {
                //do nothing
            }

            try
            {                
                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new SqlParameter("@fileName", fileName),
                    new SqlParameter("@info", info),
                };

                SqlHelper.ExecuteNonQuery(_connStr, "DoLogInfo", sqlParameters);
            }
            catch (Exception ex)
            {
                //Do nothing
                //throw ex;
            }
        }

        /// <summary>
        /// Log user events (button-clicks etc). Event id's are found in Settings.cs. Log is stored in dbDagensIndustriMISC / LogEvent.
        /// </summary>
        public Logger(int eventId, long? cusno, bool isEventSuccess)
        {
            string fileName = string.Empty;
            try
            {
                fileName = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.FilePath);
            }
            catch (Exception e)
            {
                //do nothing
            }
            
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new SqlParameter("@eventId", eventId),
                    new SqlParameter("@cusno", cusno),
                    new SqlParameter("@isEventSuccess", isEventSuccess),
                    new SqlParameter("@fileName", fileName)
                };

                if (cusno == null)
                    sqlParameters[1] = new SqlParameter("@cusno", DBNull.Value);


                SqlHelper.ExecuteNonQuery(_connStr, "DoLogEvent", sqlParameters);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("eventId=" + eventId.ToString());

                if (cusno != null)  sb.Append(", cusno=" + cusno.ToString());
                else                sb.Append(", cusno=null");

                sb.Append(", isEventSuccess=" + isEventSuccess.ToString());
                sb.Append(", fileName=" + fileName);

                new Logger("Log event failed. " + sb.ToString(), ex.ToString());
            }
        }
    }
}