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
    public partial class GasellPuff : GasellUserControlBase
    {
        public string linkURL { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageData pd = EPiServer.DataFactory.Instance.GetPage(ActualCurrentPage["PuffLink"] as PageReference);

            linkURL = GetFriendlyAbsoluteUrl(pd);
        }
    }
}