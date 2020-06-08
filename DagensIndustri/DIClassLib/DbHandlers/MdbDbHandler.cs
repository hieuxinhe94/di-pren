using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

using DIClassLib.DbHelpers;
//using DIClassLib.WineTipSms;
using DIClassLib.Mdb;
using DIClassLib.Misc;

namespace DIClassLib.DbHandlers
{
    public static class MdbDbHandler
    {
        /// <summary>
        /// Gets customers first- and last name
        /// </summary>
        /// <param name="cusno"></param>
        /// <returns>string[2] - [0]="FirstName", [1]="LastName"</returns>
        public static string[] GetCustomerName(long cusno)
        {
            string[] ret = new string[2];
            ret[0] = "";
            ret[1] = "";

            SqlConnection conn = null;
            SqlDataReader rdr = null;

            try
            {
                string sql = "select firstnameab, lastnameab from customer where cusno=" + cusno;
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MDB"].ToString());
                conn.Open();
                SqlCommand sqlComm = new SqlCommand(sql, conn);
                rdr = sqlComm.ExecuteReader();

                while (rdr.Read())
                {
                    if (!(rdr["firstnameab"] is DBNull))
                        ret[0] = rdr["firstnameab"].ToString();

                    if (!(rdr["lastnameab"] is DBNull))
                        ret[1] = rdr["lastnameab"].ToString();
                }

                return ret;
            }
            catch (Exception ex)
            {
                new Logger("GetCustomerName() failed", ex.ToString());
            }
            finally
            {
                if (rdr != null)    
                    rdr = null;

                if (conn != null)
                {
                    conn.Close();
                    conn = null;
                }
            }

            return null;
        }

        /// <summary>
        /// Customers that had a trial but do not want to continue after TM call
        /// </summary>
        public static List<CustomerDecline> GetCustomerDeclines(DateTime dateToFetch)
        {
            var cacheKey = "MdbDbHandler_GetCustomerDeclines_" + dateToFetch.ToString("yyyy-MM-dd");
            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (List<CustomerDecline>)data;
            }

            var list = new List<CustomerDecline>();
            //TODO: All fields are unfortunately varchar in the table, even column "tidpunkt", which force code below to parse dates etc.
            var sql = string.Format(@"SELECT 
                                        CONVERT(int,[tp_id]) as tp_id
                                      ,[kampanjID]
                                      ,[utfall]
                                      ,CONVERT (datetime, [tidpunkt]) as tidpunkt
                                      ,[produkt]
                                      ,[anledning]
                                      ,[säljbolag]
                                      ,[filnamn]
                                    FROM [ActionBase].[dbo].[abmt_t_S2] 
                                    WHERE  ([anledning] IS NULL OR [anledning] <> 'Avliden') AND [filnamn] LIKE '{0:yyyyMMdd}%' 
                                    ORDER BY [tidpunkt] DESC", dateToFetch);
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MDB"].ToString()))
                {
                    conn.Open();
                    using (var command = new SqlCommand(sql, conn))
                    {
                        var dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            var tableData = new CustomerDecline()
                            {
                                TpId = (int)dataReader["tp_id"],
                                DateTime = (DateTime)dataReader["tidpunkt"],
                                CampaignId = dataReader["kampanjID"].ToString(),
                                FileName = dataReader["filnamn"].ToString(),
                                Reason = dataReader["utfall"].ToString(),
                                Product = dataReader["produkt"].ToString(),
                                SalesCompany = dataReader["säljbolag"].ToString(),
                                Outcome = dataReader["utfall"].ToString()
                            };
                            list.Add(tableData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetCustomerDeclines() - failed", ex.Message);
            }
            // Cache list if it contains any data
            if (list.Any())
            {
                HttpRuntime.Cache.Insert(
                    cacheKey,
                    list,
                    null,
                    DateTime.Now.AddSeconds(Settings.CacheTimeSecondsLong),
                    Cache.NoSlidingExpiration);
            }
            return list;
        }

        /// <summary>
        /// Customers that do not want to get any emails = OptOut
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllOptOuts(int daysBack = 0,DateTime? fromDate = null, DateTime? toDate = null)
        {
            var cacheKey = string.Format("GetAllOptOuts_{0}_{1:yyyy-MM-dd}_{2:yyyy-MM-dd}", daysBack, fromDate, toDate);
            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (List<string>)data;
            }
            string sql;
            //daysBack overrides fromDate and toDate
            if (daysBack > 0)
            {
                sql = string.Format("SELECT distinct [SubscriberEmail] FROM [ActionBase].[dbo].[abmt_t_webmailWPOptoutsTotal] where DATEDIFF(d,[Inserted], GETDATE())<={0}", daysBack);
            }
            else if (fromDate == null)
            {
                sql = "SELECT distinct [SubscriberEmail] FROM [ActionBase].[dbo].[abmt_t_webmailWPOptoutsTotal]";
            }
            else
            {
                sql = toDate == null ?
                    string.Format("SELECT distinct [SubscriberEmail] FROM [ActionBase].[dbo].[abmt_t_webmailWPOptoutsTotal] where DATEDIFF(d,[Inserted], '{0}')<=0", fromDate) :
                    string.Format("SELECT distinct [SubscriberEmail] FROM [ActionBase].[dbo].[abmt_t_webmailWPOptoutsTotal] where DATEDIFF(d,[Inserted], '{0}')<=0 AND DATEDIFF(d,[Inserted], '{1}')>=0", fromDate, toDate);
            }

            var list = new List<string>();
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MDB"].ToString()))
                {
                    conn.Open();
                    using (var command = new SqlCommand(sql, conn))
                    {
                        var dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            list.Add(dataReader["SubscriberEmail"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetAllOptOuts() - failed", ex.Message);
            }
            // Cache list if it contains any data
            if (list.Any())
            {
                HttpRuntime.Cache.Insert(
                    cacheKey,
                    list,
                    null,
                    DateTime.Now.AddMinutes(Settings.CacheTimeMinutesLong),
                    Cache.NoSlidingExpiration);
            }
            return list;
        }

        public static bool IsEmailInOptOut(string email)
        {
            var optOutList = GetAllOptOuts();
            return optOutList.Any(optOutEmail => String.Equals(optOutEmail, email, StringComparison.CurrentCultureIgnoreCase));
        }

        //public static List<WineTipCustomer> GetWineTipCustomers(List<int> cusnos)
        //{
        //    List<WineTipCustomer> ret = new List<WineTipCustomer>();
        //    SqlConnection conn = null;
        //    SqlDataReader rdr = null;

        //    try
        //    {
        //        StringBuilder sql = new StringBuilder();
        //        sql.Append("select cusno, phonemobile from customer where cusno in (");

        //        for (int i = 0 ; i < cusnos.Count; i++)
        //        {
        //            sql.Append(cusnos[i].ToString());
        //            sql.Append((i < cusnos.Count - 1) ? "," : "");
        //        }

        //        sql.Append(") order by cusno");
                    
        //        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MDB"].ToString());
        //        conn.Open();
        //        SqlCommand sqlComm = new SqlCommand(sql.ToString(), conn);
        //        rdr = sqlComm.ExecuteReader();

        //        while (rdr.Read())
        //        {
        //            int cusno = 0;
        //            string phoneMob = null;

        //            if (!(rdr["cusno"] is DBNull))
        //                cusno = int.Parse(rdr["cusno"].ToString());

        //            if (!(rdr["phonemobile"] is DBNull))
        //                phoneMob = rdr["phonemobile"].ToString();

        //            ret.Add(new WineTipCustomer(cusno, phoneMob));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("GetPhoneMobile() failed", ex.ToString());
        //    }
        //    finally
        //    {
        //        if (rdr != null)
        //            rdr = null;

        //        if (conn != null)
        //        {
        //            conn.Close();
        //            conn = null;
        //        }
        //    }


        //    ret.Add(new WineTipCustomer(1, "+46709132046"));

        //    return ret;
        //}

    }
}
