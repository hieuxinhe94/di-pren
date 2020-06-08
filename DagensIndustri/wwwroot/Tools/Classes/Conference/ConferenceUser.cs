using System;
using System.Web.UI.WebControls;
using DagensIndustri.Templates.Public.Units.Placeable.Conference;
using System.Collections.Generic;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using EPiServer.Core;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes;
using System.Data;
using System.Linq;
using System.Text;


namespace DagensIndustri.Tools.Classes.Conference
{
    [Serializable]
    public class ConferenceUser
    {
        #region Properties

        private PageData CurrentPage
        {
            get
            {
                var page = System.Web.HttpContext.Current.Handler as EPiServer.PageBase;

                if (page == null)
                {
                    return null;
                }

                return page.CurrentPage;
            }
        }

        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public string Title { get; set; }

        public string OrgNo { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string InvoiceAddress { get; set; }

        public string InvoiceReference { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string Code { get; set; }

        public string InformationChannel { get; set; }

        public string ConferenceName { get; set; }

        public List<ListItem> SelectedActivities { get; set; }

        public int ConferenceId { get; set; }

        public Guid PersonGuid { get; set; }

        // Currently not available to change from public page?
        private String Price { get; set; }

        //public int AmountToPay { get; set; }

        #endregion

        public ConferenceUser(Guid guid)
        {
            DataSet dsPerson = MsSqlHandler.GetPersonByGuid(guid);
            if (dsPerson.Tables[0].Rows.Count > 0)
            {
                // p.personid, p.firstname, p.lastname, p.company, p.title, p.orgno, p.phone, p.email, 
                // p.invoiceaddress, p.invoicereference, p.zip, p.city, p.code, p.price, p.created,p.conferenceid,p.personguid
                PersonId = (int)dsPerson.Tables[0].Rows[0]["personid"];
                    PersonGuid = (Guid)dsPerson.Tables[0].Rows[0]["personguid"];
                    FirstName = dsPerson.Tables[0].Rows[0]["firstname"].ToString();
                    LastName = dsPerson.Tables[0].Rows[0]["lastname"].ToString();
                    Company = dsPerson.Tables[0].Rows[0]["company"].ToString();
                    Title = dsPerson.Tables[0].Rows[0]["title"].ToString();
                    OrgNo = dsPerson.Tables[0].Rows[0]["orgno"].ToString();
                    Phone = dsPerson.Tables[0].Rows[0]["phone"].ToString();
                    Email = dsPerson.Tables[0].Rows[0]["email"].ToString();
                    InvoiceAddress = dsPerson.Tables[0].Rows[0]["invoiceaddress"].ToString();
                    InvoiceReference = dsPerson.Tables[0].Rows[0]["invoicereference"].ToString();
                    Zip = dsPerson.Tables[0].Rows[0]["zip"].ToString();
                    City = dsPerson.Tables[0].Rows[0]["city"].ToString();
                    Code = dsPerson.Tables[0].Rows[0]["code"].ToString();
                    Price = dsPerson.Tables[0].Rows[0]["price"].ToString();


                int confId = 0;
                if(Int32.TryParse(dsPerson.Tables[0].Rows[0]["conferenceid"].ToString(), out confId))
                {
                    ConferenceId = confId;
                }

               
            }
        }

        public ConferenceUser(ConferenceRegistration confForm)
        {
            FirstName = confForm.FirstName;
            LastName = confForm.LastName;
            Company = confForm.Company;
            Title = confForm.Title;
            OrgNo = confForm.OrgNo;
            Phone = confForm.Phone;
            Email = confForm.Email;
            InvoiceAddress = confForm.InoviceAddress;
            InvoiceReference = confForm.InvoiceReference;
            Zip = confForm.Zip;
            City = confForm.City;
            Code = confForm.Code;
            InformationChannel = confForm.InformationChannel;
            ConferenceName = confForm.ConferenceName;
            SelectedActivities = confForm.SelectedActivities;
            ConferenceId = confForm.conference.ConferenceId;
            PersonGuid = Guid.NewGuid();
            //AmountToPay = confForm.AmountToPay;
        }

        public ConferenceUser()
        {
        }

        /// <summary>
        /// Insert new person
        /// </summary>
        public void Save()
        {
            int personId = InsertPerson();

            foreach (ListItem selectedItem in SelectedActivities)
            {
                InsertPersonInActivity(personId.ToString(), selectedItem.Value);
            }

            //InsertToPayTrans(personId, "faktura");

            SendWelcomeMail();
        }

        public void SendWelcomeMail()
        {
            try
            {
                string mailFrom = EPiFunctions.SettingsPageSetting(CurrentPage, "ConferenceMailFrom").ToString();
                if (string.IsNullOrEmpty(mailFrom))
                    mailFrom = "no-reply@di.se";

                string subject = ConferenceName; //string.Format(EPiServer.Core.LanguageManager.Instance.Translate("/conference/mail/confirm/subject"), ConferenceName);

                StringBuilder body = new StringBuilder();

                string WelcMailText = (EPiFunctions.HasValue(CurrentPage, "WelcomeMailText")) ? CurrentPage["WelcomeMailText"].ToString() : "";
                if (string.IsNullOrEmpty(WelcMailText))
                    body.Append(string.Format(LanguageManager.Instance.Translate("/conference/mail/welcome/body"), GetDefaultIntro()).Replace("[nl]", "<br>"));
                else
                    body.Append(WelcMailText);

                body.Append(GetCustChoicesAsHtml());
                body.Append(LanguageManager.Instance.Translate("/conference/mail/footer"));

                MiscFunctions.SendMail(mailFrom, Email, subject, body.ToString(), true);
            }
            catch (Exception ex)
            {
                new Logger("SendWelcomeMail() - failed", "Details: " + this.ToString() + " Exception: " + ex.ToString());
                throw;
            }
        }

        //Di Fin Tech 2012-10-15 08:00 på Grand Hotel, Stockholm
        private string GetDefaultIntro()
        {
            StringBuilder sb = new StringBuilder();
           
            //2012-10-17 08:30:00
            string startDate = (EPiFunctions.HasValue(CurrentPage, "Date")) ? CurrentPage["Date"].ToString() : "";
            if (startDate.Length > 16)
                startDate = startDate.Substring(0, 16);

            string place = (EPiFunctions.HasValue(CurrentPage, "Place")) ? CurrentPage["Place"].ToString() : "";

            sb.Append(ConferenceName);
            if (!string.IsNullOrEmpty(startDate)) sb.Append(" " + startDate);
            if (!string.IsNullOrEmpty(place))     sb.Append(" på " + place);

            return sb.ToString();
        }

        public string GetCustChoicesAsHtml()
        {
            if (SelectedActivities == null || SelectedActivities.Count() == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            List<int> selActIds = SelectedActivities.Select(x => Convert.ToInt32(x.Value)).ToList();
            ConferenceObject conf = new ConferenceObject(ConferenceId);
            List<ConferenceEvent> confEvents = conf.GetEvents();
            
            foreach (ConferenceEvent confEv in confEvents)
            {
                string custActs = GetCustsActivitiesAsHtml(conf, selActIds, confEv);
                
                if (!string.IsNullOrEmpty(custActs))
                {
                    sb.Append(confEv.Name);
                    sb.Append(custActs);
                    sb.Append("<br>");
                    sb.Append("----------");
                    sb.Append("<br>");
                }
            }

            if (sb.ToString().Length > 0)
                sb.Insert(0, "<b>Dina val av temaspår</b><br>----------<br>");

            return sb.ToString();
        }

        private string GetCustsActivitiesAsHtml(ConferenceObject conf, List<int> selActIds, ConferenceEvent confEv)
        {
            StringBuilder sb = new StringBuilder();
            List<EventTime> eventTimes = conf.GetEventTimes(confEv.Id);
            foreach (EventTime et in eventTimes)
            {
                List<EventActivity> activities = conf.GetEventActivities(et.Id);
                EventActivity activity = activities.SingleOrDefault(x => selActIds.Contains(x.Id));
                if (activity != null)
                {
                    sb.Append("<br>");
                    sb.Append(et.TimeStart);
                    sb.Append("-");
                    sb.Append(et.TimeEnd);
                    sb.Append(" ");
                    sb.Append(activity.Name);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Save updated fields to existing user
        /// </summary>
        public void SaveUpdate()
        {
            MsSqlHandler.UpdatePerson(PersonId, FirstName, LastName, Company, Title, OrgNo, Phone, Email, InvoiceAddress, InvoiceReference, Zip, City, Code, Price);
        }

        public void UpdatePersonInActivity(List<int> activityIds)
        {
            // Get current activities
            //
            List<int> currentIds = new List<int>();
            foreach (DataRow dr in MsSqlHandler.GetEventActivitiesForPerson(PersonId.ToString()).Tables[0].Rows)
            {
                currentIds.Add((int)dr["activityid"]);
            }

            // diff with current
            //
            //List<int> delIds = ; // delete these
            //List<int> insertIds =; // insert these

            foreach(int activityId in currentIds.Except(activityIds))
            {
                MsSqlHandler.DeletePersonInActivity(PersonId, activityId);
            }

            foreach (int activityId in activityIds.Except(currentIds))
            {
                MsSqlHandler.InsertPersonInActivity(PersonId, activityId);
            }


        }

        private int InsertPerson()
        {
            try
            {
                return MsSqlHandler.InsertPerson(FirstName, LastName, Company, Title, OrgNo, Phone, Email, InvoiceAddress, InvoiceReference, Zip, City, Code, InformationChannel,ConferenceId,PersonGuid);
            }
            catch (Exception ex)
            {
                new Logger("InsertPerson() - failed", "Details: " + this.ToString() + " Exception: " + ex.ToString());
                throw ex;
            }
        }

        private void InsertPersonInActivity(string personId, string activityId)
        {
            try
            {
                MsSqlHandler.InsertPersonInActivity(int.Parse(personId), int.Parse(activityId));
            }
            catch (Exception ex)
            {
                new Logger("InsertPersonInActivity() - failed", "Details: " + this.ToString() + " Exception: " + ex.ToString());
                throw ex;
            }
        }

        //private void InsertToPayTrans(int personid, string paymentmethod)
        //{
        //    try
        //    {
        //        MsSqlHandler.InsertToPayTrans(personid, paymentmethod, AmountToPay);
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("InsertToPayTrans() - failed", "Details: " + this.ToString() + " Exception: " + ex.ToString());
        //        throw ex;
        //    }
        //}

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<b>Anmälan</b><br>");

            sb.Append("FirstName: " + FirstName + "<br>");
            sb.Append("LastName: " + LastName + "<br>");
            sb.Append("Company: " + Company + "<br>");
            sb.Append("OrgNo: " + OrgNo + "<br>");
            sb.Append("Title: " + Title + "<br>");
            sb.Append("Phone: " + Phone + "<br>");
            sb.Append("Email: " + Email + "<br>");
            sb.Append("InvoiceAddress: " + InvoiceAddress + "<br>");
            sb.Append("InvoiceReference: " + InvoiceReference + "<br>");
            sb.Append("Zip: " + Zip + "<br>");
            sb.Append("City: " + City + "<br>");
            sb.Append("Code: " + Code + "<br>");

            if (SelectedActivities != null)
            {
                sb.Append("<hr /><b>Selectedactivities</b><hr />");
                foreach (ListItem selectedItem in SelectedActivities)
                    sb.Append(selectedItem.Text + " [" + selectedItem.Value + "]<br />");
            }

            return sb.ToString();
        }
    }
}