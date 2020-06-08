using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri.Templates.Public.Units.Placeable.DiY
{
    public partial class DiYLogin : EPiServer.UserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserMessageControl.ClearMessage();
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            string loginFailed = "Inloggningen misslyckades";
            
            long cusno = 0;
            long.TryParse(InputCusno.Text, out cusno);

            int zip = 0;
            int.TryParse(InputZip.Text, out zip);

            if (cusno <= 0)
            {
                UserMessageControl.ShowMessage(loginFailed + ": ogiltigt kundnummmer", false, true);
                return;
            }

            if (zip <= 0)
            {
                UserMessageControl.ShowMessage(loginFailed + ": ogiltigt postnummer (måste bestå av 5 siffror)", false, true);
                return;
            }

            if (LoginByCusnoAndZip(cusno, zip))
            {
                PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "MySettingsPage") as PageReference);
                Response.Redirect(EPiFunctions.GetFriendlyAbsoluteUrl(pd));
            }
            else
            {
                UserMessageControl.ShowMessage(loginFailed, false, true);
            }
        }

        private bool LoginByCusnoAndZip(long cusno, int zip)
        {
            DataSet ds = SubscriptionController.GetCustomer(cusno);
            
            if (ds != null && ds.Tables != null && ds.Tables[0] != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                int cirixZip = 0;
                int.TryParse(dr["ZIPCODE"].ToString(), out cirixZip);

                if (zip != cirixZip)
                    return false;

                string user = dr["WWWUSERID"].ToString();
                string pass = SubscriptionController.GetWwwPassword(cusno);

                return LoginUtil.ReLoginUserRefreshCookie(user, pass);
            }

            return false;
        }


    }
}