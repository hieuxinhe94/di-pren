using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using EPiServer.Core;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow;


namespace DagensIndustri.Tools.Classes.SignUp
{
    [Serializable]
    public class SignUpUser
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
        public string EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        #endregion


        public SignUpUser(RegisterParticipants participantsForm)
        {
            EventId = participantsForm.EventId;
            FirstName = participantsForm.FirstName;
            LastName = participantsForm.LastName;
            Address = participantsForm.Street;
            Zip = participantsForm.Zip;
            City = participantsForm.City;
            Phone = participantsForm.Phone;
            Email = participantsForm.Email;
        }
    }

}