using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DIClassLib.Subscriptions
{
    public class CampaignObjectHandler
    {
        /// <summary>
        /// Get a campaign object for a given campaignNo
        /// </summary>
        /// <param name="campaignNo"></param>
        /// <param name="isDirectDebit"></param>
        /// <returns></returns>
        public static CampaignObject GetCampaignObject(long campaignNo, string paperCode, string productNo, DateTime targetDate, bool isDirectDebit)
        {
            DataSet activeCampaignsDs = CirixDbHandler.GetActiveCampaigs(paperCode, productNo);
            CampaignObject campaignObject = FindCampaign(activeCampaignsDs, campaignNo, productNo, targetDate, isDirectDebit);
            return campaignObject;
        }

        /// <summary>
        /// Find a campaing in the given dataset and create a Campaign object
        /// </summary>
        /// <param name="activeCampaignsDs"></param>
        /// <param name="campaignNo"></param>
        /// <param name="productNo"></param>
        /// <param name="targetDate"></param>
        /// <param name="pageId"></param>
        /// <param name="isDirectDebit"></param>
        /// <returns></returns>
        private static CampaignObject FindCampaign(DataSet activeCampaignsDs, long campaignNo, string productNo, DateTime targetDate, bool isDirectDebit)
        {
            CampaignObject campObj = null;
            if (activeCampaignsDs != null && activeCampaignsDs.Tables.Count > 0)
            {
                foreach (DataRow drCampaign in activeCampaignsDs.Tables[0].Rows)
                {
                    long campNo = Convert.ToInt64(drCampaign["CAMPNO"]);
                    if (campNo != campaignNo)
                        continue;

                    DataSet dsCampaignDetails = null;
                    try
                    {
                        dsCampaignDetails = CirixDbHandler.GetCampaign(campNo);
                        DataRow drCampaignDetails = dsCampaignDetails.Tables[0].Rows[0];

                        DataRow drSubsChoises2 = GetSubsChoices2((string)drCampaignDetails["CAMPID"], (string)drCampaignDetails["PAPERCODE"], targetDate);
                        campObj = new CampaignObject(drCampaign, drCampaignDetails, drSubsChoises2, productNo, isDirectDebit);
                        break;

                    }
                    catch (Exception ex)
                    {
                        new Logger("FindCampaign() - failed for campNo: " + campNo.ToString(), ex.ToString());
                        throw ex;
                    }
                }
            }
            return campObj;
        }


        /// <summary>
        /// Get SubsChoises for a certain campaign, papercode and pageId
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="paperCode"></param>
        /// <param name="targetDate"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        //private static DataRow GetSubsChoices2(string campaignId, string paperCode, DateTime targetDate)
        //{
        //    DataRow subsChoices2DataRow = null;
        //    try
        //    {
        //        DataSet subsChoices2DataSet = CirixDbHandler.GetSubsChoices2(paperCode, targetDate, MiscFunctions.GetAppsettingsValue("SubscriptionCampaignPageId"));
        //        if (subsChoices2DataSet != null && subsChoices2DataSet.Tables.Count > 0)
        //        {
        //            foreach (DataRow dr in subsChoices2DataSet.Tables[0].Rows)
        //            {
        //                if ((string)dr["CAMPID"] == campaignId)
        //                {
        //                    subsChoices2DataRow = dr;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("GetSubsChoices2() - failed for campId: " + campaignId, ex.ToString());
        //        throw ex;
        //    }
        //    return subsChoices2DataRow;
        //}
    }
}
