using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Conference;

namespace DagensIndustri.Tools.Admin.Conference
{
    public partial class Activities : System.Web.UI.UserControl
    {
        #region Properties

        public ConferenceObject Conference
        {
            get
            {
                return ((ConferenceAdmin)this.Page).Conference;
            }
        }

        public string PersonId
        {
            get
            {
                return (string)ViewState["PersonId"];
            }
            set
            {
                ViewState["PersonId"] = value;
            }
        }

        public List<string> ActivityIds { get; set; }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ActivityIds = new List<string>();

            foreach (EventActivity eventAct in Conference.GetEventActivitiesForPerson(int.Parse(PersonId)))
                ActivityIds.Add(eventAct.Id.ToString());

            //Bind repeater to all activities for this conference
            repAct.DataSource = Conference.GetAllActivitiesDataSet();
            repAct.DataBind();
        }

        protected void show(object sender, EventArgs e)
        {
            ImageButton imgBtn = (ImageButton)sender;
            string activityId = imgBtn.CommandArgument;

            //Show add or delete button
            if (imgBtn.ID.Equals("ImgAdd"))
                imgBtn.Visible = !ActivityIds.Contains(activityId); //Show delete button if activity is NOT in list
            else
            {
                imgBtn.Visible = ActivityIds.Contains(activityId);  //Show add button if activity is in list
                imgBtn.Enabled = !ActivityIds.Count.Equals(1);     //Disable delete button if only one activity
            }
        }

        protected void DeleteActivity(object sender, EventArgs e)
        {
            ImageButton imgBtn = (ImageButton)sender;
            string activityId = imgBtn.CommandArgument;

            Conference.DeletePersonInActivity(int.Parse(PersonId), int.Parse(activityId));

        }

        protected void AddActivity(object sender, EventArgs e)
        {
            ImageButton imgBtn = (ImageButton)sender;
            string activityId = imgBtn.CommandArgument;

            Conference.InsertPersonInActivity(int.Parse(PersonId), int.Parse(activityId));
        }
    }
}