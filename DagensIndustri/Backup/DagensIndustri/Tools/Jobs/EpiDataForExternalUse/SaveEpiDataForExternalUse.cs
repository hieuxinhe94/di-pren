using System;
using System.Collections.Generic;
using System.Linq;
using DIClassLib.DbHelpers;
using EPiServer.PlugIn;
using EPiServer.Core;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.EPiJobs.EpiDataForExternalUse;

namespace DagensIndustri.Tools.Jobs.EpiDataForExternalUse
{
    [ScheduledPlugIn(DisplayName = "Extract EPi data for external access", Description = "Saves info for upcoming conferences to separate MSSQL DB")]
    public class SaveEpiDataForExternalUse
    {
        public static string Execute()
        {
            string ret;

            try
            {
                List<ConferenceEpiDataInput> confs = new List<ConferenceEpiDataInput>();
            
                PageData confContainer = DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "ConferenceContainerPage") as PageReference);
                foreach (PageData pd in DataFactory.Instance.GetChildren(confContainer.PageLink))
                {
                    if (!EPiFunctions.IsMatchingPageType(pd, pd.PageTypeID, "ConferencePageType"))
                        continue;

                    if (!EPiFunctions.HasValue(pd, "Date"))
                        continue;

                    if (pd.Status == VersionStatus.Published && !pd.IsDeleted)
                    {
                        ConferenceEpiDataInput conf = GetConferenceEpiDataInput(pd);
                        if (conf.DateConferenceStart.Date >= DateTime.Now.Date)
                            confs.Add(conf);
                    }
                }

                if (confs.Count > 0)
                {
                    int batchId = MsSqlHandler.EpiJob_Conf_GetNewBatchId();
                    AddConfsToDb(batchId, confs);
                    ret = "BatchId: " + batchId + ", antal konferenser: " + confs.Count;
                }
                else
                {
                    ret = "Ingen ny batch skapades (0 st framtida konferenser hittade i EPi)";
                }
            }
            catch (Exception ex)
            {
                new Logger("SaveEpiDataForExternalUse / Execute() - failed", ex.ToString());
                ret = "Ett tekniskt fel uppstod. Kolla i felloggen";
            }

            return ret;
        }

        private static ConferenceEpiDataInput GetConferenceEpiDataInput(PageData pd)
        {
            int epiPageId = pd.PageLink.ID;
            string headline = TryGetEpiPropValue(pd, "Heading");
            string shortIntroTextHtml = TryGetEpiPropValue(pd, "PuffText");
            string place = TryGetEpiPropValue(pd, "Place");
            string placeMapUrl = TryGetEpiPropValue(pd, "ShowInMapURL");
            string conferencePageUrl = EPiFunctions.GetFriendlyAbsoluteUrl(pd);

            DateTime dateConferenceStart = DateTime.MinValue;
            DateTime.TryParse(pd["Date"].ToString(), out dateConferenceStart);

            return new ConferenceEpiDataInput(0, epiPageId, headline, shortIntroTextHtml, place, placeMapUrl, conferencePageUrl, dateConferenceStart);
        }

        private static void AddConfsToDb(int batchId, List<ConferenceEpiDataInput> confs)
        {
            foreach (var conf in confs)
            {
                conf.BatchId = batchId;
                MsSqlHandler.EpiJob_Conf_InsertConfItem(conf);
            }
        }

        private static string TryGetEpiPropValue(PageData pd, string propId)
        {
            return (EPiFunctions.HasValue(pd, propId)) ? pd[propId].ToString() : "";
        }
    }
}