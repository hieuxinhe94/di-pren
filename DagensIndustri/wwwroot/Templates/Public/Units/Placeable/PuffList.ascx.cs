using System;
using EPiServer.Core;
using System.Web.UI.HtmlControls;
using EPiServer;
using DagensIndustri.Tools.Classes.WebControls;

namespace DagensIndustri.Templates.Public.Units.Placeable
{

    public partial class PuffList : UserControlBase
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                if (PhPuffList.Visible = IsValue("PuffLinks"))
                {
                    DiLinkCollection linkCollection = new DiLinkCollection(CurrentPage, "PuffLinks");
                    RepPuffList.DataSource = linkCollection.SelectedPages(false);
                    RepPuffList.DataBind(); 
                }
            }
        }

        protected string GetSizeClass(PageData page)
        {
            return page["PuffSize"] != null ? page["PuffSize"].ToString() : "standard"; 
        }

        protected string GetAlignClass(PageData page)
        {
            return page["PuffImgAlign"] != null ? page["PuffImgAlign"].ToString() : "left";
        }

        protected string GetLink(PageData page) 
        {
            string link = "<a href=\"{0}\" {1}>{2}</a>"; 

            //User got read access
            if (page.QueryDistinctAccess(EPiServer.Security.AccessLevel.Read))
            {
                string target = page["PageTargetFrame"] != null ? " target='" + page["PageTargetFrame"] + "'" : string.Empty;
                string url = page.LinkURL;

                link = string.Format(link, url, target, "läs");
            }
            //No access, show layer
            else
            {
                //what if layerbody not set?, default fallback text from langfile?
                link = string.Format(link, "javascript:alert('" + page["LayerBody"] as string + "');", string.Empty, "Inga rättigheter");
            }

            return link;
        } 
    }
}