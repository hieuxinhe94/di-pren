using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;


namespace DagensIndustri.Templates.Public.Units.Placeable.MySettings2
{
    public partial class GoldMembership : UserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void DIGoldMembership_Click(object sender, EventArgs e)
        {
        //    //If user clicked button to become Di Gold member add the user to that role.
        //    if (((Button)sender).CommandArgument.ToUpper() == "START")
        //    {
        //        EPiFunctions.RedirectToPage(Page, EPiFunctions.GetDiGoldFlowPage(CurrentPage).PageLink, EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage));
        //    }
        //    else if (((Button)sender).CommandArgument.ToUpper() == "END")
        //    {
        //        //If user clicked button to end Di Gold membership remove the user from DI Gold role.
        //        DiRoleHandler.RemoveUserFromRoles(new string[] { DiRoleHandler.RoleDiGold });

        //        new CustomerPropertyHandler(Subscriber.Cusno, null, null, null, false, null);

        //        DeactivateDiGoldButton.Visible = false;
        //        ActivateDiGoldButton.Visible = true;
        //    }
        }


    }
}