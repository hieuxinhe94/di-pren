using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class MainBody : EPiServer.UserControlBase
    {
        /// <summary>
        /// enables overwriting of text in main body
        /// </summary>
        public string Text 
        {
            set
            {
                string s = CleanUp(value);
                if (!string.IsNullOrEmpty(s))
                    MainBodyLiteral.Text = s;
            }
        }

        

        //protected override void OnLoad(EventArgs e)
        protected override void OnPreRender(EventArgs e)
        {
            base.OnLoad(e);

            MainBodyLiteral.Visible = true;

            //main body has not been overwritten by Text prop
            if (string.IsNullOrEmpty(MainBodyLiteral.Text) && IsValue("MainBody"))
                MainBodyLiteral.Text = CleanUp(CurrentPage.Property["MainBody"].ToString());

            if(string.IsNullOrEmpty(MainBodyLiteral.Text))
                MainBodyLiteral.Visible = false;
        }

        private string CleanUp(string s)
        {
            if (!string.IsNullOrEmpty(s) && s.StartsWith("<div>") && s.EndsWith("</div>"))
                return s.Remove(0, 5).Remove(s.Length - 6, 6);
            
            return s;
        }

    }
}