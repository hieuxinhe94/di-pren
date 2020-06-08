using System.Collections.Generic;
using EPiServer.Core;
using System.Web.UI.WebControls;
using System.Data;
using DIClassLib.DbHandlers;
using System;
using DIClassLib.Subscriptions;

namespace DagensIndustri.Templates.Public.Pages.Competition
{

    public partial class CompetitionQuestion : EPiServer.TemplatePage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DataBind();
        }

    }
}