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


namespace DagensIndustri.Tools.Admin.Gasell
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Gaselladmin", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Gaselladmin", UrlFromUi = "/Tools/Admin/Gasell/GasellAdmin.aspx", SortIndex = 2050)]
    public partial class GasellAdmin : System.Web.UI.Page
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                DdlConferences.DataSource = GetGasellListItemCollection();
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


        private ListItemCollection GetGasellListItemCollection()
        {
            ListItemCollection coll = new ListItemCollection();

            if (EPiFunctions.SettingsPageSetting(null, "GasellMeetingsContainer") != null)
            {
                PageData gasMeetCont = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "GasellMeetingsContainer") as PageReference);
                List<PageReference> descen = EPiServer.DataFactory.Instance.GetDescendents(gasMeetCont.PageLink).ToList();

                foreach (PageReference pRef in descen)
                {
                    try
                    {
                        PageData pd = EPiServer.DataFactory.Instance.GetPage(pRef);

                        DateTime dtStart = DateTime.MinValue;
                        if (EPiFunctions.IsMatchingPageType(pd, pd.PageTypeID, "GasellMeetingPageType"))
                        {
                            if (EPiFunctions.HasValue(pd, "Date"))
                                dtStart = DateTime.Parse(pd["Date"].ToString());
                            
                            string dt = (dtStart > DateTime.MinValue) ? dtStart.ToString("yyyy-MM-dd") : string.Empty;

                            coll.Add(new ListItem(pd.PageName + " " + dt, pd.PageLink.ID.ToString()));
                        }
                    }
                    catch { }
                }

                #region get direct children (not all descendents)
                //PageDataCollection pdc = EPiServer.DataFactory.Instance.GetChildren(gasMeetCont.PageLink);
                //foreach (PageData p in pdc)
                //if (EPiFunctions.IsMatchingPageType(p, p.PageTypeID, "GasellMeetingPageType"))
                //    coll.Add(new ListItem(p.PageName, p.PageLink.ID.ToString()));
                #endregion
            }

            SortListItems(coll, false);
            return coll;
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
            Response.Redirect("/Tools/Admin/Gasell/GasellAdmin.aspx?searchheading=" + DdlConferences.SelectedItem.Text + "&epiPageId=" + DdlConferences.SelectedValue);
        }
        

        protected void PersonDataSourceOnSelected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            DataSet ds = e.ReturnValue as DataSet;
            LblPersonCount.Text = ds.Tables[0].Rows.Count.ToString();
        }

    }
}