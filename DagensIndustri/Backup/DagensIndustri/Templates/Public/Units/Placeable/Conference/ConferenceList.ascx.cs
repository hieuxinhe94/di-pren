using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;

namespace DagensIndustri.Templates.Public.Units.Placeable.Conference
{
    public partial class ConferenceList : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PageDataCollection MainConferenceCollection = new PageDataCollection();
            //PageDataCollection SecondaryPageDataCollection = new PageDataCollection();

            if (IsValue("ConferenceStartNode"))
            {
                PageDataCollection pdc  = EPiServer.DataFactory.Instance.GetChildren(CurrentPage["ConferenceStartNode"] as PageReference);
                FilterForVisitor.Filter(pdc);
                new FilterPropertySort("Date", FilterSortDirection.Ascending).Filter(pdc);

                //int i = 0;
                foreach (PageData pd in pdc)
                {
                    //i = i + 1;
                    //if (i <= 3)
                    MainConferenceCollection.Add(pd);
                    //else
                    //    SecondaryPageDataCollection.Add(pd);
                }

                MainConferencePageList.DataSource = MainConferenceCollection;
                //SecondaryConferencePageList.DataSource = SecondaryPageDataCollection;
            }

            MainConferencePageList.DataBind();
            //SecondaryConferencePageList.DataBind();

            //if (!(SecondaryConferencePageList.DataCount > 0))
            //    Heading2PlaceHolder.Visible = false;
        }
    }
}