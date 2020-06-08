using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;

namespace DagensIndustri.Tools.Jobs.SyncSubsInfo
{
    [ScheduledPlugIn(DisplayName = "Synk av prenumeranter", Description = "Synkar kunder som flaggats som förändrade i Cirix expcustomertabell")]
    public class SyncSubsInfoJob
    {
        public static string Execute()
        {
            if (DIClassLib.Misc.MiscFunctions.GetAppsettingsValue("syncCustomersJobDeactivated") == "true")
                return "Jobbet kördes inte (ej exekverat från skarpa webbservern)";

            return DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncChangedCusts();
        }
    }
}