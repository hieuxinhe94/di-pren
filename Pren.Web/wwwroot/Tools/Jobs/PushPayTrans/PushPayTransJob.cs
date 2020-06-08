using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using EPiServer.PlugIn;

namespace Pren.Web.Tools.Jobs.PushPayTrans
{
    [ScheduledPlugIn(DisplayName = "Push paytrans, WS")]
    class PushPayTransJob
    {
        public static string Execute()
        {
            var returnValue = new StringBuilder();
            var payTransWs = new PayTransService.PayTransSoapClient();

            try
            {
                var dsRowsToPush = GetPayTransRowsToPush();

                if (dsRowsToPush.Tables[0] == null || dsRowsToPush.Tables[0].Rows.Count < 1) return "No rows to push";

                var accessToken = GetAccessToken();

                foreach (DataRow payTransRow in dsRowsToPush.Tables[0].Rows)
                {
                    var key = payTransRow["Customer_refno"].ToString();

                    try
                    {                       
                        var inserted = payTransWs.InsertPayTrans(accessToken,
                            int.Parse(key),
                            int.Parse(payTransRow["Merchant_id"].ToString()),
                            payTransRow["Currency"] as string ?? string.Empty,
                            int.Parse(payTransRow["Amount"].ToString()),
                            int.Parse(payTransRow["VAT"].ToString()),
                            payTransRow["Payment_method"] as string ?? string.Empty,
                            DateTime.Parse(payTransRow["Purchase_date"].ToString()),
                            payTransRow["Goods_description"] as string ?? string.Empty,
                            payTransRow["Comment"] as string ?? string.Empty,
                            payTransRow["Consumer_name"] as string ?? string.Empty,
                            payTransRow["Email_address"] as string ?? string.Empty,
                            payTransRow["Card_type"] as string ?? string.Empty,
                            payTransRow["Transaction_id"] as string ?? string.Empty,
                            payTransRow["Status"] as string ?? string.Empty,
                            payTransRow["Status_code"] as string ?? string.Empty,
                            payTransRow["Finish_date"] != DBNull.Value ? DateTime.Parse(payTransRow["Finish_date"].ToString()) : DateTime.Now);

                        if (!inserted)
                        {
                            returnValue.Append("Push failed for '" + key + "'<br>");
                            continue;
                        }

                        SetRowAsPushed(key);
                        returnValue.Append(key + " pushed.<br>");
                    }
                    catch (Exception exception)
                    {
                        returnValue.Append("Push failed for '" + key + "' with exception. Se log for details.<br>");
                        new Logger("PayTransPushJob failed. Push failed for '" + key + "'", exception.ToString());     
                    }

                }
            }
            catch (Exception exception)
            {
                new Logger("PayTransPushJob failed", exception.ToString());

                return "PayTransPushJob failed, se log for details.";
            }
            finally
            {
                payTransWs.Close();
            }

            return returnValue.ToString();
        }

        public static DataSet GetPayTransRowsToPush()
        {            
            return SqlHelper.ExecuteDataset("DisePren", "GetPayTransRowsToPush", null);
        }

        public static void SetRowAsPushed(string customerRefNo)
        {
            SqlHelper.ExecuteNonQuery("DisePren", "SetPayTransRowAsPushed", new[]{ new SqlParameter("@customerRefNo", customerRefNo) });
        }

        public static string GetAccessToken()
        {
            var secret = ConfigurationManager.AppSettings["InsertPayTransSecret"];
            if (string.IsNullOrEmpty(secret))
            {
                return string.Empty;
            }

            //The logic for correct accessToken
            var stringToProcess = string.Format("{0}_{1}_{2}_{3}", MiscFunctions.GetMd5Hash(secret), DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var token = MiscFunctions.GetMd5Hash(stringToProcess);
            return token;
        }
    }
}
