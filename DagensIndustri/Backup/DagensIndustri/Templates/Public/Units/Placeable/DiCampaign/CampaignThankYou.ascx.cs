using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;

namespace DagensIndustri.Templates.Public.Units.Placeable.DiCampaign
{
    public partial class CampaignThankYou : EPiServer.UserControlBase
    {

        protected List<DIClassLib.Subscriptions.Subscription> NewSubscriptions
        {
            get
            {
                List<DIClassLib.Subscriptions.Subscription> subs = new List<DIClassLib.Subscriptions.Subscription>();
                
                if (CampPage.Sub != null)
                    subs.Add(CampPage.Sub);

                if (CampPage.ThankYouSub != null)
                    subs.Add(CampPage.ThankYouSub);

                return subs;
            }
        }

        public Pages.DiCampaign.Campaign CampPage
        {
            get { return (Pages.DiCampaign.Campaign)this.Page; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (CurrentPage["CampaignIsAutogiro"] != null && ((bool)CurrentPage["CampaignIsAutogiro"]) == true)
                ShowAutogiroBox();

            if (CurrentPage["ShowThankYouCampaign1"] != null)
                PlaceHolderCampaign1.Visible = (bool)CurrentPage["ShowThankYouCampaign1"];
            else
                PlaceHolderCampaign1.Visible = false;

            if(CurrentPage["ShowThankYouCampaign2"] != null)
                PlaceHolderCampaign2.Visible = (bool)CurrentPage["ShowThankYouCampaign2"];
            else
                PlaceHolderCampaign2.Visible = false;
        }

        protected void ButtonThankYouCampaign1_Click(object sender, EventArgs e)
        {
            bool isAutogiro = false;
            
            if (CurrentPage["IsAutogiroCampaign1"] != null)
                isAutogiro = (bool)CurrentPage["IsAutogiroCampaign1"];

            StartCampSubscription((string)CurrentPage["ThankYouCampaign1CampId"], isAutogiro);
        }

        protected void ButtonThankYouCampaign2_Click(object sender, EventArgs e)
        {
            bool isAutogiro = false;
            
            if (CurrentPage["IsAutogiroCampaign2"] != null)
                isAutogiro = (bool)CurrentPage["IsAutogiroCampaign2"];
            
            StartCampSubscription((string)CurrentPage["ThankYouCampaign2CampId"], isAutogiro);
        }


        private void StartCampSubscription(string campId, bool isAutogiro)
        {
            DIClassLib.Subscriptions.Subscription newSubscription = null; 
            String c = campId;

            //DIClassLib.Subscriptions.Subscription campSub = new DIClassLib.Subscriptions.Subscription(campId, isAutogiro);
            DIClassLib.Subscriptions.Subscription campSub = new DIClassLib.Subscriptions.Subscription(campId, 0, CampPage.DefaultPayMethod, DateTime.Now, false);

            campSub.Subscriber = CampPage.Sub.Subscriber;
            campSub.TargetGroup = CampPage.GetTargetGroup();
            campSub.SubscriptionPayer = CampPage.Sub.SubscriptionPayer;

            DateTime subsStart = CirixDbHandler.GetIssueDate(CampPage.Sub.PaperCode, CampPage.Sub.ProductNo, CampPage.Sub.SubsEndDate, EnumIssue.Issue.FirstAfterInDate);
           
            campSub.SubsStartDate = subsStart;
            campSub.SubsEndDate = subsStart.AddMonths(campSub.SubsLenMons).AddDays(campSub.SubsLenDays);

            // if the "thank you" campaign is the same product as the main campaign, extend subscription instead of creating new
            bool extendSubs = (CampPage.Sub.PaperCode == campSub.PaperCode && CampPage.Sub.ProductNo == campSub.ProductNo);

            String err = "";
            if (extendSubs)
            {
                // Get the subscribers current subscriptions (so we can get extend it)
                List<DIClassLib.Subscriptions.Subscription> currentSubscriptions = CirixDbHandler.GetSubscriptions2(CampPage.Sub.Subscriber.Cusno);

                // get the newly created subscription
                newSubscription = currentSubscriptions.OrderByDescending(x => x.ExtNo).FirstOrDefault(x => x.SubsNo == CampPage.Sub.SubsNo);

                string communeCode = CirixDbHandler.GetCommuneCode(CampPage.Sub.Subscriber.ZipCode);
                long priceListNo = CirixDbHandler.GetPriceListNo(campSub.PaperCode, campSub.ProductNo, campSub.SubsStartDate, communeCode, campSub.Pricegroup, campSub.CampNo.ToString());

                long payerCusno = 0;
                if (CampPage.Sub.SubscriptionPayer != null)
                    payerCusno = CampPage.Sub.SubscriptionPayer.Cusno;

                String autogiroParam = "N";
                String invoiceMode = Settings.InvoiceMode_BankGiro;
                if (isAutogiro)
                {
                    autogiroParam = "Y";
                    invoiceMode = Settings.InvoiceMode_AutoGiro;
                }

                err = CirixDbHandler.CreateRenewal_DI("WEBCIRIX", CampPage.Sub.SubsNo, (campSub.ExtNo + 1), priceListNo, campSub.CampNo, campSub.SubsLenMons, campSub.SubsLenDays, 
                                                    campSub.SubsStartDate, campSub.SubsEndDate, campSub.SubsKind, campSub.TotalPriceExVat, campSub.TotalPriceExVat, 1, "", payerCusno, 
                                                    campSub.ProductNo, "", "", "", "", "", autogiroParam, campSub.Pricegroup, invoiceMode);
            }
            else
            {
                err = CirixDbHandler.AddNewSubs2(campSub, 0, null);
                
                // check if we got a subscription number in return
                long lSubscriptionNo = 0;
                if (long.TryParse(err, out lSubscriptionNo))
                    newSubscription = campSub;
            }
            
            PlaceHolderCampaign1.Visible = false;
            PlaceHolderCampaign2.Visible = false;
            PlaceHolderComplete.Visible = true;
            PlaceHolderNewSubscriptions.Visible = true;

            bool mainCampIsAutoGiro = false;
            if (CurrentPage["CampaignIsAutogiro"] != null && ((bool)CurrentPage["CampaignIsAutogiro"]) == true)
                mainCampIsAutoGiro = true;

            if (isAutogiro || mainCampIsAutoGiro)
            {
                LiteralThankYouHeader.Text = Translate("/campaigns/thankyou/completeautogiroheading");
                PlaceHolderCompleteAutogiro.Visible = true;
                PlaceHolderCompleteNotAutogiro.Visible = false;
                
            }
            else
            {
                LiteralThankYouHeader.Text = Translate("/campaigns/thankyou/completenotautogiroheading");
                PlaceHolderCompleteAutogiro.Visible = false;
                PlaceHolderCompleteNotAutogiro.Visible = true;
            }

            if (newSubscription != null)
            {
                CampPage.ThankYouSub = newSubscription;
                rptSubscriptions.DataBind();
            }
            
        }


        private void ShowAutogiroBox()
        {
            PlaceHolderComplete.Visible = true;
            PlaceHolderCompleteAutogiro.Visible = true;
        }
    }


}