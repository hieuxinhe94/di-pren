using System;
using EPiServer.PlugIn;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Misc;
using DIClassLib.CardPayment.Autowithdrawal;

namespace DagensIndustri.Tools.Jobs.Autowithdrawal
{
    [ScheduledPlugIn(DisplayName = "Autodragning på kontokort", Description = "Autodragning på kontokort")]
    public class MyJob
    {

        public static string Execute()
        {
            if (MiscFunctions.GetAppsettingsValue("awdJobDeactivated") == "true")
                return "Jobbet kördes inte (ej exekverat från skarpa webbservern)";

            var awh = new AutowithdrawalHandler();
            string ret = awh.MakeAutowithdrawalPayments();

            //log message displayed in EPi
            return ret;
        }

    }
}

