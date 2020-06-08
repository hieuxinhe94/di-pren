using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DIClassLib.DbHelpers;
using System.Data.SqlClient;
using DagensIndustri.Tools.Classes;
using DIClassLib.Membership;


namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class BuyTodaysPaper : EPiServer.UserControlBase
    {
        public string smsLandingPageURL { get; set; }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            //Register javascript for pdfcheck
            //RegisterClientScriptFile("/Templates/Public/js/PDFCheck.js");
            //Register javascript for functions
            //RegisterClientScriptFile("/Templates/Public/js/Functions.js");
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            
            if(EPiFunctions.HasValue(EPiFunctions.SettingsPage(CurrentPage), "SMSDescription"))
            {
                SMSDescriptionLiteral.Text = EPiFunctions.SettingsPageSetting(CurrentPage, "SMSDescription").ToString();
            }

            //Response.Buffer = true;
            //Response.Clear();

            if (!Page.IsPostBack)
            {
                string smscode = GetSmsCode();
                string myUid = Session.SessionID + uid();

                InsertEntryInSmsTransaction(myUid, smscode);

                Session["SmsCode"] = smscode;
                SMSCodeLitral.Text = smscode;
                Session["myUid"] = myUid;

                //Response.Write("smscode: " + smscode + "<br>");
                //Response.Write("myUid: " + myUid + "<br>");

            }

            PageData SMSLandingPage = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(CurrentPage, "SMSLandingPage") as PageReference);
            smsLandingPageURL = SMSLandingPage.LinkURL;
        }

        private bool IsPageInEditMode()
        {
            return CurrentPage.WorkPageID != 0;
        }

        /// <summary>
        /// GetSmsCode
        /// </summary>
        /// <returns>Get a 4 digit code used as identifyer when sms-message arrives</returns>
        public string GetSmsCode()
        {
            SqlDataReader DR = null;

            try
            {
                DR = SqlHelper.ExecuteReader("DisePren", "GetSmsNr");

                if (DR.Read())
                {
                    if (DR["result"] != System.DBNull.Value && (int)DR["result"] > 0)
                        return ZeroPad(DR["result"].ToString(), 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                new Logger("GetSmsCode() - failed", ex.ToString());
                return null;
            }
            finally
            {
                if (DR != null)
                    DR.Close();
            }
        }


        /// <summary>
        /// InsertEntryInSmsTransaction: create a record, pass values sessionid, smscode, payed=no, expire=null
        /// If record exists then do an update with smscode as key
        /// </summary>
        /// <returns>OK or NOT OK</returns>
        public string InsertEntryInSmsTransaction(string UniqueNr, string SmsCode)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@UniqueId", UniqueNr), 
                    new System.Data.SqlClient.SqlParameter("@SmsNr", SmsCode)
                };

                SqlHelper.ExecuteNonQuery("DisePren", "setSmsTrans", sqlParameters);

                return "OK";
            }
            catch (Exception ex)
            {
                new Logger("InsertEntryInSmsTransaction() - failed", ex.ToString());
                return "NOT OK";
            }
        }

        /// <summary>
        /// uid
        /// </summary>
        /// <returns>An unique id (atleast for the next 30 years from 2000)</returns>
        public long uid()
        {
            long theseconds = 0;
            //theseconds = DateDiff("s", "2000-01-01 12:00:00", Now());
            theseconds = (long)System.DateTime.Now.Subtract(System.DateTime.Parse("2000-01-01 12:00:00")).TotalSeconds;

            return theseconds;
        }

        /// <summary>
        /// ZeroPad
        /// </summary>
        /// <param name="str">the string to pad</param>
        /// <param name="iSize">How many to pad</param>
        /// <returns>A PadLeft string with 0 (zero). str="123" and iSize=5 will return 00123</returns>
        public string ZeroPad(string str, int iSize)
        {
            string tmpStr = null;
            tmpStr = str.Trim();

            return tmpStr.PadLeft(iSize, '0');
        }

    }
}