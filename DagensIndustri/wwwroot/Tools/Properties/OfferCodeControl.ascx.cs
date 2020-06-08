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

namespace DagensIndustri.Tools.Properties
{
    public partial class OfferCodeControl : System.Web.UI.UserControl
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void btnAddOnClick(object sender, EventArgs e)
        {
            if (ListAvailable.SelectedItem != null)
            {
                ListSelected.Items.Add(ListAvailable.SelectedItem);
                ListAvailable.Items.Remove(ListAvailable.SelectedItem);
                ListSelected.ClearSelection();
                ListAvailable.ClearSelection();
            }
        }

        protected void btnRemoveOnClick(object sender, EventArgs e)
        {
            if (ListSelected.SelectedItem != null)
            {
                ListAvailable.Items.Add(ListSelected.SelectedItem);
                ListSelected.Items.Remove(ListSelected.SelectedItem);
                ListSelected.ClearSelection();
                ListAvailable.ClearSelection();
            }
        }

        public void FillAvailableOfferCodes()
        {
            //Get all offercode available from db
            //DataSet dsOfferCodes = MsSqlHandler.GetOfferCodes(1);

            //foreach (DataRow dr in dsOfferCodes.Tables[0].Rows)
            //{
            //    string text = dr["offerText"] + " [" + dr["campId"] + "]";
            //    string value = dr["offerCodeId"].ToString();
            //    ListAvailable.Items.Add(new ListItem(text, value));
            //}
        }

        //public void FillSelectedOfferCodes(PageData currentPage)
        //{
        //    //Get all offercodes for this campaign
        //    DataSet dsOfferCodes = currentPage.GetOfferCodes();

        //    foreach (DataRow dr in dsOfferCodes.Tables[0].Rows)
        //    {
        //        string text = dr["offerText"] + " [" + dr["campId"] + "]";
        //        string value = dr["offerCodeId"].ToString();
        //        ListItem item = new ListItem(text, value);

        //        ListSelected.Items.Add(item);

        //        ListAvailable.Items.Remove(item);
        //    }
        //}

        public ListItemCollection GetSelectedOfferCodes()
        {
            return ListSelected.Items;
        }

    }
}