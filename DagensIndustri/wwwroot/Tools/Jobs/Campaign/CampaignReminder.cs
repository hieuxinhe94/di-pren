using System;
using EPiServer.PlugIn;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using EPiServer;
using EPiServer.Core;
using EPiServer.Security;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.Campaign;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;


//namespace DagensIndustri.Tools.Jobs.Campaign
//{
//    [ScheduledPlugIn(DisplayName = "Epostnotifikation för kampanjer", Description = "Skickar påminnelsemail till marknad för kampanjer som startar inom fyra dagar")]
//    public class CampaignReminder
//    {

//        public static string Execute()
//        {
//            //Run as specific user
//            PrincipalInfo.CurrentPrincipal = PrincipalInfo.CreatePrincipal("DiAdmin");

//            StringBuilder returnValue = new StringBuilder();
//            PageData startPage = EPiServer.DataFactory.Instance.GetPage(PageReference.StartPage);
//            PageData CampaignRootPage = EPiServer.DataFactory.Instance.GetPage((PageReference)startPage["CampaignRootPage"]);

//            if (CampaignRootPage["CampaignPageType"] != null)
//            {
//                PropertyCriteriaCollection criteriaCollection = new PropertyCriteriaCollection();
//                PropertyCriteria criteria = new PropertyCriteria()
//                {
//                    Condition = EPiServer.Filters.CompareCondition.Equal,
//                    Required = true,
//                    Type = PropertyDataType.PageType,
//                    Name = "PageTypeID",
//                    Value = CampaignRootPage["CampaignPageType"].ToString()
//                };
//                criteriaCollection.Add(criteria);

//                //Get all campaign pages
//                PageDataCollection pdc = EPiServer.DataFactory.Instance.FindAllPagesWithCriteria(CampaignRootPage.PageLink, criteriaCollection, "sv", new LanguageSelector("sv"));
//                returnValue.Append("Påminnelsemail har skickats för följande kampanjer:<br />");

//                foreach (PageData page in pdc)
//                {
//                    if (page.StartPublish > DateTime.Now.AddDays(-4) && page.StartPublish < DateTime.Now.AddDays(4) && !page.ReminderSent())
//                    {
//                        if (page.Status == VersionStatus.Published || page.Status == VersionStatus.DelayedPublish)
//                        {
//                            returnValue.Append("Kampanj: " + page.PageName + "<br />");

//                            //Build mailbody
//                            string mailBody = "<strong>Kampanjens namn:</strong> " + page.PageName + "<br />" +
//                                "<strong>Period:</strong> " + page.StartPublish.ToString("yyyy-MM-dd") + " - " + page.StopPublish.ToString("yyyy-MM-dd") + "<br />" +
//                                "<strong>Målgrupp:</strong> " + page.GetTargetGroup().Tables[0].Rows[0]["targetGroupName"] + "<br />" +
//                                "<strong>Erbjudandekod:</strong><br />";

//                            foreach (DataRow row in page.GetOfferCodes().Tables[0].Rows)
//                                mailBody += "(" + row["campId"].ToString() + ") " + row["offerText"] + "<br />";

//                            if (page.GetCampaignType().Tables[0].Rows.Count > 0)
//                                mailBody += "<strong>Kampanjtyp:</strong> " + page.GetCampaignType().Tables[0].Rows[0]["typeName"] + "<br />";

//                            mailBody += "<strong>Förväntat antal leads:</strong> " + page["ExpectedLeads"];
//                            mailBody += "<p><strong>Vid frågor kontakta:</strong> " + page.ChangedBy + "</p>";

//                            //Send mail
//                            MiscFunctions.SendMail("no-reply@di.se",
//                                CampaignRootPage["ReminderReceiver"] != null ? CampaignRootPage["ReminderReceiver"].ToString() : "pren@di.se",
//                                "Kampanj " + page.PageName + " börjar snart",
//                                mailBody,
//                                true);

//                            //Update campaign to prevent multiple email
//                            page.UpdateReminderSent(true);
//                        }
//                    }
//                }
//            }

//            return returnValue.ToString();

//        }


//    }
//}