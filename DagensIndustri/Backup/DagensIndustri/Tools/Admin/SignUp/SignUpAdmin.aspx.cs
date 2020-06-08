using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Conference;
using DagensIndustri.Tools.Classes.Extras;
using System.Data;
using System.Text;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri.Tools.Admin.SignUp
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Sign-up admin", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Sign-up admin", UrlFromUi = "/Tools/Admin/SignUp/SignUpAdmin.aspx", SortIndex = 2050)]
    public partial class SignUpAdmin : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                DdlConferences.DataSource = GetSignUpListItemCollection();
                DdlConferences.DataBind();
            }

            PlaceHolderExport.Visible = (Request.QueryString["epiPageId"] != null) ? true : false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Request.QueryString["epiPageId"] != null)
                DdlConferences.SelectedValue = Request.QueryString["epiPageId"].ToString();
        }


        private ListItemCollection GetSignUpListItemCollection()
        {
            ListItemCollection coll = new ListItemCollection();

            List<PageReference> regularStartPageDescen = EPiServer.DataFactory.Instance.GetDescendents(EPiFunctions.StartPage().PageLink).ToList();
            AddSignUpPageRefsToColl(regularStartPageDescen, coll);

            List<PageReference> goldStartPageDescen = EPiServer.DataFactory.Instance.GetDescendents(EPiFunctions.GetDiGoldStartPage().PageLink).ToList();
            AddSignUpPageRefsToColl(goldStartPageDescen, coll);

            SortListItems(coll, false);
            return coll;
        }

        private void AddSignUpPageRefsToColl(List<PageReference> descen, ListItemCollection coll)
        {
            foreach (PageReference pr in descen)
            {
                PageData pd = EPiServer.DataFactory.Instance.GetPage(pr);
                if (IsSignUpPageType(pd))
                    coll.Add(new ListItem(pd.PageName, pd.PageLink.ID.ToString()));
            }
        }

        private bool IsSignUpPageType(PageData pd)
        {
            if (EPiFunctions.IsMatchingPageType(pd, pd.PageTypeID, "SignUpPageType") ||
                EPiFunctions.IsMatchingPageType(pd, pd.PageTypeID, "SignUpWithFriendsJoinGoldPageType") ||
                EPiFunctions.IsMatchingPageType(pd, pd.PageTypeID, "SignUpGoldWithFriendsPageType"))
            {
                return true;
            }

            return false;
        }

        private void SortListItems(ListItemCollection items, bool Descending)
        {
            List<ListItem> list = new List<ListItem>();
            foreach (ListItem i in items)
                list.Add(i);

            if (Descending)
                list.Sort(delegate(ListItem x, ListItem y) { return y.Text.CompareTo(x.Text); });
            else
                list.Sort(delegate(ListItem x, ListItem y) { return x.Text.CompareTo(y.Text); });

            items.Clear();
            items.AddRange(list.ToArray());
        }

        protected void BtnExportOnClick(object sender, EventArgs e)
        {
            //Make a copy of gridview
            GridView gvToExport = GvPersons;

            gvToExport.AllowPaging = false;

            //If gridview is in editmode, reset
            if (gvToExport.EditIndex > -1)
                gvToExport.EditIndex = -1;

            //Remove initial columns
            gvToExport.Columns.RemoveAt(0);
            gvToExport.Columns.RemoveAt(0);
            //gvToExport.Columns.RemoveAt(0);
            //gvToExport.Columns.RemoveAt(1);
            //gvToExport.Columns.RemoveAt(1);

            gvToExport.DataBind();

            //Export to excel
            GridViewExportUtil.Export(DdlConferences.SelectedItem.Text + ".xls", gvToExport);
        }

        protected void BtnSearchOnClick(object sender, EventArgs e)
        {
            Response.Redirect("/Tools/Admin/SignUp/SignUpAdmin.aspx?searchheading=" + DdlConferences.SelectedItem.Text + "&epiPageId=" + DdlConferences.SelectedValue);
        }


        protected void PersonDataSourceOnSelected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            DataSet ds = e.ReturnValue as DataSet;
            LblPersonCount.Text = ds.Tables[0].Rows.Count.ToString();
        }

    }
}