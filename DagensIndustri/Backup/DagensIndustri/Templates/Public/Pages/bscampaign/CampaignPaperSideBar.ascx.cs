using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.WebControls;

namespace DagensIndustri.Templates.Public.Pages.bscampaign
{
    public partial class CampaignPaperSideBar : EPiServer.UserControlBase
    {
        protected string PropertyPrefix { get; set; }

        public void BindPapers(string propertyPrefix, string fallbackPrefix) {

            //If property is not set, use fallback property
            PropertyPrefix = IsValue(propertyPrefix + "IncludedProducts") ? propertyPrefix : fallbackPrefix;

            if (CurrentPage[PropertyPrefix + "IncludedProducts"] != null)
            {
                var pdc = new PageDataCollection();
                var productPages = CurrentPage[PropertyPrefix + "IncludedProducts"].ToString().Split(',');
                foreach (var product in productPages)
                {
                    try
                    {
                        pdc.Add(GetPage(new PageReference(int.Parse(product))));
                    }
                    catch
                    {
                        //OK, editor deleted page (and ignored warning from EpiServer)
                        //GetPage will explode, no other way to check if page exist
                    }

                }
                PlSideBar.DataSource = pdc;
                PlSideBar.DataBind();
            }
        }
    }
}