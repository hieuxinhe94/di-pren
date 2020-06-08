using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.SignUp;

namespace DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow
{
    public partial class Receipt : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Request.QueryString["responseCode"] == null && !string.IsNullOrEmpty(Request.QueryString["eventId"]))
            {
                PageData pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(Convert.ToInt32(Request.QueryString["eventId"])));

                if (EPiFunctions.HasValue(pd, "SignUpConfirmationInfo"))
                {
                    LiteralInfo.Text = pd["SignUpConfirmationInfo"].ToString();
                }
                else if (EPiFunctions.HasValue(pd, "SignUpCity"))
                {
                    LiteralInfo.Text = Translate("/signup/flow/text/welcomeconfirmation") + " " + pd["SignUpCity"].ToString();
                }

                if (EPiFunctions.HasValue(pd, "Date"))
                {
                    LiteralTime.Text = string.Format("{0}, kl.{1}", EPiFunctions.GetDate(pd, "Date"), (Convert.ToDateTime(pd["Date"])).ToString("HH:mm"));
                }

                RegisteredParticipantsLiteral.Text = Translate("/signup/flow/text/registeredparticipants");
            }
        }

        public void PopulateParticipantsRepeater(List<SignUpUser> SignedUpUsers)
        {
            ParticipantsRepeater.DataSource = SignedUpUsers;
            ParticipantsRepeater.DataBind();
        }



    }
}