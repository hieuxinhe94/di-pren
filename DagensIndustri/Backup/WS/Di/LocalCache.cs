using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DIClassLib.EPiJobs.EpiDataForExternalUse;
using System.Data;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace WS.Di
{
    public static class LocalCache
    {

        private static DateTime _lastUpdate           = DateTime.MinValue;
        private static List<ConferenceEpiData> _confs = null;
        

        public static List<ConferenceEpiData> GetUpcomingConferences(int wantedNumOfConfrences)
        {
            if (_confs == null || CacheIsOut || (_confs.Count < wantedNumOfConfrences))
            {
                _confs = GetUpcomingConferencesFromDb(wantedNumOfConfrences);
                _lastUpdate = DateTime.Now;                
            }

            return _confs;
        }


        private static bool CacheIsOut
        {
            get
            {
                int cacheMinutes = 0;
                int.TryParse(MiscFunctions.GetAppsettingsValue("UpcomingConferencesCacheMinutes"), out cacheMinutes);
                return (_lastUpdate.AddMinutes(cacheMinutes) < DateTime.Now);
            }
        }


        private static List<ConferenceEpiData> GetUpcomingConferencesFromDb(int numUpcomingConfrences)
        {
            var confs = new List<ConferenceEpiData>();

            DataSet ds = MsSqlHandler.EpiJob_Conf_GetUpcomingConferences(numUpcomingConfrences);
            
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string headline             = dr["headline"].ToString();
                    string shortIntroTextHtml   = dr["shortIntroTextHtml"].ToString();
                    string place                = dr["place"].ToString();
                    string placeMapUrl          = dr["placeMapUrl"].ToString();
                    string conferencePageUrl    = dr["conferencePageUrl"].ToString();
                    DateTime dateConferenceStart = DateTime.Parse(dr["dateConferenceStart"].ToString());

                    confs.Add(new ConferenceEpiData(headline, shortIntroTextHtml, place, placeMapUrl, conferencePageUrl, dateConferenceStart));
                }
            }

            return confs;
        }

    }
}