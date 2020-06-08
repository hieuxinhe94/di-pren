using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Wine;

namespace DagensIndustri.Tools.Admin.Wine
{
    public partial class UpdateDatabase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int importCount = 0;
            try
            {
                SystembolagetImport importer = new SystembolagetImport(tbUri.Text);
                importCount = importer.UpdateDatabase(-1);
            }
            catch (Exception ex)
            {
                lbMessage.Text = "Fel: " + ex.Message;
                return;
            }

            lbMessage.Text = "Import genomförd. Uppdaterat antal: " + importCount ;
        }
    }
}