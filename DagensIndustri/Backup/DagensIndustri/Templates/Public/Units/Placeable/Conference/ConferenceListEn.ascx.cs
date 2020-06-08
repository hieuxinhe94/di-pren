using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using System.Threading;

namespace DagensIndustri.Templates.Public.Units.Placeable.Conference
{
    public partial class ConferenceListEn : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (CurrentPage["LanguageEnglish"] != null)
            {
                System.Globalization.CultureInfo oldCulture = Thread.CurrentThread.CurrentCulture;
                System.Globalization.CultureInfo oldUICulture = Thread.CurrentThread.CurrentUICulture;
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }

            DataBind();

            if (IsValue("ConferenceStartNode"))
            {
                PageDataCollection pdc = EPiServer.DataFactory.Instance.GetChildren(CurrentPage["ConferenceStartNode"] as PageReference);
                ConferenceEnPageList.DataSource = pdc;
            }

            ConferenceEnPageList.DataBind();
        
            if(!(ConferenceEnPageList.DataCount > 0))
                this.Visible = false;
        }
        
    }
}