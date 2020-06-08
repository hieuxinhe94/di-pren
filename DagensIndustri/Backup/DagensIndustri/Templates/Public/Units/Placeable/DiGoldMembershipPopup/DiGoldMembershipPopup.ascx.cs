using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DIClassLib.Membership;
using DIClassLib.Misc;
using EPiServer.Core;
using DagensIndustri.WebServicesPublic.BonnierDigital;


namespace DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup
{
    public partial class DiGoldMembershipPopup : UserControlBase
    {
        
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            DataBind();
        }

        protected void ButtonJoinGoldGoToLogin_Click(object sender, EventArgs e)
        {
            //EPiFunctions.RedirectToPage(Page, EPiFunctions.GetDiGoldFlowPage(CurrentPage).PageLink, ReturnURLHiddenField.Value);
            //string urlGoldStartPage = EPiFunctions.GetFriendlyAbsoluteUrl(EPiFunctions.GetDiGoldStartPage());
            //string urlGoldFlow = EPiFunctions.GetFriendlyAbsoluteUrl(EPiFunctions.GetDiGoldFlowPage(CurrentPage));
            Response.Redirect(EPiFunctions.GetSsoLoginPageUrl(CurrentPage, EPiFunctions.GetDiGoldFlowPage(CurrentPage).LinkURL), true);
        }

        protected void BecomeMember_Click(object sender, EventArgs e)
        {
            //EPiFunctions.RedirectToPage(Page, EPiFunctions.GetDiGoldFlowPage(CurrentPage).PageLink, ReturnURLHiddenField.Value);
            //string urlGoldFlow = EPiFunctions.GetFriendlyAbsoluteUrl(EPiFunctions.GetDiGoldFlowPage(CurrentPage));
            Response.Redirect(EPiFunctions.GetSsoLoginPageUrl(CurrentPage, EPiFunctions.GetDiGoldFlowPage(CurrentPage).LinkURL), true);
        }

        
        /// <summary>
        /// Register script for storing a value in the hidden field when a control is clicked
        /// </summary>
        /// <param name="control"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        //public void RegisterSetReturnURLScript(Control control, string url)
        //{
        //    string script = string.Empty;
        //    if (!HttpContext.Current.User.Identity.IsAuthenticated)
        //    {
        //        script = string.Format(@"$(document).ready(function() {{
        //                                            $('#{0}').click(function () {{
        //                                            $('#{1}').val('{3}');
        //                                            $('#{2}').val('{3}');
        //                                        }})
        //                                    }});",
        //                                    control.ClientID,
        //                                    NotLoggedInPopupControl.ReturnUrlHiddenFieldClientID,
        //                                    NotLoggedInPopupControl.LoginReturnUrlHiddenFieldClientID,
        //                                    url
        //                                );
        //    }
        //    else
        //    {
        //        script = string.Format(@"$(document).ready(function() {{
        //                                            $('#{0}').click(function () {{
        //                                            $('#{1}').val('{2}');
        //                                        }})
        //                                    }});",
        //                                    control.ClientID,
        //                                    LoggedInPopupControl.ReturnUrlHiddenFieldClientID,
        //                                    url
        //                                );
        //    }

        //    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "SetReturnURL_" + control.ClientID, script, true);
        //}
        
    }
}