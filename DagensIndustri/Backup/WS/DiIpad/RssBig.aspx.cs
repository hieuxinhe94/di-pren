using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WS.DiIpad
{
    public partial class RssBig : System.Web.UI.Page
    {
        public string Id = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["id"] != null)
                Id = Request.QueryString["id"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}