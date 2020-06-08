using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;


namespace DagensIndustri.Templates.Public.Units.Placeable.CampTip
{
    public partial class PersonForm : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public string FirstName 
        {
            get { return FirstNameInput.Text; }
            set { FirstNameInput.Text = value; }
        }

        public string LastName
        {
            get { return LastNameInput.Text; }
            set { LastNameInput.Text = value; }
        }

        public string Email
        {
            get { return EmailInput.Text; }
            set { EmailInput.Text = value; }
        }

    }
}