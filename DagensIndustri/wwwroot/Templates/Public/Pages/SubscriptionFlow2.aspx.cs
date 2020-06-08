using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Membership;
using DIClassLib.CardPayment;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using EPiServer;
using DagensIndustri.Tools.Classes.WebControls;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class SubscriptionFlow2 : DiTemplatePage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var testC1 = GetPage(new PageReference(524));
            var testC2 = GetPage(new PageReference(524));
            var testC3 = GetPage(new PageReference(524));

            var campaigns = new DiLinkCollection(CurrentPage, "LinkCollectionCampaigns");
            

            //var pdc = new PageDataCollection() { testC1, testC2, testC3 };

            PlCampaigns.DataSource = campaigns.SelectedPages();
            PlCampaigns.DataBind();
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            AlterBodyCss(true);
            ((MasterPages.MasterPage)Page.Master).ShowSideBarBoxes(false);
        }

        public object GetPapers(PageData page)
        {
            if (page["Campaign1IncludedProducts"] != null)
            {
                var pdc = new PageDataCollection();
                var productPages = page["Campaign1IncludedProducts"].ToString().Split(',');
                foreach (var product in productPages)
                {
                    try
                    {
                        pdc.Add(GetPage(new PageReference(int.Parse(product))));
                    }
                    catch
                    {
                        //OK, editor deleted page (and ignored warning from EpiServer)
                        //GetPage will explode, no other way to check if page exist
                    }

                }
                return pdc;
            }
            return null;
        }

        /// <summary>
        /// Alter css of body
        /// </summary>
        /// <param name="addCss"></param>
        private void AlterBodyCss(bool addCss)
        {
            HtmlGenericControl body = Master.FindControl("Body") as HtmlGenericControl;
            if (body != null)
            {
                if (addCss)
                {
                    body.Attributes.Add("class", "pren matrix");
                }
                else
                {
                    body.Attributes.Remove("class");
                }
            }
        }
    }
}