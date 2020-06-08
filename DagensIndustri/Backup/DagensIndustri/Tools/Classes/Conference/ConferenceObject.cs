using System;
using System.Data;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using System.Collections.Generic;
using EPiServer.Core;
using System.Text;
using System.Configuration;
using System.Web;
using DIClassLib.Misc;
using EPiServer.Web.Hosting;
using System.Net.Mail;


namespace DagensIndustri.Tools.Classes.Conference
{
    [Serializable]
    public class ConferenceObject
    {
        public int ConferenceId { get; set; }
        public string PageName { get; set; }
        public int Price { get; set; }
            
        public ConferenceObject(PageData currentPage)
        {
            PageName = currentPage.PageName;
            SetUpConference(currentPage.PageLink.ID);
        }

        /// <summary>
        /// This construct is used in ConferenceAdmin
        /// </summary>
        /// <param name="conferenceId"></param>
        public ConferenceObject(int conferenceId)
        {
            ConferenceId = conferenceId;
        }

        private void SetUpConference(int epiPageId)
        {
            DataSet dsConference = MsSqlHandler.GetConference(epiPageId);

            if (dsConference.Tables[0].Rows.Count > 0)
            {
                ConferenceId = int.Parse(dsConference.Tables[0].Rows[0]["conferenceid"].ToString());
                Price = int.Parse(dsConference.Tables[0].Rows[0]["price"].ToString());
            }

            if (ConferenceId.Equals(0))
            {
                ConferenceId = CreateConference(epiPageId);
                Price = 0;
            }
        }

        private int CreateConference(int epiPageId)
        {
            return MsSqlHandler.InsertConference(epiPageId, PageName, Price);
        }

        public void UpdateConferencePrice(int price)
        {
            MsSqlHandler.UpdateConferencePrice(ConferenceId, price);
        }

        public DataSet GetPdfDownloadLog()
        {
            return MsSqlHandler.GetPdfDownloadLog(ConferenceId);
        }



        #region Events

        public List<ConferenceEvent> GetEvents()
        {
            List<ConferenceEvent> events = new List<ConferenceEvent>();

            foreach (DataRow dr in MsSqlHandler.GetEvents(ConferenceId).Tables[0].Rows)
            {
                events.Add(
                    new ConferenceEvent((int)dr["eventid"],
                        dr["name"] as string,
                        (int)dr["price"],
                        (DateTime)dr["eventdate"])
                    );
            }

            return events;
        }

        public void InsertEvent(string name, string price, DateTime date)
        {
            MsSqlHandler.InsertEvent(ConferenceId, name, price, date);
        }

        public void UpdateEvent(int eventId, string name, DateTime date)
        {
            MsSqlHandler.UpdateEvent(eventId, name, date);
        }

        public void DeleteEvent(int dayId)
        {
            List<EventTime> childTimes = GetEventTimes(dayId);
            if (childTimes.Count > 0)
                throw new Exception("Eventet har tidpunkter och kan inte tas bort.");
            MsSqlHandler.DeleteEvent(dayId);
        }

        #endregion

        
        #region EventTimes

        public List<EventTime> GetEventTimes(int eventId)
        {
            List<EventTime> eventTimes = new List<EventTime>();

            foreach (DataRow dr in MsSqlHandler.GetEventTimes(eventId).Tables[0].Rows)
            {
                eventTimes.Add(
                    new EventTime((int)dr["timeid"],
                        (string)dr["timeStart"],
                        (string)dr["timeEnd"])
                    );
            }

            return eventTimes;
        }

        public void InsertTime(int dayId, string timeStart, string timeEnd)
        {
            MsSqlHandler.InsertTime(dayId, timeStart, timeEnd);
        }

        public void UpdateTime(int timeId, string timeStart, string timeEnd)
        {
            MsSqlHandler.UpdateTime(timeId, timeStart, timeEnd);
        }

        public void DeleteTime(int timeId)
        {
            // check for child activities
            //
            List<EventActivity> activities = GetEventActivities(timeId);
            if (activities.Count > 0)
            {
                throw new Exception("Tidpunkten har aktiviteter och kan inte tas bort.");
            }
            MsSqlHandler.DeleteTime(timeId);
        }

        #endregion


        #region EventActivities

        public List<EventActivity> GetEventActivities(int timeId)
        {
            List<EventActivity> eventActivities = new List<EventActivity>();

            foreach (DataRow dr in MsSqlHandler.GetEventActivities(timeId).Tables[0].Rows)
            {
                eventActivities.Add(
                    new EventActivity((int)dr["activityid"],
                        (string)dr["name"],
                        (int)dr["maxparticipants"],
                        (int)dr["nrOfParticipants"])
                    );
            }

            return eventActivities;
        }

        public List<EventActivity> GetEventActivitiesWithCount()
        {
            List<EventActivity> eventActivities = new List<EventActivity>();

            foreach (DataRow dr in MsSqlHandler.GetEventActivities(ConferenceId).Tables[0].Rows)
            {
                eventActivities.Add(
                    new EventActivity((int)dr["activityid"],
                        (string)dr["name"],
                        (int)dr["maxparticipants"],
                        (int)dr["nrOfParticipants"])
                    );
            }

            return eventActivities;
        }

        

        public List<EventActivity> GetEventActivitiesForPerson(int personId)
        {
            List<EventActivity> eventActivities = new List<EventActivity>();

            foreach (DataRow dr in MsSqlHandler.GetEventActivitiesForPerson(personId.ToString()).Tables[0].Rows)
            {
                EventActivity eventAct = new EventActivity((int)dr["activityid"],
                        (string)dr["ActivityName"],
                        0, 0);

                //Add events name to activity
                eventAct.Info = dr["name"].ToString();
                eventActivities.Add(eventAct);
            }

            return eventActivities;
        }

        public void InsertActivity(int timeId, string name, string maxparticipants)
        {
            MsSqlHandler.InsertActivity(timeId, name, maxparticipants);
        }

        public void UpdateActivity(int activityId, string name, string maxparticipants)
        {
            MsSqlHandler.UpdateActivity(activityId, name, maxparticipants);
        }

        public void DeleteActivity(int seminarId)
        {
            MsSqlHandler.DeleteActivity(seminarId);
        }

        public void DeletePersonInActivity(int personId, int activityId)
        {
            MsSqlHandler.DeletePersonInActivity(personId, activityId);
        }

        public void InsertPersonInActivity(int personId, int activityId)
        {
            MsSqlHandler.InsertPersonInActivity(personId, activityId);
        }

        public DataSet GetAllActivitiesDataSet()
        {
            return MsSqlHandler.GetActivitiesForConference(ConferenceId);
        }

        #endregion


        public void SendMailToParticipants(String subject, String mailBody)
        {
            String editUrl = EPiFunctions.SettingsPageSetting(null, "ConferencePersonUpdatePage") as String;

            if (String.IsNullOrEmpty(editUrl))
            {
                throw new Exception("Settings: 'ConferencePersonUpdatePage' is missing.");
            }

            List<ConferenceUser> persons = new List<ConferenceUser>();
            DataSet ds = MsSqlHandler.GetPersonsForConference(ConferenceId,0,0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ConferenceUser person = new ConferenceUser()
                {
                    PersonId = (int)dr["personid"],
                    Email = (String)dr["email"],
                    PersonGuid = (Guid)dr["personguid"]
                };
                persons.Add(person);
            }

            foreach(ConferenceUser cu in persons) 
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<html><body>");
                sb.Append(mailBody);
                sb.Append(String.Format("<a href='{0}'>Ändra dina uppgifter</a>", "http://" + HttpContext.Current.Request.Url.Host + editUrl + "?code=" + cu.PersonGuid));
                sb.Append("<br><br>Vänliga hälsningar,<br>Di Konferens");
                sb.Append("</body></html>");

                MiscFunctions.SendMail("no-reply@di.se", cu.Email, subject, sb.ToString(), true);
                MsSqlHandler.InsertPersonInConferenceMail(cu.PersonId, ConferenceId);
                
            }

            
            
        }

        public void SendPdfMail(String name, String email, String phone, String pdfLink, String subject, String body, String mailFrom, String pdfName)
        {
            
            //Create message
            MailMessage message = new MailMessage(mailFrom, email)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            };


            UnifiedFile uf = System.Web.Hosting.HostingEnvironment.VirtualPathProvider.GetFile(pdfLink) as UnifiedFile;
            Attachment attachment = new Attachment(uf.LocalPath);
            attachment.Name = pdfName + uf.Extension;
            message.Attachments.Add(attachment);

            //Send mail
            MiscFunctions.SendMail(message);


            //Log to db
            MsSqlHandler.InsertPdfDownloadLog(ConferenceId , name, email, phone);
            
        }
    }

    [Serializable]
    public class ConferenceEvent
    {
        public ConferenceEvent(int id, string name, int price, DateTime date)
        {
            Id = id;
            Name = name;
            Price = price;
            Date = date;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public DateTime Date { get; set; }
    }

    [Serializable]
    public class EventTime
    {
        public EventTime(int id, string timeStart, string timeEnd)
        {
            Id = id;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
        }

        public int Id { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }
    }

    [Serializable]
    public class EventActivity
    {
        public EventActivity(int id, string name, int maxParticipants, int nrOfParticipants)
        {
            Id = id;
            Name = name;
            MaxParticipants = maxParticipants;
            NrOfParticipants = nrOfParticipants;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int MaxParticipants { get; set; }

        public int NrOfParticipants { get; set; }

        /// <summary>
        /// Property for extra info, not obtained from db
        /// </summary>
        public string Info { get; set; }

        public bool IsFull
        {
            get
            {
                return (MaxParticipants <= NrOfParticipants);
            }
        }
    }
}