using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DIClassLib.ServiceVerifier;

namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class GoogleMaps : EPiServer.UserControlBase
    {
        public string address { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!ServiceVerifier.GoogleMapsIsValid)
            {
                this.Visible = false;
                return;
            }

            if (IsValue("GoogleMapsAddress"))
                address = CurrentPage["GoogleMapsAddress"].ToString();
            else
                address = "Vasagatan 11, Stockholm, Sverige";
        }
    }
}