using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WS.DiIpad.Classes;


namespace WS.DiIpad
{
    public partial class Master : System.Web.UI.MasterPage
    {

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RssHandler.TryUpdateXmlFile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}