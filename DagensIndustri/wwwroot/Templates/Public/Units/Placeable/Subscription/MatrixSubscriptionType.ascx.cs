using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Templates.Public.Pages;
using DagensIndustri.Tools.Classes;
using DIClassLib.Subscriptions;

namespace DagensIndustri.Templates.Public.Units.Placeable.Subscription
{
    public partial class MatrixSubscriptionType : UserControlBase
    {
        #region Properties
        public string Name { get; set; }
        public string PriceImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool ShowDiGold { get; set; }
        public string CampaignNo1 { get; set; }
        public string CampaignNo2 { get; set; }
        public SubscriptionType.TypeOfSubscription SubscriptionType { get; set; }
        public bool BecomeDiGoldSelected 
        { 
            get
            {
                return DiGoldCheckBox.Checked;
            }
            set
            {
                DiGoldCheckBox.Checked = value;
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);            
            DiGoldCheckBox.Checked = ((SubscriptionFlow)Page).BecomeDiGoldMember;
        }

        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            //When the continu button is clicked, call the parent page's click event and pass on the parameters
            System.Reflection.MethodInfo mi = Page.GetType().GetMethod("ContinueButton_Click");
            if (mi != null)
                mi.Invoke(Page, new Object[] { sender, e });
        }
        #endregion
    }
}