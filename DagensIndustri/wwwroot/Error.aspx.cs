using System;
using System.Web;
using System.Web.UI;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri
{
    public partial class Error : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                //Get StartPage
                PageData startPage = EPiServer.DataFactory.Instance.GetPage(PageReference.StartPage);
                string strTitle = string.Empty;
                string strHeading = string.Empty;
                string strBody = string.Empty;

                //All exception properties are mandatory, so no need to check != null
                switch (Response.StatusCode)
                {
                    case 404:
                        //set status code to 200 for security reasons
                        Response.StatusCode = 200;
                        strTitle = EPiFunctions.SettingsPageSetting(startPage,"title404") as string;
                        strHeading = EPiFunctions.SettingsPageSetting(startPage, "title404") as string;
                        strBody = EPiFunctions.SettingsPageSetting(startPage,"text404") as string;
                        break;
                    default:
                        //set status code to 200 for security reasons
                        Response.StatusCode = 200;
                        strTitle = EPiFunctions.SettingsPageSetting(startPage,"title500") as string;
                        strHeading = EPiFunctions.SettingsPageSetting(startPage,"title500") as string;
                        strBody = EPiFunctions.SettingsPageSetting(startPage,"text500") as string;
                        break;
                }

                //set error message
                LitTitle.Text = strTitle;
                LblHeading.Text = strHeading;
                LblBody.Text = strBody;
            }
        }
    }
}