using System;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;
using System.Data;
using System.Collections.Generic;
using DagensIndustri.Tools.Classes.Conference;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Extras;
using System.Text;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Tools.Admin.Conference
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Konferensadmin", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Konferensadmin", UrlFromUi = "/Tools/Admin/Conference/ConferenceAdmin.aspx", SortIndex = 2050)]
    public partial class ConferenceAdmin : System.Web.UI.Page
    {

        public ConferenceObject Conference
        {
            get
            {
                return (ConferenceObject)ViewState["ConferenceObject"];
            }
            set
            {
                ViewState["ConferenceObject"] = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                //Bind list
                //DdlConferences.DataSource = MsSqlHandler.GetAllConferences();
                //DdlConferences.DataBind();
                //DdlConferences.Items.Insert(0, new ListItem("--Välj konferens--", "0"));

                DataSet ds = MsSqlHandler.GetAllConferences();
                if (DIClassLib.DbHelpers.DbHelpMethods.DataSetHasRows(ds))
                { 
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DateTime dtStart = DateTime.MinValue;
                        try
                        {
                            PageData pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(dr["epipageid"].ToString()));
                            if (EPiFunctions.HasValue(pd, "Date"))
                                dtStart = DateTime.Parse(pd["Date"].ToString());
                        } catch { }

                        string dt = (dtStart > DateTime.MinValue) ? dtStart.ToString("yyyy-MM-dd") : string.Empty;
                        ListItem li = new ListItem(dr["name"].ToString() + " " + dt, dr["conferenceid"].ToString());
                        DdlConferences.Items.Add(li);
                    }
                }
                DdlConferences.Items.Insert(0, new ListItem("--Välj konferens--", "0"));
                

                if (Request.QueryString["conferenceid"] != null && Request.QueryString["conferenceid"] != "0")
                {
                    Conference = new ConferenceObject(int.Parse(Request.QueryString["conferenceid"].ToString()));
                    DdlConferences.SelectedValue = Conference.ConferenceId.ToString();

                    GvInfoChannels.DataSource = MsSqlHandler.GetInfoChannelsForConference(Conference.ConferenceId);
                    GvInfoChannels.DataBind();
                }
                else
                    PhSeminar.Visible = false;
            }
            else
            {
                Conference = new ConferenceObject(int.Parse(DdlConferences.SelectedValue));
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Conference != null)
                LblOverview.Text = GetConferenceOverviewTable();
        }

        public string GetConferenceOverviewTable()
        {
            StringBuilder result = new StringBuilder();
            result.Append("<table rules='all' cellspacing='0' border='1' style='border-collapse: collapse;' class='AdminTable'>");

            foreach (ConferenceEvent confEvent in Conference.GetEvents())
            {
                result.Append("<tr class='header'>");
                result.Append("<th><a href='/Tools/Admin/Conference/ConferenceAdmin.aspx?searchheading=" + confEvent.Name + "&conferenceid=" + Conference.ConferenceId + "&dayid=" + confEvent.Id + "'>" + confEvent.Name + "</a></th>");
                result.Append("<th>Antal anmälda</th>");
                result.Append("<th>Max antal</th>");
                result.Append("</tr>");

                int counter = 0;

                foreach (EventTime eventTime in Conference.GetEventTimes(confEvent.Id))
                {
                    foreach (EventActivity eventAct in Conference.GetEventActivities(eventTime.Id))
                    {
                        result.Append("<tr class='" + (counter++ % 2 == 0 ? "even" : "odd") + "'>");
                        result.Append("<td><a href='/Tools/Admin/Conference/ConferenceAdmin.aspx?searchheading=" + eventAct.Name + "&conferenceid=" + Conference.ConferenceId + "&dayid=" + confEvent.Id + "&seminarid=" + eventAct.Id + "'>" + eventAct.Name + "</a></td>");
                        result.Append("<td>" + eventAct.NrOfParticipants + "</td>");
                        result.Append("<td>" + eventAct.MaxParticipants + "</td>");
                        result.Append("</tr>");
                    }
                }
            }

            result.Append("</table>");

            return result.ToString();
        }


        #region Butt events

        protected void BtnExportOnClick(object sender, EventArgs e)
        {
            //Make a copy of gridview
            GridView gvToExport = GvPersons;

            gvToExport.AllowPaging = false;

            //If gridview is in editmode, reset
            if (gvToExport.EditIndex > -1)
                gvToExport.EditIndex = -1;

            //Remove first three columns
            gvToExport.Columns.RemoveAt(0);
            gvToExport.Columns.RemoveAt(0);
            gvToExport.Columns.RemoveAt(0);
            gvToExport.Columns.RemoveAt(1);
            gvToExport.Columns.RemoveAt(1);

            gvToExport.DataBind();
            //Export to excel
            GridViewExportUtil.Export(DdlConferences.SelectedItem.Text + ".xls", gvToExport);
        }

        protected void BtnExportPdfLogOnClick(object sender, EventArgs e)
        {
            GridViewExportUtil.Export(DdlConferences.SelectedItem.Text + "_pdflogg.xls", GvPdfDownload);
        }

        protected void BtnSearchOnClick(object sender, EventArgs e)
        {
            Response.Redirect("/Tools/Admin/Conference/ConferenceAdmin.aspx?searchheading=" + DdlConferences.SelectedItem.Text + "&conferenceid=" + DdlConferences.SelectedValue);
        }

        protected void BtnShowPdfOnClick(object sender, EventArgs e)
        {
            PhPdf.Visible = true;
            PhSeminar.Visible = false;

            GvPdfDownload.DataSource = Conference.GetPdfDownloadLog();
            GvPdfDownload.DataBind();
        }

        #endregion


        #region Gridview stuff

        protected void PersonDataSourceOnSelected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            DataSet ds = e.ReturnValue as DataSet;
            LblPersonCount.Text = ds.Tables[0].Rows.Count.ToString();
        }

        public string GetActivitiesForPersonTable(string personId)
        {
            StringBuilder result = new StringBuilder();

            foreach (EventActivity eventAct in Conference.GetEventActivitiesForPerson(int.Parse(personId)))
            {
                string eventName = eventAct.Info;
                string activityName = eventAct.Name;

                if (eventName.Equals(Request.QueryString["searchheading"]))
                    eventName = "<span style='background-color:yellow;'>" + eventName + "</span>";

                if (activityName.Equals(Request.QueryString["searchheading"]))
                    activityName = "<span style='background-color:yellow;'>" + activityName + "</span>";

                result.Append("<span class='sem'>(" + eventName + ") " + activityName + "</span>");
            }

            return result.ToString().Trim(' ', ',');
        }

        #endregion

        #region Helpers

        private void ShowError(string strMessage)
        {
            LblError.Text = strMessage;
        }

        #endregion

    }
}