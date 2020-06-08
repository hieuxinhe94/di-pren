using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;

namespace DagensIndustri.Tools.Admin.Student
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Studentverifiering", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Studentverifiering", UrlFromUi = "/Tools/Admin/Student/VerifyStudent.aspx", SortIndex = 1010)]
    public partial class VerifyStudent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}