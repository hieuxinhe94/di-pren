using System;
using System.Data;
using System.Web;
using EPiServer.Core;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using System.Collections.Generic;


namespace DagensIndustri.Tools.Classes.Campaign
{
    public static class CampaignExtensions
    {

        public static void SaveCamp(this PageData page, string targetGroupReg, string targetGroupEmail, string targetGroupPostal, string targetGroupMobile)
        {
            MsSqlHandler.SaveCamp(page.PageLink.ID, (string)page["CampId"], targetGroupReg, targetGroupEmail, targetGroupPostal, targetGroupMobile);
        }
        
        //public static void DeleteTargetGroups(this PageData page)
        //{
        //    if(page.PageLink.ID > 0)
        //        MsSqlHandler.DeleteCampaignTargetGroups(page.PageLink.ID);
        //}
        
        //public static void InsertTargetGroup(this PageData page, int targetGroupTypeId, string targetGroup)
        //{
        //    if(page.PageLink.ID > 0 && targetGroupTypeId > 0 && !string.IsNullOrEmpty(targetGroup))
        //        MsSqlHandler.InsertCampaignTargetGroup(page.PageLink.ID, targetGroupTypeId, targetGroup);
        //}
                


        //public static void UpdateCampaignLeads(this PageData page)
        //{
        //    MsSqlHandler.UpdateCampaignLeads(page.PageLink.ID);
        //}

        //public static DataSet GetCosts(this PageData page)
        //{
        //    return MsSqlHandler.GetCampaignCosts(page.PageLink.ID);
        //}

        //public static DataSet GetCampaignType(this PageData page)
        //{
        //    return MsSqlHandler.GetCampaignType(page.PageLink.ID);
        //}

        //public static DataSet GetOfferCodes(this PageData page)
        //{
        //    return MsSqlHandler.GetCampaignOfferCodes(page.PageLink.ID);
        //}

        //public static DataSet GetTargetGroup(this PageData page)
        //{
        //    return MsSqlHandler.GetCampaignTargetGroup(page.PageLink.ID);
        //}

        //public static void ClearOfferCodes(this PageData page)
        //{
        //    MsSqlHandler.ClearCampaignOfferCodes(page.PageLink.ID);
        //}

        //public static void ClearCosts(this PageData page)
        //{
        //    MsSqlHandler.ClearCampaignCosts(page.PageLink.ID);
        //}

        //public static void InsertOfferCode(this PageData page, string offerCodeId)
        //{
        //    MsSqlHandler.InsertCampaignOfferCode(page.PageLink.ID, offerCodeId);
        //}

        //public static void InsertTargetGroup(this PageData page, string targetGroupId)
        //{
        //    MsSqlHandler.InsertCampaignTargetGroup(page.PageLink.ID, targetGroupId);
        //}

        //public static void InsertCost(this PageData page, string description, int amount)
        //{
        //    MsSqlHandler.InsertCampaignCost(page.PageLink.ID, description, amount);
        //}

        //public static void InsertType(this PageData page, int typeId, string comment)
        //{
        //    MsSqlHandler.InsertCampaignType(page.PageLink.ID, typeId, comment);
        //}

        //public static void UpdateReminderSent(this PageData page, bool sent)
        //{
        //    MsSqlHandler.UpdateCampaignReminderSent(page.PageLink.ID, sent);
        //}

        //public static bool ReminderSent(this PageData page)
        //{
        //    return MsSqlHandler.ReminderSent(page.PageLink.ID);
        //}

    }
}