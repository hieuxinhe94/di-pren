using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using DIClassLib.BonnierDigital;
using DIClassLib.Campaign;
using DIClassLib.DbHandlers;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.AddCustAndSub;

using EPiServer.PlugIn;

namespace DagensIndustri.Tools.Telemarketing
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Add free digital subscriptions", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Telemarketing - Add subscriptions", UrlFromUi = "/Tools/Telemarketing/AddSubscriptions.aspx", SortIndex = 1030)]
    public partial class AddSubscriptions : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Page.IsPostBack)
            {
                SaveSubscription();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            rptCampaigns.DataSource = GetCampaigns();
            rptCampaigns.DataBind();
        }

        private void SaveSubscription()
        {
            //TODO: If customer exist, how do I just add a subscription to the customer?
            litServerMessage.Text = string.Empty;
            var subscriptionStartDate = SubscriptionController.GetNextIssueDateIncDiRules(DateTime.Now, txtPaperCode.Value, txtProductNo.Value);
            var addCustAndSubHandler = new AddCustAndSubHandler();
            var subscriber = new Subscriber(txtFirstName.Text, txtLastName.Text, string.Empty, txtEmail.Text,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty);

            //Will create new customer and add subs or add subs to existing customer in Cirix
            var existingCusno = string.IsNullOrEmpty(txtCusno.Value) ? 0 : long.Parse(txtCusno.Value);
            var addCustSubRet = addCustAndSubHandler.AddFreeTrySub(txtCampId.Value, txtTargetGroup.Value, subscriptionStartDate, txtServicePlusId.Value, subscriber, existingCusno);
            if (addCustSubRet.HasMessages)
            {
                foreach (var mess in addCustSubRet.Messages)
                {
                    litServerMessage.Text += string.Format("{0}<br />{1}", mess.MessageCustomer, mess.MessageSweStaff);
                }
                return;
            }

            //Success to save subscription
            litServerMessage.Text = string.Format("Prenumeration skapad med kundnummer {0} och prenumerationsnummer {1}.", addCustSubRet.CirixSubscriberCusno, addCustSubRet.CirixSubsno);

            //TODO: Only create S+ account if user doesn't have one already?
            var addSubToExistingDiAccount = !string.IsNullOrEmpty(txtServicePlusId.Value);
            var password = Guid.NewGuid().ToString().ToLower().Substring(0, 7);
            var campNo = SubscriptionController.GetCampno(txtCampId.Value);
            var responseCode = BonDigHandler.TryAddCustAndSubToBonDig(txtPaperCode.Value, txtProductNo.Value, addCustSubRet.CirixSubscriberCusno, addCustSubRet.CirixSubsno,
                subscriber.Email, subscriber.FirstName, subscriber.LastName, string.Empty, password, addSubToExistingDiAccount, SubscriptionController.IsHybridCampaign(campNo));
            litServerMessage.Text += "<br />";
            switch (responseCode)
            {
                case 1:
                case 2:
                    litServerMessage.Text += "Service Plus uppdaterat.";
                    break;
                case -1:
                    litServerMessage.Text += "Tekniskt fel: efterfrågad produkt hittades inte.";
                    break;
                case -2:
                    litServerMessage.Text += "Angivet användarnamn hittades tyvärr inte.";
                    break;
                case -3:
                    litServerMessage.Text += "Angivet användarnamn är tyvärr upptaget.";
                    break;
                case -4:
                    litServerMessage.Text += "Tekniskt fel: kunduppgifterna kunde inte sparas";
                    break;
                case -5:
                    litServerMessage.Text += "Tekniskt fel: giltigt användar-ID saknas";
                    break;
                case -6:
                    litServerMessage.Text += "Tekniskt fel: Prenumerationen kunde inte sparas på kundbilden";
                    break;
                default:
                    litServerMessage.Text += "Ett oväntat fel. Kontrollera om kontot skapades i Service Plus.";
                    break;
            }

            //TODO: As customer do not know the password, should we send email about this, or is it enough with welcome email and S+ email?
            if (!addSubToExistingDiAccount)
            {
                litServerMessage.Text += string.Format("<br /><b>Användarens lösenord är: <u>{0}</u></b>", password);
            }
        }

        private IEnumerable<CampaignInfo> GetCampaigns()
        {
            var productList = TelemarketingHelper.GetProducts();
            var activeCampaigns = new List<CampaignInfo>();
            foreach (var product in productList)
            {
                activeCampaigns.AddRange(SubscriptionController.GetActiveFreeCampaigns(product.PaperCode, product.ProductNo));
            }
            return activeCampaigns;
        }
    }
}
