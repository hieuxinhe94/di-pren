using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus
{
    public partial class _4_Error : System.Web.UI.UserControl
    {
        public string ErrMess 
        { 
            set { LiteralErr.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}