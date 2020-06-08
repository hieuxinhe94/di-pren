using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.IO;
using EPiServer;
using EPiServer.PlugIn;
using System.Data;
using System.Net;
using System.Collections.Generic;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;


namespace DagensIndustri.Tools.Admin.Campaign
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Kampanjadmin", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Kampanjadmin", UrlFromUi = "/Tools/Admin/Campaign/CampaignAdmin.aspx", SortIndex = 2000)]
    public partial class CampaignAdmin : System.Web.UI.Page
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {

            }
        }

        protected void BtnAddOfferCodeOnClick(object sender, EventArgs e)
        {
            OfferCodeDataSource.Insert();
        }

        //todo fix
        protected void DdlOfferCodesOnSelectedIndexChanged(object sender, EventArgs e)
        {
            //if (DdlOfferCodes.SelectedIndex > 0)
            //{
                //string priceGroup = GetSubsChoicesItem("PRICEGROUP", 0);
                //string subsKind = GetSubsChoicesItem("SUBSKIND", 0);
                //List<string> autogiroPriceGroups = new List<string> { "25", "26", "27" };

                //if (!string.IsNullOrEmpty(priceGroup) && !string.IsNullOrEmpty(subsKind))
                //{
                //    BtnAddOfferCode.Enabled = true;

                //    ChkAutogiro.Checked = autogiroPriceGroups.Contains(priceGroup);
                //    TxtPriceGroup.Text = priceGroup;
                //    TxtSubsKind.Text = subsKind;

                    //tmp = "PriceGroup: " + priceGroup;
                    //tmp += ", SubsKind: " + subsKind + ", "; 
                //}
                //else
                //{
                //    TxtPriceGroup.Text = string.Empty;
                //    TxtSubsKind.Text = string.Empty;
                //    BtnAddOfferCode.Enabled = false;
                //    LblErrorOc.Text = "Det saknas uppgifter för erbjudandekoden (subskind, pricegroup)";
                //}

            //    TxtCampName.Text = CirixDbHandler.GetCampaign(long.Parse(DdlOfferCodes.SelectedValue)).Tables[0].Rows[0]["CAMPNAME"].ToString();
            //}
            //else
                TxtCampName.Text = string.Empty;

        }

        //todo fix
        //private string GetSubsChoicesItem(string elementname, int pageIdIndex)
        //{
        //    string[] pageIds = MiscFunctions.GetAppsettingsValue("CampaignPageId").Split(',');
        //    DataSet ds = CirixDbHandler.Ws.GetSubsChoices2_("DI", DateTime.Now, pageIds[pageIdIndex]);
        //    DataRow[] dr = ds.Tables[0].Select("CAMPID = '" + DdlOfferCodes.SelectedItem.Text + "'");

        //    if (dr.Length > 0)
        //        return dr[0][elementname].ToString();
        //    else
        //    {
        //        if (pageIdIndex < (pageIds.Length - 1))
        //            return GetSubsChoicesItem(elementname, pageIdIndex + 1);

        //        return string.Empty;
        //    }
        //}

        protected void BtnAddTargetGroupOnClick(object sender, EventArgs e)
        {
            TargetGroupDataSource.Insert();
        }

        protected void OfferCodeDeleting(object source, ObjectDataSourceMethodEventArgs e)
        {
            IDictionary paramsFromPage = e.InputParameters;

            if (paramsFromPage["offerCodeId"] != null)
            {
                //Get all campaigns with this offercode
                DataSet dsCampaigns = MsSqlHandler.GetCampaignsWithOfferCode(int.Parse(paramsFromPage["offerCodeId"].ToString()));

                //If a campaign exists, user is not allowed to delete
                if (dsCampaigns.Tables[0].Rows.Count > 0)
                {
                    LblErrorOc.Text = EPiServer.Core.LanguageManager.Instance.Translate("/campaigns/errors/offercodeexisting");
                    e.Cancel = true;
                }
            }
        }

        protected void TargetGroupDeleting(object source, ObjectDataSourceMethodEventArgs e)
        {
            IDictionary paramsFromPage = e.InputParameters;

            if (paramsFromPage["targetGroupId"] != null)
            {
                //Get all campaigns with this targetgroup
                DataSet dsCampaigns = MsSqlHandler.GetCampaignsWithTargetGroup(int.Parse(paramsFromPage["targetGroupId"].ToString()));

                //If a campaign exists, user is not allowed to delete
                if (dsCampaigns.Tables[0].Rows.Count > 0)
                {
                    LblErrorTg.Text = EPiServer.Core.LanguageManager.Instance.Translate("/campaigns/errors/targetgroupexisting");
                    e.Cancel = true;
                }
            }
        }


        private void ShowError(string strMessage)
        {

        }

    }
}