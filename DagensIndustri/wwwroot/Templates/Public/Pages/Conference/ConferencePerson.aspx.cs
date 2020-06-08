    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHandlers;
using System.Data;
using DagensIndustri.Tools.Classes.Conference;
using System.Configuration;

namespace DagensIndustri.Templates.Public.Pages.Conference
{
    public partial class ConferencePerson : DiTemplatePage
    {
        protected int ConferenceId { get; set; }
        
        protected List<ConferenceEvent> ConferenceEvents { get; set; }
        protected ConferenceUser CurrentPerson { get; set; }
        protected List<EventActivity> CurrentPersonActivities { get; set; }
        protected ConferenceObject CurrentConference { get; set; }

        // Days before event changes are allowed
        //
        private int conferenceChoiceLockDays = 3;

        protected bool ChangeAllowed { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {

            base.UserMessageControl = this.UserMessageControl;
            UserMessageControl.ClearMessage();
            if (!IsPostBack)
            {
                DataBind();
            }

            ChangeAllowed = true;
            if (CurrentConference != null)
            {
                int tmp = 0;
                if(Int32.TryParse(ConfigurationManager.AppSettings["conferenceChoiceLockDays"], out tmp))
                {
                    conferenceChoiceLockDays = tmp;
                }
                
                ConferenceEvent firstEvent = ConferenceEvents.OrderBy(x => x.Date).FirstOrDefault();
                if (firstEvent != null)
                {
                    if (DateTime.Now.AddDays(conferenceChoiceLockDays) > firstEvent.Date)
                    {
                        UserMessageControl.ShowMessage("Eftersom evenemanget startar inom kort tillåts inga ändringar.", false, false);
                        ChangeAllowed = false;
                    }
                }
               
                ConferenceEvent lastEvent = ConferenceEvents.OrderByDescending(x => x.Date).FirstOrDefault();
                if (lastEvent != null)
                {
                    if (lastEvent.Date.AddDays(1) <= DateTime.Now.Date)
                    {
                        UserMessageControl.ShowMessage("Efterfrågat evenemeng är avslutat", false, false);
                        PlaceHolderConferenceUpdate.Visible = false;
                    }
                }
                
            }
            else
            {
                UserMessageControl.ShowMessage("Information saknas för angiven kod.", false, true);
                PlaceHolderConferenceUpdate.Visible = false;
            }

            phDisableAllInputs.DataBind();
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            GetPerson();
            if (CurrentPerson == null)
            {
                CurrentPerson = new ConferenceUser();
                UserMessageControl.ShowMessage("Felaktig länk.", false, true);
                return;
            }
            
            CurrentConference = new ConferenceObject(CurrentPerson.ConferenceId);
            ConferenceId = CurrentConference.ConferenceId;
            GetEvents();

            CurrentPersonActivities = CurrentConference.GetEventActivitiesForPerson(CurrentPerson.PersonId);
            
            ConferenceRepeater.DataBind();
        }

        protected List<ConferenceEvent> GetConferenceEvents()
        {
            return ConferenceEvents;
        }

        protected String GetEventName(int id)
        {
            ConferenceEvent ce = ConferenceEvents.SingleOrDefault(x => x.Id == id);
            if (ce == null)
                return "";
            else
                return ce.Name;
        }

        protected String GetEventDate(int id)
        {
            ConferenceEvent ce = ConferenceEvents.SingleOrDefault(x => x.Id == id);
            if (ce == null)
                return "";
            else
                return ce.Date.ToString("yyyy-MM-dd");
        }

        protected List<EventTime> GetEventTimes(int eventId)
        {
            return CurrentConference.GetEventTimes(eventId);
            //return ConferenceEvents.Where;
        }

        protected List<EventActivity> GetEventActivities(int timeId)
        {
            return CurrentConference.GetEventActivities(timeId);
        }

        protected void GetPerson()
        {
            if (Request.QueryString["code"] != null)
            {
                try
                {
                    Guid guid = new Guid(Request.QueryString["code"]);
                    CurrentPerson = new ConferenceUser(guid);
                    if (CurrentPerson.PersonId <= 0)
                        CurrentPerson = null;
                }
                catch(Exception e)
                {
                    
                }
            }
        }

        protected String GetActivityCheckedString(int activityId)
        {
            if (CurrentPersonActivities.SingleOrDefault(x => x.Id == activityId) != null)
                return "checked=\"checked\"";
            else
                return "";
        }

        protected String GetActivityDisabledString(bool isFull)
        {
            if (isFull)
                return "disabled=\"disabled\"";
            else
                return "";
        }

        protected void GetEvents()
        {
            DataSet dsConf = MsSqlHandler.GetEvents(ConferenceId);

            ConferenceEvents = new List<ConferenceEvent>();
            foreach (DataRow dr in dsConf.Tables[0].Rows)
            {
                ConferenceEvents.Add(
                    new ConferenceEvent((int)dr["eventid"],
                        dr["name"] as string,
                        (int)dr["price"],
                        (DateTime)dr["eventdate"])
                    );
            }
        }

        #region events
        protected void ConferenceRepeater_ItemDataBound(object sender, EventArgs e)
        {

        }
        
        protected void EventTimesRepeater_ItemDatabound(object sender, EventArgs e)
        {
        }


        protected void PersonUpdateButton_Click(object sender, EventArgs e)
        {
            if (ChangeAllowed)
            {
                GetPerson();

                // Get user info fields
                //
                CurrentPerson.FirstName = FirstNameInput.Text;
                CurrentPerson.LastName = LastNameInput.Text;
                CurrentPerson.Company = CompanyInput.Text;
                CurrentPerson.Title = TitleInput.Text;
                CurrentPerson.OrgNo = OrgNumberInput.Text;
                CurrentPerson.Phone = TelephoneInput.Text;
                CurrentPerson.Email = EmailInput.Text;
                CurrentPerson.InvoiceAddress = InvoiceAddressInput.Text;
                CurrentPerson.InvoiceReference = InvoiceReferenceInput.Text;
                CurrentPerson.Zip = ZipCodeInput.Text;
                CurrentPerson.City = StateInput.Text;

                // Get selected activities
                //
                CurrentConference = new ConferenceObject(CurrentPerson.ConferenceId);
                ConferenceId = CurrentConference.ConferenceId;
                GetEvents();

                List<int> selectedActivities = new List<int>();
                foreach (ConferenceEvent ce in ConferenceEvents)
                {
                    List<EventTime> eventTimes = GetEventTimes(ce.Id);
                    foreach (EventTime et in eventTimes)
                    {
                        String val = Request.Form["time_" + et.Id];
                        if (!String.IsNullOrEmpty(val))
                        {
                            selectedActivities.Add(Convert.ToInt32(val));
                        }
                    }
                }

                CurrentPerson.SaveUpdate();
                CurrentPerson.UpdatePersonInActivity(selectedActivities);
                CurrentPersonActivities = CurrentConference.GetEventActivitiesForPerson(CurrentPerson.PersonId);
                //ConferenceRepeater.DataBind();
                UserMessageControl.ShowMessage("Dina uppgifter har sparats", false, false);
                PlaceHolderConferenceUpdate.Visible = true;
            }
            DataBind();
        }
        #endregion
    }
}