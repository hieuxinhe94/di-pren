using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.Gasell;
using DagensIndustri.Tools.Classes.Campaign;

namespace DagensIndustri.Templates.Public.Units.Placeable.GasellFlow
{
    public partial class Receipt : EPiServer.UserControlBase
    {
        protected PageReference CampaignPage { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Request.QueryString["responseCode"] == null)
            {
                int gasellID = Convert.ToInt32(Request.QueryString["gasellid"]);
                if (gasellID <= 0)
                    return;

                PageData pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(gasellID));

                if (EPiFunctions.HasValue(pd, "GasellConfirmationInfo"))
                {
                    GasellInfoLiteral.Text = pd["GasellConfirmationInfo"].ToString();
                }
                else if (EPiFunctions.HasValue(pd, "GasellCity"))
                {
                    GasellInfoLiteral.Text = Translate("/gasell/flow/text/welcomeconfirmation") + " " + pd["GasellCity"].ToString();
                }

                if (EPiFunctions.HasValue(pd, "Date"))
                {
                    GasellTimeLiteral.Text = string.Format("{0}, kl.{1}", EPiFunctions.GetDate(pd, "Date"), (Convert.ToDateTime(pd["Date"])).ToString("HH:mm"));
                }

                RegisteredParticipantsLiteral.Text = Translate("/gasell/flow/text/registeredparticipants");


                String campImage = EPiFunctions.SettingsPageSetting(CurrentPage, "CampaignBannerImage") as String;
                CampaignBannerButton.ImageUrl = campImage;
                CampaignPage = EPiFunctions.SettingsPageSetting(CurrentPage, "CampaignPage") as PageReference;
                if (campImage != null && CampaignPage != null)
                {
                    CampaignBannerButton.Visible = true;
                }
                else
                {
                    CampaignBannerButton.Visible = false;
                }
            }
        }

        public void PopulateParticipantsRepeater(List<GasellUser> GasellUsers)
        {
            ParticipantsRepeater.DataSource = GasellUsers;
            ParticipantsRepeater.DataBind();
        }

        protected void CampaignBannerButton_Click(object sender, EventArgs e)
        {
            GasellOrder gasellOrder = Session["GasellOrderObject"] as GasellOrder;
            if (gasellOrder != null && gasellOrder.GasellUsersList != null && gasellOrder.GasellUsersList.Count() > 0)
            {
                GasellUser user = gasellOrder.GasellUsersList.First();
                UserFields fields = new UserFields()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    StreetAddress = user.Address,
                    City = user.City,
                    Company = user.Company,
                    Zip = user.Zip,
                    Email = user.Email,
                    Telephone = user.Phone                    
                };
                UserFields.StoreInSession(fields);    
            }
            // store gasell user in temporary session object
            //


            // Redirect to Campaign Page
            //
            PageData pd = EPiServer.DataFactory.Instance.GetPage(CampaignPage);
            Response.Redirect(pd.LinkURL);
            
        }

    }
}