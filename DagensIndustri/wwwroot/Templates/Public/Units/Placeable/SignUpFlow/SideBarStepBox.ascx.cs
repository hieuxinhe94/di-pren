using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow
{
    public partial class SideBarStepBox : EPiServer.UserControlBase
    {
        #region Properties

        private Pages.SignUp.SignUpFlow SignUpFlow_
        {
            get
            {
                return (Pages.SignUp.SignUpFlow)Page;
            }
        }

        public string Step { get; set; }

        bool _isInvoice = false;

        #endregion


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int numParticipants = Convert.ToInt32(Request.QueryString["nop"]);
            Step = String.Empty;
            string steps = Translate("/signup/flow/sidebar/totalsteps");

            if (Request.QueryString["pm"] != null && Request.QueryString["pm"].ToString() == "2")
                _isInvoice = true;
            
            List<int> stepsToShow = new List<int>();
            if(!_isInvoice)
                stepsToShow = new List<int>{1,2,3,4};
            else
                stepsToShow = new List<int>{1,2,4};


            MultiView FlowMultiView = SignUpFlow_.MyFlowMultiView;

            if (FlowMultiView.ActiveViewIndex <= numParticipants - 1)
            {
                Step = GetStepText(1);
                AddListItems(stepsToShow, 1);
            }
            else if (FlowMultiView.ActiveViewIndex == numParticipants)
            {
                Step = GetStepText(2);
                AddListItems(stepsToShow, 2);
            }
            else if (FlowMultiView.ActiveViewIndex == numParticipants + 1)
            {
                Step = GetStepText(3);

                if(_isInvoice)
                    AddListItems(stepsToShow, 4);
                else
                    AddListItems(stepsToShow, 3);
            }
            else if (FlowMultiView.ActiveViewIndex == numParticipants + 2)
            {
                Step = GetStepText(4);
                AddListItems(stepsToShow, 4);
            }
        }

        private string GetStepText(int i)
        {
            string s = "4";
            if (_isInvoice)
                s = "3";

            return Translate("/signup/flow/sidebar/step") + " " + Translate("/signup/flow/sidebar/step" + i) + " " + Translate("/signup/flow/sidebar/of") + " " + s;    //Translate("/signup/flow/sidebar/totalsteps");
        }


        protected void AddListItems(List<int> items, int strongItem)
        {
            foreach (int i in items)
            {
                string langStep = "step" + i.ToString() + ".text";
                System.Web.UI.HtmlControls.HtmlGenericControl li = new System.Web.UI.HtmlControls.HtmlGenericControl("li");

                if (i == strongItem)
                {
                    li.Attributes.Add("class", "active");
                }

                li.InnerText = Translate("/signup/flow/sidebar/" + langStep);
                checkoutsteps.Controls.Add(li);
            }
        }
    }
}