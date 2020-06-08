using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using System.Configuration;
using DIClassLib.Misc;

namespace DIClassLib.DbHandlers
{
    public class MembershipDbHandler
    {
        private static string _connStrDisePren = "DisePren";


        public static bool CustHasActiveSubs(long cusno)
        {
            string userid = GetUserid(cusno);

            if (!string.IsNullOrEmpty(userid))
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(_connStrDisePren, "GetActiveSubscriptions", new SqlParameter("@userId", userid));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DateTime dt;
                    DateTime expireDate = (DateTime.TryParse(dr["expireDate"].ToString(), out dt) ? dt : DateTime.MinValue);
                    string productNo = dr["productNo"].ToString().ToUpper();
                    string paperCode = dr["paperCode"].ToString().ToUpper();

                    if (expireDate > DateTime.MinValue)
                    {
                        if ( (productNo == Settings.ProductNo_Regular && (paperCode == Settings.PaperCode_DI || paperCode == Settings.PaperCode_DISE)) || (paperCode == Settings.PaperCode_IPAD) )
                            return true;
                    }
                }
            }

            return false;
        }

        public static string GetUserid(long cusno)
        {
            try
            {
                if(cusno > 0)
                    return SqlHelper.ExecuteScalar(_connStrDisePren, "GetUseridByCusno", new SqlParameter("@cusno", cusno)).ToString();

                return string.Empty;
            }
            catch (Exception ex)
            {
                new Logger("GetUserid(cusno) failed for cusno:" + cusno, ex.ToString());
            }

            return string.Empty;
        }

        public static bool IsInRole(long cusno, string roleName)
        {
            return IsInRole(GetUserid(cusno), roleName);
        }

        public static bool IsInRole(string userid, string roleName)
        {
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(roleName))
            {
                SqlParameter[] prms = new SqlParameter[] { 
                                            new SqlParameter("@userId", userid), 
                                            new SqlParameter("@role", roleName) };

                int IsInRole = int.Parse(SqlHelper.ExecuteScalarParam(_connStrDisePren, "IsCustomerInRole", prms).ToString());
                
                if (IsInRole > 0)
                    return true;
            }

            return false;
        }

        
        //public static DateTime GetExpireDate(string userid)
        //{
        //    //DateTime dt = TryGetExpireDateFromSession();
        //    //if (dt > DateTime.MinValue)
        //    //    return dt;

        //    return GetExpireDateFromDb(userid);
        //}

        //private static DateTime TryGetExpireDateFromSession()
        //{
        //    if (System.Web.HttpContext.Current.Session["Expires"] != null)
        //    {
        //        DateTime dt = (DateTime)System.Web.HttpContext.Current.Session["Expires"];
        //        System.Web.HttpContext.Current.Session["Expires"] = null;
        //        return dt;
        //    }

        //    return DateTime.MinValue;
        //}

        //private static DateTime GetExpireDateFromDb(string userid)
        //{
        //    DateTime expireDate = DateTime.MinValue;
            
        //    if (!string.IsNullOrEmpty(userid))
        //    {
        //        DataSet ds = null;
        //        ds = SqlHelper.ExecuteDataset(_connStrDisePren, "GetActiveSubscriptions", new SqlParameter("@userId", userid));

        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            DateTime dt;
        //            expireDate = (DateTime.TryParse(dr["expireDate"].ToString(), out dt) ? dt : DateTime.MinValue);
        //            string productNo = dr["productNo"].ToString().ToUpper();
        //            string paperCode = dr["paperCode"].ToString().ToUpper();

        //            if (expireDate > DateTime.MinValue)
        //            {
        //                if ( (productNo == Settings.ProductNo_Regular && (paperCode == Settings.PaperCode_DI || paperCode == Settings.PaperCode_DISE)) || (paperCode == Settings.PaperCode_IPAD) )
        //                    return expireDate;
        //            }
        //        }
        //    }

        //    return expireDate;
        //}

        public static int GetCusno(string userid)
        {
            try
            {
                return int.Parse(SqlHelper.ExecuteScalar(_connStrDisePren, "GetCusnoByUserid", new SqlParameter("@userid", userid)).ToString());
            }
            catch (Exception ex)
            {
                new Logger("GetCusno() - failed for userid: " + userid, ex.ToString());
            }

            return -1;
        }

        /// <summary>
        /// BirthNo format: YYYYMMDD / YYYYMMDDXXXX. 
        /// Will not execute sql if birthNo.Length < 8
        /// </summary>
        public static List<long> GetCusnosByBirthNo(string birthNo)
        {
            List<long> cusnos = new List<long>();

            if (birthNo.Length < 8)
                return cusnos;

            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(_connStrDisePren, "GetCusnosByBirthNo", new SqlParameter("@birthNo", birthNo));

                foreach (DataRow dr in ds.Tables[0].Rows)
                    cusnos.Add(long.Parse(dr["cusno"].ToString()));
            
            }
            catch (Exception ex)
            {
                new Logger("GetCusnosByBirthNo() - failed", ex.ToString());
            }

            return cusnos;
        }


    }
}
