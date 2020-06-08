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
using System.Collections.Generic;
using DagensIndustri.Tools.Classes.Conference;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Tools.Properties
{
   public partial class SeminarControl : System.Web.UI.UserControl
    {
       protected ConferenceEvent SelectedEvent
       {
           get { return ViewState["SelectedEvent"] as ConferenceEvent; }
           set { ViewState["SelectedEvent"] = value; }
       }

       protected EventTime SelectedTime
       {
           get { return ViewState["SelectedTime"] as EventTime; }
           set { ViewState["SelectedTime"] = value; }
       }

       protected EventActivity SelectedActivity
       {
           get { return ViewState["SelectedActivity"] as EventActivity; }
           set { ViewState["SelectedActivity"] = value; }
       }


       protected override void OnInit(EventArgs e)
       {
           base.OnInit(e);
           
       }

       protected override void OnLoad(EventArgs e)
       {
           base.OnLoad(e);
           errorDiv.Visible = false;
       }

        public ConferenceObject Conference
        {
            get
            {
                return (ConferenceObject)ViewState["Conference"];
            }
            set
            {
                ViewState["Conference"] = value;
            }
        }

        public void InitConference(PageData currentPage)
        {
            Conference = new ConferenceObject(currentPage);
        }

        public void ShowPlaceHolders(bool showEvent, bool showTime, bool showActivity)
        {
            PhAddEvent.Visible = showEvent;
            PhAddTime.Visible = showTime;
            PhAddActivity.Visible = showActivity;
        }

        #region Events

        public void ShowEvents()
        {
            LoadTree();
            ListEvents.Items.Clear();
            
            foreach (ConferenceEvent confEvent in Conference.GetEvents())
            {
                ListEvents.Items.Add(new ListItem(confEvent.Name + " [" + confEvent.Date.ToString("yyyy-MM-dd") + "]", confEvent.Id.ToString()));                
            }
        }

        protected void LoadTree()
        {
            
                tvEvents.Nodes.Clear();
                foreach (ConferenceEvent confEvent in Conference.GetEvents())
                {
                    // The event
                    //
                    TreeNode eventNode = new TreeNode(confEvent.Name + " [" + confEvent.Date.ToString("yyyy-MM-dd") + "]", confEvent.Id.ToString());
                    if (SelectedEvent != null && SelectedEvent.Id == confEvent.Id)
                        eventNode.Selected = true;

                    tvEvents.Nodes.Add(eventNode);

                    // The times
                    //
                    List<EventTime> eventTimes = Conference.GetEventTimes(confEvent.Id);
                    foreach (EventTime eventTime in eventTimes)
                    {
                        TreeNode timeNode = new TreeNode(eventTime.TimeStart + "-" + eventTime.TimeEnd, eventTime.Id.ToString());
                        if (SelectedTime != null && SelectedTime.Id == eventTime.Id)
                            timeNode.Selected = true;

                        eventNode.ChildNodes.Add(timeNode);
                        foreach (EventActivity eventAct in Conference.GetEventActivities(eventTime.Id))
                        {
                            TreeNode activityNode = new TreeNode(eventAct.Name + " [" + eventAct.MaxParticipants + "]", eventAct.Id.ToString());
                            if (SelectedActivity != null && SelectedActivity.Id == eventAct.Id)
                                activityNode.Selected = true;
                            timeNode.ChildNodes.Add(activityNode);
                        }
                    }


                
                tvEvents.ExpandAll();
            }
        }

        protected void btnShowNewEventFields_Click(object sender, EventArgs e)
        {
            multiEditFields.SetActiveView(viewAddEvent);
        }

        protected void btnAddNewEvent_Click(object sender, EventArgs e)
        {
            String eventName = tbAddNewEventName.Text.Trim();
            if (String.IsNullOrEmpty(eventName))
            {
                SetErrorMessage("Ange namn");
                return;
            }

            DateTime dt = DateTime.MinValue;
            if (!DateTime.TryParse(tbAddNewEventDate.Text, out dt))
            {
                SetErrorMessage("Ange datum på formatet åååå-mm-dd");
                return;
            }

            try
            {
                Conference.InsertEvent(eventName,"0",dt);
                tbAddNewEventName.Text = "";
                tbAddNewEventDate.Text = "";
                ShowEvents();
            }
            catch (Exception ex)
            {
                SetErrorMessage("Misslyckades att spara event: " + ex.Message);
                return;
            }
            multiEditFields.ActiveViewIndex = -1;
        }

        protected void btnUpdateEvent_Click(object sender, EventArgs e)
        {
            String eventName = tbEventName.Text.Trim();
            if (String.IsNullOrEmpty(eventName))
            {
                SetErrorMessage("Ange namn");
                return;
            }

            DateTime dt = DateTime.MinValue;
            if (!DateTime.TryParse(tbEventDate.Text, out dt))
            {
                SetErrorMessage("Ange datum på formatet åååå-mm-dd");
                return;
            }

            try
            {
                Conference.UpdateEvent(SelectedEvent.Id, eventName, dt);
                ShowEvents();
            }
            catch (Exception ex)
            {
                SetErrorMessage("Misslyckades att uppdatera event: " + ex.Message);
            }
        }

        protected void btnDeleteEvent_Click(object sender, EventArgs e)
        {
            if (SelectedEvent == null)
            {
                SetErrorMessage("Inget event är valt");
                return;
            }
            try
            {
                Conference.DeleteEvent(SelectedEvent.Id);
            }
            catch (Exception ex)
            {
                SetErrorMessage("Misslyckades att radera event: " + ex.Message);
                return;
            }
            ShowEvents();
        }

        protected void BtnShowAddEventOnClick(object sender, EventArgs e)
        {
            ShowPlaceHolders(!PhAddEvent.Visible, false, false);
        }

        protected void BtnAddEventOnClick(object sender, EventArgs e)
        {
            Conference.InsertEvent(TxtEventName.Text, "0", DateTime.Parse(TxtEventDate.Text));

            ShowEvents();

            TxtEventName.Text = string.Empty;
            TxtEventDate.Text = string.Empty;
            PhAddEvent.Visible = false;
        }        

        protected void BtnDeleteEventOnClick(object sender, EventArgs e)
        {
            if (ListEvents.SelectedIndex > -1)
            {
                try
                {
                    Conference.DeleteEvent(int.Parse(ListEvents.SelectedValue));

                    ShowEvents();
                }
                catch
                {
                    LblError.Text = "Ett fel uppstod. Du kan inte ta bort ett event som har tidpunkter.";
                }
            }
            else
            {
                LblError.Text = "Du måste välj ett event att ta bort";
            }
        }

        protected void tvEvents_NodeChanged(object sender, EventArgs e)
        {
            //Response.Write("Selected value" + tvEvents.SelectedValue);
            SelectedEvent = null;
            SelectedActivity = null;
            SelectedTime = null;

            int id = 0;
            Int32.TryParse(tvEvents.SelectedValue, out id);
            if (tvEvents.SelectedNode.Depth == 0)
            {
                // Event
                multiEditFields.SetActiveView(viewEditEvent);
                ConferenceEvent ce = Conference.GetEvents().SingleOrDefault(x => x.Id == id);
                tbEventName.Text = ce.Name;
                tbEventDate.Text = ce.Date.ToString("yyyy-MM-dd");
                ddlAddNewTimeStartHour.DataBind();
                ddlAddNewTimeStartMinute.DataBind();
                ddlAddNewTimeEndHour.DataBind();
                ddlAddNewTimeEndMinute.DataBind();

                SelectedEvent = ce;
            }
            else if (tvEvents.SelectedNode.Depth == 1)
            {
                // Time
                multiEditFields.SetActiveView(viewEditTime);
                ddlTimeStartHour.DataBind();
                ddlTimeStartMinute.DataBind();
                ddlTimeEndHour.DataBind();
                ddlTimeEndMinute.DataBind();
                ddlTimeStartHour.ClearSelection();
                ddlTimeStartMinute.ClearSelection();
                ddlTimeEndHour.ClearSelection();
                ddlTimeEndMinute.ClearSelection();

                int eventId = Convert.ToInt32(tvEvents.SelectedNode.Parent.Value);
                EventTime et = Conference.GetEventTimes(eventId).SingleOrDefault(x => x.Id == id);

                String[] start = et.TimeStart.Split(new char[] { ':' });
                String[] end = et.TimeEnd.Split(new char[] { ':' });
                ddlTimeStartHour.Items.FindByValue(start[0]).Selected = true;
                ddlTimeStartMinute.Items.FindByValue(start[1]).Selected = true;
                ddlTimeEndHour.Items.FindByValue(end[0]).Selected = true;
                ddlTimeEndMinute.Items.FindByValue(end[1]).Selected = true;

                SelectedTime = et;

            }
            else if (tvEvents.SelectedNode.Depth == 2)
            {
                // Activity
                multiEditFields.SetActiveView(viewEditActivity);

                int eventId = Convert.ToInt32(tvEvents.SelectedNode.Parent.Parent.Value);
                EventTime et = Conference.GetEventTimes(eventId).SingleOrDefault(x => x.Id == Convert.ToInt32(tvEvents.SelectedNode.Parent.Value));

                EventActivity activity = Conference.GetEventActivities(et.Id).SingleOrDefault(x => x.Id == id);
                tbActivityName.Text = activity.Name;
                tbMaxParticipants.Text = activity.MaxParticipants.ToString();

                SelectedActivity = activity;
            }


        }
        #endregion

        #region Time

        protected void btnAddNewTime_Click(object sender, EventArgs e)
        {
            // validate input
            //
            String startHour = ddlAddNewTimeStartHour.SelectedValue;
            String startMinute = ddlAddNewTimeStartMinute.SelectedValue;
            String endHour = ddlAddNewTimeEndHour.SelectedValue;
            String endMinute = ddlAddNewTimeEndMinute.SelectedValue;

            if (String.IsNullOrEmpty(startHour) || String.IsNullOrEmpty(startMinute)
                || String.IsNullOrEmpty(endHour) || String.IsNullOrEmpty(endMinute))
            {
                SetErrorMessage("Ange starttid och sluttid");
                return;
            }

            if ((Convert.ToInt32(startHour) * 60 + Convert.ToInt32(startMinute)) >= (Convert.ToInt32(endHour) * 60 + Convert.ToInt32(endMinute)))
            {
                SetErrorMessage("Starttid måste vara tidigare än sluttid");
                return;
            }

            String start = startHour + ":" + startMinute;
            String end = endHour + ":" + endMinute;

            int eventId = Convert.ToInt32(tvEvents.SelectedValue);
            Conference.InsertTime(eventId, start, end);

            ddlAddNewTimeStartHour.ClearSelection();
            ddlAddNewTimeStartMinute.ClearSelection();
            ddlAddNewTimeEndHour.ClearSelection();
            ddlAddNewTimeEndMinute.ClearSelection();

            ShowEvents();
        }

        protected void btnDeleteTime_Click(object sender, EventArgs e)
        {

            if (SelectedTime != null)
            {
                try
                {
                    Conference.DeleteTime(SelectedTime.Id);
                    SelectedTime = null;
                    ShowEvents();
                    multiEditFields.ActiveViewIndex = -1;
                }
                catch (Exception ex)
                {
                    SetErrorMessage(ex.Message);
                }
            }
        }

        protected void btnUpdateTime_Click(object sender, EventArgs e)
        {
            String startHour = ddlTimeStartHour.SelectedValue;
            String startMinute = ddlTimeStartMinute.SelectedValue;
            String endHour = ddlTimeEndHour.SelectedValue;
            String endMinute = ddlTimeEndMinute.SelectedValue;

            if (String.IsNullOrEmpty(startHour) || String.IsNullOrEmpty(startMinute)
                || String.IsNullOrEmpty(endHour) || String.IsNullOrEmpty(endMinute))
            {
                SetErrorMessage("Ange starttid och sluttid");
                return;
            }

            if ((Convert.ToInt32(startHour) * 60 + Convert.ToInt32(startMinute)) >= (Convert.ToInt32(endHour) * 60 + Convert.ToInt32(endMinute)))
            {
                SetErrorMessage("Starttid måste vara tidigare än sluttid");
                return;
            }

            String start = startHour + ":" + startMinute;
            String end = endHour + ":" + endMinute;

            Conference.UpdateTime(SelectedTime.Id, start, end);
            ShowEvents();
        }
        protected void ShowTimesForEvent(object sender, EventArgs e)
        {
            ShowTimesForEvent();
        }

        private void ShowTimesForEvent()
        {
            ListTime.Items.Clear();
            ListSeminars.Items.Clear();

            foreach (EventTime eventTime in Conference.GetEventTimes(int.Parse(ListEvents.SelectedValue)))
                ListTime.Items.Add(new ListItem(eventTime.TimeStart + "-" + eventTime.TimeEnd, eventTime.Id.ToString()));
        }

        protected void BtnShowAddTimeOnClick(object sender, EventArgs e)
        {
            ShowPlaceHolders(false, !PhAddTime.Visible, false);
        }

        protected void BtnAddTimeOnClick(object sender, EventArgs e)
        {
            if (ListEvents.SelectedIndex > -1)
            {
                Conference.InsertTime(int.Parse(ListEvents.SelectedValue), StartHour.SelectedValue + ":" + StartMinute.SelectedValue, EndHour.SelectedValue + ":" + EndMinute.SelectedValue);

                ShowTimesForEvent();

                PhAddTime.Visible = false;
            }
            else
            {
                LblError.Text = "Du måste välja event för att kunna lägga till en tidpunkt.";
            }
        }

        protected void BtnDeleteTimeOnClick(object sender, EventArgs e)
        {
            if (ListTime.SelectedIndex > -1)
            {
                try
                {
                    Conference.DeleteTime(int.Parse(ListTime.SelectedValue));

                    ShowTimesForEvent();
                }
                catch
                {
                    LblError.Text = "Ett fel uppstod. Du kan inte ta bort en tidpunkt som har aktiviteter.";                    
                }
            }
            else 
            {
                LblError.Text = "Du måste välj en tid att ta bort";
            }
        }

        #endregion

        #region Activities

        protected void btnDeleteActivity_Click(object sender, EventArgs e)
        {

            if (SelectedActivity != null)
            {
                try
                {
                    Conference.DeleteActivity(SelectedActivity.Id);
                    SelectedTime = null;
                    ShowEvents();
                    multiEditFields.ActiveViewIndex = -1;
                }
                catch (Exception ex)
                {
                    SetErrorMessage(ex.Message);
                }
            }
        }


        protected void btnAddNewActivity_Click(object sender, EventArgs e)
        {
            String activityName = tbAddnewActivityName.Text.Trim();

            if (String.IsNullOrEmpty(activityName))
            {
                SetErrorMessage("Ange namn på aktivitet");
                return;
            }

            int maxParticipants = 0;
            Int32.TryParse(tbAddNewActivityMaxParticipants.Text, out maxParticipants);
            if (maxParticipants <= 0)
            {
                SetErrorMessage("Ange max antal platser");
                return;
            }

            if (SelectedTime == null)
            {
                SetErrorMessage("Ingen tid är vald");
                return;
            }

            try
            {
                Conference.InsertActivity(SelectedTime.Id, activityName, tbAddNewActivityMaxParticipants.Text.Trim());
                tbAddnewActivityName.Text = "";
                tbAddNewActivityMaxParticipants.Text = "";
                ShowEvents();
            }
            catch (Exception ex)
            {
                SetErrorMessage("Misslyckades att lägga till aktivitet: " + ex.Message);
            }

        }

        protected void btnUpdateActivity_Click(object sender, EventArgs e)
        {
            String activityName = tbActivityName.Text.Trim();

            if (String.IsNullOrEmpty(activityName))
            {
                SetErrorMessage("Ange namn på aktivitet");
                return;
            }

            int maxParticipants = 0;
            Int32.TryParse(tbMaxParticipants.Text, out maxParticipants);
            if (maxParticipants <= 0)
            {
                SetErrorMessage("Ange max antal platser");
                return;
            }

            if (SelectedActivity == null)
            {
                SetErrorMessage("Ingen aktivitet är vald");
                return;
            }

            try
            {
                Conference.UpdateActivity(SelectedActivity.Id, activityName, tbMaxParticipants.Text.Trim());
                ShowEvents();
            }
            catch (Exception ex)
            {
                SetErrorMessage("Misslyckades att lägga till aktivitet: " + ex.Message);
            }
        }



        protected void ShowActivitiesForTime(object sender, EventArgs e)
        {
            ShowActivitiesForTime();
        }

        protected void ShowActivitiesForTime()
        {
            ListSeminars.Items.Clear();

            foreach (EventActivity eventAct in Conference.GetEventActivities(int.Parse(ListTime.SelectedValue)))
                ListSeminars.Items.Add(new ListItem(eventAct.Name + " [" + eventAct.MaxParticipants + "]", eventAct.Id.ToString()));
        }

        protected void BtnShowAddActivityOnClick(object sender, EventArgs e)
        {
            ShowPlaceHolders(false, false, !PhAddActivity.Visible);
        }

        protected void BtnAddActivityOnClick(object sender, EventArgs e)
        {
            if (ListTime.SelectedIndex > -1)
            {
                Conference.InsertActivity(int.Parse(ListTime.SelectedValue), TxtActivityName.Text, TxtActivityMax.Text);

                ShowActivitiesForTime();

                TxtActivityName.Text = string.Empty;
                TxtActivityMax.Text = string.Empty;
                PhAddActivity.Visible = false;
            }
            else
            {
                LblError.Text = "Du måste välja tidpunkt för att kunna lägga till en aktvititet.";
            }
        }

        protected void BtnDeleteActivityOnClick(object sender, EventArgs e)
        {
            if (ListSeminars.SelectedIndex > -1)
            {
                try
                {
                    Conference.DeleteActivity(int.Parse(ListSeminars.SelectedValue));

                    ShowActivitiesForTime();
                }
                catch
                {
                    LblError.Text = "Ett fel uppstod. Du kan inte ta bort ett temaspår som har anmälningar.";
                }
            }
            else
            {
                LblError.Text = "Du måste välj en tid att ta bort";
            }
        }
    
        #endregion

        #region datasource methods
        
        protected String[] GetHours()
        {
            String[] hours = new String[24];
            for (int i = 0; i < hours.Length; ++i)
            {
                hours[i] = i.ToString("00");
            }
            return hours;
        }

        protected String[] GetMinutes()
        {
            String[] minutes = new String[12];
            for (int i = 0; i < minutes.Length; ++i)
            {
                minutes[i] = (i*5).ToString("00");
            }
            return minutes;
        }
        #endregion

        protected void SetErrorMessage(String msg)
        {
            
            LblError.Text = msg;
            errorDiv.Visible = true;
        }


    }
}
