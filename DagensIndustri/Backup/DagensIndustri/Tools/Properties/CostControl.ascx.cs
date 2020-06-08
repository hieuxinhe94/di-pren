using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EPiServer.Core;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Campaign;
using DIClassLib.Misc;


namespace DagensIndustri.Tools.Properties
{
    public partial class CostControl : System.Web.UI.UserControl
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                Sum();
            }
        }

        private void Sum()
        {
            int sum = 0;

            foreach (ListItem item in ListCosts.Items)
            {
                sum += int.Parse(item.Value);
            }

            LblSum.Text = sum + " kr";
        }

        protected void btnAddOnClick(object sender, EventArgs e)
        {
            //I got some problems using asp:validators in Epi-editmode. So ugly check will do.

            if (string.IsNullOrEmpty(TxtDescription.Text))
            {
                LblError.Text = EPiServer.Core.LanguageManager.Instance.Translate("/campaigns/errors/description");
                return;
            }

            if (string.IsNullOrEmpty(TxtAmount.Text))
            {
                LblError.Text = EPiServer.Core.LanguageManager.Instance.Translate("/campaigns/errors/amount"); ;
                return;
            }

            if (!MiscFunctions.IsNumeric(TxtAmount.Text))
            {
                LblError.Text = EPiServer.Core.LanguageManager.Instance.Translate("/campaigns/errors/amountint"); ;
                return;
            }

            ListItem item = new ListItem(TxtDescription.Text + " (" + TxtAmount.Text + " kr)", TxtAmount.Text);
            ListCosts.Items.Add(item);

            TxtDescription.Text = string.Empty;
            TxtAmount.Text = string.Empty;

            Sum();
        }

        protected void btnDeleteOnClick(object sender, EventArgs e)
        {
            if (ListCosts.SelectedItem != null)
            {
                ListCosts.Items.Remove(ListCosts.SelectedItem);
                Sum();
            }
        }

        //public void FillCosts(PageData currentPage)
        //{
        //    //Get all offercodes for this campaign
        //    DataSet dsCosts = currentPage.GetCosts();

        //    foreach (DataRow dr in dsCosts.Tables[0].Rows)
        //    {
        //        string text = dr["description"].ToString() + " (" + dr["amount"].ToString() + " kr)";
        //        string value = dr["amount"].ToString();
        //        ListItem item = new ListItem(text, value);

        //        ListCosts.Items.Add(item);
        //    }

        //    Sum();
        //}

        public ListItemCollection GetCosts()
        {
            return ListCosts.Items;
        }


    }
}