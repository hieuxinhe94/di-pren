using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using DIClassLib.DbHelpers;
using System.IO;
using System.Configuration;
using DIClassLib.Misc;

namespace WS.DiIpad.Classes
{
    public static class RssHandler
    {
        
        static DateTime _timeUpdated = DateTime.MinValue;
        static XmlDocument _xmlDoc = null;

        public static bool ShouldUpdateXml 
        {
            get 
            {
                if (_xmlDoc == null || _timeUpdated.AddMinutes(1) < DateTime.Now)
                    return true;

                return false;
            }
        }

        
        public static void TryUpdateXmlFile()
        {
            try
            {
                if (!ShouldUpdateXml)
                    return;

                XmlDocument doc = TryGetRssFromDise();
                if (doc == null)
                    return;

                string pathBase = MiscFunctions.GetAppsettingsValue("DiIpadPathToData");
                string pathTmp = pathBase + "DataLatestTmp.xml";    //why tmp file? org file might be in use / on error org file is hopefully ok
                string pathOrg = pathBase + "DataLatest.xml";

                doc.Save(pathTmp);
                FileInfo fi = new FileInfo(pathTmp);
                fi.CopyTo(pathOrg, true);               //replace org file
            }
            catch (Exception ex)
            {
                new Logger("TryUpdateXmlFile() - failed", ex.ToString());
            }
        }

        private static XmlDocument TryGetRssFromDise()
        {
            try
            {
                string url = MiscFunctions.GetAppsettingsValue("DiIpadPathDiRss");
                XmlTextReader reader = new XmlTextReader(url + "?latest=" + MiscFunctions.GetAppsettingsValue("DiIpadNumRssItems") + "&text=1&comments=1");
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                _xmlDoc = doc;
                _timeUpdated = DateTime.Now;
                //return doc.SelectSingleNode("root/status").InnerText;
                //return _xmlDocRssLatest.InnerXml;  //string
            }
            catch (Exception ex)
            {
                new Logger("TryGetRssFromDise() failed", ex.ToString());
                _xmlDoc = null;
            }

            return _xmlDoc;
        }


    }
}