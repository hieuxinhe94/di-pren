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


namespace DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup_BU
{
    public partial class DiGoldMembershipPopup : UserControlBase
    {
        #region Events
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            DataBind();
        }
        #endregion 

        #region Methods
        /// <summary>
        /// Register script for storing a value in the hidden field when a control is clicked
        /// </summary>
        /// <param name="control"></param>
        /// <param name="url"></param>
        /// <returns></returns>
//        public void RegisterSetReturnURLScript(Control control, string url)
//        {
//            string script = string.Empty;
//            if (!HttpContext.Current.User.Identity.IsAuthenticated)
//            {
//                script = string.Format(@"$(document).ready(function() {{
//                                                    $('#{0}').click(function () {{
//                                                    $('#{1}').val('{3}');
//                                                    $('#{2}').val('{3}');
//                                                }})
//                                            }});",
//                                            control.ClientID,
//                                            NotLoggedInPopupControl.ReturnUrlHiddenFieldClientID,
//                                            NotLoggedInPopupControl.LoginReturnUrlHiddenFieldClientID,
//                                            url
//                                        );
//            }
//            else
//            {
//                script = string.Format(@"$(document).ready(function() {{
//                                                    $('#{0}').click(function () {{
//                                                    $('#{1}').val('{2}');
//                                                }})
//                                            }});",
//                                            control.ClientID,
//                                            LoggedInPopupControl.ReturnUrlHiddenFieldClientID,
//                                            url
//                                        );
//            }

//            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "SetReturnURL_" + control.ClientID, script, true);
//        }
        #endregion
    }
}