using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Conference;
using EPiServer.Core;
using EPiServer;

namespace DagensIndustri.Tools.Admin.Conference
{
    
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Mail till konferensdeltagare", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Konferensmail", UrlFromUi = "/Tools/Admin/Conference/ConferenceMail.aspx", SortIndex = 2050)]
    public partial class ConferenceMail : System.Web.UI.Page
    {

        protected ConferenceObject SelectedConference { get; set; }
        
        protected String MailText { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            phSent.Visible = false;

            if (!IsPostBack)
            {
                ddlConferences.DataBind();
                phConferenceMail.DataBind();
            }
        }

        protected void ddlConferences_SelectedIndexChanged(object sender, EventArgs e)
        {
            int epiPageId = -1;
            if (Int32.TryParse(ddlConferences.SelectedValue, out epiPageId) && epiPageId > 0)
            {

                PageData confPage = null;
                try
                {
                    confPage = DataFactory.Instance.GetPage(new PageReference(epiPageId));
                }
                catch (EPiServer.Core.PageNotFoundException) { }

                if (confPage != null)
                {
                    SelectedConference = new ConferenceObject(confPage);

                    PropertyData pd = confPage.Property["MailText"];
                    if (pd != null && pd.Value != null)
                    {
                        MailText = pd.Value.ToString();
                    }
                }
            }

            phConferenceMail.DataBind();
        }

        protected void btnSendAll_Click(object sender, EventArgs e)
        {
            LoadConference();

            if (SelectedConference != null)
            {
                SelectedConference.SendMailToParticipants(SelectedConference.PageName, MailText);
            }

            phConferenceMail.DataBind();

            phMailText.Visible = false;
            phNoMailText.Visible = false;
            phSent.Visible = true;
        }

        private void LoadConference()
        {
            int epiPageId = -1;
            if (Int32.TryParse(ddlConferences.SelectedValue, out epiPageId) && epiPageId > 0)
            {

                PageData confPage = null;
                try
                {
                    confPage = DataFactory.Instance.GetPage(new PageReference(epiPageId));
                }
                catch (EPiServer.Core.PageNotFoundException) { }

                if (confPage != null)
                {
                    SelectedConference = new ConferenceObject(confPage);

                    PropertyData pd = confPage.Property["MailText"];
                    if (pd != null && pd.Value != null)
                    {
                        MailText = pd.Value.ToString();
                    }
                }
            }
        }
    }
}