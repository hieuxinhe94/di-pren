using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.Security;
using DagensIndustri.Templates.Public.Units.Placeable;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.WebControls;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class StartPagePuffList : EPiServer.UserControlBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                DiLinkCollection linkCollection = new DiLinkCollection(CurrentPage, "PuffLinks");
                PuffListRepeater.DataSource = linkCollection.SelectedPages(false);
                PuffListRepeater.DataBind();
            }
        }

        protected string GetPuffUrl(PageData pd, string propertyName)
        {
            return EPiFunctions.HasValue(pd, "PuffUrl")
                    ? pd["PuffUrl"].ToString()
                    : pd.LinkURL;
        }

        protected string GetPuffHeading(PageData pd)
        {
            string puffHeading = pd.PageName;
            if (!string.IsNullOrEmpty(pd["PuffHeading"] as string))
            {
                puffHeading = pd["PuffHeading"].ToString();
            }
            else if (!string.IsNullOrEmpty(pd["Heading"] as string))
            {
                puffHeading = pd["Heading"].ToString();
            }

            return puffHeading;
        }

        protected void PuffListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink membershipRequiredHyperLink;

                membershipRequiredHyperLink = EPiFunctions.HasValue((EPiServer.Core.PageData)e.Item.DataItem, "PuffText") 
                    ? e.Item.FindControl("MembershipRequiredWithTextHyperLink") as HyperLink
                    : e.Item.FindControl("MembershipRequiredWithoutTextHyperLink") as HyperLink;

                DiGoldMembershipPopup.DiGoldMembershipPopup diGoldMembershipPopup = EPiFunctions.FindDiGoldMembershipPopup(Page) as DiGoldMembershipPopup.DiGoldMembershipPopup;
                if (diGoldMembershipPopup != null)
                {
                    string puffUrl = GetPuffUrl((PageData)e.Item.DataItem, "PuffUrl");
                    string absoluteFriendlyURL = EPiFunctions.GetFriendlyAbsoluteUrl(puffUrl);
                    //diGoldMembershipPopup.RegisterSetReturnURLScript(membershipRequiredHyperLink, absoluteFriendlyURL);
                }
            }
        }


        protected bool ShowPuff(PageData pd)
        {
            return (!EPiFunctions.IsUserDIGoldMember() || !EPiFunctions.HasValue(pd, "PuffHideFromDiGoldMember"));
        }
    }
}