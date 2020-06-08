using System;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;

namespace DagensIndustri.Scripts.SMS
{
    public partial class Ipxsms4792 : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //http://localhost/functions/ipxsms4792.aspx?OriginatorAddress=08123456789&Message=PDF 1685 20101231

            //Check that valid ip address
            if (Request.UserHostAddress != "217.151.193.79" & Request.UserHostAddress != "217.151.193.80" & Request.UserHostAddress != "195.198.185.100")
            {
                Response.Write("Access denied.");
                Response.End();
            }

            Response.ContentType = "text/xml";
            Response.Expires = 0;

            //Get originator address, which means the phonenumber that the sms originates from
            string RecPhoneNbr = Request.QueryString["OriginatorAddress"];
            //Remove swedish landskod
            string PhoneNbr = ("+" + RecPhoneNbr).Replace("+46", "0");
            //If not swedish, just remove +prefix
            PhoneNbr = PhoneNbr.Replace("+", "");

            // Textstring contains; PDF smsnr [date]            
            string TextStr = Request.QueryString["Message"] != null ? Request.QueryString["Message"].ToUpper() : string.Empty;

            // Remove PDF prefix                        
            TextStr = TextStr.Replace("PDF", "").Trim();

            //Insert stuff to DB. If ok, ResultStr will be empty
            string ResultStr = SmsPay(PhoneNbr, "PDF", TextStr);

            //Build xml with result
            //What is this used for, is this xml parsed?
            StringBuilder sb = new StringBuilder();
            sb.Append("<sms><servicetype>Text</servicetype>");
            sb.Append("<receiverphonenumber>" + RecPhoneNbr + "</receiverphonenumber>");
            sb.Append("<data>" + ResultStr + "</data>");
            sb.Append("<eventsource>1</eventsource></sms>");
            Response.Write(sb.ToString());
            Response.End();
        }

        //***************************************************************************************
        //**
        //** SmsPay: Register SMS-payment in database for product in "Prod"
        //** and smsnr and date in "Text"
        //**
        //***************************************************************************************
        public static string SmsPay(string PhoneNbr, string Prod, string TextStr)
        {
            string ResultStr = string.Empty;
            string strError = EPiServer.Core.LanguageManager.Instance.Translate("/sms/erroripx");
            DateTime aDate = default(DateTime);

            //Split TextStr, which originates from QueryString["Message"]
            string[] Fields = TextStr.Split(" ".ToCharArray());
            //First index in array, is the smsnr used to pay
            string SmsNr = Fields.Length > 0 ? Fields[0].Trim(' ') : string.Empty;
            //Second index is a date string
            string DateStr = Fields.Length > 1 ? Fields[1].Trim(' ') : string.Empty;

            //If length of date is 6 in length (100317), add 20 and format string (2010-03-17)
            if (DateStr.Length == 6)
                DateStr = (DateTime.Now.Year / 100) + DateStr.Substring(0, 2) + "-" + DateStr.Substring(2, 2) + "-" + DateStr.Substring(4, 2);
            //If length of date is 8 in length (20100317), format string (2010-03-17)
            else if (DateStr.Length == 8)
                DateStr = DateStr.Substring(0, 4) + "-" + DateStr.Substring(4, 2) + "-" + DateStr.Substring(6, 2);

            //If DateStr is empty, set it to Now
            //If not empty, try parse it as DateTime, if that fails, the transaction will fail
            try
            {
                aDate = string.IsNullOrEmpty(DateStr) ? DateTime.Now.Date : DateTime.Parse(DateStr);
            }
            catch (FormatException)
            {
                aDate = DateTime.Now.Date;
                Prod = "FAIL";
            }

            SqlDataReader DR = null;
            try
            {
                // Subscription
                if (Prod == "PDF")
                {
                    //ChangeSmsPayState
                    SqlParameter[] changeSmsPayStateParameters = new SqlParameter[] { new System.Data.SqlClient.SqlParameter("@SmsNr", SmsNr), new System.Data.SqlClient.SqlParameter("@NewValue", "Yes") };
                    SqlHelper.ExecuteNonQuery("DisePren", "ChangeSmsPayState", changeSmsPayStateParameters);

                    //SetSmsPay
                    SqlParameter[] setSmsPayParameters = new SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@PaymentType", "SMS"), 
                    new System.Data.SqlClient.SqlParameter("@Userid", PhoneNbr), 
                    new System.Data.SqlClient.SqlParameter("@Amount", 1500), 
                    new System.Data.SqlClient.SqlParameter("@ProductType", "DIDL1"), 
                    new System.Data.SqlClient.SqlParameter("@Product", aDate.ToString("yyyy-MM-dd")) };

                    DR = SqlHelper.ExecuteReader("DisePren", "SetSmsPay", setSmsPayParameters);

                    if (DR.Read())
                        if (DR["result"] == System.DBNull.Value | (int)DR["result"] == -1)
                            ResultStr = strError + " " + PhoneNbr;
                }
                else
                {
                    //SetSmsPay with FAIL-status
                    SqlParameter[] setSmsPayParameters = new SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@PaymentType", "SMS"), 
                    new System.Data.SqlClient.SqlParameter("@Userid", PhoneNbr), 
                    new System.Data.SqlClient.SqlParameter("@Amount", 1500), 
                    new System.Data.SqlClient.SqlParameter("@ProductType", "FAIL"), 
                    new System.Data.SqlClient.SqlParameter("@Product", aDate.ToString("yyyy-MM-dd")) };

                    SqlHelper.ExecuteNonQuery("DisePren", "SetSmsPay", setSmsPayParameters);

                    ResultStr = strError + " " + PhoneNbr;
                }
            }
            catch (Exception ex)
            {
                new Logger("SmsPay() - failed", ex.ToString());
                ResultStr = strError + " " + PhoneNbr;
            }
            finally
            {
                if (DR != null)
                    DR.Close();
            }

            return ResultStr;
        }
    }
}