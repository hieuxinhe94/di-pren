using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
using System.Web.Security;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.Wine;
using System.Configuration;

namespace DagensIndustri.Tools.Jobs.Wine
{
    [ScheduledPlugIn(DisplayName = "Update Systembolaget Articles", Description = "Updates the local Systembolaget database")]
    public class UpdateSystebolagetArticles
    {
        public static string Execute()
        {
            try
            {
                String uri = ConfigurationManager.AppSettings["wineSystembolagetApiUri"];
                if (String.IsNullOrEmpty(uri))
                {
                    throw new Exception("URI to Systembolaget XML is missing in AppSettings. (wineSystembolagetApiUri)");
                }
                SystembolagetImport importer = new SystembolagetImport(uri);
                importer.UpdateDatabase();
            } catch(Exception ex){
                return "Failed: " + ex.Message;
            }
            return "";

        }

        
    }
}
       