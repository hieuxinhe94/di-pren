using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Tools.Classes.Campaign
{
    /// <summary>
    /// CampaignODS is used as objectdatasource in CampaignAdmin plugin tool
    /// </summary>
    public class CampaignODS
    {

        #region Getters

        public DataView GetProducts()
        {
            DataSet dsProducts = MsSqlHandler.GetProducts();
            DataView dwProducts = dsProducts.Tables[0].DefaultView;
            dwProducts.Sort = "productNo ASC";

            DataRowView drv = dwProducts.AddNew();
            drv["productName"] = EPiServer.Core.LanguageManager.Instance.Translate("/campaigns/choose/product");
            drv["productNo"] = "00";
            drv.EndEdit();

            return dwProducts;
        }

        public DataSet GetOfferCodes(int onlyActive)
        {
            return MsSqlHandler.GetOfferCodes(onlyActive);
        }

        public DataSet GetTargetGroups()
        {
            return MsSqlHandler.GetTargetGroups();
        }

        #endregion

        #region Updaters

        public void UpdateOfferCode(int offercodeId, string offerText, int sortOrder, bool isActive, bool isStudent)
        {
            MsSqlHandler.UpdateOfferCode(offercodeId, offerText, sortOrder, isActive, isStudent);
        }

        #endregion

        #region Deleters

        public void DeleteOfferCode(int offerCodeId)
        {
            MsSqlHandler.DeleteOfferCode(offerCodeId);
        }

        public void DeleteTargetGroup(int targetGroupId)
        {
            MsSqlHandler.DeleteTargetGroup(targetGroupId);
        }

        #endregion

        #region Inserters

        public void InsertOfferCode(string campNo, string campId, string offerText, int sortOrder, bool isAutogiro, bool isActive, string productNo, bool isStudent, string priceGroup, string subsKind)
        {
            MsSqlHandler.InsertOfferCode(campNo, campId, offerText, sortOrder, isAutogiro, isActive, productNo, isStudent, priceGroup, subsKind);
        }

        public void InsertTargetGroup(string targetGroupName)
        {
            MsSqlHandler.InsertTargetGroup(targetGroupName);
        }

        #endregion

        #region CIRIX functions

        public DataView GetActiveCampaigns(string productNo, bool addChooseItem)
        {
            //Get dataset from WS
            DataSet dsActiveCampaigns = CirixDbHandler.GetActiveCampaigs(productNo);
            //Get dataview for sorting
            DataView dwActiveCampaigns = dsActiveCampaigns.Tables[0].DefaultView;
            //Sort it
            dwActiveCampaigns.Sort = "PRODUCTNO, CAMPID ASC";

            if (addChooseItem)
            {
                DataRowView drv = dwActiveCampaigns.AddNew();
                drv["CAMPID"] = EPiServer.Core.LanguageManager.Instance.Translate("/campaigns/choose/offercode");
                drv["PRODUCTNO"] = "0";
                drv.EndEdit();
            }

            return dwActiveCampaigns;
        }

        public DataView GetParameterValuesByGroup(string paperCode)
        {
            DataSet dsTargetGroups = CirixDbHandler.Ws.GetParameterValuesByGroup_(paperCode.ToUpper(), "TARGETGRPS");

            //Get dataview for sorting
            DataView dwTargetGroups = dsTargetGroups.Tables[0].DefaultView;
            //Filter empty items
            dwTargetGroups.RowFilter = "CODEVALUE <> ''";
            //Sort it
            dwTargetGroups.Sort = "CODEVALUE ASC";

            return dwTargetGroups;
        }

        #endregion
    }
}