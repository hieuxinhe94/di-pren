using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup
{
    public partial class DiGoldMembershipPopupNotLoggedIn : UserControlBase
    {
        #region Properties
        /// <summary>
        /// Client ID of the hiddenfield containing the return url
        /// </summary>
        public string ReturnUrlHiddenFieldClientID
        {
            get
            {
                return ReturnURLHiddenField.ClientID;
            }
        }

        /// <summary>
        /// Client ID of login control's hiddenfield containing the return url
        /// </summary>
        public string LoginReturnUrlHiddenFieldClientID
        {
            get
            {
                return LoginCtrl.HiddenFieldClientID;
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                DataBind();
            }
        }
        #endregion
    }
}