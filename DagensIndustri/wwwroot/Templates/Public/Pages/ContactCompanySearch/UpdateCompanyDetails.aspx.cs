using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Pages.ContactCompanySearch
{
    public partial class UpdateCompanyDetails : DiTemplatePage
    {
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            if (!IsPostBack)
            {
                EPiFunctions.SetAttributeOnControl(Page.Master, "Body", "class", "bizbook");
            }
        }
        #endregion
    }
}