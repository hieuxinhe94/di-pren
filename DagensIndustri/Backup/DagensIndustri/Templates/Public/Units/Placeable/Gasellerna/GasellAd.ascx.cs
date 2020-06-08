using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Units.Placeable.Gasellerna
{
    public partial class GasellAd : GasellUserControlBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            //hide image if property not set
            PhImage.Visible = ActualCurrentPage["AdImage"] != null;

            //only register js if property is set
            if (ActualCurrentPage["AdFlashMovie"] != null)
            {
                //Register javascript for swfobject
                RegisterClientScriptFile("/Templates/Public/js/swfobject.js");
                //Show placeholder for script
                PhFlashScript.Visible = true;
            }
        }
    }
}